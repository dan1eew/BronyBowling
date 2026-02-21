public record CenterResponse(
    int CenterId,
    string? Name,
    string? City,
    string? Street,
    string? House,
    decimal WeekdayPrice,
    decimal WeekendPrice
);