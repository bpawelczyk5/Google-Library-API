using NUnit.Framework;
using System.Windows;

namespace WpfApp2Test1
{
    [TestFixture]
    public class Tests
    {
        public bool IsPasswordValidTest(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool UpperCase = false;
            bool LowerCase = false;
            bool Digit = false;
            bool SpecialChar = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    UpperCase = true;
                }
                else if (char.IsLower(c))
                {
                    LowerCase = true;
                }
                else if (char.IsDigit(c))
                {
                    Digit = true;
                }
                else if (char.IsPunctuation(c) || char.IsSymbol(c))
                {
                    SpecialChar = true;
                }
            }

            return UpperCase && LowerCase && Digit && SpecialChar;
        }

        [Test]
        [Apartment(ApartmentState.STA)]
        public void IsPasswordValid_ValidPasswordTest_ReturnsTrue()
        {
            // Arrange
            var password = "Abcd123!"; // A valid password with uppercase, lowercase, digit, and special character

            // Act
            var result = IsPasswordValidTest(password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsPasswordValid_InvalidPasswordTest_ReturnsFalse()
        {
            // Arrange

            var password = "invalidpassword"; // An invalid password without uppercase

            // Act
            var result = IsPasswordValidTest(password);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
