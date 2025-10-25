using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public class AvailableBusDto
    {
        public Guid BusScheduleId { get; set; }
        public string CompanyName { get; set; } 
        public string BusName { get; set; }     
        public DateTime StartTime { get; set; } 
        public DateTime ArrivalTime { get; set; } 
        public int SeatsLeft { get; set; }      
        public decimal Price { get; set; }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }
        public string CancellationPolicy { get; set; }
    }
}
