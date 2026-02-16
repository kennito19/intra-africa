using IDServer.Domain.DTO;
using IDServer.Domain.Entity;

namespace IDServerApplication.IRepositories
{
    public interface IPageRoleRepository
    {
        BaseResponse<string> CreateModule(PageRoleModule module);

        BaseResponse<string> EditModule(PageRoleModule module);
        BaseResponse<string> DeleteModule(int Id);

        BaseResponse<List<PageRoleModule>> GetPageModuleById(int Id);

        BaseResponse<List<RoleType>> GetRoleTypeById(int Id);
        BaseResponse<string> CreateRoleType(RoleType model);
        BaseResponse<string> EditRoleType(RoleType model);
        BaseResponse<string> DeleteRoleType(int Id);

        BaseResponse<List<RoleType>> GetRoleTypes(int pageIndex, int pageSize);
        BaseResponse<UserTypeFormModel> GetRoleTypeWithPageAccess(int Id);

        UserTypeFormModel GetRoleTypeWithPageAccess(int Id, bool isClass);
        BaseResponse<string> AssignPage(AssignPageRole model);

        BaseResponse<string> GetAssignedPagesByRoleType(int RoleTypeId);

        Task<BaseResponse<string>> GetAssignedPagesByUserId(string UserId);

        Task<BaseResponse<string>> GetAssignedPagesByUserIdandRoleTypeId(string UserId, int RoletypeId);

        UserTypeFormModel GetRoleTypeWithPageAccesswithUserSpecific(string UserId, int Id, bool isClass);
        BaseResponse<List<PageRoleModule>> GetPages(int pageIndex, int pageSize, string? searchString);
    }
}