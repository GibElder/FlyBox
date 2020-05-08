using System;
namespace FlyBox2.Models
{
    public class Nymph : Fly
    {

        public int HookSize { get; set; }
        public int Weight { get; set; }

        public override void Assign(Fly OtherFly)
        {

            if (OtherFly is Nymph)
            {
                var nymph = OtherFly as Nymph;
                this.HookSize = nymph.HookSize;
                this.Weight = nymph.Weight;

            }
            base.Assign(OtherFly);
        }

        public Nymph()
        {
        }
    }
}
