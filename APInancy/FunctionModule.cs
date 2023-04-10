using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APInancy
{
    public class FunctionsModule : NancyModule
    {
        public FunctionsModule()
        {
            Get("/func1", _ => {
                var response = "Hello" + "World";
                return response;
            });

            Get("/func2", _ => {
                var response = "";
                for (int count = 0; count < 10; count++)
                {
                    response = response + count.ToString() + ",";
                }
                response = response.Trim(',');
                return response;
            });

        }
    }
}
