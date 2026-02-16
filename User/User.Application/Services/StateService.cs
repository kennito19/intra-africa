using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        public Task<BaseResponse<long>> Create(StateLibrary stateLibrary)
        {
            var data = _stateRepository.Create(stateLibrary);
            return data;
        }
        public Task<BaseResponse<long>> Update(StateLibrary stateLibrary)
        {
            var data = _stateRepository.Update(stateLibrary);
            return data;
        }
        public Task<BaseResponse<long>> Delete(StateLibrary stateLibrary)
        {
            var data = _stateRepository.Delete(stateLibrary);
            return data;
        }

        public Task<BaseResponse<List<StateLibrary>>> Get(StateLibrary stateLibrary, int PageIndex, int PageSize, string Mode)
        {
            var data = _stateRepository.Get(stateLibrary, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}
