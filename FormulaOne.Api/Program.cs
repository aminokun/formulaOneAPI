using Microsoft.EntityFrameworkCore;
using FormulaOne.DataService.Data;
using FormulaOne.DataService.Repositories.Interfaces;
using FormulaOne.DataService.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Get connectionstring
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

//initializing my dbcontext inside the Dependency Injection container the options that are beibg injected here will be used inside dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//injecting the MediatR to our DI

//what this does is tell the MediatR to scan all the assemblies of the program.cs
//because its the main entry point in order for it to utilize it
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
