using System.Text;
using CommunityToolkit.WinUI.Helpers;
using PocketClient.Desktop.Contracts.Services;
using PocketClient.Desktop.Helpers;
using Windows.Storage;

namespace PocketClient.Desktop.Services;

public class LocalCacheFileService : ILocalCacheFileService
{
    private readonly string systemCacheFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private const string _defaultApplicationDataFolder = "PocketClient.Desktop";

    public LocalCacheFileService()
    {
    }

    public async Task<bool> Exists(string filename)
    {
        if (RuntimeHelper.IsMSIX)
        {
            return await ApplicationData.Current.TemporaryFolder.FileExistsAsync(filename);
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
            return Path.Combine(ApplicationData.Current.TemporaryFolder.Path, filename);
        }

        return Path.Combine(systemCacheFolder, _defaultApplicationDataFolder, filename);
    }

    public async Task SaveFileContent(string filename, string fileContent)
    {
        if (RuntimeHelper.IsMSIX)
        {
            await ApplicationData.Current.TemporaryFolder.WriteTextToFileAsync(fileContent, filename, CreationCollisionOption.ReplaceExisting);
        }
        else
        {
            File.WriteAllText(GetFullPath(filename), fileContent, Encoding.UTF8);
        }
    }
}
