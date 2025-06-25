using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TE.Core.Services;
using TE.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "SGPH",
            ValidAudience = "SGPH",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("MyUltraS3cur3AndRand0mKey!@#2025"))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<FicheMedicaleService>();
builder.Services.AddScoped<MedecinService>();
builder.Services.AddScoped<AgendaService>();
builder.Services.AddScoped<RendezVousService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<MedicamentService>();
builder.Services.AddScoped<OrdenanceService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddHttpContextAccessor();





builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Permet toutes les origines (pas recommandé en production)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<TEContext>();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Ajoute ce mapping pour activer les API Controllers
app.MapControllers(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
