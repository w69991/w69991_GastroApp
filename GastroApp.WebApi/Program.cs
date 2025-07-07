using GastroApp.Application.Customers.Commands;
using GastroApp.Domain.Entities;
using GastroApp.Infrastructure;
using GastroApp.Domain.Interfaces;
using GastroApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GastroAppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<RegisterCustomerCommand>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GastroAppDbContext>();
    db.Database.Migrate();
    
    //Wczytanie przykladowego Menu
    if (!db.MenuItems.Any())
    {
        db.MenuItems.AddRange(
            new MenuItem { Name = "Margherita",  Price = 24, Category = "Pizza",  Description = "Classic cheese & tomato" },
            new MenuItem { Name = "Carbonara",   Price = 28, Category = "Pasta",  Description = "Creamy bacon pasta" },
            new MenuItem { Name = "Caesar",      Price = 19, Category = "Salad",  Description = "Chicken & romaine" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDefaultFiles();
    app.UseStaticFiles();              
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.MapPost("/api/customers/register",
    async ([FromServices] IMediator mediator, RegisterCustomerCommand cmd) =>
    {
        var id = await mediator.Send(cmd);
        return Results.Created($"/api/customers/{id}", null);
    });
app.Run();