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

        public UserService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userCollection = database.GetCollection<User>(databaseSettings.UserCollections);
            _mapper = mapper;
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
            if (isUsernameExist) return ResponseDto<User>.Fail("Bu username ait kayıt bulunuyor! Lütfen başka bir username ile deneyin!", 400);
            else if (isEmailExist) return ResponseDto<User>.Fail("Bu email adresine bağlı kullanıcı bulunuyor! Lütfen başka bir email adresi ile deneyin!", 400);
            else
            {
                await _userCollection.InsertOneAsync(_user);
                return ResponseDto<User>.Success(_user, 200);
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
            var isLoginUser = CheckLoginByUsernameAndPassword(userLogin.UserName, userLogin.Password);
            if (!isUsernameExist) return ResponseDto<User>.Fail("Kullanıcı bulunamadı.Lütfen üye kaydı oluşturun!", 400);
            else if (!isLoginUser) return ResponseDto<User>.Fail("Kullanıcı adı ya da şifre yanlış.Kontrol ediniz!", 400);
            else return ResponseDto<User>.Success(_user, 200);
        }
    }
}
