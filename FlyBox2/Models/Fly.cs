using System;
using System.Collections.Generic;

namespace FlyBox2.Models
{
    public class Fly
    {
        public string FlyID { get; set; }
        public string FlyName { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }

        public ICollection<Catch> Catches { get; set; }

        public virtual void Assign(Fly OtherFly)
        {
            this.FlyID = OtherFly.FlyID;
            this.FlyName = OtherFly.FlyName;
            this.Description = OtherFly.Description;
            this.Color = OtherFly.Color;
        }
    }

    
}
