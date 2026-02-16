using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface IStateService
    {
        Task<BaseResponse<long>> Create(StateLibrary stateLibrary);
        Task<BaseResponse<long>> Update(StateLibrary stateLibrary);
        Task<BaseResponse<long>> Delete(StateLibrary stateLibrary);
        Task<BaseResponse<List<StateLibrary>>> Get(StateLibrary stateLibrary, int PageIndex, int PageSize, string Mode);
    }
}
