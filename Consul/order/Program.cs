using Consul;
using OrderApi;
using OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHostedService, ConsulRegisterServices>();
builder.Services.Configure<OrderConfigration>(builder.Configuration.GetSection("Order"));
builder.Services.Configure<ConsulConfigration>(builder.Configuration.GetSection("Consul"));
var consulUrl = builder.Configuration.GetSection("Consul")["Url"];
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(
    config => config.Address = new Uri(consulUrl)));

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
