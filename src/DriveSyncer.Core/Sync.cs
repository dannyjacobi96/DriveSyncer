using System;
using System.Linq;
using System.Threading.Tasks;

namespace DriveSyncer.Core
{
    public class Sync
    {
        private BaseDrive _sourceDrive;
        private BaseDrive _destDrive;
        private Options _options;
        private int _current;
        private ISyncLog _log;

        public Task SyncDrive(string tokenSource, string tokenDest, string folderSource, string folderDest, Options options, ISyncLog log = null)
        {
            _sourceDrive = new BaseDrive();
            _sourceDrive.Init(tokenSource);
            _sourceDrive.BaseFolder = folderSource;
            _destDrive = new BaseDrive();
            _destDrive.Init(tokenDest);
            _destDrive.BaseFolder = folderDest;
            _destDrive.SyncLog = log;
            _options = options;
            _log = log;
            return Copy();
        }

        private async Task Copy()
        {
            var srcitem = await _sourceDrive.LoadFolderFromPath(_sourceDrive.BaseFolder);
            var destItem = await _destDrive.LoadFolderFromPath(_destDrive.BaseFolder);
            await Copy(new MyDriveItem(srcitem, $"/{srcitem.Name}"), new MyDriveItem(destItem, $"/{destItem.Name}"));
        }

        private async Task Copy(MyDriveItem srcItem, MyDriveItem destItem)
        {
            if (_current > _options.Count)
            {
                return;
            }
            if (srcItem.Item.Folder != null && srcItem.Item.Children != null && srcItem.Item.Children.CurrentPage != null)
            {
                foreach (var sourceChild in srcItem.Item.Children.CurrentPage)
                {
                    if (_current > _options.Count)
                    {
                        break;
                    }

                    string childName = sourceChild.Name;
                    var sourceChildPath = $"{srcItem.Path}/{childName}";
                    var destChildPath = $"{destItem.Path}/{childName}";
                    _log?.Log($"Processing {sourceChildPath}");
                    if (sourceChild.Folder != null)
                    {
                        var destChild = destItem.Item.Children.CurrentPage.FirstOrDefault(x => x.Name == childName) ??
                                        await _destDrive.CreateFolder(destItem.Item.Id, childName);
                        sourceChild.Children = await _sourceDrive.GetChildren(sourceChild.Id);
                        destChild.Children = await _destDrive.GetChildren(destChild.Id);
                        await Copy(new MyDriveItem(sourceChild, sourceChildPath),new MyDriveItem(destChild, destChildPath));
                    }
                    else
                    {
                        var destChild = destItem.Item.Children.CurrentPage.FirstOrDefault(x => x.Name == childName);
                        if (_options.Override || destChild == null)
                        {
                               var content = await _sourceDrive.Download(sourceChild.Id);
                            _log?.Downloaded(sourceChildPath);
                            try
                            {
                                destChild = await _destDrive.Upload(destItem.Item.Id, childName, content);
                            }
                            catch (Exception ex)
                            {
                                _log?.Exception(ex);
                            }

                            _log?.Uploaded(destChildPath);
                            _current++;
                        }
                    }
                }
            }
        }
    }
    
}
