using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zd3_Melekhova_Marina
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FoodProduct> baseProducts;  //коллекция List для базовых продуктов
        private Dictionary<int, ExtendedFoodProduct> extendedProducts;  //коллекция Dictionary для наследников
        private int nextExtendedId = 1;  //следующий ID для Dictionary

        public MainWindow()
        {
            InitializeComponent();
            InitializeCollections();
            UpdateProductList();
        }

        //заполнение тестовыми данными
        private void InitializeCollections()
        {
            baseProducts = new List<FoodProduct>();
            extendedProducts = new Dictionary<int, ExtendedFoodProduct>();

            baseProducts.Add(new FoodProduct("Хлеб", 8, 45, 2, DateTime.Now.AddDays(-5)));
            baseProducts.Add(new FoodProduct("Молоко", 3, 5, 3.5, DateTime.Now.AddDays(-2)));

            extendedProducts.Add(nextExtendedId++, new ExtendedFoodProduct("Сыр", 25, 2, 30,
                DateTime.Now.AddDays(-10), 350, "ООО Молпродукт", 200));
            extendedProducts.Add(nextExtendedId++, new ExtendedFoodProduct("Йогурт", 4, 7, 2.5,
                DateTime.Now.AddDays(-3), 80, "ЗАО Здоровье", 150));
        }

        //перегрузка 1 для базового класса (добавление через параметры)
        private void BtnAddBaseByParams_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateCommonFields()) return;

                double protein = double.Parse(txtProtein.Text);
                double carbs = double.Parse(txtCarbs.Text);
                double fats = double.Parse(txtFats.Text);
                DateTime date = dpDate.SelectedDate ?? DateTime.Now;

                FoodProduct.Add(txtName.Text, protein, carbs, fats, date, baseProducts);

                lblStatus.Text = "Добавлено (перегрузка: параметры)";
                UpdateProductList();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        //перегрузка 2 для базового класса (добавление через массив)
        private void BtnAddBaseByArray_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateCommonFields()) return;

                double protein = double.Parse(txtProtein.Text);
                double carbs = double.Parse(txtCarbs.Text);
                double fats = double.Parse(txtFats.Text);
                DateTime date = dpDate.SelectedDate ?? DateTime.Now;

                object[] data = new object[] { txtName.Text, protein, carbs, fats, date };
                FoodProduct.Add(data, baseProducts);

                lblStatus.Text = "Добавлено (перегрузка: массив)";
                UpdateProductList();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        //перегрузка 3 для базового класса (удаление по имени)
        private void BtnDeleteBaseByName_Click(object sender, RoutedEventArgs e)
        {
            if (lstProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            string selected = lstProducts.SelectedItem.ToString();
            if (!selected.Contains("[Базовый]"))
            {
                MessageBox.Show("Выберите базовый продукт");
                return;
            }

            string name = ExtractProductName(selected);
            bool removed = FoodProduct.Remove(name, baseProducts);

            if (removed)
            {
                lblStatus.Text = "Удалено (перегрузка: по имени)";
                UpdateProductList();
                txtInfo.Clear();
            }
        }

        //перегрузка 4 для базового класса (удаление по индексу)
        private void BtnDeleteBaseByIndex_Click(object sender, RoutedEventArgs e)
        {
            if (lstProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            string selected = lstProducts.SelectedItem.ToString();
            if (!selected.Contains("[Базовый]"))
            {
                MessageBox.Show("Выберите базовый продукт");
                return;
            }

            int index = lstProducts.SelectedIndex;
            bool removed = FoodProduct.Remove(index, baseProducts);

            if (removed)
            {
                lblStatus.Text = "Удалено (перегрузка: по индексу)";
                UpdateProductList();
                txtInfo.Clear();
            }
        }

        //добавление продукта-наследника
        private void BtnAddExtended_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowExtendedFields();
                if (!ValidateCommonFields()) return;
                if (!ValidateExtendedFields()) return;

                double protein = double.Parse(txtProtein.Text);
                double carbs = double.Parse(txtCarbs.Text);
                double fats = double.Parse(txtFats.Text);
                DateTime date = dpDate.SelectedDate ?? DateTime.Now;
                double calories = double.Parse(txtCalories.Text);
                string manufacturer = txtManufacturer.Text;
                double weight = double.Parse(txtWeight.Text);

                ExtendedFoodProduct product = new ExtendedFoodProduct(
                    txtName.Text, protein, carbs, fats, date, calories, manufacturer, weight);

                extendedProducts.Add(nextExtendedId++, product);

                lblStatus.Text = "Наследник добавлен";
                UpdateProductList();
                ClearInputs();
                HideExtendedFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        //удаление продукта-наследника
        private void BtnDeleteExtended_Click(object sender, RoutedEventArgs e)
        {
            if (lstProducts.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукт");
                return;
            }

            string selected = lstProducts.SelectedItem.ToString();
            if (!selected.Contains("[Наследник]"))
            {
                MessageBox.Show("Выберите продукт-наследник");
                return;
            }

            string name = ExtractProductName(selected);
            var item = extendedProducts.FirstOrDefault(x => x.Value.ProductName == name);

            if (item.Value != null)
            {
                extendedProducts.Remove(item.Key);
                lblStatus.Text = "Наследник удален";
                UpdateProductList();
                txtInfo.Clear();
            }
        }

        //проверка даты (не может быть позже сегодняшней)
        private bool ValidateDate()
        {
            DateTime selectedDate = dpDate.SelectedDate ?? DateTime.Now;
            if (selectedDate > DateTime.Now)
            {
                throw new Exception("Дата не может быть позже сегодняшней");
            }
            return true;
        }

        //проверка общих полей (название, белки, углеводы, жиры)
        private bool ValidateCommonFields()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
                throw new Exception("Введите название");

            if (!double.TryParse(txtProtein.Text, out double protein) || protein < 0)
                throw new Exception("Некорректные белки");

            if (!double.TryParse(txtCarbs.Text, out double carbs) || carbs < 0)
                throw new Exception("Некорректные углеводы");

            if (!double.TryParse(txtFats.Text, out double fats) || fats < 0)
                throw new Exception("Некорректные жиры");

            ValidateDate();
            return true;
        }

        //проверка дополнительных полей (для наследника)
        private bool ValidateExtendedFields()
        {
            if (!double.TryParse(txtCalories.Text, out double calories) || calories < 0)
                throw new Exception("Некорректная калорийность");

            if (string.IsNullOrWhiteSpace(txtManufacturer.Text))
                throw new Exception("Введите производителя");

            if (!double.TryParse(txtWeight.Text, out double weight) || weight <= 0)
                throw new Exception("Некорректный вес");

            return true;
        }

        //показать дополнительные поля (для наследника)
        private void ShowExtendedFields()
        {
            lblExtended.Visibility = Visibility.Visible;
            txtCalories.Visibility = Visibility.Visible;
            txtManufacturer.Visibility = Visibility.Visible;
            txtWeight.Visibility = Visibility.Visible;
        }

        //скрыть дополнительные поля
        private void HideExtendedFields()
        {
            lblExtended.Visibility = Visibility.Collapsed;
            txtCalories.Visibility = Visibility.Collapsed;
            txtManufacturer.Visibility = Visibility.Collapsed;
            txtWeight.Visibility = Visibility.Collapsed;
        }

        //очистить все поля ввода
        private void ClearInputs()
        {
            txtName.Text = "";
            txtProtein.Text = "";
            txtCarbs.Text = "";
            txtFats.Text = "";
            txtCalories.Text = "";
            txtManufacturer.Text = "";
            txtWeight.Text = "";
            dpDate.SelectedDate = null;
        }

        //извлечь имя продукта из строки списка
        private string ExtractProductName(string selectedItem)
        {
            int startIndex = selectedItem.IndexOf(']') + 2;
            int endIndex = selectedItem.IndexOf('(', startIndex);
            if (endIndex == -1) endIndex = selectedItem.Length;
            return selectedItem.Substring(startIndex, endIndex - startIndex).Trim();
        }

        //обновить список продуктов на экране
        private void UpdateProductList(bool showOnlyBase = false, bool showOnlyExtended = false)
        {
            lstProducts.Items.Clear();

            if (showOnlyBase)
            {
                foreach (var p in baseProducts)
                    lstProducts.Items.Add($"[Базовый] {p.ProductName} (Q: {p.CalculateQuality():F2})");
            }
            else if (showOnlyExtended)
            {
                foreach (var p in extendedProducts.Values)
                    lstProducts.Items.Add($"[Наследник] {p.ProductName} (Qp: {p.CalculateQuality():F2})");
            }
            else
            {
                foreach (var p in baseProducts)
                    lstProducts.Items.Add($"[Базовый] {p.ProductName} (Q: {p.CalculateQuality():F2})");
                foreach (var p in extendedProducts.Values)
                    lstProducts.Items.Add($"[Наследник] {p.ProductName} (Qp: {p.CalculateQuality():F2})");
            }
        }

        //показать информацию о выбранном продукте
        private void LstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstProducts.SelectedItem == null)
            {
                txtInfo.Clear();
                return;
            }

            string selected = lstProducts.SelectedItem.ToString();
            string name = ExtractProductName(selected);

            if (selected.Contains("[Базовый]"))
            {
                var product = baseProducts.FirstOrDefault(p => p.ProductName == name);
                if (product != null) txtInfo.Text = product.GetInfo();
            }
            else if (selected.Contains("[Наследник]"))
            {
                var product = extendedProducts.Values.FirstOrDefault(p => p.ProductName == name);
                if (product != null) txtInfo.Text = product.GetInfo();
            }
        }

        //меню: выход
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //меню: показать все продукты
        private void MenuShowAll_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductList();
            txtMode.Text = "Все продукты";
        }

        //меню: показать только базовые
        private void MenuShowBase_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductList(showOnlyBase: true);
            txtMode.Text = "Только базовые";
        }

        //меню: показать только наследников
        private void MenuShowExtended_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductList(showOnlyExtended: true);
            txtMode.Text = "Только наследники";
        }
    }
}