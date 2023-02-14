using System.Text;
using CommunityToolkit.WinUI.Helpers;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using Windows.Storage;

namespace PocketClient.Desktop.Services;

public class LocalFileService : ILocalFileService
{
    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

    public LocalFileService()
    {
    }

    public async Task<bool> Exists(string filename)
    {
        if (RuntimeHelper.IsMSIX)
        {
            return await ApplicationData.Current.LocalCacheFolder.FileExistsAsync(filename);
        }
        else
        {
            return File.Exists(GetFullPath(filename));
        }

    }

    public string GetFullPath(string filename)
    {
        if (RuntimeHelper.IsMSIX)
        {
            return Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, filename);
        }

        return Path.Combine(_localApplicationData, filename);
    }

    public async Task SaveFileContent(string filename, string fileContent)
    {
        if (RuntimeHelper.IsMSIX)
        {
            await ApplicationData.Current.LocalCacheFolder.WriteTextToFileAsync(fileContent, filename, CreationCollisionOption.ReplaceExisting);
        }
        else
        {
            File.WriteAllText(GetFullPath(filename), fileContent, Encoding.UTF8);
        }
    }
}
