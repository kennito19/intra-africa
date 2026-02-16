using Log.Domain.Entity;
using Log.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.IRepositories
{
    public interface INotificationRepository
    {
        Task<BaseResponse<long>> Create(Notification notification);
        Task<BaseResponse<long>> Update(Notification notification);
        Task<BaseResponse<List<Notification>>> get(Notification notification, int PageIndex, int PageSize, string Mode);
    }
}
