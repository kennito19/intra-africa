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
    public class ExtraChargesService : IExtraChargesService
    {
        private readonly IExtraChargesRepository _extraChargesRepository;

        public ExtraChargesService(IExtraChargesRepository extraChargesRepository)
        {
            _extraChargesRepository = extraChargesRepository;
        }

        public async Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            var responce = await _extraChargesRepository.AddExtraCharges(extraChargesLibrary);
            return responce;
        }

        public async Task<BaseResponse<long>> DeleteExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            var responce = await _extraChargesRepository.DeleteExtraCharges(extraChargesLibrary);
            return responce;
        }

        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetCatExtraCharges(int CategoryId)
        {
            var responce = await _extraChargesRepository.GetCatExtraCharges(CategoryId);
            return responce;
        }

        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetExtraCharges(ExtraChargesLibrary extraChargesLibrary, int PageIndex, int PageSize, string Mode)
        {
            var responce =await _extraChargesRepository.GetExtraCharges(extraChargesLibrary, PageIndex, PageSize, Mode);
            return responce;
        }

        public async Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            var responce = await _extraChargesRepository.UpdateExtraCharges(extraChargesLibrary);
            return responce;
        }
    }
}
