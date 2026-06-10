using System;
using System.Collections.Generic;
using System.Text;

namespace zd3_Melekhova_Marina
{
    // класс-потомок
    public class ExtendedFoodProduct : FoodProduct
    {
        //поле P (калорийность)
        public double CaloriesPer100g { get; set; }

        //дополнительные свойства
        public string Manufacturer { get; set; }
        public double Weight { get; set; }

        //конструктор
        public ExtendedFoodProduct(string productName, double protein, double carbohydrates,
                                   double fats, DateTime productionDate, double caloriesPer100g,
                                   string manufacturer, double weight)
            : base(productName, protein, carbohydrates, fats, productionDate)
        {
            CaloriesPer100g = caloriesPer100g;
            Manufacturer = manufacturer;
            Weight = weight;
        }

        //перекрытие функции качества Qp = Q*1,2 + P*7
        public override double CalculateQuality()
        {
            double baseQuality = base.CalculateQuality();
            return baseQuality * 1.2 + CaloriesPer100g * 7;
        }

        //перекрытие вывода информации
        public override string GetInfo()
        {
            return base.GetInfo() +
                   $"\nКалорийность: {CaloriesPer100g} ккал\n" +
                   $"Производитель: {Manufacturer}\n" +
                   $"Вес: {Weight} г\n" +
                   $"Качество (Qp): {CalculateQuality():F2}";
        }
    }
}
