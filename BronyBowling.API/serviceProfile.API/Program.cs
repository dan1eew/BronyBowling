using BronyBowling.Shared.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using serviceProfile.API.Data;
using serviceProfile.API.DTOs;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddDbContext<ProfileDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,

            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthOptions.SecurityKey
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var app = builder.Build();

// -------------------- MIDDLEWARE --------------------

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

// -------------------- ENDPOINTS --------------------

app.MapGet("/profile", async (
    ClaimsPrincipal user,
    ProfileDbContext db) =>
{
    var userId = Guid.Parse(
        user.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    var entity = await db.Users.FindAsync(userId);

    if (entity is null)
        return Results.NotFound();

    return Results.Ok(new ProfileResponse
    {
        PhoneNumber = entity.PhoneNumber,
        FullName = entity.FullName,
        BirthDate = entity.BirthDate,
        City = entity.City
    });
}) // profile
.RequireAuthorization();

app.MapPut("/profile", async (
    UpdateProfileRequest request,
    ClaimsPrincipal user,
    ProfileDbContext db) =>
{
    var userId = Guid.Parse(
        user.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    var entity = await db.Users.FindAsync(userId);

    if (entity is null)
        return Results.NotFound();

    entity.FullName = request.FullName;
    entity.BirthDate = request.BirthDate;
    entity.City = request.City;

    await db.SaveChangesAsync();
    return Results.Ok();
}) // PUT profile
.RequireAuthorization();

app.MapDelete("/profile", async (
    ClaimsPrincipal user,
    ProfileDbContext db) =>
{
    var userId = Guid.Parse(
        user.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    var entity = await db.Users.FindAsync(userId);

    if (entity is null)
        return Results.NotFound();

    db.Users.Remove(entity);
    await db.SaveChangesAsync();

    return Results.Ok("Профиль удалён");
}) // DELETE profile
.RequireAuthorization();

app.Run();
