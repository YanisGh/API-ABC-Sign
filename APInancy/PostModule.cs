using Nancy;
using Nancy.ModelBinding;

namespace APInancy
{
    public class PostModule : NancyModule
    {
        public PostModule()
        {

            Post("/sendname", _  =>
            {
                PostData recievedData = this.Bind<PostData>();

                return new { success = true, message = $"Record recieved name = {recievedData.Name}" };

            });
        }
    }
}
