using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

using GasTrackerDemo.Data;

namespace GasTrackerDemo
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            // Pull root URL and api keys from web.config or app.config
            //
            // Its likely that you will have different keys or urls in development
            // than in production, so that you can have a safe environment to
            // test in.
            var rootUrl = ConfigurationManager.AppSettings["ApiRoot"];
            var apiKey = ConfigurationManager.AppSettings["ApiKey"];

            // Generally want to inject these values into your custom
            // client through its constructor rather than having the client go
            // into Configuration<anager itself. This way you can move
            // the client to a different program that uses some other
            // mechanism for configuration settings
            var client = new GasTrackerClient(rootUrl, apiKey);

            Console.WriteLine("Starting program...");

            var status = client.GetStatus();
            Console.WriteLine($"API Version: {status.Version}-{status.GitCommit}");
            Console.WriteLine($"Server Uptime: {status.Uptime}");

            // Just to keep time to show the version before loading the menu
            Thread.Sleep(3000);

            // GameLoop encapsulates the menu driven behavior for the app
            var loop = new GameLoop(client);
            loop.Start();
        }
    }
}