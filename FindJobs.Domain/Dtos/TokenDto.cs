using DotNek.Common.Models;

namespace FindJobs.Domain.Dtos
{
    public class TokenResDto
    {
        public string Token { get; set; }
        public MessageCodes? MessageCode { get; set; }
    }

    public class TokenParametersDto
    {
        public string Email { get; set; }
        public bool IsApplicant { get; set; }
        public bool IsCompany { get; set; }
    }
}