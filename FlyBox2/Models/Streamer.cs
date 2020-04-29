using System;
namespace FlyBox2.Models
{
    public class Streamer : Fly
    {
        public int Length { get; set; }
        public string WaterColumn { get; set; }

        public virtual void Assign(Streamer OtherFly)
        {
            this.Length = OtherFly.Length;
            this.WaterColumn = OtherFly.WaterColumn;
            base.Assign(OtherFly);
        }


        public Streamer()
        {
        }
    }
}
