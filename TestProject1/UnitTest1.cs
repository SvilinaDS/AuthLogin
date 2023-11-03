using NUnit.Framework;
using AuthLogin;
namespace TestProject1
{
    public class Tests
    {
        [Test]
        public void ValidEmailInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("", "Регистрация прошла успешно");

            var actual = auth.CheckData("user@example.com", "Пароль123$", "Пароль123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void ValidPhoneInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("", "Регистрация прошла успешно");

            var actual = auth.CheckData("+79534245674", "Пароль123$", "Пароль123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void EmptyLoginInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Задан пустой логин");

            var actual = auth.CheckData("", "Пароль123", "Пароль123", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
       
        [Test]
        public void ShortLoginInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Длина логина должна быть минимум 5 символов");

            var actual = auth.CheckData("abc", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void InvalidPasswordInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Пароль должен содержать хотя бы одну букву в верхнем регистре");

            var actual = auth.CheckData("valid_username", "пароль123$", "пароль123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void PasswordWithoutDigitsInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Пароль должен содержать хотя бы одну цифру");

            var actual = auth.CheckData("valid_username", "Парольбезцифр$", "Парольбезцифр$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void PasswordWithoutSpecialSymbolInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Пароль должен содержать хотя бы один спецсимвол");

            var actual = auth.CheckData("valid_username", "Парольбезсимвола1", "Парольбезсимвола1", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void ShortPasswordInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Пароль меньше 7-ми символов");

            var actual = auth.CheckData("valid_username", "Пар1$", "Пар1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailNoDomainInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Отсутствует .");

            var actual = auth.CheckData("invalid_email@", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailIncorrectSequenceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Неверная последовательность символов почты");

            var actual = auth.CheckData("invalid.email@com", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailContainsSpaceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Логин не должен содержать пробелов");

            var actual = auth.CheckData("invalid email@example.com", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailCyrillicCharactersInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Логин должен содержать только латиницу");

            var actual = auth.CheckData("кириллическая@почта.com", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void ValidPasswordWithSpaceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("", "Регистрация прошла успешно");

            var actual = auth.CheckData("valid_username", "Пароль1$ ", "Пароль1$ ", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void PhoneLetterInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Номер телефона не должен содержать букв");

            var actual = auth.CheckData("+794373247n", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailIncorrectCharacterSequence()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Неверная последовательность символов почты");

            var actual = auth.CheckData("user.mer@ci", "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);
            Assert.AreEqual(expect, actual);
        }

        [TestCase("+7971423519.")]
        [TestCase("user@!example.com")]
        [TestCase("login.")]
        public void Test4(string login)
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Логин не должен содержать знаки препинания");


            var actual = auth.CheckData(login, "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);
            Assert.AreEqual(expect, actual);
        }

        [TestCase("+7 953 234 11 00")]
        [TestCase("user @example.com")]
        [TestCase("u ser")]
        public void Test5(string login)
        {

            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("Ошибка", "Логин не должен содержать пробелов");


            var actual = auth.CheckData(login, "Пароль1$", "Пароль1$", out maskPass, out maskRepeatPass);


            Assert.AreEqual(expect, actual);
        }

    }
}