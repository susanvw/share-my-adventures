using ShareMyAdventures.Domain.SeedWork;
using ShareMyAdventures.Domain.SeedWork.Interfaces;

namespace ShareMyAdventures.Domain.Entities.LocationAggregate;

public sealed class Location: BaseAuditableEntity, IAggregateRoot
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string? Suburb {  get; set; } 
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostalCode { get; set; }
    public string Country { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
