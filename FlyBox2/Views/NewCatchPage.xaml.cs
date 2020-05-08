using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Plugin.Media;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewCatchPage : ContentPage
    {
        private bool isEdit;
        public string title { get; set; }
        private Fly selectedFly;

        public Catch Catch { get; set; }
        private Catch OriginalCatch { get; set; }
        public ObservableCollection<Fly> FlyList { get; set; }
        public Fly SelectedFly { get => selectedFly; set { selectedFly = value; OnPropertyChanged("SelectedFly"); } }

        public NewCatchPage(Catch Catch)
        {
            isEdit = true;
            title = "Edit Catch";
            InitializeComponent();
            InitPicker();

            this.Catch = Catch;
            SelectedFly = FlyList.SingleOrDefault(f => f.FlyID == Catch.Fly?.FlyID);

            OriginalCatch = new Catch();
            OriginalCatch.Assign(Catch);

            BindingContext = this;
        }

        public NewCatchPage()
        {
            isEdit = false;
            title = "New Catch";
            Catch = new Catch();
            InitPicker();
            InitializeComponent();
            Catch.CatchID = Guid.NewGuid().ToString();
            BindingContext = this;
        }


        private void InitPicker()
        {
            using var context = new FlyBoxcontext();
            FlyList = new ObservableCollection<Fly>(context.Fly.ToList());
        }


        async void Save_Clicked(object sender, EventArgs e)
        {
            Catch.FlyID = SelectedFly?.FlyID;
            using var context = new FlyBoxcontext();

            if (SelectedFly?.FlyID != null)
            {
                Catch.Fly = context.Fly.Where(f => f.FlyID == Catch.FlyID).SingleOrDefault();
            }
            // check if fish type is set
            if (string.IsNullOrEmpty(Catch.FishType))
            {
                await DisplayAlert("Error", "You must enter a type of fish!", "OK");
                return;
            }

            // Make sure catch size > 0
            if (Catch.Size <= 0) {
                
                await DisplayAlert("Error", "Catch must be greater than zero!", "OK");
                return;
                
            }
            // Check if fly is specified
            if (Catch.Fly == null)
            {
                await DisplayAlert("Error", "You must choose a fly.", "OK");
                return;
            }

            // Check that if description is set, that its less than 240 characters
            if (Catch.Description != null && Catch.Description.Length > 240)
            {
                await DisplayAlert("Error", "Catch description must be less than 240 characters!", "OK");
                return;
            }


            // Check if location was specified
            if (string.IsNullOrEmpty(Catch.Location))
            {
                bool answer = await DisplayAlert("Location", "Save catch without location?", "Yes", "No");
                if(answer) {
                    return;
                }
            }
           
            if (isEdit == false)
            {
                context.Catch.Add(Catch);
                context.SaveChanges();
            }
            else
            {
                context.Catch.Update(Catch);
                context.SaveChanges();
            }
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            if (isEdit == false)
                await Navigation.PopModalAsync();
            else
            {
                Catch.Assign(OriginalCatch);
                await Navigation.PopModalAsync();

            }
        }


        public async void TakePicture(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true
            }); ;

            if (file == null)
                return;

            Catch.ImagePath = file.Path;

            OnPropertyChanged("Catch.ImagePath");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });


        }
    }
}