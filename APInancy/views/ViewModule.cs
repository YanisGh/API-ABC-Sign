using APInancy;
using Nancy;

namespace NancyStandalone
{
    public class ViewModule : NancyModule
    {
        public ViewModule()
        {
            Get("/viewtest", parameters => {
                ClientPostData data = new ClientPostData()
                {
                    Name = "Peter Shaw",
                    Email = "top@secret.com"
                };

                return View["ViewTest.html", data];
            });
        }
    }
}
