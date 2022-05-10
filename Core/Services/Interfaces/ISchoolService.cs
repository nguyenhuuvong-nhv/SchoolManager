using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Dtos.Shared;
using Services.Dtos.Shared.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISchoolService
    {
        Task<SchoolDto[]> GetAll();

        Task<IPagedResult<SchoolDto>> FilterAsync(PagingDto paging, SchoolFilterDto filter);

        Task<bool> CreateAsync(SchoolCreateDto dto);

        Task<bool> UpdateAsync(SchoolUpdateDto dto);

        Task<bool> DeleteAsync(EntityDto dto);
    }
}
