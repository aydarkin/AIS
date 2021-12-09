using Back;
using Back.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ������� ��
builder.Services.AddDbContext<AppDBContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // ��������, ����� �� �������������� �������� ��� ��������� ������
            ValidateIssuer = true,
            // ������, �������������� ��������
            ValidIssuer = AuthOptions.ISSUER,

            // ����� �� �������������� ����������� ������
            ValidateAudience = true,
            // ��������� ����������� ������
            ValidAudience = AuthOptions.AUDIENCE,
            // ����� �� �������������� ����� �������������
            ValidateLifetime = true,

            // ��������� ����� ������������
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // ��������� ����� ������������
            ValidateIssuerSigningKey = true,
        };
    })
    .AddCookie(); ;
// builder.Services.AddControllersWithViews();

var app = builder.Build();

// TODO ���� ��� ���������� ������������ ��
using (var db = new AppDBContext())
{
    // api/Model/ :
    // (��������) - POST, ���� json � ������, ���������� json �������
    // (����������) - PUT, ���� json � ������, ���������� json �������

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

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Genders.Add(new Gender() { Title = "�������" });
    db.Genders.Add(new Gender() { Title = "�������" });
    db.Genders.Add(new Gender() { Title = "������" });

    var rf = new Country() { Title = "������" };
    db.Countries.Add(rf);
    var ua = new Country() { Title = "�������" };
    db.Countries.Add(ua);
    db.Countries.Add(new Country() { Title = "���������" });
    var by = new Country() { Title = "��������" };
    db.Countries.Add(by);
    db.Countries.Add(new Country() { Title = "������" });

    var cityUfa = new City() { Title = "���", Country = rf };
    db.Cities.Add(cityUfa);
    db.Cities.Add(new City() { Title = "������", Country = rf });
    db.Cities.Add(new City() { Title = "�����", Country = rf });
    db.Cities.Add(new City() { Title = "�������", Country = rf });
    var cityKiev = new City() { Title = "����", Country = ua };
    db.Cities.Add(cityKiev);
    db.Cities.Add(new City() { Title = "�������", Country = ua });
    db.Cities.Add(new City() { Title = "������", Country = ua });
    var cityMinsk = new City() { Title = "�����", Country = by };
    db.Cities.Add(cityMinsk);
    db.Cities.Add(new City() { Title = "�����", Country = by });
    db.Cities.Add(new City() { Title = "������", Country = by });
    db.Cities.Add(new City() { Title = "�������", Country = by });

    var demo = new User() { Login = "demo", Password = "demo" };
    db.Users.Add(demo);
    db.Persons.Add(new Person() { Name = "����", Surname = "������", User = demo, GenderId = 1, City = cityUfa });

    var demo1 = new User() { Login = "demo1", Password = "demo1" };
    db.Users.Add(demo1);
    db.Persons.Add(new Person() { Name = "���������", User = demo1 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo1.Person, Direction = FriendDirection.SecondToFirst});

    var demo2 = new User() { Login = "demo2", Password = "demo2" };
    db.Users.Add(demo2);
    db.Persons.Add(new Person() { Name = "����", User = demo2 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo2.Person, Direction = FriendDirection.Both });

    var newUser = new User() { Login = "new", Password = "new" };
    db.Persons.Add(new Person() { Name = "����� ������������", User = newUser });

    var banana = new User() { Login = "banana", Password = "banana" };
    db.Users.Add(banana);
    db.Persons.Add(new Person() { Surname = "���������", Name = "�����", Patronymic = "���������", User = banana, GenderId = 2, City =  cityMinsk});
    db.Friendships.Add(new Friendship() { First = demo1.Person, Second = banana.Person, Direction = FriendDirection.Both });

    var cherry = new User() { Login = "cherry", Password = "cherry" };
    db.Users.Add(cherry);
    db.Persons.Add(new Person() { Surname = "������", Name = "��������", Patronymic = "�����������", User = cherry, GenderId = 1, City = cityUfa });
    db.Friendships.Add(new Friendship() { First = banana.Person, Second = cherry.Person, Direction = FriendDirection.SecondToFirst });

    var cooky = new User() { Login = "cooky", Password = "cooky" };
    db.Users.Add(cooky);
    db.Persons.Add(new Person() { Surname = "���������", Name = "��������", Patronymic = "����������", User = cooky, GenderId = 1, City = cityKiev });
    db.Friendships.Add(new Friendship() { First = banana.Person, Second = cooky.Person, Direction = FriendDirection.Both });

    var apple = new User() { Login = "apple", Password = "apple" };
    db.Users.Add(apple);
    db.Persons.Add(new Person() { Surname = "����������", Name = "�����������", Patronymic = "�����������", User = apple, GenderId = 2, City = cityKiev });
    db.Friendships.Add(new Friendship() { First = apple.Person, Second = cooky.Person, Direction = FriendDirection.FirstToSecond });

    var orange = new User() { Login = "orange", Password = "orange" };
    db.Users.Add(orange);
    db.Persons.Add(new Person() { Surname = "�����������", Name = "�������", Patronymic = "���������", User = orange, GenderId = 2, City = cityMinsk });
    db.Friendships.Add(new Friendship() { First = orange.Person, Second = apple.Person, Direction = FriendDirection.Both });

    var berry = new User() { Login = "berry", Password = "berry" };
    db.Users.Add(berry);
    db.Persons.Add(new Person() { Surname = "�������", Name = "����", Patronymic = "���������", User = berry, GenderId = 3, City = cityUfa });
    db.Friendships.Add(new Friendship() { First = berry.Person, Second = banana.Person, Direction = FriendDirection.FirstToSecond });

    var interest1 = new Interest() { Title = "�����" };
    var interest2 = new Interest() { Title = "������" };
    var interest3 = new Interest() { Title = "����������" };
    var interest4 = new Interest() { Title = "�����" };
    var interest5 = new Interest() { Title = "���������" };
    var interest6 = new Interest() { Title = "�������" };
    var interest7 = new Interest() { Title = "K-pop" };
    var interest8 = new Interest() { Title = "�����" };
    db.Interests.Add(interest1);
    db.Interests.Add(interest2);
    db.Interests.Add(interest4);
    db.Interests.Add(interest5);
    db.Interests.Add(interest6);
    db.Interests.Add(interest7);
    db.Interests.Add(interest8);
    db.Interests.Add(interest3);
    demo.Person.Interests.Add(interest1);
    demo.Person.Interests.Add(interest2);
    berry.Person.Interests.Add(interest7);
    berry.Person.Interests.Add(interest8);
    banana.Person.Interests.Add(interest7);
    banana.Person.Interests.Add(interest6);
    banana.Person.Interests.Add(interest1);
    orange.Person.Interests.Add(interest3);
    orange.Person.Interests.Add(interest4);
    apple.Person.Interests.Add(interest5);
    apple.Person.Interests.Add(interest2);
    cooky.Person.Interests.Add(interest5);
    cooky.Person.Interests.Add(interest3);
    cherry.Person.Interests.Add(interest8);
    cherry.Person.Interests.Add(interest4);


    db.SaveChanges();

    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = demo1.Id, Text = "������ demo1" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = demo2.Id, Text = "������ demo2" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo1.Id, ToId = demo.Id, Text = "������ demo" });

    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = cooky.Id, Text = "���, ������, ��������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "� ������� ���" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "����� ���?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "���������������������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = banana.Id, Text = "�������� ����. �������� ������ �� ���� � ����� ��������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = apple.Id, Text = "������ � ���� ��� ������? ��� ��� �������..." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = apple.Id, ToId = orange.Id, Text = "�� ����... ����� ���� ����� �������� ����?:D" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = apple.Id, Text = "......." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = demo.Id, Text = "������ ��� �� �� ���������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = banana.Id, Text = "���� �����, ����������� �������....." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = apple.Id, ToId = cooky.Id, Text = "��� �� ������?) � �������? ������ ������� �� ������ ��:X" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = apple.Id, Text = "����� �������, ��������? ���� ������ �� ������. � ��-�� ���� � ���� � ����� ������� ������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "��, ��, ����� ����? � ���� �����?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "�� ��, ����� �� 6 ���� ����� �����?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "��, ����� ��������� ���������� ����, ������ � ��� ������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "�� �� ���, �������� �����" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = banana.Id, Text = "�����, ���� �������� �������� �� �������?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = berry.Id, Text = "��... ���" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = banana.Id, Text = "��, �������. ����� ��� �� ������� � �������?:D" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = apple.Id, Text = "��� ��������� �� ��� � �������, ��� ������ ����� �������� �����. �� �� �����, � ���� �� �����!:*" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = banana.Id, Text = "� ������ �� ���� ��� ������ �� ��������!" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = orange.Id, Text = "�������, �������� �����. ��� �� ������?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = cherry.Id, Text = "������" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = orange.Id, Text = "������. ��� ��� �������� ��� ����� ��� ����...." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = cooky.Id, Text = "���� �����" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = cherry.Id, Text = "��� ����� ������� ���� �����. � �� ���� �����" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = cooky.Id, Text = "�����. � �������, ��� ���� ��� ���� ����� ��������?)" });

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI((options) =>
    {
        //options.SwaggerEndpoint("/swagger/v1/swagger.json", "our super API v1");
        //options.RoutePrefix = string.Empty;
    });
}

 //app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
