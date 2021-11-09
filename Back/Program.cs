using Back.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// цепляем БД
builder.Services.AddDbContext<Back.AppDBContext>();

var app = builder.Build();

// TODO пока для первичного пересоздания бд
using (var db = new Back.AppDBContext()) 
{
    db.Genders.Add(new Gender() { Title = "Мужской" });
    db.Genders.Add(new Gender() { Title = "Женский" });
    db.Genders.Add(new Gender() { Title = "Другой" });

    var rf = new Country() { Title = "Россия" };
    db.Countries.Add(rf);
    db.Countries.Add(new Country() { Title = "Украина" });
    db.Countries.Add(new Country() { Title = "Казахстан" });

    db.Cities.Add(new City() { Title = "Уфа", Country = rf });
    db.Cities.Add(new City() { Title = "Москва", Country = rf });
    db.Cities.Add(new City() { Title = "Чишмы", Country = rf });
    db.Cities.Add(new City() { Title = "Туймазы", Country = rf });

    var demo = new User() { Login = "demo", Password = "demo" };
    db.Users.Add(demo);
    db.Persons.Add(new Person() { Name = "Демо пользователь", User = demo });

    db.SaveChanges();
}

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
