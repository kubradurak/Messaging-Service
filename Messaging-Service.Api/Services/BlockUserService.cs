using AutoMapper;
using Messaging_Service.Api.Settings;
using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public class BlockUserService : IBlockUserService
    {
        private readonly IMongoCollection<BlockUser> _blockUserCollection;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public BlockUserService(IDatabaseSettings databaseSettings, IUserService userService, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _blockUserCollection = database.GetCollection<BlockUser>(databaseSettings.BlockUserCollections);
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<BlockUser>> BlockUserAsync(BlockUser user)
        {
            var _user = _mapper.Map<BlockUser>(user);
            var blockedUser = _userService.CheckByUserName(_user.BlockedUserName);
            if (!blockedUser) return ResponseDto<BlockUser>.Fail("BlockUserService.BlockUserAsync(): Username is not valid. Check the username!", 404);
            _user.IsLocked = true;
            await _blockUserCollection.InsertOneAsync(_user);

            return ResponseDto<BlockUser>.Success(_user,"Created", 201);
        }
        public async Task<ResponseDto<BlockUser>> RemoveBlockUserAsync(BlockUser user)
        {
            var _user = _mapper.Map<BlockUser>(user);
            var blockedUser = _userService.CheckByUserName(_user.BlockedUserName);
            if (!blockedUser) return ResponseDto<BlockUser>.Fail("BlockUserService.RemoveBlockUserAsync(): Username is not valid. Check the username.", 404);

            var blockUser = CheckBlockedUser(_user);
            await _blockUserCollection.DeleteOneAsync(x => x.Id == blockUser.Id);

            return ResponseDto<BlockUser>.Success(_user,"OK", 200);
        }

        public bool IsBlockedUser(BlockUserDto user)
        {
            var _user = _mapper.Map<BlockUser>(user);
            var result = _blockUserCollection.Find<BlockUser>(x => x.BlockedUserName == user.BlockedUserName
            && x.BlockerUserName == _user.BlockerUserName && x.IsLocked == true).FirstOrDefault();
            if (result != null) return true;
            return false;
        }
        public BlockUser CheckBlockedUser(BlockUser user)
        {
            var result = _blockUserCollection.Find<BlockUser>(x => x.BlockedUserName == user.BlockedUserName
            && x.BlockerUserName == user.BlockerUserName).FirstOrDefault();
            if (result == null) return null;
            return result;
        }
    }
}
