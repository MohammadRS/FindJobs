using FindJobs.Domain.Dtos;

namespace FindJobs.Domain.Services
{
    public interface IJWTManagerService
    {
       string GenerateToken(TokenParametersDto tokenParameters);
    }
}
