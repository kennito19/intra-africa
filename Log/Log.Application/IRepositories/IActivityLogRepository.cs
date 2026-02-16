using Log.Domain;
using Log.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.IRepositories
{
    public interface IActivityLogRepository
    {
        Task<BaseResponse<long>> Create(ActivityLog activityLog);

        Task<BaseResponse<List<ActivityLog>>> get(ActivityLog activityLog, int PageIndex, int PageSize, string Mode);

    }
}
