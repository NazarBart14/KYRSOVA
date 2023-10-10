using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KYRSOVA
{
    public partial class RegistrationWindow : Window
    {
        
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Registred;Integrated Security=True";

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async Task InsertUserAsync(string username, string password)
        {
            try
            {
                // Хешуємо пароль перед збереженням його в базі даних
                string hashedPassword = HashPassword(password);

                using (SqlConnection connection = new SqlConnection(connectionString)) // Підключаємося до бази даних
                {
                    await connection.OpenAsync();

                    string query = "INSERT INTO Regist (Username, Password) VALUES (@Username, @Password)"; // Запит на вставку даних

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", hashedPassword); // Зберігаємо хеш паролю

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Помилка при вставці даних: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            // Використовуємо SHA256 для хешування паролю
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Перетворюємо байти в рядок для збереження в базі даних
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введіть email і пароль!");
            }
            else
            {
                await InsertUserAsync(username, password); // Викликаємо метод вставки даних

                MessageBox.Show("Реєстрація успішна!");
            }
        }

    }
}
