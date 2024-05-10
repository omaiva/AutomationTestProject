using AutomationTestProject.Application.Services;
using AutomationTestProject.Application.Interfaces;
using Microsoft.Extensions.Options;
using AutomationTestProject.Cloudinary;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton<Cloudinary>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(
        settings.CloudName,
        settings.ApiKey,
        settings.ApiSecret
    );
    return new Cloudinary(account);
});

builder.Services.AddTransient<ILinkedInAutomationService, LinkedInAutomationService>();
builder.Services.AddTransient<IImageStorageService, ImageStorageService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenPolicy", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("OpenPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
