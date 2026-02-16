using Log.Application.IRepositories;
using Log.Application.IServices;
using Log.Domain;
using Log.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;

        public ActivityLogService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        public Task<BaseResponse<long>> Create(ActivityLog activityLog)
        {
            var response = _activityLogRepository.Create(activityLog);
            return response;
        }

        public Task<BaseResponse<List<ActivityLog>>> get(ActivityLog activityLog, int PageIndex, int PageSize, string Mode)
        {
            var response = _activityLogRepository.get(activityLog, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
