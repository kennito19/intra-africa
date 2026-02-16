using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Application.IServices;

namespace Catalogue.Application.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationRepository _specificationRepository;
        public SpecificationService(ISpecificationRepository specificationRepository)
        {
            _specificationRepository = specificationRepository;
        }

        public Task<BaseResponse<long>> Create(SpecificationLibrary specification)
        {
            var response = _specificationRepository.Create(specification);
            return response;
        }

        public Task<BaseResponse<long>> Update(SpecificationLibrary specification)
        {
            var response = _specificationRepository.Update(specification);
            return response;
        }

        public Task<BaseResponse<long>> Delete(SpecificationLibrary specification)
        {
            var reponse = _specificationRepository.Delete(specification);
            return reponse;
        }

        public Task<BaseResponse<List<SpecificationLibrary>>> get(SpecificationLibrary specification, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            var response = _specificationRepository.get(specification, Getparent, Getchild, PageIndex, PageSize, Mode);
            return response;
        }
    }
}
