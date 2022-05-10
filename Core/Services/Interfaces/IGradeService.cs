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
    public interface IGradeService
    {
        Task<GradeDto[]> GetAll();
        Task<GradeDetailDto> GetAsync(Guid id);

        Task<IPagedResult<GradeDto>> FilterAsync(PagingDto paging, GradeFilterDto filter);

        Task<bool> CreateAsync(GradeCreateDto dto);

        Task<bool> UpdateAsync(GradeUpdateDto dto);

        Task<bool> DeleteAsync(EntityDto dto);
    }
}
