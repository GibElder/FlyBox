using System;
namespace FlyBox2.Models
{
    public class Catch
    {
        public string CatchID { get; set; }
        public string FishType { get; set; }
        public string Location { get; set; }
        public string FlyID { get; set; }
        public Fly Fly { get; set; }
        public string Description { get; set; }
        public double Size { get; set; }
        public string ImagePath { get; set; }


        public void Assign(Catch OtherCatch)
        {
            this.CatchID = OtherCatch.CatchID;
            this.FishType = OtherCatch.FishType;
            this.Description = OtherCatch.Description;
            this.Location = OtherCatch.Location;
            this.ImagePath = OtherCatch.ImagePath;
            if (OtherCatch.Fly == null)
                this.Fly = null;
            else
            {
                this.Fly = new Fly();
                this.Fly.Assign(OtherCatch.Fly);
            }
        }

    }
}
