using System;
using System.Collections.Generic;
using System.Text;

namespace zd3_Melekhova_Marina
{
    // базовый класс продукта питания
    public class FoodProduct
    {
        //поля из задания
        public string ProductName { get; set; }      //название продукта
        public double Protein { get; set; }          //белки
        public double Carbohydrates { get; set; }    //углеводы

        //2 дополнительных поля
        public double Fats { get; set; }             //жиры
        public DateTime ProductionDate { get; set; } //дата производства

        //конструктор
        public FoodProduct(string productName, double protein, double carbohydrates,
                          double fats, DateTime productionDate)
        {
            ProductName = productName;
            Protein = protein;
            Carbohydrates = carbohydrates;
            Fats = fats;
            ProductionDate = productionDate;
        }

        //функция качества
        public virtual double CalculateQuality()
        {
            return Carbohydrates * 4 + Protein * 4;
        }

        //вывод информации
        public virtual string GetInfo()
        {
            return $"Продукт: {ProductName}\n" +
                   $"Белки: {Protein} г\n" +
                   $"Углеводы: {Carbohydrates} г\n" +
                   $"Жиры: {Fats} г\n" +
                   $"Дата: {ProductionDate.ToShortDateString()}\n" +
                   $"Качество: {CalculateQuality():F2}";
        }

        //основной метод: добавление через объект
        public static void Add(FoodProduct product, List<FoodProduct> collection)
        {
            if (product != null && collection != null)
            {
                collection.Add(product);
            }
        }

        //перегрузка 1: добавление через параметры
        public static void Add(string name, double protein, double carbs,
                               double fats, DateTime date, List<FoodProduct> collection)
        {
            FoodProduct newProduct = new FoodProduct(name, protein, carbs, fats, date);
            collection.Add(newProduct);
        }

        //перегрузка 2: добавление через массив
        public static void Add(object[] data, List<FoodProduct> collection)
        {
            if (data != null && data.Length >= 5)
            {
                string name = data[0].ToString();
                double protein = Convert.ToDouble(data[1]);
                double carbs = Convert.ToDouble(data[2]);
                double fats = Convert.ToDouble(data[3]);
                DateTime date = (DateTime)data[4];
                FoodProduct newProduct = new FoodProduct(name, protein, carbs, fats, date);
                collection.Add(newProduct);
            }
        }

        //основной метод: удаление по объекту
        public static bool Remove(FoodProduct product, List<FoodProduct> collection)
        {
            if (collection.Contains(product))
            {
                return collection.Remove(product);
            }
            return false;
        }

        //перегрузка 1: удаление по имени
        public static bool Remove(string productName, List<FoodProduct> collection)
        {
            FoodProduct productToRemove = collection.Find(p => p.ProductName == productName);
            if (productToRemove != null)
            {
                return collection.Remove(productToRemove);
            }
            return false;
        }

        //перегрузка 2: удаление по индексу
        public static bool Remove(int index, List<FoodProduct> collection)
        {
            if (collection != null && index >= 0 && index < collection.Count)
            {
                collection.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}
