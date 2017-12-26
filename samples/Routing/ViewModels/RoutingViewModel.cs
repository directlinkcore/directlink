using DirectLinkCore;
using System.Collections.Generic;

namespace RoutingExample
{
    public class RoutingViewModel : RouterViewModel
    {
        public List<Link> Links { get; }

        public RoutingViewModel()
        {
            this.Links = new List<Link> {
                new Link("Companies", Routes.GetLink<Company>()),
                new Link("Employees", Routes.GetLink("Employee"))
            };
        }
    }
}