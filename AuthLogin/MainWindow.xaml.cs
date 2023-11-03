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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string login = userLogin.Text;
            string password = userPass.Password;
            string repeatPassword = repeatPass.Password;
            string maskedPass = "";
            string maskRepeat = "";

            var auth = new CheckAuth();

                (string result, string message) = auth.CheckData(login, password, repeatPassword, out maskedPass, out maskRepeat);
            MessageBox.Show(message,result);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    
}
