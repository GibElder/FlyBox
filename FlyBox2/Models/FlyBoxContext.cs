using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace FlyBox2.Models
{
    public class FlyBoxcontext : DbContext
    {

        private const string databaseName = "FlyBox2.db3";

        public FlyBoxcontext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = "";

            switch (Device.RuntimePlatform)
            {
                case (Device.Android):
                    dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName);
                    break;
                case (Device.iOS):
                    {
                        SQLitePCL.Batteries_V2.Init();
                        dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName);
                        break;
                    }
            }

            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        public DbSet<Fly> Fly { get; set; }
        public DbSet<Catch> Catch { get; set; }



    }
}
