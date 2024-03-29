﻿namespace PocketClient.Desktop.Contracts.Services;

public interface ILocalCacheFileService
{
    public Task<bool> Exists(string subPath);

    public string GetFullPath(string subPath);

    public Task SaveFileContent(string subPath, string content);
}
