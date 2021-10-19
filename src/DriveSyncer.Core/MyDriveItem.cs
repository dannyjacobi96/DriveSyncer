using Microsoft.Graph;

namespace DriveSyncer.Core
{
    public class MyDriveItem
    {
        public MyDriveItem(DriveItem item, string path)
        {
            Item = item;
            Path = path;
        }
        public DriveItem Item { get; set; }
        public string Path { get; set; }
    }
}