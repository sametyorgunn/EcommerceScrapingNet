using BusinessLayer.ServiceExtension;
using DataAccessLayer.Contexts;
using EntityLayer.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Servisleri container'a ekleyin.
builder.Services.AddControllers();
builder.Services.AddServiceRouting();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP istek hattýný yapýlandýrýn.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
