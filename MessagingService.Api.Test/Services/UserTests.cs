using Microsoft.VisualStudio.TestTools.UnitTesting;
using Messaging_Service.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingService.Entities.Dtos;
using MongoDB.Driver.Core.Misc;
using AutoFixture;

namespace Messaging_Service.Api.Services.Tests
{
    [TestClass()]
    public class UserTests
    {
        private    readonly IUserService _userService;
        public UserTests(IUserService userService)
        {
            _userService = userService;
        }
        [TestMethod()]
        public async Task RegisterAsyncTest()
        {

            // arrange
            var fixture = new Fixture();
            var @object = fixture.Build<UserDto>()
                .With(x => x.UserName, "user01")
                .With(x => x.FirstName, "first")
                .With(x => x.LastName, "lastname")
                .With(x => x.Email, "mail@mail.com")
                .With(x => x.Password, "123")
                .Create();

            // act
            var account = await _userService.RegisterAsync(@object);


            // assert
            Assert.AreEqual(account, account);
            //Assert.Fail();
        }
    }
}