using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace AuthLogin
{
    public class CheckAuth
    {
        public static string Encrypt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }
        public (string, string) CheckData(string login, string password, string repeatPassword, out string maskedPass, out string maskedRepeat)
        {
            maskedPass = Encrypt(password);
            maskedRepeat = Encrypt(repeatPassword);
            string message = " ";

            if (login.Length == 0)
            {
                message = "Задан пустой логин";
                Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                return ("Ошибка", "Задан пустой логин");
            }
            else
            {
                if (login.Contains("+"))
                {
                    if (login.Any(c => char.IsLetter(c)))
                    {
                        message = "Номер телефона не может содержать букв";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Номер телефона не должен содержать букв");
                    }
                    else if (login.Substring(1).Any(c => char.IsSymbol(c)))
                    {
                        message = "Номер телефона не может содержать символов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не  должен содержать символов");

                    }
                    else if (login.Any(c => char.IsPunctuation(c)))
                    {
                        message = "Номер телефона не может содержать знаки препинания";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать знаки препинания");
                    }
                    else if (login.Any(c => char.IsWhiteSpace(c)))
                    {
                        message = "Номер телефона не должен содержать пробелов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать пробелов");
                    }
                    else if (login.Length > 0 && login.Length - 1 < 11)
                    {
                        message = "Номер должен состоять из 11 цифр. Количество цифр меньше 11";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Номер должен состоять из 11 цифр. Количество цифр меньше 11");
                    }

                    else if (login.Length - 1 > 11)
                    {
                        message = "Номер должен состоять из 11 цифр. Количество цифр больше 11";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Номер должен состоять из 11 цифр. Количество цифр больше 11");
                    }


                }
                else if (login.Contains("@"))
                {
                    if (!login.Contains("."))
                    {
                        message = "Отсутствует .";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Отсутствует .");

                    }
                    else if (login.Last() == '.')
                    {
                        message = "Отсутствует домен";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Отсутствует домен");
                    }
                    else if (login.IndexOf('.') < login.IndexOf('@'))
                    {
                        message = "Неверная последовательность символов почты";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Неверная последовательность символов почты");
                    }
                    
                    else if (login.Any(c => char.IsWhiteSpace(c)))
                    {
                        message = "Логин не должен содержать пробелов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать пробелов");
                    }
                    else if (login.Any(x => char.IsLetter(x) && x >= 1072 && x <= 1103))
                    {
                        message = "Почта должна содержать только латиницу";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин должен содержать только латиницу");
                    }

                    else if (login.Any(c => char.IsSymbol(c)))
                    {
                        message = "Почта не может содержать символов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать символов");
                    }
                    else if (login.Any(c => char.IsPunctuation(c) && c != '.' && c != '@'))
                    {
                        message = "Почта не может содержать знаков препинания, только '.' и '@'";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать знаки препинания");
                    }

                }
                else if (!login.Contains('@') && !login.Contains('+'))
                {
                    if (login.Length < 5 && login.Length != 0)
                    {
                        message = "Длина логина должна быть минимум 5 символов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Длина логина должна быть минимум 5 символов");
                    }

                    else if (login.Any(x => char.IsLetter(x) && (x >= 1072 && x <= 1103) || (x >= 1040 && x <= 1071)))
                    {
                        message = "Логин должен содержать только латиницу";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин должен содержать только латиницу");
                    }

                    else if (login.Any(c => char.IsSymbol(c)))
                    {
                        message = "Логин не может содержать символов, только '_'";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать символов");

                    }
                    else if (login.Any(c => char.IsPunctuation(c) && c != '_'))
                    {
                        message = "Логин не может содержать знаков препинания, только '_'";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать знаки препинания");
                    }
                    else if (login.Any(c => char.IsWhiteSpace(c)))
                    {
                        message = "Вводите логин без пробелов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Логин не должен содержать пробелов");
                    }

                }

                if (password.Length == 0)
                {
                    message = "Задан пустой пароль";
                    Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                    return ("Ошибка", "Задан пустой пароль");
                }
                else
                {
                    if (password.Length > 0 && password.Length < 7)
                    {
                        message = "Пароль должен иметь минимум 7 символов";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль меньше 7-ми символов");
                    }
                    else if (password.Any(x => char.IsLetter(x) && (x >= 97 && x <= 122) || (x >= 65 && x <= 90)))
                    {
                        message = "Пароль должен содержать только кириллицу";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль должен содержать только кириллицу");
                    }
                    else if (!password.Any(c => char.IsDigit(c)))
                    {
                        message = "Пароль должен содержать хотя бы одну цифру";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль должен содержать хотя бы одну цифру");
                    }
                    else if (!password.Any(c => char.IsSymbol(c)))
                    {
                        message = "Пароль должен содержать хотя бы один спецсимвол";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль должен содержать хотя бы один спецсимвол");
                    }
                    else if (!password.Any(x => char.IsLetter(x) && x >= 1072 && x <= 1103))
                    {
                        message = "Пароль должен содержать хотя бы одну букву в нижнем регистре";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль должен содержать хотя бы одну букву в нижнем регистре");
                    }
                    else if (!password.Any(x => char.IsLetter(x) && x >= 1040 && x <= 1071))
                    {
                        message = "Пароль должен содержать хотя бы одну букву в верхнем регистре";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароль должен содержать хотя бы одну букву в верхнем регистре");
                    }
                    else if (repeatPassword != password)
                    {
                        message = "Пароли не совпадают";
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, {3}!", login, maskedPass, maskedRepeat, message);
                        return ("Ошибка", "Пароли не совпадают");
                    }

                    else
                    {
                        Log.Information("Логин: {0}, Пароль: {1}, Повторный пароль: {2}, Успешная регистрация!", login, maskedPass, maskedRepeat, message);
                        message = "Успешная регистрация";
                        return ("", "Регистрация прошла успешно");
                    }
                }

            }
        }
    }
}
