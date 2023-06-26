using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.App.helper;
using FindJobs.App.helper.Constant;
using FindJobs.App.Themes;
using FindJobs.Domain.Dtos;
using FindJobs.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindJobs.App.Pages.ApplicantPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalInformation : MainTheme
	{
        public ApplicantDto applicantDto { get; set; }
        public PersonalInformation()
		{
			InitializeComponent ();

            Load_Data();

			Fill_DropDown();



        }

        private async void Load_Data()
        {
           
            var token = await SecureStorage.GetAsync("AuthToken");
            var result = await API.GetData<ResultDto<ApplicantProfileDto>>(Urls.ApiPath, "Applicant/GetPersonalInformation", token);
            if (result.MessageCode ==(int) MessageCodes.Success)
            {
                LoadPersonalInformationData(result.Data);
            }
        }

        private void LoadPersonalInformationData(ApplicantProfileDto data)
        {
            GenderPicker.SelectedIndex =(int) data.Gender;
            firstName.Text = data.FirstName;
            lastName.Text = data.LastName;
            if(data.DateOfBirth !=null)
            birtOfDate.Date = data.DateOfBirth.Value;
            readyToWorkStatusPicker.SelectedIndex =(int) data.ReadyToWorkStatus;
            if (data.AvailableDate != null)
                availableDate.Date = data.AvailableDate.Value;
            jobPosition.Text = data.JobPosition;
        }
      
        private void Fill_DropDown()
        {
            GenderPicker.Items.Add(Res.ApplicantProfile.male);
            GenderPicker.Items.Add(Res.ApplicantProfile.female);
            GenderPicker.Items.Add(Res.ApplicantProfile.RatherNotToSay);

            readyToWorkStatusPicker.Items.Add(Res.ApplicantProfile.NotAvailabeUntil);
            readyToWorkStatusPicker.Items.Add(Res.ApplicantProfile.ASAP);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if(plAvalableDate.IsVisible == true)
            {

            }
            var applicant = new ApplicantDto
            {
                FirstName = firstName.Text,
                LastName = lastName.Text,
                DateOfBirth = birtOfDate.Date,
                Gender = (Gender)GenderPicker.SelectedIndex,
               
                JobPosition = jobPosition.Text,
                ReadyToWorkStatus = (ReadyToWork)readyToWorkStatusPicker.SelectedIndex
            };
            if (plAvalableDate.IsVisible)
            {
                applicant.AvailableDate = availableDate.Date;
            }
            else
            {
                applicant.AvailableDate = null;
            }
            var token = await SecureStorage.GetAsync("AuthToken");
            var result = await API.PostData<ResultDto<ApplicantProfileDto>>(Urls.ApiPath, "Applicant/CreateOrUpdateApplicantPersonalInformation", applicant, token);
            if (result.MessageCode == (int)MessageCodes.Success)
            {
                LoadPersonalInformationData(result.Data);
               await DisplayAlert(Res.ApplicantProfile.Save, Res.ApplicantProfile.SaveSuccessfully, Res.ApplicantProfile.ButtonOk);
            }
            else
            {
               await DisplayAlert(Res.ApplicantProfile.Save, Res.ApplicantProfile.NotSaveSuccessfully, Res.ApplicantProfile.ButtonOk);
            }

        }

        private void readyToWorkStatusPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItemText = readyToWorkStatusPicker.Items[readyToWorkStatusPicker.SelectedIndex];
            if (selectedItemText == "ASAP")
            {
                plAvalableDate.IsVisible = false;
            }
            else
            {
                plAvalableDate.IsVisible = true;
            }

        }
    }
}