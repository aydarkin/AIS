using Back.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ������� ��
builder.Services.AddDbContext<Back.AppDBContext>();

var app = builder.Build();

// TODO ���� ��� ���������� ������������ ��
using (var db = new Back.AppDBContext()) 
{
    db.Genders.Add(new Gender() { Title = "�������" });
    db.Genders.Add(new Gender() { Title = "�������" });
    db.Genders.Add(new Gender() { Title = "������" });

    var rf = new Country() { Title = "������" };
    db.Countries.Add(rf);
    db.Countries.Add(new Country() { Title = "�������" });
    db.Countries.Add(new Country() { Title = "���������" });

    db.Cities.Add(new City() { Title = "���", Country = rf });
    db.Cities.Add(new City() { Title = "������", Country = rf });
    db.Cities.Add(new City() { Title = "�����", Country = rf });
    db.Cities.Add(new City() { Title = "�������", Country = rf });

    var demo = new User() { Login = "demo", Password = "demo" };
    db.Users.Add(demo);
    db.Persons.Add(new Person() { Name = "���� ������������", User = demo });

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
