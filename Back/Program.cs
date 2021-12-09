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

// цепляем БД
builder.Services.AddDbContext<AppDBContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,

            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    })
    .AddCookie(); ;
// builder.Services.AddControllersWithViews();

var app = builder.Build();

// TODO пока для первичного пересоздания бд
using (var db = new AppDBContext())
{
    // api/Model/ :
    // (Создание) - POST, вход json с полями, возвращает json объекта
    // (Обновление) - PUT, вход json с полями, возвращает json объекта

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

    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    db.Genders.Add(new Gender() { Title = "Мужской" });
    db.Genders.Add(new Gender() { Title = "Женский" });
    db.Genders.Add(new Gender() { Title = "Другой" });

    var rf = new Country() { Title = "Россия" };
    db.Countries.Add(rf);
    var ua = new Country() { Title = "Украина" };
    db.Countries.Add(ua);
    db.Countries.Add(new Country() { Title = "Казахстан" });
    var by = new Country() { Title = "Беларусь" };
    db.Countries.Add(by);
    db.Countries.Add(new Country() { Title = "Грузия" });

    var cityUfa = new City() { Title = "Уфа", Country = rf };
    db.Cities.Add(cityUfa);
    db.Cities.Add(new City() { Title = "Москва", Country = rf });
    db.Cities.Add(new City() { Title = "Чишмы", Country = rf });
    db.Cities.Add(new City() { Title = "Туймазы", Country = rf });
    var cityKiev = new City() { Title = "Киев", Country = ua };
    db.Cities.Add(cityKiev);
    db.Cities.Add(new City() { Title = "Харьков", Country = ua });
    db.Cities.Add(new City() { Title = "Одесса", Country = ua });
    var cityMinsk = new City() { Title = "Минск", Country = by };
    db.Cities.Add(cityMinsk);
    db.Cities.Add(new City() { Title = "Брест", Country = by });
    db.Cities.Add(new City() { Title = "Могилёв", Country = by });
    db.Cities.Add(new City() { Title = "Витебск", Country = by });

    var demo = new User() { Login = "demo", Password = "demo" };
    db.Users.Add(demo);
    db.Persons.Add(new Person() { Name = "Демо", Surname = "Иванов", User = demo, GenderId = 1, City = cityUfa });

    var demo1 = new User() { Login = "demo1", Password = "demo1" };
    db.Users.Add(demo1);
    db.Persons.Add(new Person() { Name = "Подписчик", User = demo1 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo1.Person, Direction = FriendDirection.SecondToFirst});

    var demo2 = new User() { Login = "demo2", Password = "demo2" };
    db.Users.Add(demo2);
    db.Persons.Add(new Person() { Name = "Друг", User = demo2 });
    db.Friendships.Add(new Friendship() { First = demo.Person, Second = demo2.Person, Direction = FriendDirection.Both });

    var newUser = new User() { Login = "new", Password = "new" };
    db.Persons.Add(new Person() { Name = "Новый пользователь", User = newUser });

    var banana = new User() { Login = "banana", Password = "banana" };
    db.Users.Add(banana);
    db.Persons.Add(new Person() { Surname = "Клубничка", Name = "Елена", Patronymic = "Антоновна", User = banana, GenderId = 2, City =  cityMinsk});
    db.Friendships.Add(new Friendship() { First = demo1.Person, Second = banana.Person, Direction = FriendDirection.Both });

    var cherry = new User() { Login = "cherry", Password = "cherry" };
    db.Users.Add(cherry);
    db.Persons.Add(new Person() { Surname = "Бикини", Name = "Григорий", Patronymic = "Версальевич", User = cherry, GenderId = 1, City = cityUfa });
    db.Friendships.Add(new Friendship() { First = banana.Person, Second = cherry.Person, Direction = FriendDirection.SecondToFirst });

    var cooky = new User() { Login = "cooky", Password = "cooky" };
    db.Users.Add(cooky);
    db.Persons.Add(new Person() { Surname = "Чебурашко", Name = "Анатолий", Patronymic = "Генадьевич", User = cooky, GenderId = 1, City = cityKiev });
    db.Friendships.Add(new Friendship() { First = banana.Person, Second = cooky.Person, Direction = FriendDirection.Both });

    var apple = new User() { Login = "apple", Password = "apple" };
    db.Users.Add(apple);
    db.Persons.Add(new Person() { Surname = "Мармышкина", Name = "Апполинария", Patronymic = "Вадимановна", User = apple, GenderId = 2, City = cityKiev });
    db.Friendships.Add(new Friendship() { First = apple.Person, Second = cooky.Person, Direction = FriendDirection.FirstToSecond });

    var orange = new User() { Login = "orange", Password = "orange" };
    db.Users.Add(orange);
    db.Persons.Add(new Person() { Surname = "Скайримовна", Name = "Валерия", Patronymic = "Даваковна", User = orange, GenderId = 2, City = cityMinsk });
    db.Friendships.Add(new Friendship() { First = orange.Person, Second = apple.Person, Direction = FriendDirection.Both });

    var berry = new User() { Login = "berry", Password = "berry" };
    db.Users.Add(berry);
    db.Persons.Add(new Person() { Surname = "Корейка", Name = "Олег", Patronymic = "Марийевич", User = berry, GenderId = 3, City = cityUfa });
    db.Friendships.Add(new Friendship() { First = berry.Person, Second = banana.Person, Direction = FriendDirection.FirstToSecond });

    var interest1 = new Interest() { Title = "Книги" };
    var interest2 = new Interest() { Title = "Музыка" };
    var interest3 = new Interest() { Title = "Кинофильмы" };
    var interest4 = new Interest() { Title = "Танцы" };
    var interest5 = new Interest() { Title = "Видеоигры" };
    var interest6 = new Interest() { Title = "Сериалы" };
    var interest7 = new Interest() { Title = "K-pop" };
    var interest8 = new Interest() { Title = "Аниме" };
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

    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = demo1.Id, Text = "Привет demo1" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = demo2.Id, Text = "Привет demo2" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo1.Id, ToId = demo.Id, Text = "Привет demo" });

    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = cooky.Id, Text = "Чао, персик, дозревай" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "Я поменял пол" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "Какой пол?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "ЛАМИНАААААААААААААААТ" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = banana.Id, Text = "Классный трек. Послухай вмести со мной и моими братками" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = apple.Id, Text = "Почему у меня нет друзей? Мне так одиноко..." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = apple.Id, ToId = orange.Id, Text = "Не знаю... Может тебе стоит поменять полы?:D" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = apple.Id, Text = "......." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = demo.Id, Text = "Докажи что ты не кремлебот" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = demo.Id, ToId = banana.Id, Text = "Олег занят, перезвоните позднее....." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = apple.Id, ToId = cooky.Id, Text = "Тут не занято?) Я подсяду? Скинть домашку по матеше пж:X" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = apple.Id, Text = "Какую домашку, предурок? Тебе завтра на работу. А ну-ка ноги в руки и пошел чистить унитаз" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "Эй, сп, семки есть? А если найду?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "Ты шо, опять до 6 утра аниме пялил?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = berry.Id, Text = "Да, вчера посмотрел токийского гуля, теперь я дед инсайд" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = cherry.Id, Text = "Ты не дед, максимум дебил" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = banana.Id, Text = "Детка, твои родители случайно не фриганы?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = banana.Id, ToId = berry.Id, Text = "Эм... Нет" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = berry.Id, ToId = banana.Id, Text = "Хм, странно. Тогда что вы делаете в помойке?:D" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = apple.Id, Text = "Мне позвонили из рая и сказали, что пропал самый красивый ангел. Но не бойся, я тебя не выдал!:*" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = banana.Id, Text = "Я тащусь от тебя как приора по асфальту!" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = orange.Id, Text = "Дэвушка, закройте глаза. Что вы видите?" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = orange.Id, ToId = cherry.Id, Text = "Ничего" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = orange.Id, Text = "Именно. Вот так выглядит моя жизнь без тебя...." });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = cooky.Id, Text = "Купи гараж" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cooky.Id, ToId = cherry.Id, Text = "Все могут сказать купи гараж. А ты купи гараж" });
    db.Messages.Add(new Message() { Date = new DateTime(), FromId = cherry.Id, ToId = cooky.Id, Text = "Ладно. Я говорил, что твоя ава тебе очень подходит?)" });

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
