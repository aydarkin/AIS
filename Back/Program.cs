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
    // api/Model/ :
    // (��������) - POST, ���� json � ������, ���������� json �������
    // (����������) - PATCH, ���� json � ������, ���������� json �������

    // api/Model/:id
    // (��������) - DELETE, ���� id � ������, ���������� true
    // (������) - GET, ���� id � ������, ���������� true

    // api/Model?���������_���������� (���� http://localhost:57086/Home/Area?altitude=20&height=4)
    // (���������) GET, ���� � ������, ���������� json ������ ��������

    // ��� Friendships ��������� ������
    // api/friendship/link - POST, �� ����� json {From: ��_���������, To: ��_����}, ���������� ��� ������ (0, 1, 2)
    //       ����������: FirstID < SecondID
    // api/friendship/unlink - POST, �� ����� json {From: ��_���������, To: ��_����}, ���������� ��� ������ (0, 1, 2)


    // ��� Interests ��������� �� ����������
    // api/interests?IncludePersons=true � ��������� �������� ������ �������� �������������

    // ��� Persons ��������� �� ����������
    // api/persons?IncludeInterests=true � ��������� �������� ������ �������� ���������

    // ��� Persons ��������� �� ����������
    // ���������� �� ������� �� ��������

    // ��� Messages ��������� �� ����������
    // api/messages?Person=123 ��� �������� � ��������� ��������� ��������������� �� ���� (������� �����)
    // ����� � ���������� �� ������ id �������������, � �� �������
    // ������ => [{Id: 1, Text: "������", From: { Id: 123, Name: ... }, To: { Id: 124, Name: ... }}, ...]

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

    var demo1 = new User() { Login = "demo1", Password = "demo1" };
    db.Users.Add(demo1);
    db.Persons.Add(new Person() { Name = "���������", User = demo1 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo1.Person, Direction = FriendDirection.SecondToFirst});


    var demo2 = new User() { Login = "demo2", Password = "demo2" };
    db.Users.Add(demo2);
    db.Persons.Add(new Person() { Name = "����", User = demo2 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo2.Person, Direction = FriendDirection.Both });

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
