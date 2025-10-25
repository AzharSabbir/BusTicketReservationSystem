using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Stop : BaseEntity
    {
        public Guid RouteId { get; private set; }
        public string Name { get; private set; }
        public StopType Type { get; private set; }

        private Stop() { }

        public Stop(Guid routeId, string name, StopType type)
        {
            RouteId = routeId;
            Name = name;
            Type = type;
        }
    }
}
