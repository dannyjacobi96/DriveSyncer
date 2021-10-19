using System;

namespace DriveSyncer.Core
{
    public class Options
    {
        public bool Override { get; set; }
        public int Count { get; set; }
    }

    public interface ISyncLog
    {
        void Downloaded(string fileName);
        void Uploaded(string fileName);
        void UploadPercent(string fileName, long percent);
        void Log(string message);
        void Exception(Exception ex);
    }
    public class SyncCallBack
    {
        public Action<string> Downloaded { get; set; }
        public Action<string> Uploaded { get; set; }
        public Action<string, int> UploadPercent { get; set; }
    }
}