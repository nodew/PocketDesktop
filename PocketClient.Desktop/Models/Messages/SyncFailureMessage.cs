namespace PocketClient.Desktop.Models;

public class SyncFailureMessage(string reason, Exception ex)
{
    public string Reason { get; set; } = reason;

    public Exception Exception { get; set; } = ex;
}
