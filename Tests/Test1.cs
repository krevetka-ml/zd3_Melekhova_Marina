using Microsoft.VisualStudio.TestTools.UnitTesting;
using zd3_Melekhova_Marina;
namespace Tests
{
    [TestClass]
    public sealed class Test1
    {
        // тест 1: проверка формулы качества базового класса
        [TestMethod]
        public void TestQualityBaseClass()
        {
            FoodProduct product = new FoodProduct("Тест", 10, 20, 5, DateTime.Now);
            double result = product.CalculateQuality();
            Assert.AreEqual(120, result, 0.001);
        }

        // тест 2: проверка формулы качества класса-наследника
        [TestMethod]
        public void TestQualityExtendedClass()
        {
            ExtendedFoodProduct product = new ExtendedFoodProduct("Тест", 10, 20, 5, DateTime.Now, 50, "Тест", 100);
            double result = product.CalculateQuality();
            Assert.AreEqual(494, result, 0.001);
        }

        // тест 3: проверка с нулевыми значениями
        [TestMethod]
        public void TestQualityZeroValues()
        {
            FoodProduct product = new FoodProduct("Пустой", 0, 0, 0, DateTime.Now);
            double result = product.CalculateQuality();
            Assert.AreEqual(0, result);
        }

        // тест 4: проверка метода добавления через параметры
        [TestMethod]
        public void TestAddByParameters()
        {
            List<FoodProduct> collection = new List<FoodProduct>();
            FoodProduct.Add("Тестовый продукт", 15, 25, 10, DateTime.Now, collection);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("Тестовый продукт", collection[0].ProductName);
        }

        // тест 5: проверка метода добавления через массив
        [TestMethod]
        public void TestAddByArray()
        {
            List<FoodProduct> collection = new List<FoodProduct>();
            DateTime testDate = new DateTime(2024, 5, 15);
            object[] data = new object[] { "Тест", 12.5, 30.2, 8.7, testDate };

            FoodProduct.Add(data, collection);

            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("Тест", collection[0].ProductName);
        }

        // тест 6: проверка метода удаления по имени
        [TestMethod]
        public void TestRemoveByName()
        {
            List<FoodProduct> collection = new List<FoodProduct>();
            FoodProduct.Add("Продукт1", 10, 10, 10, DateTime.Now, collection);
            FoodProduct.Add("Продукт2", 20, 20, 20, DateTime.Now, collection);

            bool result = FoodProduct.Remove("Продукт1", collection);

            Assert.IsTrue(result);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("Продукт2", collection[0].ProductName);
        }

        // тест 7: проверка метода удаления по индексу
        [TestMethod]
        public void TestRemoveByIndex()
        {
            List<FoodProduct> collection = new List<FoodProduct>();
            FoodProduct.Add("Продукт1", 10, 10, 10, DateTime.Now, collection);
            FoodProduct.Add("Продукт2", 20, 20, 20, DateTime.Now, collection);

            bool result = FoodProduct.Remove(0, collection);

            Assert.IsTrue(result);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual("Продукт2", collection[0].ProductName);
        }

        // тест 8: проверка вывода информации базового класса
        [TestMethod]
        public void TestGetInfoBaseClass()
        {
            DateTime date = new DateTime(2024, 5, 15);
            FoodProduct product = new FoodProduct("Гречка", 12, 60, 3, date);
            string info = product.GetInfo();

            Assert.IsTrue(info.Contains("Гречка"));
            Assert.IsTrue(info.Contains("12"));
            Assert.IsTrue(info.Contains("60"));
            Assert.IsTrue(info.Contains("Качество"));
        }

        // тест 9: проверка вывода информации класса-наследника
        [TestMethod]
        public void TestGetInfoExtendedClass()
        {
            ExtendedFoodProduct product = new ExtendedFoodProduct("Творог", 18, 3, 5, new DateTime(2024, 5, 15), 150, "Фермер", 250);
            string info = product.GetInfo();

            Assert.IsTrue(info.Contains("Творог"));
            Assert.IsTrue(info.Contains("150"));
            Assert.IsTrue(info.Contains("Фермер"));
            Assert.IsTrue(info.Contains("Qp"));
        }

        // тест 10: проверка наследования
        [TestMethod]
        public void TestInheritance()
        {
            DateTime date = new DateTime(2024, 1, 1);
            ExtendedFoodProduct product = new ExtendedFoodProduct("Йогурт", 4, 7, 2.5, date, 80, "Здоровье", 150);

            Assert.AreEqual("Йогурт", product.ProductName);
            Assert.AreEqual(4, product.Protein);
            Assert.AreEqual(7, product.Carbohydrates);
            Assert.AreEqual(2.5, product.Fats);
            Assert.AreEqual(80, product.CaloriesPer100g);
            Assert.AreEqual("Здоровье", product.Manufacturer);
            Assert.AreEqual(150, product.Weight);
        }

        // тест 11: проверка LINQ фильтрации
        [TestMethod]
        public void TestLinqFilter()
        {
            List<FoodProduct> products = new List<FoodProduct>
            {
                new FoodProduct("А", 10, 10, 0, DateTime.Now),
                new FoodProduct("Б", 20, 20, 0, DateTime.Now),
                new FoodProduct("В", 5, 5, 0, DateTime.Now)
            };

            var filtered = products.Where(p => p.CalculateQuality() > 100);

            Assert.AreEqual(1, filtered.Count());
            Assert.AreEqual("Б", filtered.First().ProductName);
        }

        // тест 12: проверка LINQ поиска максимального качества
        [TestMethod]
        public void TestLinqMax()
        {
            List<FoodProduct> products = new List<FoodProduct>
            {
                new FoodProduct("А", 10, 10, 0, DateTime.Now),
                new FoodProduct("Б", 30, 30, 0, DateTime.Now),
                new FoodProduct("В", 20, 20, 0, DateTime.Now)
            };

            double max = products.Max(p => p.CalculateQuality());

            Assert.AreEqual(240, max);
        }

        // тест 13: проверка LINQ среднего значения
        [TestMethod]
        public void TestLinqAverage()
        {
            List<FoodProduct> products = new List<FoodProduct>
            {
                new FoodProduct("А", 10, 10, 0, DateTime.Now),
                new FoodProduct("Б", 20, 20, 0, DateTime.Now),
                new FoodProduct("В", 30, 30, 0, DateTime.Now)
            };

            double avg = products.Average(p => p.CalculateQuality());

            Assert.AreEqual(160, avg);
        }

        // тест 14: проверка работы с Dictionary
        [TestMethod]
        public void TestDictionaryOperations()
        {
            Dictionary<int, ExtendedFoodProduct> dict = new Dictionary<int, ExtendedFoodProduct>();
            ExtendedFoodProduct product = new ExtendedFoodProduct("Сыр", 25, 2, 30, DateTime.Now, 350, "Завод", 200);

            dict.Add(1, product);
            Assert.AreEqual(1, dict.Count);
            Assert.AreEqual("Сыр", dict[1].ProductName);

            bool removed = dict.Remove(1);
            Assert.IsTrue(removed);
            Assert.AreEqual(0, dict.Count);
        }

        // тест 15: проверка валидации даты
        [TestMethod]
        public void TestDateValidation()
        {
            DateTime futureDate = DateTime.Now.AddDays(10);
            bool exceptionThrown = false;

            try
            {
                if (futureDate > DateTime.Now)
                {
                    throw new Exception("Дата не может быть позже сегодняшней");
                }
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Assert.AreEqual("Дата не может быть позже сегодняшней", ex.Message);
            }

            Assert.IsTrue(exceptionThrown, "Исключение не было выброшено");
        }
    }
}