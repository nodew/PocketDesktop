using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketClient.Desktop.Contracts.Services;

public interface IAuthService
{
    public Task InitializeAsync();

    public bool IsAuthorized();

    public Task LaunchAuthorizationAsync();

    public Task AuthorizeAsync();
}
