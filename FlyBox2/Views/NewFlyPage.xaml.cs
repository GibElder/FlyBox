﻿using System;
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
        private Fly fly;
        private Nymph nymph;
        private Dry dry;
        private Streamer streamer;

        public Fly Fly { get => fly; set { fly = value; OnPropertyChanged("Fly"); } }

        public Nymph Nymph { get => nymph; set { nymph = value; OnPropertyChanged("Nymph"); } }

        public Dry Dry { get => dry; set { dry = value; OnPropertyChanged("Dry"); } }

        public Streamer Streamer { get => streamer; set { streamer = value; OnPropertyChanged("Streamer"); } }

        private Fly OriginalFLy { get; set; }


        public NewFlyPage(Fly fly)
        {
            isEdit = true;
            this.Fly = fly;
            OriginalFLy = new Fly();
            OriginalFLy.Assign(fly);
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
            isEdit = false;
            Fly = new Fly();
            InitializeComponent();
            Fly.FlyID = Guid.NewGuid().ToString();
            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
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
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            if (isEdit == false)
                await Navigation.PopModalAsync();
            else
            {
                Fly.Assign(OriginalFLy);
                await Navigation.PopModalAsync();

            }
        }


        public void DisplayDry(object sender, EventArgs e)
        {
            Dry1.IsVisible = true;
            Nymph1.IsVisible = false;
            Streamer1.IsVisible = false;
            BtDry.TextColor = Color.Black;
            BtNymph.TextColor = Color.Blue;
            BtStreamer.TextColor = Color.Blue;

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
            BtDry.TextColor = Color.Blue;
            BtNymph.TextColor = Color.Black;
            BtStreamer.TextColor = Color.Blue;


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
            BtDry.TextColor = Color.Blue;
            BtNymph.TextColor = Color.Blue;
            BtStreamer.TextColor = Color.Black;


            var tmpstreamer = new Streamer();
            tmpstreamer.Assign(Fly);
            Fly = tmpstreamer;
            Dry = null;
            Nymph = null;
            Streamer = tmpstreamer;
        }

    }
}