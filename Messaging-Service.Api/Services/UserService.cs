using AutoMapper;
using Messaging_Service.Api.Settings;
using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        private readonly IUserActivityLogService _userActivityLogService;



        public UserService(IDatabaseSettings databaseSettings, IMapper mapper, ITokenServices tokenServices, IUserActivityLogService userActivityLogService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<User>(databaseSettings.UserCollections);
            _mapper = mapper;
            _tokenServices = tokenServices;
            _userActivityLogService = userActivityLogService;
        }

        public async Task<List<User>> GetAll()
        {
            return await _userCollection.Find(user => true).ToListAsync();
        }

        public async Task<ResponseDto<User>> RegisterAsync(UserDto registerUser)
        {
            var _user = _mapper.Map<User>(registerUser);
            var isUsernameExist = CheckByUserName(registerUser.UserName);
            var isEmailExist = CheckByEmail(registerUser.Email);
            if (isUsernameExist) return ResponseDto<User>.Fail("There is a user account by the username! Please try with another username!", 406);
            else if (isEmailExist) return ResponseDto<User>.Fail("There is a user account by the email! Please try with another email!", 406);
            else
            {
                await _userCollection.InsertOneAsync(_user);
                return ResponseDto<User>.Success(_user, "Created", 201);
            }
        }
        public bool CheckByUserName(string userName)
        {
            var user = _userCollection.Find<User>(x => x.UserName == userName).FirstOrDefault();
            if (user != null) return true;
            return false;
        }
        public bool CheckByEmail(string email)
        {
            var user = _userCollection.Find<User>(x => x.Email == email).FirstOrDefault();
            if (user != null) return true;
            return false;
        }
        public bool CheckLoginByUsernameAndPassword(string userName, string password)
        {
            var user = _userCollection.Find<User>(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (user != null) return true;
            return false;
        }
        
        public async Task<ResponseDto<User>> LoginAsync(UserLoginDto userLogin)
        {
            var _user = _mapper.Map<User>(userLogin);
            var isUsernameExist = CheckByUserName(userLogin.UserName);
            var userAccount = CheckUser(userLogin.UserName, userLogin.Password);
            if (!isUsernameExist) return ResponseDto<User>.Fail("User not found. Please register!", 401);
            else if (userAccount == null)
            {
                var user = GetUserByUsername(_user.UserName);
                var activityLog = new UserActivityLogDto
                {
                    UserEmail = user.Email,
                    ProcessName = "Login",
                    Description = "Fail login",
                    IsSuccess = false
                };
                return ResponseDto<User>.Fail("Username or password is incorrect. Please check!", 400);
            }
            else
            {
                var token = _tokenServices.CreateToken(new CreateTokenDto { UserName = userLogin.UserName });

                var activityLog = new UserActivityLogDto
                {
                    UserEmail = userAccount.Email,
                    ProcessName = "Login",
                    Description = "successfull login",
                    IsSuccess = true
                };
                await _userActivityLogService.AddLogForUser(activityLog);
            }
            return ResponseDto<User>.Success(_user,"OK", 200);
        }
        public User CheckUser(string userName, string password)
        {
            var user = _userCollection.Find<User>(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (user != null) return null;
            return user;
        }
        public User GetUserByUsername(string userName)
        {
            var user = _userCollection.Find<User>(x => x.UserName == userName).FirstOrDefault();
            if (user != null) return null;
            return user;
        }
    }
}
