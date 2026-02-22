public record CreatePaymentRequest(Guid BookingId, decimal Amount);
public record PaymentResponse(
    Guid PaymentId,
    Guid BookingId,
    decimal Amount,
    string Status,
    DateTime CreatedAt,
    DateTime? PaidAt
);

