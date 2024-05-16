using Microsoft.VisualStudio.TestTools.UnitTesting;
using PraktikaActivity;
using System;

namespace PrakUnitTest
{
    [TestClass]
    public class PhoneNumberValidationTestв
    {
        [TestMethod]
        public void TestValidPhoneNumber()
        {
            string validPhoneNumber = "+7(999)-999-99-99";

            bool isPhoneNumberValid = RgistrationOfJuryModerator.IsPhoneNumberValid(validPhoneNumber);

            Assert.IsTrue(isPhoneNumberValid);
        }

        [TestMethod]
        public void TestInvalidPhoneNumberWithIncorrectFormat()
        {
            string invalidPhoneNumber = "+79999999999";

            bool isPhoneNumberValid = RgistrationOfJuryModerator.IsPhoneNumberValid(invalidPhoneNumber);

            Assert.IsFalse(isPhoneNumberValid);
        }

        [TestMethod]
        public void TestInvalidPhoneNumberWithMissingPrefix()
        {
            string invalidPhoneNumber = "(999)-999-99-99";

            bool isPhoneNumberValid = RgistrationOfJuryModerator.IsPhoneNumberValid(invalidPhoneNumber);

            Assert.IsFalse(isPhoneNumberValid);
        }

        [TestMethod]
        public void TestEmptyPhoneNumber()
        {
            string invalidPhoneNumber = "";

            bool isPhoneNumberValid = RgistrationOfJuryModerator.IsPhoneNumberValid(invalidPhoneNumber);

            Assert.IsFalse(isPhoneNumberValid);
        }
    }
}
