using DirectLinkCore;
using System.Collections.Generic;

namespace RoutingExample
{
    public class CompanyViewModel : RouterViewModel
    {
        public int? Id { get; }
        public string Name { get; }
        public string View { get; }
        public List<Link> Companies { get; }
        public List<Link> Products { get; }
        public Link BackLink { get; }

        public CompanyViewModel()
        {
            this.Companies = new List<Link> {
                new Link("Microsoft", Routes.GetLink<Company>(new { name = "Microsoft" })),
                new Link("Facebook", Routes.GetLink<Company>(new { name = "Facebook", view = "info" })),
                new Link("42 View", Routes.GetLink<Company>(new { id = 42 })),
                new Link("42 About", Routes.GetLink<Company>(new { id = 42, view = "about" }))
            };
            this.Products = new List<Link> {
                new Link("[React]", Routes.GetLink("Product", new { name = "Facebook" }, new { id = 52 })),
                new Link("[SignalR]", Routes.GetLink("Product", new { id = 42, view = "group" }, new { id = 37 })),
                new Link("[ASP.NET Core]", Routes.GetLink("Product", new { name = "Microsoft" }, new { id = 25 }))
            };
        }

        public CompanyViewModel(string name)
        {
            this.Name = name;
            this.BackLink = new Link("back", Routes.GetLink<Company>());
        }

        public CompanyViewModel(string name, string view)
        {
            this.Name = name;
            this.View = view;
            this.BackLink = new Link("back", Routes.GetLink<Company>());
        }

        public CompanyViewModel(int id, string view)
        {
            this.Id = id;
            this.View = view;
            this.BackLink = new Link("back", Routes.GetLink<Company>());
        }
    }
}