using Xamarin.Forms;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FindJobs.App.Pages;
using Rg.Plugins.Popup.Services;
using FindJobs.App.Pages.ApplicantPages;
using Rg.Plugins.Popup.Events;
using System.Linq;
using FindJobs.App.Pages.Offers;
using FindJobs.Domain.Enums;

namespace FindJobs.App.helper
{
    public class Nav
    {
        public static async Task Find()
        {
            var userState = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrWhiteSpace(userState))
            {

                if (Application.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                   Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(FindPage))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new FindPage("",""));
                }

            }
            else
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);

        }
        public static async Task MyJob()
        {
            var userState = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrWhiteSpace(userState))
            {

                if (Application.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                   Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(MyJobsPage))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new MyJobsPage());
                }

            }
            else
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);

        }
        public static async Task JobAlert()
        {
            var userState = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrWhiteSpace(userState))
            {

                if (Application.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                   Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(JobAlertPage))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new JobAlertPage());
                }
            }
            else
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);

        }
        public static async Task Suggest()
        {
            var userState = await SecureStorage.GetAsync("AuthToken");
            if (!string.IsNullOrWhiteSpace(userState))
            {

                if (Application.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                   Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(FavouritePage))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new FavouritePage());
                }
            }
            else
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);


        }
        public static async Task Profile()
        {
            var userState = await SecureStorage.GetAsync("AuthToken");
            var loginType = await SecureStorage.GetAsync("LoginType");
            if (!string.IsNullOrWhiteSpace(userState) && !string.IsNullOrEmpty(loginType) && int.Parse(loginType)==(int) ClientTypes.Applicant)
            {
                if (Application.Current.MainPage.Navigation.NavigationStack.Count==0 ||
                   Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(ApplicantProfile))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new ApplicantProfile());
                }
            }else if(!string.IsNullOrWhiteSpace(userState) && !string.IsNullOrEmpty(loginType) && int.Parse(loginType) == (int)ClientTypes.Company)
            {
                if (Application.Current.MainPage.Navigation.NavigationStack.Count == 0 ||
                  Application.Current.MainPage.Navigation.NavigationStack.Last().GetType() != typeof(CompanyProfile))
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                    await Application.Current.MainPage.Navigation.PushAsync(new CompanyProfile());
                }
            }
            else
                await PopupNavigation.Instance.PushAsync(new LoginPage(() => { }), false);
        }


     
        public static async Task ApplicantProfileDetails()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ApplicantProfilePage());
        }
        public static async Task ApplicantContactInfo()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ContactPage());


        }
        public static async Task ApplicantUploadDocuments()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new UploadDocumentPage());

        }

        public static async Task ApplicantWorkExperince()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new WorkExperiencePage());
        }

        public static async Task ApplicantWorkEducation()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new EducationPage());
        }

        public static async Task ApplicantKnowledge()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new KnowLedgePage());
        }
        public static async Task ApplicantLanguage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LanguagePage());
        }

        public static async Task ApplicantAdditional()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AdditionalSectionPage());
        }

    }
}
