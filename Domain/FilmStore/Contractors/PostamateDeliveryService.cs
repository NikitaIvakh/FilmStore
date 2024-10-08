﻿namespace FilmStore.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            { "1", "Минск" },
            { "2", "Могилев" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Проспект победителей" },
                    { "2", "Немига" },
                    { "3", "Восток" },
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Площадь звезд" },
                    { "5", "«Дунькин клуб» " },
                    { "6", "Площадь Ленина" },
                }
            }
        };

        public string Name => "Postamate";

        public string Title => "Доставка через постаматы в Минске и Могилеве";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name).AddParameter("orderId", order.Id.ToString()).AddField(new SelectionField("Город", "city", "1", cities));
        }

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                    return Form.CreateNext(Name, 2, values).AddField(new SelectionField("Постамат", "postamate", "1", postamates["1"]));

                else if (values["city"] == "2")
                    return Form.CreateNext(Name, 2, values).AddField(new SelectionField("Постамат", "postamate", "4", postamates["2"]));

                else
                    throw new InvalidOperationException("Invalid postamate city.");

            }

            else if (step == 2)
                return Form.CreateLast(Name, 3, values);

            else
                throw new InvalidOperationException("Invalid postamate step.");
        }

        public OrderDelivery GetDelivery(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid form.");

            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var postamateId = form.Parameters["postamate"];
            var postamateName = postamates[cityId][postamateId];

            var parameters = new Dictionary<string, string>
            {
                { nameof(cityId), cityId },
                { nameof(cityName), cityName },
                { nameof(postamateId), postamateId },
                { nameof(postamateName), postamateName },
            };

            var description = $"Город: {cityName}\nПостамат: {postamateName}";
            return new OrderDelivery(Name, description, 10m, parameters);
        }
    }
}