using DirectLinkCore;

namespace RoutingExample
{
    public class Company : DirectLinkRouter<CompanyViewModel>
    {
        public Company()
        {
            this.MapRoutes(routes => routes
                .MapDefaultRoute<CompanyHome>("/home")
                .MapRoute("Product", "/product/{id:int}")
            );
        }
    }
}