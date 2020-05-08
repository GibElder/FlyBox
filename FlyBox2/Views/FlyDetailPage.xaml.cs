using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class FlyDetailPage : ContentPage
    {
        private Fly fly;
        private Nymph nymph;
        private Dry dry;
        private Streamer streamer;

        public Fly Fly { get => fly;
            set
            {
                fly = value;

                Nymph = null;
                Dry = null;
                Streamer = null;

                ChangeFlyType();

                OnPropertyChanged("Fly");
            } }

        public Nymph Nymph { get => nymph; set { nymph = value; OnPropertyChanged("Nymph"); } }

        public Dry Dry { get => dry; set { dry = value; OnPropertyChanged("Dry"); } }

        public Streamer Streamer { get => streamer; set { streamer = value; OnPropertyChanged("Streamer"); } }

        public FlyDetailPage(Fly fly)
        {
            InitializeComponent();
            this.Fly = fly;
            BindingContext = this;
        }

        private void ChangeFlyType()
        {
            
            if (fly is Dry)
            {
                Dry = fly as Dry;
                Dry1.IsVisible = true;
                Nymph1.IsVisible = false;
                Streamer1.IsVisible = false;
            }
            else if (fly is Nymph)
            {
                Nymph = fly as Nymph;
                Dry1.IsVisible = false;
                Nymph1.IsVisible = true;
                Streamer1.IsVisible = false;
            }
            else if (fly is Streamer)
            {
                Streamer = fly as Streamer;
                Dry1.IsVisible = false;
                Nymph1.IsVisible = false;
                Streamer1.IsVisible = true;
            }
        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            await Navigation.PushModalAsync(new NavigationPage(new NewFlyPage(Fly, this)));
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            if ( fly.Catches?.Count > 0 )
            {
                await DisplayAlert("Alert", "This fly has caught fish you can't delete it!", "OK");

            }
            else
            {
                bool delete = await DisplayAlert("Delete Fly", "Are you sure you want to delete this fly?", "Yes", "No");
                if (!delete)
                {
                    return;
                }
                using var context = new FlyBoxcontext();
                context.Fly.Remove(Fly);
                await context.SaveChangesAsync();
                await Navigation.PopAsync();

            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OnPropertyChanged("Fly");
        }
    }
}