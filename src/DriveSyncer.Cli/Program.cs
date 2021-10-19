using DriveSyncer.Cli.Commands;
using Spectre.Console.Cli;

namespace DriveSyncer.Cli
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<ShowCommand>("show");
                config.AddCommand<ConfigCommand>("config");
                config.AddCommand<RunCommand>("run");
                config.AddCommand<SleepCommand>("sleep");
            }); 
            return app.Run(args);
        }
    }
}
