using DirectLinkCore;

namespace RoutingExample
{
    public class CompanyHomeViewModel : ViewModel
    {
        public string Path { get; }

        public CompanyHomeViewModel(DirectLinkContext context)
        {
            this.Path = context.Path;
        }
    }
}