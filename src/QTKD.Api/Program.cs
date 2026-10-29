using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Repositories;
using QTKD.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    // Trả Status dạng chữ ("Draft") thay vì số
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string lấy từ configuration (user-secrets / biến môi trường), không commit mật khẩu lên Git
builder.Services.AddDbContext<QtkdDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controller -> Service -> Repository -> DbContext
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IContractService, ContractService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
