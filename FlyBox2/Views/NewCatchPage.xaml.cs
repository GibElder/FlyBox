using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using FlyBox2.Models;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlyBox2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewCatchPage : ContentPage
    {
        private bool isEdit;
        private Fly selectedFly;

        public Catch Catch { get; set; }
        private Catch OriginalCatch { get; set; }
        public ObservableCollection<Fly> FlyList { get; set; }
        public Fly SelectedFly { get => selectedFly; set { selectedFly = value; OnPropertyChanged("SelectedFly"); } }

        public NewCatchPage(Catch Catch)
        {
            isEdit = true;

            InitializeComponent();
            InitPicker();

            this.Catch = Catch;
            SelectedFly = FlyList.SingleOrDefault(f => f.FlyID == Catch.Fly?.FlyID);
            BackgroundColor = Color.FromHex("E7A16E");

            OriginalCatch = new Catch();
            OriginalCatch.Assign(Catch);

            BindingContext = this;
        }

        public NewCatchPage()
        {
            isEdit = false;
            BackgroundColor = Color.FromHex("E7A16E");

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
    }
}