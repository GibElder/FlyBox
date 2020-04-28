using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewFlyPage : ContentPage
    {
        private bool isEdit;
        public Fly fly { get; set; }
        private Fly OriginalFLy { get; set; }


        public NewFlyPage(Fly fly)
        {
            isEdit = true;
            this.fly = fly;
            OriginalFLy = new Fly();
            OriginalFLy.Assign(fly);
            InitializeComponent();
            BindingContext = this;
        }

        public NewFlyPage()
        {
            isEdit = false;
            fly = new Fly();
            InitializeComponent();
            fly.FlyID = Guid.NewGuid().ToString();
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            using var context = new FlyBoxcontext();
            if ( isEdit == false)
            {
                context.Fly.Add(fly);
                context.SaveChanges();
            }
            else
            {
                context.Fly.Update(fly);
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
                fly.Assign(OriginalFLy);
                await Navigation.PopModalAsync();

            }
        }


        public void DisplayDry(object sender, EventArgs e)
        {
            Dry.IsVisible = true;
            Nymph.IsVisible = false;
            Streamer.IsVisible = false;
        }

        public void DisplayNymph(object sender, EventArgs e)
        {
            Dry.IsVisible = false;
            Nymph.IsVisible = true;
            Streamer.IsVisible = false;
        }

        public void DisplayStreamer(object sender, EventArgs e)
        {
            Dry.IsVisible = false;
            Nymph.IsVisible = false;
            Streamer.IsVisible = true;
        }

    }
}