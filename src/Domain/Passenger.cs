using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Passenger : BaseEntity
    {
        public string Name { get; private set; }
        public string Mobile { get; private set; }

        private Passenger() { }
        public Passenger(string name, string mobile)
        {
            Name = name;
            Mobile = mobile;
        }
    }
}
