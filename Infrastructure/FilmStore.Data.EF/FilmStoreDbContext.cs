using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace FilmStore.Data.EF
{
    public class FilmStoreDbContext : DbContext
    {
        public DbSet<FilmDTO> Films { get; set; }

        public DbSet<OrderDTO> Orders { get; set; }

        public DbSet<OrderItemDTO> OrderItems { get; set; }

        public FilmStoreDbContext(DbContextOptions<FilmStoreDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildFilms(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);

            modelBuilder.Entity<FilmDTO>().HasGeneratedTsVectorColumn(p =>
            p.SearchVector, "russian", p => new { p.Title, p.Author }).HasIndex(p => p.SearchVector).HasMethod("GiST");
        }

        private static void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDTO>(action =>
            {
                action.Property(key => key.Price).HasColumnType("money");
                action.HasOne(key => key.Order).WithMany(key => key.Items).IsRequired();
            });
        }

        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDTO>(action =>
            {
                action.Property(key => key.CellPhone).HasMaxLength(20).IsRequired(false);
                action.Property(key => key.DeliveryUniqueCode).HasMaxLength(40).IsRequired(false);
                action.Property(key => key.DeliveryDescription).IsRequired(false);
                action.Property(key => key.DeliveryPrice).HasColumnType("money");
                action.Property(key => key.PaymentServiceName).HasMaxLength(40).IsRequired(false);
                action.Property(KEY => KEY.PaymentDescription).IsRequired(false);

                action.Property(dto => dto.DeliveryParameters)
                     .HasConversion(
                         value => JsonConvert.SerializeObject(value),
                         value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                     .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentParameters)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);
            });
        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>(
                (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
                dictionary => dictionary.Aggregate(
                    0,
                    (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())
                )
            );

        private static void BuildFilms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmDTO>(action =>
            {
                action.Property(key => key.IMDb).HasMaxLength(20).IsRequired();
                action.Property(key => key.Title).IsRequired();
                action.Property(key => key.Price).HasColumnType("money");

                action.HasData(
                    new FilmDTO
                    {
                        Id = 1,
                        IMDb = "ID1399103",
                        Author = "Майкл Бэй",
                        Title = "Трансформеры: Темная сторона Луны",
                        Description = "Таинственное событие из прошлого Земли угрожает разжечь настолько масштабную войну, что Трансформеры в одиночку не смогут спасти планету. Сэм Уитвики и автоботы должны сражаться с тьмой, " +
                        "чтобы защитить наш мир от десептиконов.",
                        Price = 9.99m,
                    },

                     new FilmDTO
                     {
                         Id = 2,
                         IMDb = "ID0120915",
                         Author = "Джордж Лукас",
                         Title = "Звёздные войны: Эпизод 1 — Скрытая угроза",
                         Description = "Еще совсем недавно было трудно поверить, что убеленный сединами наставник Оби-Ван Кеноби когда-то был безусым юнцом, а повергающий Галактику в трепет Император Палпратин - лишь амбициозным сенатором. " +
                         "И совершенно невозможно представить зловещего Дарта Вейдера маленьким мальчиком, который живет в рабстве на Татуине, а по ночам мечтает о далеких звездах. Мы попадаем в то самое время, " +
                         "когда рыцари-джедаи еще хранят мир и спокойствие в Галактике. Но темная сила, готовая поглотить планеты и народы, уже пробудилась, и только древнее пророчество дждаев об избранном, который принесет равновесие в Силу, " +
                         "дает надежду в преддверии катастрофы. Скрытая угроза нависла над Республикой, простоявшей не одну тысячу лет, но теперь погрязшей в пучине политических разногласий и интриг. " +
                         "Мастер-джедай Квай-Гон Джин и его ученик Оби-Ван Кеноби приходят на помощь королеве Амидале, которая стремиться спасти планету Набу от блокады алчной Торговой Федерации. " +
                         "Бегство с Набу приводит джедаев на песчаные просторы Татуина, где им суждена встреча с Энакином Скайуокером. .Здесь начинается история, которая впоследствии станет легендой…",
                         Price = 17.99m,
                     },

                     new FilmDTO
                     {
                         Id = 3,
                         IMDb = "ID0121765",
                         Author = "Джордж Лукас",
                         Title = "Звёздные войны: Эпизод 2 — Атака клонов",
                         Description = "Действие разворачивается через 10 лет после событий, описанных в первом эпизоде знаменитой саги. Республика все глубже погружается в пучину противоречий и хаоса. " +
                         "Движение сепаратистов, представленное сотнями планет и могущественным альянсом корпораций, грозит стать новой угрозой для Галактики, с которой не смогут справиться даже джедаи. " +
                         "Назревающий конфликт, заранее спланированный могущественными, но пока еще не разоблаченными силами, ведет к началу Клонических войн и к закату республики. Чтобы противостоять угрозе вселенских масштабов, " +
                         "Верховный канцлер Палпатин добивается консолидации власти в республике в своих руках и отдает приказ о создании республиканской армии для поддержки малочисленной группы джедаев в их борьбе с превосходящим противником.",
                         Price = 19.99m,
                     },

                    new FilmDTO
                    {
                        Id = 4,
                        IMDb = "ID0121766",
                        Author = "Мэтью Стовер",
                        Title = "Звездные войны: Эпизод 3 — Месть ситхов",
                        Description = "Войны клонов бушуют по всей галактике. Зловещий Лорд Ситхов захватывает контроль над Республикой и развращает Энакина Скайуокера, чтобы тот стал его темным учеником, Дартом Вейдером. " +
                        "Оби-Ван Кеноби должен сразиться со своим павшим другом в эпической дуэли на световых мечах.",
                        Price = 19.99m,
                    },

                    new FilmDTO
                    {
                        Id = 5,
                        IMDb = "ID 0383574",
                        Author = "Гор Вербински",
                        Title = "Пираты Карибского моря: Сундук мертвеца",
                        Description = "Капитан пиратов Джек Воробей должен завладеть легендарным сундуком Мертвеца, чтобы обмануть смерть и вечное проклятие. Но открытое море таит в себе множество препятствий, включая могучего кракена!",
                        Price = 17.99m,
                    },

                    new FilmDTO
                    {
                        Id = 6,
                        IMDb = "ID0449088",
                        Author = "Гор Вербински",
                        Title = "Пираты Карибского моря: на краю света",
                        Description = "Когда пиратский капитан Джек Воробей оказывается в ловушке в море песка в шкафчике Дэви Джонса, его товарищи по кораблю начинают отчаянные поиски, чтобы найти и спасти его. ",
                        Price = 17.99m,
                    },

                    new FilmDTO
                    {
                        Id = 7,
                        IMDb = "ID1790809",
                        Author = "Гор Вербински",
                        Title = "Пираты Карибского моря: Мертвецы не рассказывают сказок",
                        Description = "Капитана Джека Воробья преследует старый соперник капитан Салазар и команда смертоносных призраков, которые сбегают из Дьявольского треугольника, полные решимости убить каждого пирата в море...особенно его самого. ",
                        Price = 19.99m,
                    },


                     new FilmDTO
                     {
                         Id = 8,
                         IMDb = "ID0259324",
                         Author = "Марк Невелдайн",
                         Title = "Призрачный Гонщик",
                         Description = "Джонни Блейз рано остался сиротой, и воспитанием юного сорвиголовы занялся лучший мотогонщик Америки Крэш Симпсон. С годами Джонни стал полноправным членом семьи Симпсонов и когда Крэш оказался смертельно болен, " +
                         "Блейз был готов пойти на всё ради спасения жизни приёмного отца… в том числе и на сделку с Сатаной! Теперь Джонни лучший в мире каскадёр днём и Призрачный гонщик ночью! Искренняя любовь невинной Роксаны Симпсон не даёт " +
                         "Сатане получить власть над душой Джонни, но долго ли влюблённые смогут противостоять власти Владыки ада? Поможет ли им Деймон Хеллстром, Сын Сатаны? Или спасение стоит искать в древней индейской магии? А может быть, " +
                         "стоит просто изо всех сил гнать по шоссе? Прижмитесь к обочине, смертные, на дорогу выезжает Призрачный гонщик! " +
                         "Станьте свидетелем рождения самого знаменитого мистического героя Marvel! Вас ждут безумные погони, " +
                         "смертельно опасные трюки, схватки с потусторонними слугами дьявола, жуткие обряды индейских шаманов и история о настоящей, чистой любви! В книге собраны выпуски комиксов, в которых состоялись первые появления персонажей, " +
                         "которые окажут серьёзное влияние на мистическую сторону вселенной Marvel, в том числе Деймона Хеллстрома, Сына Сатаны! ",
                         Price = 19.99m,
                     });
            });
        }
    }
}