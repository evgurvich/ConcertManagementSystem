public class EventTicketAvailabilityResponse
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int TotalCapacity { get; set; }

    public int TotalReservedCount => TicketTypes.Sum(tt => tt.ReservedCount);
    public int TotalPurchasedCount => TicketTypes.Sum(tt => tt.PurchasedCount);
    public int TotalAvailableCount => TicketTypes.Sum(tt => tt.AvailableCount);

    public List<TicketTypeAvailability> TicketTypes { get; set; } = new();
}

public class TicketTypeAvailability
{
    public Guid TicketTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int TotalQuantity { get; set; }
    public int ReservedCount { get; set; }
    public int PurchasedCount { get; set; }
    public int AvailableCount => TotalQuantity - ReservedCount - PurchasedCount;
}
