using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Bus : BaseEntity
    {
        public string CompanyName { get; private set; }
        public string BusName { get; private set; }
        public int TotalSeats { get; private set; }
        private Bus() { }
        public Bus(string companyName, string busName, int totalSeats)
        {
            CompanyName = companyName;
            BusName = busName;
            TotalSeats = totalSeats;
        }
    }
}
