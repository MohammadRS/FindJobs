using DotNek.Common.Helpers;
using DotNek.Common.Models;
using DotNek.WebComponents.Areas.Auth.Enums;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


namespace FindJobs.Web.Helper
{
    public static class CheckUserAccess
    {
        public static async  Task<bool> IsUserHasAccess(string tokenAuth,ClientType type,IConfiguration configuration)
        {
            if (type == ClientType.Company)
            {
                var isCompanyRole = await API.GetData<ResultDto>(configuration["GlobalSettings:ApiUrl"], "Company/checkcompanyrole",
               tokenAuth);
                if (isCompanyRole.MessageCode.Equals((int)MessageCodes.Unauthorized)) return false;
            }
            if(type == ClientType.Applicant)
            {
                var isApplicantRole = await API.GetData<ResultDto>(configuration["GlobalSettings:ApiUrl"], "Applicant/checkapplicantrole",
            tokenAuth);
                if (isApplicantRole.MessageCode.Equals((int)MessageCodes.Unauthorized)) return false;
            }
            return true;
        }
    }
}
