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
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CatchesPage : ContentPage
    {
        private ObservableCollection<Catch> _Catches;

        public ObservableCollection<Catch> Catches { get => _Catches; set { _Catches = value; OnPropertyChanged("Catches"); } }

        public CatchesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            await Navigation.PushAsync(new CatchDetailPage(args.CurrentSelection.FirstOrDefault() as Catch));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewCatchPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var context = new FlyBoxcontext();
            var list = context.Catch.Include(o => o.Fly).ToList();
            if(list.Count == 0)
            {
                var newfish = new Catch();
                newfish.CatchID = "1";
                newfish.FishType = "Smallmouth Bass";
                newfish.Location = "Secret River";
                newfish.Description = "A very beautiful catch";
                newfish.FlyID = "1";
                newfish.Size = 10;
                context.Catch.Add(newfish);
                context.SaveChanges();
                list = context.Catch.Include(o => o.Fly).ToList();

            }
            Catches = new ObservableCollection<Catch>(list);
        }
    }
}