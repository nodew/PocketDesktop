namespace Pocket.Sdk;

public class PocketItem
{
    public long Id 
    { 
        get; set; 
    }

    public string Title 
    { 
        get; set; 
    }

    public string Excerpt 
    { 
        get; set; 
    }

    public PocketItemStatus Status 
    { 
        get; set; 
    }

    public bool IsFavorited
    {
        get; set; 
    }

    public bool IsArticle 
    {
        get; set; 
    }

    public bool HasVideo
    {
        get; set;
    }

    public int HasImage
    {
        get; set;
    }

    public string Lang
    {
        get; set;
    }

    public Uri? TopImageUrl
    {
        get; set;
    }

    public int? TimeToRead
    {
        get; set;
    }

    public int? ListenDurationEstimate
    {
        get; set;
    }

    public long WordCount 
    { 
        get; set; 
    }

    public string DomainOrHost
    {
        get; set; 
    }

    public DateTime TimeAdded
    {
        get; set;
    }

    public DateTime TimeUpdated
    {
        get; set;
    }

    public DateTime TimeRead
    {
        get; set;
    }

    public DateTime TimeFavorited
    {
        get; set;
    }

    public Author Author
    {
        get; set; 
    }

    public List<PocketItemTag> Tags
    {
        get; set;
    }
}
