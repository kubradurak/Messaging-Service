using AutoMapper;
using Messaging_Service.Api.Settings;
using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public class UserActivityLogService : IUserActivityLogService
    {
        private readonly IMongoCollection<UserActivityLog> _userActivityLogCollection;
        private readonly IMapper _mapper;

        public UserActivityLogService(IDatabaseSettings databaseSettings,IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _userActivityLogCollection = database.GetCollection<UserActivityLog>(databaseSettings.UserActivityLogCollections);
            _mapper = mapper;
        }
        public async Task<ResponseDto<UserActivityLog>> AddLogForUser(UserActivityLogDto activityLog)
        {
            var _activityLog = _mapper.Map<UserActivityLog>(activityLog);
             await _userActivityLogCollection.InsertOneAsync(_activityLog);
             return ResponseDto<UserActivityLog>.Success(_activityLog, "Created", 201);
        }

        public async Task<ResponseDto<List<UserActivityLog>>> GetUserActivityLogsAsync(string email)
        {
            var logList = _userActivityLogCollection.Find(x => x.UserEmail == email).ToList();
            var lastFiveLog = logList.OrderByDescending(x => x.CreateDate).Take(5).ToList();
            return ResponseDto<List<UserActivityLog>>.Success(logList, "Success",200);
        }
        public async Task<ResponseDto<UserActivityLog>> DeleteLastLogByEmail(UserActivityLogDto activityLog)
        {
            var _activityLog = _mapper.Map<UserActivityLog>(activityLog);
            var lastLog = _userActivityLogCollection.Find(x => x.UserEmail == _activityLog.UserEmail).ToList().OrderBy(x => x.CreateDate).Take(1).First();
            await _userActivityLogCollection.DeleteOneAsync(x => x.Id == lastLog.Id);

            return ResponseDto<UserActivityLog>.Success(lastLog, "OK", 200);
        }
    }
}
