using System;
using System.IO;
using System.Text.Json;
using Spectre.Console;

namespace DriveSyncer.Cli
{
    public class Options
    {
        public Options()
        {
            SourceToken = string.Empty;
            DestToken = string.Empty;
            FolderSource = string.Empty;
            FolderDest = string.Empty;
        }
        public string SourceToken { get; set; }


        public string DestToken { get; set; }


        public string FolderSource { get; set; }

        public string FolderDest { get; set; }


        public bool Override { get; set; }


        public int Count { get; set; }
    }

    public static class Utils
    {
        public static Options ReadOptions()
        {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "drive-syncer.settings");
            if (File.Exists(file))
            {
                var str = File.ReadAllText(file);
                return JsonSerializer.Deserialize<Options>(str);
            }

            return new Options();
        }

        public static void WriteOptions(Options options)
        {
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "drive-syncer.settings");
            File.WriteAllText(file, JsonSerializer.Serialize(options, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
        }

        public static void ShowOptions(Options options)
        {
            var grid = new Grid()
                .AddColumn(new GridColumn().NoWrap().PadRight(4))
                .AddColumn()
                .AddRow("[b]SourceToken[/]", options.SourceToken)
                .AddRow("[b]DestToken[/]", $"{options.DestToken}")
                .AddRow("[b]FolderSource[/]", $"{options.FolderSource}")
                .AddRow("[b]FolderDest[/]", $"{options.FolderDest}")
                .AddRow("[b]Override[/]", $"{options.Override}")
                .AddRow("[b]Count[/]", $"{options.Count}");
            AnsiConsole.Render(
                new Panel(grid)
                    .Header("Options"));
        }
    }
}
