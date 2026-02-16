using Log.Domain;
using Log.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.IServices
{
    public interface INotificationService
    {
        Task<BaseResponse<long>> Create(Notification notification);
        Task<BaseResponse<long>> Update(Notification notification);
        Task<BaseResponse<List<Notification>>> get(Notification notification, int PageIndex, int PageSize, string Mode);
    }
}
