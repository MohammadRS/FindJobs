using Android.Content.PM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FindJobs.App.helper
{

   public class CameraGallery
    {
        static string _ActionSheetTitle = Res.Messages.ChoosAnOption;
        static string _ActionSheetCancel =Res.Messages.Cancel;
        static string[] _ActionSheetButtons = new string[] { Res.Messages.TakeAPhoto, Res.Messages.PickFromGallery };

        [Obsolete]
        public static void Init(string ActionSheetTitle = null, string ActionSheetCancel = null, string[] ActionSheetButtons = null)
        {
            Plugin.Media.CrossMedia.Current.Initialize();
            if (ActionSheetTitle != null) _ActionSheetTitle = ActionSheetTitle;
            if (ActionSheetCancel != null) _ActionSheetCancel = ActionSheetCancel;
            if (ActionSheetButtons != null) _ActionSheetButtons = ActionSheetButtons;
        }


#if __ANDROID__
    public void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
#endif


        public static async Task<List<byte[]>> PickOrTakePhoto(Xamarin.Forms.Page page, Action<List<byte[]>> CallBack)
        {
            string action = await page.DisplayActionSheet(_ActionSheetTitle, _ActionSheetCancel, null, _ActionSheetButtons);
            try
            {
                //pick or take photo
                List<Plugin.Media.Abstractions.MediaFile> photos = new List<Plugin.Media.Abstractions.MediaFile>();
                if (action == _ActionSheetButtons[0])
                {
                    try
                    {
                        photos.Clear();
                        photos.Add(await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                        {
                            SaveToAlbum = true,
                            DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                            Directory = Xamarin.Essentials.AppInfo.Name,
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 700,
                            CompressionQuality = 30,
                        }));
                    }
                    catch (Exception ex)
                    {
                        await page.DisplayAlert(Res.Messages.Camera, Res.Messages.CameraNotFound, Res.Messages.Cancel);
                    }
                }
                if (action == _ActionSheetButtons[1])
                {
                    try
                    {
                        photos = await Plugin.Media.CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 1600,
                            CompressionQuality = 80,
                        }, new Plugin.Media.Abstractions.MultiPickerOptions() { MaximumImagesCount = 7, DoneButtonTitle = Res.Messages.Done, BackButtonTitle = Res.Messages.Back, PhotoSelectTitle = Res.Messages.Select, AlbumSelectTitle = Res.Messages.Gallery, BarStyle = Plugin.Media.Abstractions.MultiPickerBarStyle.Default, LoadingTitle = Res.Messages.Loading });
                    }
                    catch
                    {
                    }
                }

                //check result
                Stream stream = null;
                if (photos != null)
                {

                    List<byte[]> Images = new List<byte[]>();
                    Task<bool> task = Task.Run(async () =>
                    {
                        foreach (var photo in photos)
                        {
                            if (photo != null)
                            {
                                stream = photo.GetStream();
                                if (stream != null)
                                {
                                    Images.Add(StreamToByteArray(stream));
                                }
                            }
                        }
                        return true;
                    });

                    Task UItask = task.ContinueWith(async (t) =>
                    {
                        if (t.Result)
                        {
                            CallBack(Images);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    return Images;


                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static byte[] StreamToByteArray(Stream stream)
        {
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Position = 0;
            return data;
        }
    }
}