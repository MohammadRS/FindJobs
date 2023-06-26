using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterOffersPage : PopupPage
    {
        ObservableCollection<JobCategoryFilter> jobCategoryFilterObservable;

        ObservableCollection<TypeOfEmployeeFilter> EmployeeTypeFilterObservable;
        ObservableCollection<CompanyOfferFilter> CompanyFilterObservable;
        ObservableCollection<LanguageSkillFilter> LanguageFilterObservable;
        ObservableCollection<WorkAreaFilter> WorkAreaFilterObservable;
        public EventHandler<OffersFilter> FilterEvent;
        OffersFilter OffersFilter;
        public FilterOffersPage(OffersFilter offersFilter)
        {
            OffersFilter= offersFilter;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            InitialData();
        }

        private void InitialData()
        {
            
            Task<FilterOffersPage>.Run(async () =>
            {
                var result = await API.GetData<ResultDto<OffersFilter>>(helper.Constant.Urls.ApiPath, "Company/GetOffersFilter");

                if (result != null)
                {
                    if (OffersFilter.jobCategoryFilters.Count()>0)
                    {
                        foreach (var item in OffersFilter.jobCategoryFilters)
                        {
                            foreach (var item2 in result.Data.jobCategoryFilters)
                            {
                                if (item.CategoryId == item2.CategoryId)
                                {
                                    item2.IsChecked= true;
                                }
                            }
                        }
                    }
                    if (OffersFilter.TypeOfEmployeeFilters.Count() > 0)
                    {
                        foreach (var item in OffersFilter.TypeOfEmployeeFilters)
                        {
                            foreach (var item2 in result.Data.TypeOfEmployeeFilters)
                            {
                                if (item.Type == item2.Type)
                                {
                                    item2.IsChecked = true;
                                }
                            }
                        }
                    }
                    if (OffersFilter.CompanyOfferFilters.Count() > 0)
                    {
                        foreach (var item in OffersFilter.CompanyOfferFilters)
                        {
                            foreach (var item2 in result.Data.CompanyOfferFilters)
                            {
                                if (item.ComapnyName == item2.ComapnyName)
                                {
                                    item2.IsChecked = true;
                                }
                            }
                        }
                    }
                    if (OffersFilter.LanguageSkillFilters.Count() > 0)
                    {
                        foreach (var item in OffersFilter.LanguageSkillFilters)
                        {
                            foreach (var item2 in result.Data.LanguageSkillFilters)
                            {
                                if (item.LanguageSkillId == item2.LanguageSkillId)
                                {
                                    item2.IsChecked = true;
                                }
                            }
                        }
                    }
                    if (OffersFilter.WorkAreaFilters.Count() > 0)
                    {
                        foreach (var item in OffersFilter.WorkAreaFilters)
                        {
                            foreach (var item2 in result.Data.WorkAreaFilters)
                            {
                                if (item.WorkAreaName == item2.WorkAreaName)
                                {
                                    item2.IsChecked = true;
                                }
                            }
                        }
                    }

                    jobCategoryFilterObservable = new ObservableCollection<JobCategoryFilter>(result.Data.jobCategoryFilters.ToList());
                    
                       
                         EmployeeTypeFilterObservable = new ObservableCollection<TypeOfEmployeeFilter>(result.Data.TypeOfEmployeeFilters);
                         CompanyFilterObservable = new ObservableCollection<CompanyOfferFilter>(result.Data.CompanyOfferFilters);
                         LanguageFilterObservable = new ObservableCollection<LanguageSkillFilter>(result.Data.LanguageSkillFilters);
                         WorkAreaFilterObservable = new ObservableCollection<WorkAreaFilter>(result.Data.WorkAreaFilters);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (OffersFilter.jobCategoryFilters.Count() > 0 || OffersFilter.TypeOfEmployeeFilters.Count() > 0 ||
                        OffersFilter.CompanyOfferFilters.Count() > 0 || OffersFilter.WorkAreaFilters.Count() > 0 || OffersFilter.LanguageSkillFilters.Count() > 0)
                        {
                            ClearFilterButton.SetValue(IsVisibleProperty, true);
                        }
                        else
                        {
                            ClearFilterButton.SetValue(IsVisibleProperty, false);
                        }
                           
                        jobCategoryFilterCollectionView.ItemsSource = jobCategoryFilterObservable;
                         EmployeeTypeFilterCollectionView.ItemsSource = EmployeeTypeFilterObservable;
                         CompanyFilterCollectionView.ItemsSource = CompanyFilterObservable;
                         LanguageFilterCollectionView.ItemsSource = LanguageFilterObservable;
                         WorkAreaFilterCollectionView.ItemsSource = WorkAreaFilterObservable;
                     });
                }
            });
        }

        private async void onCloseButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private void FilterBtn_Clicked(object sender, System.EventArgs e)
        {
            OffersFilter = new OffersFilter();
            foreach (var item in jobCategoryFilterObservable)
            {
                if (item.IsChecked)
                {
                    OffersFilter.jobCategoryFilters.Add(new JobCategoryFilter { CategoryId = item.CategoryId, CategoryName = item.CategoryName, Count = item.Count, IsChecked = item.IsChecked });
                }
            }
            foreach (var item in EmployeeTypeFilterObservable)
            {
                if (item.IsChecked)
                    OffersFilter.TypeOfEmployeeFilters.Add(new TypeOfEmployeeFilter { Type = item.Type, Count = item.Count, IsChecked = item.IsChecked });
            }
            foreach (var item in CompanyFilterObservable)
            {
                if (item.IsChecked)
                    OffersFilter.CompanyOfferFilters.Add(new CompanyOfferFilter { ComapnyName = item.ComapnyName, Count = item.Count, IsChecked = item.IsChecked });
            }
            foreach (var item in LanguageFilterObservable)
            {
                if (item.IsChecked)
                    OffersFilter.LanguageSkillFilters.Add(new LanguageSkillFilter { LanguageName = item.LanguageName, LanguageSkillId = item.LanguageSkillId, Count = item.Count, IsChecked = item.IsChecked });
            }
            foreach (var item in WorkAreaFilterObservable)
            {
                if (item.IsChecked)
                    OffersFilter.WorkAreaFilters.Add(new WorkAreaFilter { WorkAreaName = item.WorkAreaName, Count = item.Count, IsChecked = item.IsChecked });
            }
            FilterEvent?.Invoke(this, OffersFilter);
            Navigation.PopPopupAsync();
        }

        private async void CancelBtn_Clicked(object sender, System.EventArgs e)
        {
           await PopupNavigation.Instance.PopAsync();
        }

        private void OnClearButtonCliced(object sender, EventArgs e)
        {
            OffersFilter = new OffersFilter();
            FilterEvent?.Invoke(this, OffersFilter);
            Navigation.PopPopupAsync();
        }
    }
}