using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServer.Infrastructure.Data;
using IDServerApplication.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IDServer.Infrastructure.Repositories
{
    public class PageRoleRepository : IPageRoleRepository
    {
        private readonly AspNetIdentityDBContext _dbContext;
        private readonly IConfiguration _configuration;

        private readonly UserManager<Users> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;

        public PageRoleRepository(AspNetIdentityDBContext dBContext, IConfiguration configuration, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dBContext;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public BaseResponse<string> CreateModule(PageRoleModule module)
        {
            var res = _dbContext.PageRoleModules.Add(module);

            var rowsAffected = _dbContext.SaveChanges();
            if (rowsAffected > 0)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Module created Successfully", module.Id);
                return baseResponse;
            }
            else
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                return baseResponse;
            }
        }

        public BaseResponse<string> EditModule(PageRoleModule module)
        {
            _dbContext.PageRoleModules.Update(module);

            var rowsAffected = _dbContext.SaveChanges();
            if (rowsAffected > 0)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Module Updated Successfully");
                return baseResponse;
            }
            else
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                return baseResponse;
            }
        }

        public BaseResponse<string> DeleteModule(int Id)
        {
            var module = _dbContext.PageRoleModules.FirstOrDefault(x => x.Id == Id);

            if (module != null)
            {
                _dbContext.PageRoleModules.Remove(module);

                var rowsAffected = _dbContext.SaveChanges();
                if (rowsAffected > 0)
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Module Deleted Successfully");
                    return baseResponse;
                }
                else
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                    return baseResponse;
                }
            }
            else
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Record Doesn't Exist");
                return baseResponse;
            }

            
        }

        public BaseResponse<string> CreateRoleType(RoleType model)
        {
            if (_dbContext.UserTypes.Where(x => x.Name.ToLower().Equals(model.Name.ToLower())).Any())
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "RoleType Already Exist");
                return baseResponse;
            }
            else
            {
                _dbContext.UserTypes.Add(model);
                var rowsAffected = _dbContext.SaveChanges();
                if (rowsAffected > 0)
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "RoleType Inserted Successfully", model.Id);
                    return baseResponse;
                }
                else
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                    return baseResponse;
                }
            }
            
        }


        public BaseResponse<string> EditRoleType(RoleType model)
        {
            if (_dbContext.UserTypes.Where(x => x.Name.ToLower().Equals(model.Name.ToLower())).Any()){
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "RoleType Already Exist");
                return baseResponse;
            }
            else
            {
                _dbContext.UserTypes.Update(model);
                var rowsAffected = _dbContext.SaveChanges();
                if (rowsAffected > 0)
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "RoleType Updated Successfully",model.Id);
                    return baseResponse;
                }
                else
                {
                    BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                    return baseResponse;
                }
            }
            
        }

        public BaseResponse<string> DeleteRoleType(int Id)
        {
            var userType = _dbContext.UserTypes.Where(x=>x.Id == Id).FirstOrDefault();

            if (userType == null)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(404, "UserType not found");
                return baseResponse;
            }

            // Here we check if the UserType ID is associated with other tables
            bool isAssociatedWithOtherTables = IsUserTypeAssociatedWithOtherTables(userType);
            if (isAssociatedWithOtherTables)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(400, "Cannot delete UserType. It is associated with users tables.");
                return baseResponse;
            }

            _dbContext.UserTypes.Remove(userType);
            var rowsAffected = _dbContext.SaveChanges();
            if (rowsAffected > 0)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "RoleType Deleted Successfully");
                return baseResponse;
            }
            else
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Unknown Error occurred");
                return baseResponse;
            }
        }

        private bool IsUserTypeAssociatedWithOtherTables(RoleType userType)
        {
            bool isAssociatedWithUsers = _dbContext.users.Any(U => U.UserTypeId == userType.Id);
            bool isAssociatedWithAssignPage = _dbContext.AssignPageRoles.Any(U => U.RoleTypeId == userType.Id);
            if (isAssociatedWithUsers || isAssociatedWithAssignPage)
            {
                return true;
            }
            return false;
        }


        public BaseResponse<List<RoleType>> GetRoleTypes(int pageIndex, int pageSize)
        {
            int totalRecordCount = _dbContext.UserTypes.Count();
            Pagination pagination = null;
            List <RoleType> ListofuserType = new List<RoleType>();
            if (pageIndex!=0 || pageSize!=0)
            {
                ListofuserType = _dbContext.UserTypes.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
            }
            else
            {
                ListofuserType = _dbContext.UserTypes.ToList();
            }

            BaseResponse<List<RoleType>> baseResponse = new BaseResponse<List<RoleType>>(200, "Record Bind Successfully", pagination, ListofuserType);
            return baseResponse;

        }

        public BaseResponse<List<RoleType>> GetRoleTypeById(int Id)
        {
            var totalRoles = _dbContext.UserTypes.Where(x=>x.Id==Id).ToList();
           
            if (totalRoles.Any())
            {
                BaseResponse<List<RoleType>> baseResponse = new BaseResponse<List<RoleType>>(200, "Record Bind Successfully", null, totalRoles);
                return baseResponse;
            }
            else
            {
                BaseResponse<List<RoleType>> baseResponse = new BaseResponse<List<RoleType>>(200, "Record Doesn't Exist", null, totalRoles);
                return baseResponse;
            }

            

        }


        public BaseResponse<UserTypeFormModel> GetRoleTypeWithPageAccess(int Id)
        {
            var userType = _dbContext.UserTypes.Where(x=>x.Id == Id).FirstOrDefault();
            var AssignedPages = _dbContext.AssignPageRoles.Where(x =>x.RoleTypeId == Id).Include(b => b.Page).ToList();
            foreach(var item in AssignedPages)
            {
                item.PageRoleName = item.Page.Name;
            }
            UserTypeFormModel response = new UserTypeFormModel();
            response.UserTypeId = userType.Id;
            response.UserTypeName = userType.Name;

            response.pagesAssigned = AssignedPages;


            BaseResponse<UserTypeFormModel> baseResponse = new BaseResponse<UserTypeFormModel>(200, "Record Bind Successfully",null, response);
            return baseResponse;
        }

        public UserTypeFormModel GetRoleTypeWithPageAccess(int Id, bool isClass)
        {
            var userType = _dbContext.UserTypes.Where(x => x.Id == Id).FirstOrDefault();
            var AssignedPages = _dbContext.AssignPageRoles.Where(x => x.RoleTypeId == Id).Include(b => b.Page).ToList();
            foreach (var item in AssignedPages)
            {
                item.PageRoleName = item.Page.Name;
            }
            UserTypeFormModel response = new UserTypeFormModel();
            response.UserTypeId = userType.Id;
            response.UserTypeName = userType.Name;

            response.pagesAssigned = AssignedPages;

            return response;
        }

        public BaseResponse<List<PageRoleModule>> GetPages(int pageIndex, int pageSize, string? searchString = null)
        {
            var ListOfPages = _dbContext.PageRoleModules.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                ListOfPages = ListOfPages.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).OrderByDescending(x => x.Id).ToList();
            }
            ListOfPages = ListOfPages.OrderByDescending(x => x.Id).ToList();
            var totalRecordCount = ListOfPages.Count();
            Pagination pagination = null;
            if (pageIndex != 0 || pageSize != 0)
            {
                ListOfPages = ListOfPages.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
            }
            
            BaseResponse<List<PageRoleModule>> baseResponse = new BaseResponse<List<PageRoleModule>>(200, "Record Bind Successfully", pagination, ListOfPages);
            return baseResponse;
        }

        public BaseResponse<List<PageRoleModule>> GetPageModuleById(int Id)
        {
            var PageCheck = _dbContext.PageRoleModules.Where(x=>x.Id==Id).ToList();
            if (PageCheck.Any())
            {
               
                BaseResponse<List<PageRoleModule>> baseResponse = new BaseResponse<List<PageRoleModule>>(200, "Record Bind Successfully", null, PageCheck);
                return baseResponse;
            }
            else
            {
                BaseResponse<List<PageRoleModule>> baseResponse = new BaseResponse<List<PageRoleModule>>(200, "Record Doesn't Exist");
                return baseResponse;
            }
            
        }

        public BaseResponse<string> AssignPage(AssignPageRole model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");
            var recordExists = _dbContext.AssignPageRoles.Where(x => x.RoleTypeId == model.RoleTypeId /*|| x.UserID == model.UserID*/).Any();
            if (recordExists)
            {
                _dbContext.Update(model);
            }
            else
            {
                _dbContext.Add(model);
            }
            int rowkey = _dbContext.SaveChanges();
            if(rowkey>0)
            {
                baseResponse = new BaseResponse<string>(200, "Change Successfully");
            }

            return baseResponse;
        }


        public BaseResponse<string> GetAssignedPagesByRoleType_old(int RoleTypeId)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");
            var Pages = _dbContext.PageRoleModules.ToList();
            var records = from assignPage in _dbContext.AssignPageRoles
                          join pageRole in _dbContext.PageRoleModules
                          on assignPage.PageRoleId equals pageRole.Id
                          where assignPage.RoleTypeId == RoleTypeId // or assignPage.UserId == UserId
                          select new AssignPageRole
                          {
                              Id= assignPage.Id,
                              RoleTypeId = assignPage.RoleTypeId,
                              UserID = assignPage.UserID,
                              PageRoleId = assignPage.PageRoleId,
                              PageRoleName = pageRole.Name,
                              Add = assignPage.Add,
                              Update = assignPage.Update,
                              Read = assignPage.Read,
                              Delete = assignPage.Delete,
                          };
            var result = records.ToList();
            List<AssignPageRole> listAPR = new List<AssignPageRole>();
            if (!result.Any())
            {
                foreach (var Page in Pages)
                {
                    AssignPageRole assignPageUserWise = new AssignPageRole();
                    assignPageUserWise.PageRoleId = Page.Id;
                    assignPageUserWise.RoleTypeId = RoleTypeId;
                    assignPageUserWise.Read = false;
                    assignPageUserWise.Add = false;
                    assignPageUserWise.Update = false;
                    assignPageUserWise.Delete = false;
                    listAPR.Add(assignPageUserWise);
                }
                _dbContext.AddRange(listAPR);
                _dbContext.SaveChanges();
                records = from assignPage in _dbContext.AssignPageRoles
                          join pageRole in _dbContext.PageRoleModules
                          on assignPage.PageRoleId equals pageRole.Id
                          where assignPage.RoleTypeId == RoleTypeId
                          select new AssignPageRole
                          {
                              Id = assignPage.Id,
                              RoleTypeId = RoleTypeId,
                              UserID =null,
                              PageRoleId = assignPage.PageRoleId,
                              PageRoleName = pageRole.Name,
                              Add = assignPage.Add,
                              Update = assignPage.Update,
                              Read = assignPage.Read,
                              Delete = assignPage.Delete
                          };
                result = records.ToList();

            }

            baseResponse.Code = 200;
            baseResponse.Message = "Record Bind Successfully";
            baseResponse.Data = result;
            return baseResponse;
        }

        public async Task<BaseResponse<string>> GetAssignedPagesByUserId_old(string UserId)
        {
            var Pages = _dbContext.PageRoleModules.ToList();
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");
            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                // User does exist
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                {

                    var records = from assignPage in _dbContext.AssignPageRoles
                                  join pageRole in _dbContext.PageRoleModules
                                  on assignPage.PageRoleId equals pageRole.Id
                                  where assignPage.UserID == UserId 
                                  select new AssignPageRole
                                  {
                                      Id = assignPage.Id,
                                      RoleTypeId = assignPage.RoleTypeId,
                                      UserID = assignPage.UserID,
                                      PageRoleId = assignPage.PageRoleId,
                                      PageRoleName = pageRole.Name,
                                      Add = assignPage.Add,
                                      Update = assignPage.Update,
                                      Read = assignPage.Read,
                                      Delete = assignPage.Delete,
                                  };
                    var result = records.ToList();


                  
                    List<AssignPageRole> listAPR = new List<AssignPageRole>();
                    if (!result.Any())
                    {
                        foreach (var Page in Pages)
                        {
                            AssignPageRole assignPageUserWise = new AssignPageRole();
                            assignPageUserWise.PageRoleId = Page.Id;
                            assignPageUserWise.UserID = UserId;
                            assignPageUserWise.Read = false;
                            assignPageUserWise.Add = false;
                            assignPageUserWise.Update = false;
                            assignPageUserWise.Delete = false;
                            listAPR.Add(assignPageUserWise);
                        }
                        _dbContext.AddRange(listAPR);
                        _dbContext.SaveChanges();
                         records = from assignPage in _dbContext.AssignPageRoles
                                      join pageRole in _dbContext.PageRoleModules
                                      on assignPage.PageRoleId equals pageRole.Id
                                      where assignPage.UserID == UserId
                                      select new AssignPageRole
                                      {
                                          Id = assignPage.Id,
                                          RoleTypeId = null,
                                          UserID = assignPage.UserID,
                                          PageRoleId = assignPage.PageRoleId,
                                          PageRoleName = pageRole.Name,
                                          Add = assignPage.Add,
                                          Update = assignPage.Update,
                                          Read = assignPage.Read,
                                          Delete = assignPage.Delete
                                      };
                         result = records.ToList();
                    }


                    baseResponse.Code = 200;
                    baseResponse.Message = "Record Bind Successfully";
                    baseResponse.Data = result;
                    return baseResponse;
                }
                else
                {
                    baseResponse.Code = 200;
                    baseResponse.Message = "User is Not admin";
                    return baseResponse;
                }
            }
            else
            {
                baseResponse.Code = 200;
                baseResponse.Message = "User Doesn't Exist";
                return baseResponse;
            }
            
        }

        public BaseResponse<string> GetAssignedPagesByRoleType(int RoleTypeId)
        {
            var Pages = _dbContext.PageRoleModules.ToList();
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");


            List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
            pageRoleModules = _dbContext.PageRoleModules.ToList();

            List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
            assignPageRoles = _dbContext.AssignPageRoles.Where(x => x.RoleTypeId == RoleTypeId).ToList();

            List<AssignPageRole> listAPR = new List<AssignPageRole>();
            foreach (var _pages in pageRoleModules)
            {
                AssignPageRole pageRole = new AssignPageRole();
                var _assignData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.RoleTypeId == RoleTypeId).ToList();

                if (_assignData.Count > 0)
                {
                    var data = _assignData.FirstOrDefault();
                    pageRole.Id = data.Id;
                    pageRole.RoleTypeId = data.RoleTypeId;
                    pageRole.UserID = data.UserID;
                    pageRole.PageRoleId = data.PageRoleId;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = data.Add;
                    pageRole.Update = data.Update;
                    pageRole.Read = data.Read;
                    pageRole.Delete = data.Delete;
                }
                else
                {
                    pageRole.Id = 0;
                    pageRole.RoleTypeId = RoleTypeId;
                    pageRole.PageRoleId = _pages.Id;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = false;
                    pageRole.Update = false;
                    pageRole.Read = false;
                    pageRole.Delete = false;
                }
                listAPR.Add(pageRole);
            }

            baseResponse.Code = 200;
            baseResponse.Message = "Record Bind Successfully";
            baseResponse.Data = listAPR;
            return baseResponse;

        }

        public async Task<BaseResponse<string>> GetAssignedPagesByUserId(string UserId)
        {
            var Pages = _dbContext.PageRoleModules.ToList();
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");


            List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
            pageRoleModules = _dbContext.PageRoleModules.ToList();

            List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
            assignPageRoles = _dbContext.AssignPageRoles.Where(x => x.UserID == UserId).ToList();

            List<AssignPageRole> listAPR = new List<AssignPageRole>();
            foreach (var _pages in pageRoleModules)
            {
                AssignPageRole pageRole = new AssignPageRole();
                var _assignUserData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.UserID == UserId).ToList();

                if (_assignUserData.Count > 0)
                {
                    var data = _assignUserData.FirstOrDefault();
                    pageRole.Id = data.Id;
                    pageRole.RoleTypeId = data.RoleTypeId;
                    pageRole.UserID = data.UserID;
                    pageRole.PageRoleId = data.PageRoleId;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = data.Add;
                    pageRole.Update = data.Update;
                    pageRole.Read = data.Read;
                    pageRole.Delete = data.Delete;
                }
                else
                {
                    pageRole.Id = 0;
                    pageRole.UserID = UserId;
                    pageRole.PageRoleId = _pages.Id;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = false;
                    pageRole.Update = false;
                    pageRole.Read = false;
                    pageRole.Delete = false;
                }
                listAPR.Add(pageRole);
            }

            baseResponse.Code = 200;
            baseResponse.Message = "Record Bind Successfully";
            baseResponse.Data = listAPR;
            return baseResponse;

        }

        public async Task<BaseResponse<string>> GetAssignedPagesByUserIdandRoleTypeId(string UserId, int RoletypeId)
        {
            var Pages = _dbContext.PageRoleModules.ToList();
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");
            var user = await _userManager.FindByIdAsync(UserId);

            if (user != null)
            {
                // User does exist
                bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                if (isAdmin)
                {
                    List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
                    pageRoleModules = _dbContext.PageRoleModules.ToList();

                    List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
                    assignPageRoles = _dbContext.AssignPageRoles.Where(x=>x.RoleTypeId == RoletypeId || x.UserID == UserId).ToList();

                    List<AssignPageRole> listAPR = new List<AssignPageRole>();
                    foreach (var _pages in pageRoleModules)
                    {
                        AssignPageRole pageRole = new AssignPageRole();
                        var _assignData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.RoleTypeId == RoletypeId).ToList();
                        var _assignUserData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.UserID == UserId).ToList();

                        if (_assignUserData.Count > 0)
                        {
                            var data = _assignUserData.FirstOrDefault();
                            pageRole.Id = data.Id;
                            pageRole.RoleTypeId = data.RoleTypeId;
                            pageRole.UserID = data.UserID;
                            pageRole.PageRoleId = data.PageRoleId;
                            pageRole.PageRoleName = _pages.Name;
                            pageRole.Add = data.Add;
                            pageRole.Update = data.Update;
                            pageRole.Read = data.Read;
                            pageRole.Delete = data.Delete;
                        }
                        else if(_assignData.Count>0)
                        {
                            var data = _assignData.FirstOrDefault();
                            pageRole.Id = data.Id;
                            pageRole.RoleTypeId = data.RoleTypeId;
                            pageRole.UserID = data.UserID;
                            pageRole.PageRoleId = data.PageRoleId;
                            pageRole.PageRoleName = _pages.Name;
                            pageRole.Add = data.Add;
                            pageRole.Update = data.Update;
                            pageRole.Read = data.Read;
                            pageRole.Delete = data.Delete;
                        }
                        else
                        {
                            pageRole.Id = 0;
                            pageRole.RoleTypeId = RoletypeId;
                            pageRole.PageRoleId = _pages.Id;
                            pageRole.PageRoleName = _pages.Name;
                            pageRole.Add = false;
                            pageRole.Update = false;
                            pageRole.Read = false;
                            pageRole.Delete = false;
                        }
                        listAPR.Add(pageRole);
                    }

                    baseResponse.Code = 200;
                    baseResponse.Message = "Record Bind Successfully";
                    baseResponse.Data = listAPR;
                    return baseResponse;
                }
                else
                {
                    baseResponse.Code = 200;
                    baseResponse.Message = "User is Not admin";
                    return baseResponse;
                }
            }
            else
            {
                baseResponse.Code = 200;
                baseResponse.Message = "User Doesn't Exist";
                return baseResponse;
            }

        }

        public UserTypeFormModel GetRoleTypeWithPageAccesswithUserSpecific(string UserId, int Id, bool isClass)
        {
            var userType = _dbContext.UserTypes.Where(x => x.Id == Id).FirstOrDefault();
            //var AssignedPages = _dbContext.AssignPageRoles.Where(x => x.RoleTypeId == Id).Include(b => b.Page).ToList();
            //foreach (var item in AssignedPages)
            //{
            //    item.PageRoleName = item.Page.Name;
            //}

            List<PageRoleModule> pageRoleModules = new List<PageRoleModule>();
            pageRoleModules = _dbContext.PageRoleModules.ToList();

            List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
            assignPageRoles = _dbContext.AssignPageRoles.Where(x => x.RoleTypeId == Id || x.UserID == UserId).ToList();

            List<AssignPageRole> listAPR = new List<AssignPageRole>();
            foreach (var _pages in pageRoleModules)
            {
                AssignPageRole pageRole = new AssignPageRole();
                var _assignData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.RoleTypeId == Id).ToList();
                var _assignUserData = assignPageRoles.Where(x => x.PageRoleId == _pages.Id && x.UserID == UserId).ToList();

                if (_assignUserData.Count > 0)
                {
                    var data = _assignUserData.FirstOrDefault();
                    pageRole.Id = data.Id;
                    pageRole.RoleTypeId = data.RoleTypeId;
                    pageRole.UserID = data.UserID;
                    pageRole.PageRoleId = data.PageRoleId;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = data.Add;
                    pageRole.Update = data.Update;
                    pageRole.Read = data.Read;
                    pageRole.Delete = data.Delete;
                }
                else if (_assignData.Count > 0)
                {
                    var data = _assignData.FirstOrDefault();
                    pageRole.Id = data.Id;
                    pageRole.RoleTypeId = data.RoleTypeId;
                    pageRole.UserID = data.UserID;
                    pageRole.PageRoleId = data.PageRoleId;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = data.Add;
                    pageRole.Update = data.Update;
                    pageRole.Read = data.Read;
                    pageRole.Delete = data.Delete;
                }
                else
                {
                    pageRole.Id = 0;
                    pageRole.RoleTypeId = Id;
                    pageRole.PageRoleId = _pages.Id;
                    pageRole.PageRoleName = _pages.Name;
                    pageRole.Add = false;
                    pageRole.Update = false;
                    pageRole.Read = false;
                    pageRole.Delete = false;
                }
                listAPR.Add(pageRole);
            }

            UserTypeFormModel response = new UserTypeFormModel();
            response.UserTypeId = userType.Id;
            response.UserTypeName = userType.Name;

            response.pagesAssigned = listAPR;

            return response;
        }
    }
}
