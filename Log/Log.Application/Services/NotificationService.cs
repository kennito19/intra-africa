using Log.Application.IRepositories;
using Log.Application.IServices;
using Log.Domain;
using Log.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task<BaseResponse<long>> Create(Notification notification)
        {
            var response = _notificationRepository.Create(notification);
            return response;
        }

        public Task<BaseResponse<List<Notification>>> get(Notification notification, int PageIndex, int PageSize, string Mode)
        {
            var response = _notificationRepository.get(notification, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<long>> Update(Notification notification)
        {
            var response = _notificationRepository.Update(notification);
            return response;
        }
    }
}
