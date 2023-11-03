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
            var expect = ("", "����������� ������ �������");

            var actual = auth.CheckData("user@example.com", "������123$", "������123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void ValidPhoneInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("", "����������� ������ �������");

            var actual = auth.CheckData("+79534245674", "������123$", "������123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void EmptyLoginInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����� ������ �����");

            var actual = auth.CheckData("", "������123", "������123", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
       
        [Test]
        public void ShortLoginInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����� ������ ������ ���� ������� 5 ��������");

            var actual = auth.CheckData("abc", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void InvalidPasswordInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "������ ������ ��������� ���� �� ���� ����� � ������� ��������");

            var actual = auth.CheckData("valid_username", "������123$", "������123$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void PasswordWithoutDigitsInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "������ ������ ��������� ���� �� ���� �����");

            var actual = auth.CheckData("valid_username", "�������������$", "�������������$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void PasswordWithoutSpecialSymbolInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "������ ������ ��������� ���� �� ���� ����������");

            var actual = auth.CheckData("valid_username", "����������������1", "����������������1", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void ShortPasswordInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "������ ������ 7-�� ��������");

            var actual = auth.CheckData("valid_username", "���1$", "���1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailNoDomainInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����������� .");

            var actual = auth.CheckData("invalid_email@", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailIncorrectSequenceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "�������� ������������������ �������� �����");

            var actual = auth.CheckData("invalid.email@com", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailContainsSpaceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����� �� ������ ��������� ��������");

            var actual = auth.CheckData("invalid email@example.com", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailCyrillicCharactersInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����� ������ ��������� ������ ��������");

            var actual = auth.CheckData("�������������@�����.com", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void ValidPasswordWithSpaceInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("", "����������� ������ �������");

            var actual = auth.CheckData("valid_username", "������1$ ", "������1$ ", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }
        [Test]
        public void PhoneLetterInput()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "����� �������� �� ������ ��������� ����");

            var actual = auth.CheckData("+794373247n", "������1$", "������1$", out maskPass, out maskRepeatPass);

            Assert.AreEqual(expect, actual);
        }

        [Test]
        public void InvalidEmailIncorrectCharacterSequence()
        {
            string maskPass = "";
            string maskRepeatPass = "";
            var auth = new CheckAuth();
            var expect = ("������", "�������� ������������������ �������� �����");

            var actual = auth.CheckData("user.mer@ci", "������1$", "������1$", out maskPass, out maskRepeatPass);
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
            var expect = ("������", "����� �� ������ ��������� ����� ����������");


            var actual = auth.CheckData(login, "������1$", "������1$", out maskPass, out maskRepeatPass);
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
            var expect = ("������", "����� �� ������ ��������� ��������");


            var actual = auth.CheckData(login, "������1$", "������1$", out maskPass, out maskRepeatPass);


            Assert.AreEqual(expect, actual);
        }

    }
}