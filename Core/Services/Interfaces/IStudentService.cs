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
    public interface IStudentService
    {
        Task<StudentDto[]> GetAll();

        Task<StudentDetailDto> GetAsync(Guid id);

        Task<IPagedResult<StudentDto>> FilterAsync(PagingDto paging, StudentFilterDto filter);

        Task<bool> CreateAsync(StudentCreateDto dto);

        Task<bool> UpdateAsync(StudentUpdateDto dto);

        Task<bool> DeleteAsync(EntityDto dto);
    }
}
