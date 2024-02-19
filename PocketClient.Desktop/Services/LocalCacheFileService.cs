using System.Text;
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
            return await FileExistsInFolderAsync(ApplicationData.Current.TemporaryFolder, filename);
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
            await WriteTextToFileAsync(ApplicationData.Current.TemporaryFolder, fileContent, filename, CreationCollisionOption.ReplaceExisting);
        }
        else
        {
            File.WriteAllText(GetFullPath(filename), fileContent, Encoding.UTF8);
        }
    }

    private static async Task<bool> FileExistsInFolderAsync(StorageFolder folder, string fileName)
    {
        var item = await folder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false);
        return (item != null) && item.IsOfType(StorageItemTypes.File);
    }

    private static async Task<StorageFile> WriteTextToFileAsync(
        StorageFolder fileLocation,
        string text,
        string fileName,
        CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
    {
        if (fileLocation == null)
        {
            throw new ArgumentNullException(nameof(fileLocation));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var storageFile = await fileLocation.CreateFileAsync(fileName, options);
        await FileIO.WriteTextAsync(storageFile, text);

        return storageFile;
    }
}
