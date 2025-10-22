using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Route : BaseEntity
    {
        public string From { get; private set; }
        public string To { get; private set; }
        private Route() { }
        public Route(string from, string to)
        {
            From = from;
            To = to;
        }
    }
}
