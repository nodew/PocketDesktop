namespace Pocket.Client.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
