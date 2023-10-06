using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Serilog;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web.Security;

namespace AuthLogin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.File("C:/Users/User/source/repos/AuthLogin/logs/log.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public static string Encrypt(string value)
        {
            using(MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }

        private List<string> errorMessages = new List<string>();


        public string ValidateInput(string login, string password, string repeatPassword)
        {
            string passcrypt = Encrypt(password);

            try
            {
                if (login.Length < 5)
                {
                    errorMessages.Add("Логин должен содержать минимум 5 символов.");
                }

                if (!Regex.IsMatch(login, @"^\+\d{1,3}-\d{1,3}-\d{1,4}-\d{1,4}$") &&
                    !Regex.IsMatch(login, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$") &&
                    !Regex.IsMatch(login, @"^[a-zA-Z0-9_]+$"))
                {
                    errorMessages.Add("Некорректный формат логина. Допустимый формат: +x-xxx-xxx-xxxx, xxx@xxx.xxx или строка символов.");
                }

                if (password.Length < 7 || !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).+$"))
                {
                    errorMessages.Add("Пароль должен содержать минимум 7 символов и включать хотя бы одну букву в верхнем и нижнем регистре, одну цифру и один спецсимвол (@#$%^&+=).");
                }

                if (password != repeatPassword)
                {
                    errorMessages.Add("Пароль и подтверждение пароля не совпадают.");
                }

                return passcrypt;
            }
            catch (Exception ex)
            {
                Log.Information("Ошибка регистрации: Логин={login}, Маскированный Пароль={password}", login, passcrypt);
                return null; 
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string login = userLogin.Text;
            string password = userPass.Password;
            string repeatPassword = repeatPass.Password;

            errorMessages.Clear();

            string passcrypt = ValidateInput(login, password, repeatPassword);

            if (errorMessages.Count == 0)
            {
                MessageBox.Show("Регистрация успешно завершена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Log.Information("Успешная регистрация: Логин={login}, Маскированный Пароль={password}", login, passcrypt);
            }
            else
            {
                string errorMessage = "Ошибка валидации:\n" + string.Join("\n", errorMessages);
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
}
