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
        public Fly Fly { get; set; }

        public FlyDetailPage(Fly fly)
        {
            InitializeComponent();
            this.Fly = fly;
            BindingContext = this;
        }

        async void Edit_Clicked(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            await Navigation.PushModalAsync(new NavigationPage(new NewFlyPage(Fly)));
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            using var context = new FlyBoxcontext();
            context.Fly.Remove(Fly);
            await context.SaveChangesAsync();
            await Navigation.PopAsync();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OnPropertyChanged("Fly");
        }
    }
}