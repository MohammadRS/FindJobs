using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace FindJobs.Domain.Services
{
    public interface IAuthService:IDisposable
    {
        Task<EmailSendResult> SendVerificationCode(string email);
        Task<TokenResDto> SignInWithVerificationCode(string email, string verificationCode,int type);
       Task<TokenResDto> SignInWithGoogle(string token);
       Task<TokenResDto> SignInWithFacebook(FacebookDto facebookDto);





    }
}
