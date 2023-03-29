using Nancy;

namespace APInancy
{
    public class HelloModule : NancyModule
    {
        //test
        public HelloModule()
        {
            
            Get("/Hello", _ => {
                return "Hello World";
            });
            Get("/", _ => {
                return "root";
            });
        }
    }
}
