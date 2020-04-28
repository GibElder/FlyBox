﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;
using FlyBox2.Views;
using FlyBox2.ViewModels;
using System.Collections.ObjectModel;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class FlyBoxPage : ContentPage
    {
        private ObservableCollection<Fly> flies;

        public ObservableCollection<Fly> Flies { get => flies; set { flies = value; OnPropertyChanged("Flies"); } }

        public FlyBoxPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var fly = (Fly)layout.BindingContext;
            await Navigation.PushAsync(new FlyDetailPage(fly));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewFlyPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = new FlyBoxcontext();
            var list = context.Fly.ToList();

            if (list.Count == 0)
            {
                var newfly = new Fly();
                newfly.FlyID = "1";
                newfly.FlyName = "Wolly Bugger";
                newfly.Color = "Brown";
                newfly.Description = "This is a good discription";

                context.Fly.Add(newfly);

                newfly = new Fly();
                newfly.FlyID = "2";
                newfly.FlyName = "Test";
                newfly.Color = "Blue";

                context.Fly.Add(newfly);

                context.SaveChanges();

                list = context.Fly.ToList();
            }

            Flies = new ObservableCollection<Fly>(list);
        }
    }
}