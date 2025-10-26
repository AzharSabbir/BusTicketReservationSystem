using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public class RouteLocationsDto
    {
        public List<string> FromLocations { get; set; } = new List<string>();
        public List<string> ToLocations { get; set; } = new List<string>();
    }
}
