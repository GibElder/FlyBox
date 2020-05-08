using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewFlyPage : ContentPage
    {
        public string title { get; set; }
        private bool isEdit;
        private Fly fly;
        private Nymph nymph;
        private Dry dry;
        private Streamer streamer;
        private FlyDetailPage parentPage;

        private Xamarin.Forms.Color textColor = Color.Blue;
        public Fly Fly { get => fly; set { fly = value; OnPropertyChanged("Fly"); } }

        public Nymph Nymph { get => nymph; set { nymph = value; OnPropertyChanged("Nymph"); } }

        public Dry Dry { get => dry; set { dry = value; OnPropertyChanged("Dry"); } }

        public Streamer Streamer { get => streamer; set { streamer = value; OnPropertyChanged("Streamer"); } }

        private Fly OriginalFly { get; set; }


        public NewFlyPage(Fly fly, FlyDetailPage parentPage)
        {
            title = "Edit Fly";
            isEdit = true;
            this.Fly = fly;
            OriginalFly = new Fly();
            OriginalFly.Assign(fly);
            this.parentPage = parentPage;
            InitializeComponent();
            BindingContext = this;

            if (fly is Dry)
            {
                DisplayDry(null, null);
            }
            else if ( Fly is Nymph)
            {
                DisplayNymph(null, null);
            }
            else if (fly is Streamer)
            {
                DisplayStreamer(null, null);
            }

        }

        public NewFlyPage()
        {
            title = "New Fly";
            isEdit = false;
            Fly = new Fly();
            InitializeComponent();
            Fly.FlyID = Guid.NewGuid().ToString();
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            // Make sure fly name isn't empty 
            if(string.IsNullOrEmpty(Fly.FlyName))
            {
                await DisplayAlert("Error", "Fly name cannot be empty!", "OK");
                return;
            }
            // Make sure fly name is less than 30 characters
            if (Fly.FlyName.Length > 30)
            {
                await DisplayAlert("Error", "Fly name must be less than 30 characters!", "OK");
                return;
            }

            // Make sure fly color is specified
            if (string.IsNullOrEmpty(Fly.Color))
            {
                await DisplayAlert("Error", "Fly color must be specified!", "OK");
                return;
            }
            // Make sure fly description is less than 240 characters
            if(Fly.Description != null && Fly.Description.Length > 240)
            {
                await DisplayAlert("Error", "Fly description must be less than 240 characters!", "OK");
                return;
            }
            using var context = new FlyBoxcontext();
            if (isEdit == false)
            {
                context.Fly.Add(Fly);
                context.SaveChanges();
            }
            else
            {
                context.Fly.Update(Fly);
                context.SaveChanges();
            }
            if (parentPage != null)
                parentPage.Fly = Fly;
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            if (isEdit == false)
            {
                //if (parentPage != null)
                //    parentPage.Fly = Fly;
                await Navigation.PopModalAsync();
            }
            else
            {
                Fly.Assign(OriginalFly);
                if (parentPage != null)
                    parentPage.Fly = Fly;

                await Navigation.PopModalAsync();

            }
        }


        public void DisplayDry(object sender, EventArgs e)
        {
            Dry1.IsVisible = true;
            Nymph1.IsVisible = false;
            Streamer1.IsVisible = false;
            BtDry.TextColor = Color.Black;
            BtNymph.TextColor = textColor;
            BtStreamer.TextColor = textColor;

            var tmpdry = new Dry();
            tmpdry.Assign(Fly);
            Fly = tmpdry;
            Dry = tmpdry;
            Nymph = null;
            Streamer = null;

        }

        public void DisplayNymph(object sender, EventArgs e)
        {
            Dry1.IsVisible = false;
            Nymph1.IsVisible = true;
            Streamer1.IsVisible = false;
            BtDry.TextColor = textColor;
            BtNymph.TextColor = Color.Black;
            BtStreamer.TextColor = textColor;


            var tmpnymph = new Nymph();
            tmpnymph.Assign(Fly);
            Fly = tmpnymph;
            Dry = null;
            Nymph = tmpnymph;
            Streamer = null;
        }

        public void DisplayStreamer(object sender, EventArgs e)
        {
            Dry1.IsVisible = false;
            Nymph1.IsVisible = false;
            Streamer1.IsVisible = true;
            BtDry.TextColor = textColor;
            BtNymph.TextColor = textColor;
            BtStreamer.TextColor = Color.Black;


            var tmpstreamer = new Streamer();
            tmpstreamer.Assign(Fly);
            Fly = tmpstreamer;
            Dry = null;
            Nymph = null;
            Streamer = tmpstreamer;
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
                //Directory = "Sample",
                //Name = Fly.FlyID + ".jpg"
                SaveToAlbum = true
            }); ;

            if (file == null)
                return;

            Fly.ImagePath = file.Path;

            OnPropertyChanged("Fly.ImagePath");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });


        }

    }
}