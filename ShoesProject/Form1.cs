using Microsoft.EntityFrameworkCore;
using ShoesProject.Models;

namespace ShoesProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Введите логин.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new ShoeShopDbContext();
                var user = db.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Login == login);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с таким логином не найден.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                OpenMainForm(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            OpenMainForm(null);
        }

        private void OpenMainForm(User? user)
        {
            var mainForm = new MainForm(user);
            mainForm.FormClosed += (s, args) => this.Show(); // при закрытии каталога возвращаемся к окну входа
            this.Hide();
            mainForm.Show();
        }
    }
}
