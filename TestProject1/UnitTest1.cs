using BankSystemWebApplication1;
using BankSystemWebApplication1.Account;
using BankSystemWebApplication1.Controllers;
using BankSystemWebApplication1.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;


namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<ApiContext> _apiContext;
        private ILogger<UserController> _logger;
        private UserController _controlller;

        public UnitTest1()
        {
            _apiContext = new Mock<ApiContext>();
            //_logger = new Logger<UserController>();
            _controlller = new UserController((IUserRespository)new UserRepository());
           
        }
        [DataTestMethod]
        [DataRow(null, null, null, 0)]
        [DataRow("Anfernee", "Hardaway", "Little Penny", 1)]
        public void CreateUser(string firstName,string lastName, string username, int balance)
        {
            

            var users = _controlller.CreateUser(new User() {FirstName = firstName, LastName = lastName, UserName = username, Accounts = new List<Account>() { new Account() { Balance = balance } } });
            
            User user = (((ObjectResult)users.Result).Value as User);
            Assert.AreEqual(200, ((ObjectResult)users.Result).StatusCode);
            Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value).FirstName);
            Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value).LastName);
            Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value).UserName);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value).Accounts[0].Balance);

        }

        [DataTestMethod]
        [DataRow(null, null, null, 0)]
        [DataRow("Karl", "Malone", "Mail Man", 10000)]
        public void DeleteUser(string firstName, string lastName, string username, int balance)
        {


            var users = _controlller.CreateUser(new User() { FirstName = firstName, LastName = lastName, UserName = username, Accounts = new List<Account>() { new Account() { Balance = balance } } });
            User user = (((ObjectResult)users.Result).Value as User);
            
            
            _controlller.DeleteUser(user.UserId);
            
            Assert.AreEqual(200, ((ObjectResult)users.Result).StatusCode);
            Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value).FirstName);
            Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value).LastName);
            Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value).UserName);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value).Accounts[0].Balance);

        }

        [DataTestMethod]
        [DataRow(true,"Karl", "Malone", "Mail Man", 10000,100)]
        [DataRow(false,"Karl", "Malone", "Mail Man", 10000, 10001)]
        [DataRow(false, "Karl", "Malone", "Mail Man", 10000, -1)]
        public void Deposit(bool descision, string firstName, string lastName, string username, int balance, int deposit)
        {
            var user1 =new User() { FirstName = firstName, LastName = lastName, UserName = username };
            user1.Accounts = new List<Account>();
            user1.Accounts.Add( new Account() { Balance = balance });
            var users = _controlller.CreateUser(user1);
            User user2 = (((ObjectResult)users.Result).Value as User);
            var accountNumber = user2.Accounts[0].AccountNumber;

            var user3 = _controlller.Deposit(user2.UserId,accountNumber, deposit);

            
            
            if (descision)
            {
                Assert.AreEqual(200, (int)((OkObjectResult)users.Result).StatusCode);
                Assert.AreEqual(balance + deposit, ((User)((OkObjectResult)user3?.Result)?.Value).Accounts[0].Balance);
                Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value)?.FirstName);
                Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value)?.LastName);
                Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value)?.UserName);
                Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
                Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
            }
            else
            {
                Assert.AreEqual(200, (int)((OkObjectResult)users.Result).StatusCode);
            }
            
            
        }

        [DataTestMethod]
        [DataRow(true, null, null, null, 0, 0)]
        [DataRow(true, "Karl", "Malone", "Mail Man", 10000, -100)]
        [DataRow(false, "Karl", "Malone", "Mail Man", 100, -1)]
        [DataRow(true, "Karl", "Malone", "Mail Man", 10000, -9000)]
        [DataRow(false, "Karl", "Malone", "Mail Man", 10000, -9001)]
        public void Withdrawl(bool descicion, string firstName, string lastName, string username, int balance, int withdrawl)
        {



            var user1 = new User() { FirstName = firstName, LastName = lastName, UserName = username };
            user1.Accounts = new List<Account>();
            user1.Accounts.Add(new Account() { Balance = balance });
            var users = _controlller.CreateUser(user1);
            User user2 = (((ObjectResult)users.Result).Value as User);
            var accountNumber = user2.Accounts[0].AccountNumber;

            var user3 = _controlller.Withdrawl(user2.UserId, accountNumber, withdrawl);

            if (descicion)
            {
                Assert.AreEqual(balance + withdrawl, ((User)((OkObjectResult)user3?.Result)?.Value).Accounts[0].Balance);
                Assert.AreEqual(200, ((ObjectResult)users.Result).StatusCode);
                Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value).FirstName);
                Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value).LastName);
                Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value).UserName);
                Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value).Accounts[0].Balance);
                Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
                Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
            }
            else
            {
                Assert.AreEqual(200, (int)((OkObjectResult)users.Result).StatusCode);
            }


            
        }


        [DataTestMethod]
        [DataRow(null, null, null, 0, 0)]
        [DataRow("Karl", "Malone", "Mail Man", 10000, -100)]
        public void DeletAccount(string firstName, string lastName, string username, int balance, int withdrawl)
        {



            var user1 = new User() { FirstName = firstName, LastName = lastName, UserName = username };
            user1.Accounts = new List<Account>();
            user1.Accounts.Add(new Account() { Balance = balance });
            var users = _controlller.CreateUser(user1);
            User user2 = (((ObjectResult)users.Result).Value as User);
            var accountNumber = user2.Accounts[0].AccountNumber;

            var user3 = _controlller.DeleteAccount(user2.UserId, accountNumber);

            //Assert.AreEqual(balance + withdrawl, ((User)((OkObjectResult)user3?.Result)?.Value).Accounts[0].Balance);
            Assert.AreEqual(200, ((ObjectResult)users.Result).StatusCode);
            Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value).FirstName);
            Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value).LastName);
            Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value).UserName);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value).Accounts[0].Balance);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
        }

        [DataTestMethod]
        [DataRow(null, null, null, 0, 0)]
        [DataRow("Karl", "Malone", "Mail Man", 10000, 100)]
        public void CreateAccount(string firstName, string lastName, string username, int balance, int withdrawl)
        {



            var user1 = new User() { FirstName = firstName, LastName = lastName, UserName = username };
            user1.Accounts = new List<Account>();
            user1.Accounts.Add(new Account() { Balance = balance });
            var users = _controlller.CreateUser(user1);
            User user2 = (((ObjectResult)users.Result).Value as User);
            //var accountNumber = user2.Accounts[0].AccountNumber;
            var previousCount = user2.Accounts.Count;

            var user3 = _controlller.CreateAccount(user2.UserId);

            //Assert.AreEqual(++previousCount, ((User)((OkObjectResult)user3?.Result)?.Value).Accounts.Count);
            //Assert.AreEqual(balance + withdrawl, ((User)((OkResult)user3?.Result)?.Value).Accounts[0].Balance);
            Assert.AreEqual(200, ((ObjectResult)users.Result).StatusCode);
            Assert.AreEqual(firstName, ((User)((ObjectResult)users.Result).Value).FirstName);
            Assert.AreEqual(lastName, ((User)((ObjectResult)users.Result).Value).LastName);
            Assert.AreEqual(username, ((User)((ObjectResult)users.Result).Value).UserName);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value).Accounts[0].Balance);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
            Assert.AreEqual(balance, ((User)((ObjectResult)users.Result).Value)?.Accounts[0].Balance);
        }

    }
    
}