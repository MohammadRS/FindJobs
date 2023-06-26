using FindJobs.App.helper.Constant;
using FindJobs.App.ViewModels;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Global;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobCategorySelectPage : PopupPage
    {
        private List<JobCategoryDto> jobCategories;
        public ObservableCollection<jobCategoryViewModel> observeJobCategory;
        public ObservableCollection<jobCategoryViewModel> selectedList;
        public EventHandler<string> dataEventHandler;
        public string categoryNames = "";
        public JobCategorySelectPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            LoadData();
            jobSearchListView.ItemsSource = observeJobCategory;
            base.OnAppearing();
        }
       
        private void serchJobPosition_TextChanged(object sender, TextChangedEventArgs e)
        {

            var text = e.NewTextValue;
            if (!string.IsNullOrWhiteSpace(text)  && text != null)
            {
                LoadData();
                var list = observeJobCategory.Where(x => x.JobCategory.ToLower().Trim().Contains(text.ToLower().Trim())).ToList();
                jobSearchListView.ItemsSource = list;
            }
            else
            {
                LoadData();
                jobSearchListView.ItemsSource = observeJobCategory;
            }

        }

        private void jobCategoryCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            selectedList = new ObservableCollection<jobCategoryViewModel>();

            for (int i = 0; i < observeJobCategory.Count; i++)
            {
                jobCategoryViewModel item = observeJobCategory[i];

                if (item.IsChecked)
                {
                    selectedList.Add(item);
                }
            }
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }

        private void Cancel_Jobcategory_Clicked(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
            dataEventHandler?.Invoke(this, "");
        }

        private void LoadData()
        {
            jobCategories = Task.Run(async () =>
            {
                return await Global.GetJobCategories(Urls.ApiPath);
            }).Result.ToList();

            List<jobCategoryViewModel> list = new List<jobCategoryViewModel>();

            foreach (var item in jobCategories)
            {
                list.Add(new jobCategoryViewModel()
                {
                    Id = item.Id,
                    IsChecked = false,
                    JobCategory = item.Jobcategory
                });
            }
            observeJobCategory = new ObservableCollection<jobCategoryViewModel>(list);
           
        }
        private void Done_JobCategories_Clicked(object sender, EventArgs e)
        {
            if (selectedList.Count > 0)
            {
                foreach (var item in selectedList)
                {
                    categoryNames += item.JobCategory + ",";
                }
                dataEventHandler?.Invoke(this, categoryNames);
                Navigation.PopPopupAsync();
            }
            else
            {
                dataEventHandler?.Invoke(this, "");
                Navigation.PopPopupAsync();
            }
        }
    }
}