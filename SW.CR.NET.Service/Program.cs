using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.CR.NET.Service
{
    class Program
    {
        static void Main(string[] args)
        {

            string baseAddress = "http://localhost:9000";

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Service on {baseAddress} started!");

                Console.WriteLine("Press any key to exit.");

                Console.ReadKey();
            }
        }
    }
}
