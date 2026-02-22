public record UserBookings(
    string CentreName,
    int LaneNumber,
    DateTime StartTime,
    DateTime EndTime,
    string Status
);