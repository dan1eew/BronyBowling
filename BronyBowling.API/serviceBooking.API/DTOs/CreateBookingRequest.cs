public class CreateBookingRequest
{
    public int CenterId { get; set; }
    public int LaneNumber { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
}