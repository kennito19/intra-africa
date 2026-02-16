using Log.Domain.Entity;
using Log.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.IServices
{
    public interface IActivityLogService
    {
        Task<BaseResponse<long>> Create(ActivityLog activityLog);

        Task<BaseResponse<List<ActivityLog>>> get(ActivityLog activityLog, int PageIndex, int PageSize, string Mode);

    }
}
