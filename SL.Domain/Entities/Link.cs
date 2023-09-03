namespace SL.Domain.Entities;

public class Link
{
    [Obsolete("EF Core only")]
    protected Link() { }
    
    public Link(string originalUrl, string shortUrl)
    {
        Id = Guid.NewGuid();
        OriginalUrl = originalUrl;
        ShortUrl = shortUrl;
        CreatedDate = DateTimeOffset.UtcNow;
    }
    
    public Guid Id { get; set; }
    
    public string OriginalUrl { get; set; }
    
    public string ShortUrl { get; set; }

    public DateTimeOffset CreatedDate { get; set; } 
}