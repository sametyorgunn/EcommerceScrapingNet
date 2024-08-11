using BusinessLayer.ServiceExtension;
using EntityLayer.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Servisleri container'a ekleyin.
builder.Services.AddControllers();
builder.Services.AddServiceRouting();
builder.Services.AddAutoMapper(typeof(ProductProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP istek hattını yapılandırın.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
