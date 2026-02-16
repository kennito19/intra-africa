using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServerApplication.IRepositories;
using IDServerApplication.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IDServerApplication.Services
{
    public class PageRolesServices : IPageRolesServices
    {
        private readonly IPageRoleRepository _pageRoleRepository;
        public PageRolesServices(IPageRoleRepository pageRoleRepository)
        {
            _pageRoleRepository = pageRoleRepository;
        }

        public BaseResponse<string> AssignPage(AssignPageRole model)
        {
            var dataresponse = _pageRoleRepository.AssignPage(model);
            return dataresponse;
        }

        public BaseResponse<string> GetAssignedPagesByRoleType(int RoleTypeId)
        {
            var dataresponse = _pageRoleRepository.GetAssignedPagesByRoleType(RoleTypeId);
            return dataresponse;
        }

        public async Task<BaseResponse<string>> GetAssignedPagesByUserId(string UserId)
        {
            var dataresponse = await _pageRoleRepository.GetAssignedPagesByUserId(UserId);
            return dataresponse;
        }

        public BaseResponse<string> CreateModule(PageRoleModule module)
        {
            var dataresponse = _pageRoleRepository.CreateModule(module);
            return dataresponse;
        }

        public BaseResponse<string> EditModule(PageRoleModule module)
        {
            var dataresponse = _pageRoleRepository.EditModule(module);
            return dataresponse;
        }

        public BaseResponse<string> DeleteModule(int moduleId)
        {
            var dataresponse = _pageRoleRepository.DeleteModule(moduleId);
            return dataresponse;
        }

        public BaseResponse<List<PageRoleModule>> GetPageModuleById(int moduleId)
        {
            var dataresponse = _pageRoleRepository.GetPageModuleById(moduleId);
            return dataresponse;
        }

        public BaseResponse<string> CreateRoleType(RoleType model)
        {
            var dataresponse = _pageRoleRepository.CreateRoleType(model); 
            return dataresponse;
        }

        public BaseResponse<string> DeleteRoleType(int Id)
        {
            var dataresponse = _pageRoleRepository.DeleteRoleType(Id); 
            return dataresponse;
        }

        public BaseResponse<string> EditRoleType(RoleType model)
        {
            var dataresponse = _pageRoleRepository.EditRoleType(model);
            return dataresponse;
        }

        public BaseResponse<List<PageRoleModule>> GetPages(int pageIndex, int pageSize, string? searchString)
        {
            var dataresponse = _pageRoleRepository.GetPages(pageIndex, pageSize, searchString);
            return dataresponse;
        }

        public BaseResponse<List<RoleType>> GetRoleTypes(int pageIndex, int pageSize)
        {
            var dataresponse = _pageRoleRepository.GetRoleTypes(pageIndex, pageSize); 
            return dataresponse;
        }

        public BaseResponse<List<RoleType>> GetRoleTypeById(int Id)
        {
            var dataresponse = _pageRoleRepository.GetRoleTypeById(Id);
            return dataresponse;
        }

        public BaseResponse<UserTypeFormModel> GetUserTypeWithPageAccess(int Id)
        {
            var dataresponse = _pageRoleRepository.GetRoleTypeWithPageAccess(Id); 
            return dataresponse;
        }

        public async Task<BaseResponse<string>> GetAssignedPagesByUserIdandRoleTypeId(string UserId, int RoletypeId)
        {
            var dataresponse = await _pageRoleRepository.GetAssignedPagesByUserIdandRoleTypeId(UserId, RoletypeId);
            return dataresponse;
        }
    }
}
