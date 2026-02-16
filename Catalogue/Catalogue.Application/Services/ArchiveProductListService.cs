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
    public class ArchiveProductListService : IArchiveProductListService
    {
        private readonly IArchiveProductListRepository _archiveproductListRepository;

        public ArchiveProductListService(IArchiveProductListRepository archiveproductListRepository)
        {
            _archiveproductListRepository = archiveproductListRepository;
        }

        public async Task<BaseResponse<List<ArchiveProductList>>> get(ArchiveProductList productList, int PageIndex, int PageSize, string Mode)
        {
            var data = await _archiveproductListRepository.get(productList, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
