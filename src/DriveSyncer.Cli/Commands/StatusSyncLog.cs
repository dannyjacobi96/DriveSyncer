using System;
using DriveSyncer.Core;
using Spectre.Console;

namespace DriveSyncer.Cli.Commands
{
    public class StatusSyncLog : ISyncLog
    {
        private StatusContext _context;

        public StatusSyncLog(StatusContext context)
        {
            _context = context;
        }

        public void Downloaded(string fileName)
        {
            AnsiConsole.MarkupLine($"[grey]DOWNLOADED:[/] {fileName}[grey]...[/]");
            _context.Status($"[bold blue]DOWNLOADED: {fileName}[/]");
        }

        public void Uploaded(string fileName)
        {
            AnsiConsole.MarkupLine($"[grey]UPLOADED:[/] {fileName}[grey]...[/]");
            _context.Status($"[bold blue]UPLOADED: {fileName}[/]");
        }

        public void UploadPercent(string fileName, long percent)
        {
            _context.Status($"[bold blue]{fileName} {percent} %[/]");
        }

        public void Log(string message)
        {
            _context.Status($"[bold blue]{message}[/]");
        }

        public void Exception(Exception ex)
        {
            AnsiConsole.MarkupLine($"[grey]Exception:[/] {ex}[grey]...[/]");
        }
    }
}