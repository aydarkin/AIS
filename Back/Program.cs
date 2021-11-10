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
    // api/Model/ :
    // (Создание) - POST, вход json с полями, возвращает json объекта
    // (Обновление) - PATCH, вход json с полями, возвращает json объекта

    // api/Model/:id
    // (Удаление) - DELETE, вход id в строке, возвращает true
    // (Чтение) - GET, вход id в строке, возвращает true

    // api/Model?параметры_фильтрации (типа http://localhost:57086/Home/Area?altitude=20&height=4)
    // (Списочный) GET, вход в строке, возвращает json массив объектов

    // Для Friendships отдельные методы
    // api/friendship/link - POST, на входе json {From: ид_заявителя, To: ид_кого}, возвращает тип дружбы (0, 1, 2)
    //       Примечание: FirstID < SecondID
    // api/friendship/unlink - POST, на входе json {From: ид_заявителя, To: ид_кого}, возвращает тип дружбы (0, 1, 2)


    // Для Interests уточнение по списочному
    // api/interests?IncludePersons=true в результат добавить массив объектов пользователей

    // Для Persons уточнение по списочному
    // api/persons?IncludeInterests=true в результат добавить массив объектов интересов

    // Для Persons уточнение по списочному
    // сортировка по фамилии по алфавиту

    // Для Messages уточнение по списочному
    // api/messages?Person=123 все входящие и исходящие сообщения отсортированные по дате (сначала новее)
    // также в результате не просто id пользователей, а их объекты
    // пример => [{Id: 1, Text: "Привет", From: { Id: 123, Name: ... }, To: { Id: 124, Name: ... }}, ...]

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

    var demo1 = new User() { Login = "demo1", Password = "demo1" };
    db.Users.Add(demo1);
    db.Persons.Add(new Person() { Name = "Подписчик", User = demo1 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo1.Person, Direction = FriendDirection.SecondToFirst});


    var demo2 = new User() { Login = "demo2", Password = "demo2" };
    db.Users.Add(demo2);
    db.Persons.Add(new Person() { Name = "Друг", User = demo2 });
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
