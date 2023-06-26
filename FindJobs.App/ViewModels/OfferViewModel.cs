using System;
using Xamarin.Forms;

namespace FindJobs.App.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public ImageSource Image1 { get; set; }
        public string CompanyEmail { get; set; }
        public string JobCategory { get; set;}
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string City { get; set;}
        public DateTime DateOfOffer { get; set; }
        public string RegisterDate { get; set; }
        public string FavouriteIcon { get; set; } 
    }
}
