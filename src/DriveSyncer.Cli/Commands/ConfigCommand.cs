using System.ComponentModel;
using Spectre.Console.Cli;

namespace DriveSyncer.Cli.Commands
{
    [Description("Config variable.")]
    public class ConfigCommand : Command<ConfigCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-s|--source-token")]
            [Description("Source token")]
            public string SourceToken { get; set; }

            [CommandOption("-d|--dest-token")]
            [Description("Dest token")]
            public string DestToken { get; set; }

            [CommandOption("-f|--folder-source")]
            [Description("Folder source")]
            public string FolderSource { get; set; }

            [CommandOption("-o|--folder-dest")]
            [Description("Folder dest")]
            public string FolderDest { get; set; }

            [CommandOption("--override")]
            [Description("Override")]
            public bool Override { get; set; }

            [CommandOption("-c|--count")]
            [Description("Count")]
            [DefaultValue(5)]
            public int Count { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var options = Utils.ReadOptions();
            if (!string.IsNullOrEmpty( settings.SourceToken))
            {
                options.SourceToken = settings.SourceToken;
            }

            if (!string.IsNullOrEmpty(settings.DestToken))
            {
                options.DestToken = settings.DestToken;
            }
            if (!string.IsNullOrEmpty(settings.FolderSource))
            {
                options.FolderSource = settings.FolderSource;
            }
            if (!string.IsNullOrEmpty(settings.FolderDest))
            {
                options.FolderDest = settings.FolderDest;
            }

            options.Override = settings.Override;
            options.Count = settings.Count;
            Utils.WriteOptions(options);
            Utils.ShowOptions(options);
            return 0;
        }
    }
}
