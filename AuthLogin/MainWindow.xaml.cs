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

        private void ValidateLogin(string login)
        {
            {
                if (login.Length < 5)
                {
                    throw new Exception("Логин должен содержать минимум 5 символов.");
                }

                // Проверка формата телефона
                if (Regex.IsMatch(login, @"^\+\d{1,3}-\d{1,3}-\d{1,4}-\d{1,4}$"))
                {
                    // Формат телефона верен
                    return;
                }

                // Проверка формата email
                if (Regex.IsMatch(login, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"))
                {
                    // Формат email верен
                    return;
                }

                // Если не соответствует ни формату телефона, ни формату email, можно считать это строкой
                if (Regex.IsMatch(login, @"^[a-zA-Z0-9_]+$"))
                {
                    // Формат строки верен
                    return;
                }

                // Если ни один из форматов не соответствует, генерируем исключение
                throw new Exception("Некорректный формат логина. Допустимый формат: +x-xxx-xxx-xxxx, xxx@xxx.xxx или строка символов.");
            }
        }

        private void ValidatePassword(string password)
        {
            if (password.Length < 7 || !Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=]).+$"))
            {
                throw new Exception("Пароль должен содержать минимум 7 символов и включать хотя бы одну букву в верхнем и нижнем регистре, одну цифру и один спецсимвол (@#$%^&+=).");
            }
        }

        private void ValidatePasswordMatch(string password, string repeatPassword)
        {
            if (password != repeatPassword)
            {
                throw new Exception("Пароль и подтверждение пароля не совпадают.");
            }
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
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string login = userLogin.Text;
            string password = userPass.Password;
            string repeatPassword = repeatPass.Password;
            string passcrypt = Encrypt(password);
            string repeatcrypt = Encrypt(repeatPassword);
            try
            {
               ValidateLogin(login);
               ValidatePassword(password);
               ValidatePasswordMatch(password, repeatPassword);
               MessageBox.Show("Регистрация успешно завершена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
               Log.Information("Успешная регистрация: Логин={login}, Маскированный Пароль={password}, Маскированное Подтверждение Пароля={repeatPassword}",
                login, passcrypt, repeatcrypt);
            }
            catch (Exception ex)
            {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Log.Information("Ошибка регистрации: Логин={login}, Маскированный Пароль={password}, Маскированное Подтверждение Пароля={repeatPassword}",
           login, passcrypt, repeatcrypt);
            }
            
        }

        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
}
