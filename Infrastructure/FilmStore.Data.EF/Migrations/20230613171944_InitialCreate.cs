using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FilmStore.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IMDb = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                        .Annotation("Npgsql:TsVectorConfig", "russian")
                        .Annotation("Npgsql:TsVectorProperties", new[] { "Title", "Author" })
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CellPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DeliveryUniqueCode = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    DeliveryDescription = table.Column<string>(type: "text", nullable: true),
                    DeliveryPrice = table.Column<decimal>(type: "money", nullable: false),
                    DeliveryParameters = table.Column<string>(type: "text", nullable: false),
                    PaymentServiceName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    PaymentDescription = table.Column<string>(type: "text", nullable: true),
                    PaymentParameters = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilmId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "Author", "Description", "IMDb", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "Майкл Бэй", "Таинственное событие из прошлого Земли угрожает разжечь настолько масштабную войну, что Трансформеры в одиночку не смогут спасти планету. Сэм Уитвики и автоботы должны сражаться с тьмой, чтобы защитить наш мир от десептиконов.", "ID1399103", 9.99m, "Трансформеры: Темная сторона Луны" },
                    { 2, "Джордж Лукас", "Еще совсем недавно было трудно поверить, что убеленный сединами наставник Оби-Ван Кеноби когда-то был безусым юнцом, а повергающий Галактику в трепет Император Палпратин - лишь амбициозным сенатором. И совершенно невозможно представить зловещего Дарта Вейдера маленьким мальчиком, который живет в рабстве на Татуине, а по ночам мечтает о далеких звездах. Мы попадаем в то самое время, когда рыцари-джедаи еще хранят мир и спокойствие в Галактике. Но темная сила, готовая поглотить планеты и народы, уже пробудилась, и только древнее пророчество дждаев об избранном, который принесет равновесие в Силу, дает надежду в преддверии катастрофы. Скрытая угроза нависла над Республикой, простоявшей не одну тысячу лет, но теперь погрязшей в пучине политических разногласий и интриг. Мастер-джедай Квай-Гон Джин и его ученик Оби-Ван Кеноби приходят на помощь королеве Амидале, которая стремиться спасти планету Набу от блокады алчной Торговой Федерации. Бегство с Набу приводит джедаев на песчаные просторы Татуина, где им суждена встреча с Энакином Скайуокером. .Здесь начинается история, которая впоследствии станет легендой…", "ID0120915", 17.99m, "Звёздные войны: Эпизод 1 — Скрытая угроза" },
                    { 3, "Джордж Лукас", "Действие разворачивается через 10 лет после событий, описанных в первом эпизоде знаменитой саги. Республика все глубже погружается в пучину противоречий и хаоса. Движение сепаратистов, представленное сотнями планет и могущественным альянсом корпораций, грозит стать новой угрозой для Галактики, с которой не смогут справиться даже джедаи. Назревающий конфликт, заранее спланированный могущественными, но пока еще не разоблаченными силами, ведет к началу Клонических войн и к закату республики. Чтобы противостоять угрозе вселенских масштабов, Верховный канцлер Палпатин добивается консолидации власти в республике в своих руках и отдает приказ о создании республиканской армии для поддержки малочисленной группы джедаев в их борьбе с превосходящим противником.", "ID0121765", 19.99m, "Звёздные войны: Эпизод 2 — Атака клонов" },
                    { 4, "Мэтью Стовер", "Войны клонов бушуют по всей галактике. Зловещий Лорд Ситхов захватывает контроль над Республикой и развращает Энакина Скайуокера, чтобы тот стал его темным учеником, Дартом Вейдером. Оби-Ван Кеноби должен сразиться со своим павшим другом в эпической дуэли на световых мечах.", "ID0121766", 19.99m, "Звездные войны: Эпизод 3 — Месть ситхов" },
                    { 5, "Гор Вербински", "Капитан пиратов Джек Воробей должен завладеть легендарным сундуком Мертвеца, чтобы обмануть смерть и вечное проклятие. Но открытое море таит в себе множество препятствий, включая могучего кракена!", "ID 0383574", 17.99m, "Пираты Карибского моря: Сундук мертвеца" },
                    { 6, "Гор Вербински", "Когда пиратский капитан Джек Воробей оказывается в ловушке в море песка в шкафчике Дэви Джонса, его товарищи по кораблю начинают отчаянные поиски, чтобы найти и спасти его. ", "ID0449088", 17.99m, "Пираты Карибского моря: на краю света" },
                    { 7, "Гор Вербински", "Капитана Джека Воробья преследует старый соперник капитан Салазар и команда смертоносных призраков, которые сбегают из Дьявольского треугольника, полные решимости убить каждого пирата в море...особенно его самого. ", "ID1790809", 19.99m, "Пираты Карибского моря: Мертвецы не рассказывают сказок" },
                    { 8, "Марк Невелдайн", "Джонни Блейз рано остался сиротой, и воспитанием юного сорвиголовы занялся лучший мотогонщик Америки Крэш Симпсон. С годами Джонни стал полноправным членом семьи Симпсонов и когда Крэш оказался смертельно болен, Блейз был готов пойти на всё ради спасения жизни приёмного отца… в том числе и на сделку с Сатаной! Теперь Джонни лучший в мире каскадёр днём и Призрачный гонщик ночью! Искренняя любовь невинной Роксаны Симпсон не даёт Сатане получить власть над душой Джонни, но долго ли влюблённые смогут противостоять власти Владыки ада? Поможет ли им Деймон Хеллстром, Сын Сатаны? Или спасение стоит искать в древней индейской магии? А может быть, стоит просто изо всех сил гнать по шоссе? Прижмитесь к обочине, смертные, на дорогу выезжает Призрачный гонщик! Станьте свидетелем рождения самого знаменитого мистического героя Marvel! Вас ждут безумные погони, смертельно опасные трюки, схватки с потусторонними слугами дьявола, жуткие обряды индейских шаманов и история о настоящей, чистой любви! В книге собраны выпуски комиксов, в которых состоялись первые появления персонажей, которые окажут серьёзное влияние на мистическую сторону вселенной Marvel, в том числе Деймона Хеллстрома, Сына Сатаны! ", "ID0259324", 19.99m, "Призрачный Гонщик" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_SearchVector",
                table: "Films",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GiST");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
