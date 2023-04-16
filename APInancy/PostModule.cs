﻿using Nancy;
using Nancy.ModelBinding;

namespace APInancy
{
    public class PostModule : NancyModule
    {
        public PostModule()
        {
            

            Post("/sendname/{id:int}", parameters =>
            {
                /*if (parameters.id == null)
                {
                    return new { success = false, message = $"No id received" };
                }*/

                PostData recievedData = this.Bind<PostData>();

                return new { success = true, message = $"Record recieved name = {recievedData.Name} and id = {parameters.id} or {recievedData.Id}" };

            });
        }
    }
}
