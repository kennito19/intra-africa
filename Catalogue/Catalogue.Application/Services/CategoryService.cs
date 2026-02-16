using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<BaseResponse<long>> Create(CategoryLibrary category)
        {
            var response = _categoryRepository.Create(category);
            return response;
        }

        public Task<BaseResponse<long>> Update(CategoryLibrary category)
        {
            var response = _categoryRepository.Update(category);
            return response;
        }

        public Task<BaseResponse<long>> Delete(CategoryLibrary category)
        {
            var reponse = _categoryRepository.Delete(category);
            return reponse;
        }

        public Task<BaseResponse<List<CategoryLibrary>>> get(CategoryLibrary category, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            var response = _categoryRepository.get(category, Getparent, Getchild, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParent(int Categoryid)
        {
            var response = _categoryRepository.GetCategoryWithParent(Categoryid);
            return response;
        }
    }
}
