using BronyBowling.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.MapScalarApiReference();
}

// ---------- CREATE PAYMENT ----------

app.MapPost("/payments", async (
    CreatePaymentRequest request,
    ApplicationDbContext db) =>
{
    var payment = new Payment
    {
        PaymentId = Guid.NewGuid(),
        BookingId = Guid.NewGuid(),
        Amount = request.Amount,
        Status = "Pending",
        CreatedAt = DateTime.UtcNow
    };
    db.Payments.Add(payment);
    await db.SaveChangesAsync();

    return Results.Ok(new PaymentResponse(
        payment.PaymentId,
        payment.BookingId,
        payment.Amount,
        payment.Status,
        payment.CreatedAt,
        payment.PaidAt
    ));
});

// ---------- GET PAYMENT ----------

app.MapGet("/payment/{id:guid}", async (
    Guid id,
    ApplicationDbContext db) =>
{
    var payment = await db.Payments.FindAsync(id);

    if (payment is null)
        return Results.NotFound();

    return Results.Ok(new PaymentResponse(
        payment.PaymentId,
        payment.BookingId,
        payment.Amount,
        payment.Status,
        payment.CreatedAt,
        payment.PaidAt
    ));
});

// ---------- PAY ----------
app.MapPost("/payments/{id:guid}/pay", async (
    Guid id,
    ApplicationDbContext db) =>
{
    var payment = await db.Payments.FindAsync(id);

    if (payment is null)
        return Results.NotFound();

    if (payment.Status == "Paid")
        return Results.BadRequest("Already paid");

    payment.Status = "Paid";
    payment.PaidAt = DateTime.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok();
});

// ---------- FAIL ----------
app.MapPost("/payments/{id:guid}/fail", async (
    Guid id,
    ApplicationDbContext db) =>
{
    var payment = await db.Payments.FindAsync(id);

    if (payment is null)
        return Results.NotFound();

    payment.Status = "Failed";

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run("http://localhost:7195");