using AutoMapper;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Enums;
using FindJobs.DataAccess.Entities;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Dtos.Email;
using FindJobs.Domain.Enums;
using FindJobs.Domain.Repositories;
using FindJobs.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace FindJobs.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMailSenderService sender;
        private readonly IGenericRepository<Applicant> applicantRepository;
        private readonly IGenericRepository<VerificationCode> verificationCodeRepository;
        private readonly IGenericRepository<Company> companyRepository;
        private readonly IMapper mapper;
        private readonly IJWTManagerService jWTManagerService;
        private readonly IRazorPartialToStringService renderer;
        private readonly IConfiguration configuration;

        public AuthService(IMailSenderService sender, IGenericRepository<Applicant> applicantRepository,
            IGenericRepository<VerificationCode> verificationCodeRepository,
            IGenericRepository<Company> companyRepository,
            IMapper mapper, IJWTManagerService jWTManagerService,
            IRazorPartialToStringService renderer,
            IConfiguration configuration)
        {
            this.sender = sender;
            this.applicantRepository = applicantRepository;
            this.verificationCodeRepository = verificationCodeRepository;
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.jWTManagerService = jWTManagerService;
            this.renderer = renderer;
            this.configuration = configuration;
        }



        public async Task<EmailSendResult> SendVerificationCode(string email)
        {
            var verifyCode = MakeRandomDigit();
            var vCodeDto = new VerificationCodeDto()
            {
                Email = email,
                CreateVerifyTime = DateTime.Now.AddMinutes(3),
                VerifyCode = verifyCode
            };
            var vCode = mapper.Map<VerificationCode>(vCodeDto);
            await verificationCodeRepository.AddEntity(vCode);
            await verificationCodeRepository.SaveChange();

            var mailModel = new MailModelDto();
            mailModel.Email = email;
            mailModel.VerifyCode = verifyCode;

            SendVerificationCode verificationModel = new SendVerificationCode
            {
                HeaderTitle = Res.Email.HeaderTitle,
                HeaderSubTitle = Res.Email.HeaderSubTitle,
                InstagramLink = configuration["GlobalSettings:InstagramLink"],
                FacebookLink = configuration["GlobalSettings:FacebookLink"],
                TwitterLink = configuration["GlobalSettings:TwitterLink"],
                Email = email,
                VerifyCode = verifyCode
            };
            var body = await renderer.RenderPartialToStringAsync("/Views/Email/VerificationCode.cshtml", verificationModel);
            mailModel.Body = body;
            mailModel.Subject = "Verification Code";
            mailModel.to = email;
            if (await sender.sendMail(mailModel, "Verify Code"))
            {
                return EmailSendResult.Success;

            }
            return EmailSendResult.NotSend;
        }
        public async Task<TokenResDto> SignInWithVerificationCode(string email, string verifyCode, int type)
        {
            var verificationCodes = await verificationCodeRepository.GetEntities().Where(x => x.CreateVerifyTime >= DateTime.Now).ToListAsync();
            if (verificationCodes == null) return new TokenResDto
            {
                MessageCode = MessageCodes.BadRequest,
                Token = ""
            };
            verificationCodes.Add(new VerificationCode
            {
                Email = email,
                VerifyCode = configuration["AuthSettings:MasterVerifyCode"],
                CreateVerifyTime = DateTime.Now.AddMinutes(3),
            });
            foreach (var vc in verificationCodes)
            {
                if (vc.VerifyCode.Equals(verifyCode))
                {
                    string token = "";
                    if (type == (int)ClientType.Applicant)
                    {
                        var app = await applicantRepository.GetEntities().FirstOrDefaultAsync(x => x.Email == email);
                        var applicantDto = mapper.Map<ApplicantDto>(app);
                        if (applicantDto == null)
                        {
                            applicantDto = new ApplicantDto
                            {
                                Email = email,


                            };
                            var applicants = applicantRepository.GetEntities().ToList();
                            var applicantId = 1;
                            if (applicants.Count > 0)
                            {
                                applicantId = Convert.ToInt32(applicants.Max(x => x.Id)) + 1;
                            }

                            applicantDto.Id = applicantId;
                            var applicant = new Applicant()
                            {
                                Id = applicantId,
                                Email = applicantDto.Email
                            };

                            await applicantRepository.AddEntity(applicant);
                            await applicantRepository.SaveChange();


                        }
                        token = jWTManagerService.GenerateToken(new TokenParametersDto { Email = email, IsApplicant = true });
                        return new TokenResDto { Token = token, MessageCode = MessageCodes.Success };
                    }
                    else if (type == (int)ClientType.Company)
                    {
                        var com = await companyRepository.GetEntities().FirstOrDefaultAsync(x => x.Email == email);
                        var companyDto = mapper.Map<CompanyDto>(com);
                        if (companyDto == null)
                        {
                            var companydto = new CompanyDto()
                            {
                                Email = email,

                            };
                            var company = new Company();
                            company = mapper.Map<Company>(companydto);
                            await companyRepository.AddEntity(company);
                            await companyRepository.SaveChange();


                        }
                        token = jWTManagerService.GenerateToken(new TokenParametersDto { Email = email, IsCompany = true });
                        return new TokenResDto { Token = token, MessageCode = MessageCodes.Success };
                    }
                }

            }
            return new TokenResDto
            {
                MessageCode = MessageCodes.VerificationCodeNotValid,
                Token = ""
            };

        }
        private string MakeRandomDigit()
        {
            var random = new Random();
            string number = random.Next(0, 1000000).ToString("D6");
            return number.ToString();
        }

        public async Task<TokenResDto> SignInWithGoogle(string token)
        {

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var email = jwtSecurityToken.Claims.Where(c => c.Type == "email")
                   .Select(c => c.Value).SingleOrDefault();
            var firstName = jwtSecurityToken.Claims.Where(c => c.Type == "given_name")
                   .Select(c => c.Value).SingleOrDefault();
            var lastName = jwtSecurityToken.Claims.Where(c => c.Type == "family_name")
                   .Select(c => c.Value).SingleOrDefault();
            var picture = jwtSecurityToken.Claims.Where(c => c.Type == "picture")
                   .Select(c => c.Value).SingleOrDefault();
            var applicant = await applicantRepository.GetEntities().SingleOrDefaultAsync(x => x.Email == email);
            if (applicant == null)
            {
                var applicantDto = new ApplicantDto
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                };
                var applicants = applicantRepository.GetEntities().ToList();
                var applicantId = 1;
                if (applicants != null)
                {
                    if (applicants.Count() > 1)
                    {
                        applicantId = Convert.ToInt32(applicants.Max(x => x.Id)) + 1;

                    }
                    else
                        applicantId = 1;

                }
                applicantDto.Id = applicantId;
                var applicantNew = new Applicant
                {
                    Email = applicantDto.Email,
                    FirstName = applicantDto.FirstName,
                    LastName = applicantDto.LastName,
                    Id = applicantDto.Id,
                    ApplicantGoogleImage = picture
                };

                await applicantRepository.AddEntity(applicantNew);
                await applicantRepository.SaveChange();
            }
            token = jWTManagerService.GenerateToken(new TokenParametersDto { Email = email, IsApplicant = true });
            var tokenResDto = new TokenResDto() { Token = token, MessageCode = MessageCodes.Success };
            return tokenResDto;
        }

        public void Dispose()
        {
            applicantRepository?.Dispose();
            companyRepository?.Dispose();
            applicantRepository?.Dispose();
        }

        public async Task<TokenResDto> SignInWithFacebook(FacebookDto facebookDto)
        {
            var applicant = await applicantRepository.GetEntities().SingleOrDefaultAsync(x => x.Email == facebookDto.email);
            if (applicant == null)
            {
                var applicantDto = new ApplicantDto
                {
                    Email = facebookDto.email,
                    FirstName = facebookDto.first_name,
                    LastName = facebookDto.last_name,
                };
                var applicants = applicantRepository.GetEntities().ToList();
                var applicantId = 1;
                if (applicants != null)
                {
                    applicantId = Convert.ToInt32(applicants.Max(x => x.Id)) + 1;
                }
                applicantDto.Id = applicantId;
                var applicantNew = mapper.Map<Applicant>(applicantDto);
                await applicantRepository.AddEntity(applicantNew);
                await applicantRepository.SaveChange();
            }
            var token = jWTManagerService.GenerateToken(new TokenParametersDto { Email = facebookDto.email, IsApplicant = true });
            var tokenResDto = new TokenResDto() { Token = token, MessageCode = MessageCodes.Success };
            return tokenResDto;
        }
    }
}

