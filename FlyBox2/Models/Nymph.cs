using System;
namespace FlyBox2.Models
{
    public class Nymph : Fly
    {

        public int HookSize { get; set; }
        public int Weight { get; set; }

        public virtual void Assign(Nymph OtherFly)
        {
            this.HookSize = OtherFly.HookSize;
            this.Weight = OtherFly.Weight;
            base.Assign(OtherFly);
        }

        public Nymph()
        {
        }
    }
}
