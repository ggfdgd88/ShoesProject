using Microsoft.EntityFrameworkCore;
using ShoesProject.Models;

namespace ShoesProject
{
    public partial class MainForm : Form
    {
        private readonly User? _currentUser;
        private List<Product> _allProducts = new();

        public MainForm(User? user = null)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Отображение ФИО и роли пользователя
            if (_currentUser != null)
            {
                string fullName = $"{_currentUser.LastName} {_currentUser.FirstName} {_currentUser.MiddleName}".Trim();
                string roleName = _currentUser.Role?.Name ?? "Пользователь";
                lblUserInfo.Text = $"{fullName} ({roleName})";
            }
            else
            {
                lblUserInfo.Text = "Гость";
            }

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using var db = new ShoeShopDbContext();

                // Загружаем производителей для фильтра
                var manufacturers = db.Manufacturers.OrderBy(m => m.Name).ToList();
                cmbManufacturer.Items.Clear();
                cmbManufacturer.Items.Add("Все производители");
                foreach (var m in manufacturers)
                {
                    cmbManufacturer.Items.Add(m.Name);
                }

                // Загружаем товары вместе со связями
                _allProducts = db.Products
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Subcategory)
                    .Include(p => p.ProductStocks)
                    .ToList();

                cmbManufacturer.SelectedIndex = 0;
                cmbSort.SelectedIndex = 0;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterChanged(object? sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<Product> filtered = _allProducts;

            // 1. Поиск по названию, описанию и составу
            string search = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filtered = filtered.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    (p.Composition != null && p.Composition.ToLower().Contains(search)));
            }

            // 2. Фильтрация по производителю
            if (cmbManufacturer.SelectedIndex > 0 && cmbManufacturer.SelectedItem != null)
            {
                string selectedManufacturer = cmbManufacturer.SelectedItem.ToString()!;
                filtered = filtered.Where(p => p.Manufacturer != null && p.Manufacturer.Name == selectedManufacturer);
            }

            // 3. Сортировка по цене
            if (cmbSort.SelectedIndex == 1)
                filtered = filtered.OrderBy(p => p.Price);
            else if (cmbSort.SelectedIndex == 2)
                filtered = filtered.OrderByDescending(p => p.Price);

            var resultList = filtered.ToList();

            // Формируем строки для таблицы
            dgvProducts.DataSource = resultList.Select(p => new
            {
                Артикул = p.Id,
                Наименование = p.Name,
                Категория = p.Subcategory?.Name ?? "-",
                Производитель = p.Manufacturer?.Name ?? "-",
                Описание = p.Description ?? "",
                Состав = p.Composition ?? "",
                Цена = $"{p.Price:N2} руб.",
                Остаток = p.ProductStocks.Sum(s => s.Quantity)
            }).ToList();

            // Подсветка товаров, которых нет в наличии (Остаток == 0)
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.Cells["Остаток"].Value is int qty && qty == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            lblCount.Text = $"Показано: {resultList.Count} из {_allProducts.Count}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
