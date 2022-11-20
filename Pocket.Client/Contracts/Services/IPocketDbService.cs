using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket.Client.Contracts.Services;

public interface IPocketDbService
{
    public Task InitializeAsync();

    public string GetPocketDbPath();

    public Task SyncItemsAsync(bool fullSync = false);
}
