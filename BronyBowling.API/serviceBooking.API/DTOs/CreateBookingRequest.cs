namespace serviceBooking.API.DTOs;
public class CreateBookingRequest
{
    public int LaneId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? GuestFullName { get; set; }
    public string? GuestPhone { get; set; }
}