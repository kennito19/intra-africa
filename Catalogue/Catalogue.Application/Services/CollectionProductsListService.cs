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
    public class CollectionProductsListService : ICollectionProductsListService
    {
        private readonly ICollectionProductsListRepository _collectionProductsListRepository;

        public CollectionProductsListService(ICollectionProductsListRepository collectionProductsListRepository)
        {
            _collectionProductsListRepository = collectionProductsListRepository;
        }

        public async Task<BaseResponse<List<CollectionProductsList>>> get(CollectionProductsList productList, int PageIndex, int PageSize)
        {
            var data = await _collectionProductsListRepository.get(productList, PageIndex, PageSize);
            return data;
        }
    }
}
