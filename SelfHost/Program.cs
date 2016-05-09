using Microsoft.Owin.Hosting;
using StreamingDemo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            var dummy = typeof(MediaController);

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("Started at " + baseAddress);
                Console.ReadLine();
            }
        }
    }
}
