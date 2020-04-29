using System;
namespace FlyBox2.Models
{
    public class Dry : Fly
    {

        public int HookSize { get; set; }


        public virtual void Assign(Dry OtherFly)
        {
            this.HookSize = OtherFly.HookSize;
            base.Assign(OtherFly);
        }

        public Dry()
        {
        }
    }
}
