using System;
namespace FlyBox2.Models
{
    public class Streamer : Fly
    {
        public int Length { get; set; }
        public string WaterColumn { get; set; }

        public override void Assign(Fly OtherFly)
        {
            if (OtherFly is Streamer)
            {
                var streamer = OtherFly as Streamer;
                this.Length = streamer.Length;
                this.WaterColumn = streamer.WaterColumn;
            }
            base.Assign(OtherFly);
        }


        public Streamer()
        {
        }
    }
}
