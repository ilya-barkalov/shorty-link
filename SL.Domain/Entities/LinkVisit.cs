namespace SL.Domain.Entities;

public class LinkVisit
{
    [Obsolete("EF Core only")]
    protected LinkVisit() { }

    public LinkVisit(Guid linkId)
    {
        Id = Guid.NewGuid();
        LinkId = linkId;
        CreatedDate = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; set; }
    public Guid LinkId { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public Link Link { get; set; }
}