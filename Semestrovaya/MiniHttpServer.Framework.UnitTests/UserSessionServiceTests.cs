using MiniHttpServer.Services.Authentication;

namespace MiniHttpServer.Framework.UnitTests
{
    [TestClass]
    public class UserSessionServiceTests
    {
        [TestMethod]
        public void CreateSession_ReturnCorrectId()
        {
            var service = new UserSessionService();
            string sessionId = service.CreateSession(1);

            Assert.IsNotNull(sessionId);
            Assert.IsTrue(sessionId.Length > 0);
        }

        [TestMethod]
        public void GetUserId_CorrectSession_ReturnUserId()
        {
            var service = new UserSessionService();
            string sessionId = service.CreateSession(2);

            int? id = service.GetUserId(sessionId);

            Assert.AreEqual(2, id);
        }

        [TestMethod]
        public void GetUserId_MissingInvalidSession_ReturnNull()
        {
            var service = new UserSessionService();

            Assert.IsNull(service.GetUserId(null));
            Assert.IsNull(service.GetUserId(""));
        }

        [TestMethod]
        public void GetUserId_ExpiredSession_ReturnNull()
        {
            var service = new UserSessionService(TimeSpan.FromMilliseconds(10));

            string sessionId = service.CreateSession(3);

            Thread.Sleep(20);

            int? userId = service.GetUserId(sessionId);
            Assert.IsNull(userId);
        }

        [TestMethod]
        public void CreateMultiSessions_ReturnUniqueIds()
        {
            var service = new UserSessionService();
            string firstSessionId = service.CreateSession(4);
            string secondSessionId = service.CreateSession(5);

            Assert.AreNotEqual(firstSessionId, secondSessionId);
        }

        [TestMethod]
        public void RemoveDeleleSession()
        {
            var service = new UserSessionService();
            string sessionId = service.CreateSession(6);

            service.RemoveSession(sessionId);

            int? userId = service.GetUserId(sessionId);
            Assert.IsNull(userId);
        }
    }
}
