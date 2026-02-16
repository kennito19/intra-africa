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
    //public class ProductsVideoLinksService : IProductsVideoLinksService
    //{
    //    private readonly IProductsVideoLinksRepository _linksRepository;
    //    public ProductsVideoLinksService(IProductsVideoLinksRepository linksRepository)
    //    {
    //        _linksRepository = linksRepository;
    //    }
    //    public Task<BaseResponse<long>> Create(ProductVideoLinks productVideoLinks)
    //    {
    //        var response = _linksRepository.Create(productVideoLinks);
    //        return response;
    //    }
    //    public Task<BaseResponse<long>> Update(ProductVideoLinks productVideoLinks)
    //    {
    //        var response = _linksRepository.Update(productVideoLinks);
    //        return response;
    //    }
    //    public Task<BaseResponse<long>> Delete(ProductVideoLinks productVideoLinks)
    //    {
    //        var response = _linksRepository.Delete(productVideoLinks);
    //        return response;
    //    }

    //    public Task<BaseResponse<List<ProductVideoLinks>>> get(ProductVideoLinks productVideoLinks, int PageIndex, int PageSize, string Mode)
    //    {
    //        var response = _linksRepository.get(productVideoLinks, PageIndex, PageSize, Mode);
    //        return response;
    //    }

        
    //}
}
