using Microsoft.VisualStudio.TestTools.UnitTesting;
using PraktikaActivity;
using System;

namespace PrakUnitTest
{
    [TestClass]
    public class AuthorizationTest
    {
        [TestMethod]
        public void TestSuccessfulAuthorization()
        {
            int userId = 1;
            string password = "Test1!";

            Users user = Authorization.Authorize(userId, password);

            Assert.IsNotNull(user);
        }
        [TestMethod]
        public void TestUnsuccessfulAuthorizationWithInvalidPassword()
        {
            int userId = 1;
            string password = "Test1!,,,,";

            Users user = Authorization.Authorize(userId, password);

            Assert.IsNull(user);
        }
        [TestMethod]
        public void TestUnsuccessfulAuthorizationWithNonexistentUser()
        {
            int userId = 1000;
            string password = "Test1!";

            Users user = Authorization.Authorize(userId, password);

            Assert.IsNull(user);
        }
        [TestMethod]
        public void TestParticipantAuthorization()
        {
            int userId = 1;
            string password = "Test1!";

            Users user = Authorization.Authorize(userId, password);

            Assert.AreEqual(user.RoleId, 1);
        }

        [TestMethod]
        public void TestModeratorAuthorization()
        {
            int userId = 24;
            string password = "Yvm2tF73C3";

            Users user = Authorization.Authorize(userId, password);

            Assert.AreEqual(user.RoleId, 2);
        }

        [TestMethod]
        public void TestJuryAuthorization()
        {
            int userId = 49;
            string password = "Mou5kd";

            Users user = Authorization.Authorize(userId, password);

            Assert.AreEqual(user.RoleId, 3);
        }

        [TestMethod]
        public void TestOrganizerAuthorization()
        {
            int userId = 58;
            string password = "9NfgC82Z3i";

            Users user = Authorization.Authorize(userId, password);

            Assert.AreEqual(user.RoleId, 4);
        }
    }
}
