using Microsoft.Graph;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DriveSyncer.Core
{
    public class BaseDrive
    {
        protected GraphServiceClient _graphClient;
        public string BaseFolder { get; set; }

        public string Token { get; set; }
        public ISyncLog SyncLog { get; set; }

        public BaseDrive()
        {
        }

        public void Init(string token)
        {
            try
            {
                _graphClient = new GraphServiceClient(
                    "https://graph.microsoft.com/v1.0",
                    new DelegateAuthenticationProvider(
                        async (requestMessage) =>
                        {
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                                // This header has been added to identify our sample in the Microsoft Graph service.  If extracting this code for your project please remove.
                                requestMessage.Headers.Add("SampleID", "uwp-csharp-apibrowser-sample");

                        }));
            }

            catch (Exception ex)
            {
                //return null;
                //Debug.WriteLine("Could not create a graph client: " + ex.Message);
            }
        }

        public async Task<DriveItem> LoadFolderFromPath(string path = null)
        {
            DriveItem folder;

            try
            {
                var expandValue = "thumbnails,children";
                //? "thumbnails,children($expand=thumbnails)"
                //: "thumbnails,children";

                if (path == null)
                {
                    folder = await _graphClient.Me.Drive.Root.Request()
                        .Expand(expandValue)
                        .GetAsync();
                }
                else
                {
                    folder =
                        await
                            _graphClient.Me.Drive.Root.ItemWithPath("/" + path)
                                .Request()
                                .Expand(expandValue)
                                .GetAsync();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return folder;
        }

        public Task<IDriveItemChildrenCollectionPage> GetChildren(string id)
        {
            return _graphClient.Me.Drive.Items[id].Children.Request().GetAsync();
        }

        public Task<DriveItem> CreateFolder(string id, string name)
        {
            var folderToCreate = new DriveItem { Name = name, Folder = new Folder() };
            return _graphClient.Me.Drive.Items[id].Children.Request()
                    .AddAsync(folderToCreate);
        }

        public Task<System.IO.Stream> Download(string id)
        {
            return _graphClient.Me.Drive.Items[id].Content.Request().GetAsync();
        }

        public async Task<DriveItem> Upload(string folderId, string fileName, System.IO.Stream content, Action<string> callback=null)
        {
            var uploadSession = await _graphClient.Me.Drive.Items[folderId]
                .ItemWithPath(fileName).CreateUploadSession().Request().PostAsync();
            int maxSlice = 320 * 1024;
            var largeFileUpload = new LargeFileUploadTask<DriveItem>(uploadSession, content, maxSlice);
            IProgress<long> progress = new Progress<long>(x =>
            {
                SyncLog?.UploadPercent(fileName, (x * 100 / content.Length));
                callback?.Invoke($"Uploading large file: {x} bytes of {content.Length} bytes already uploaded.");
            });

            UploadResult<DriveItem> uploadResult = await largeFileUpload.UploadAsync(progress);
            return uploadResult.ItemResponse;
        }
    }
}
