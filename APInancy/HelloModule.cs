using Nancy;

namespace APInancy
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get("/", _ => {
                return "Hello World";
            });
        }
    }
}
