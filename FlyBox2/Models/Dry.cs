using System;
namespace FlyBox2.Models
{
    public class Dry : Fly
    {

        public int HookSize { get; set; }


        public override void Assign(Fly OtherFly)
        {
            if ( OtherFly is Dry )
            {
                var dry = OtherFly as Dry;
                this.HookSize = dry.HookSize;

            }

            base.Assign(OtherFly);
        }

        public Dry()
        {
        }
    }
}
