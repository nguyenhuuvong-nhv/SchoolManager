using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Dtos.Respose;
using Services.Dtos.Shared;
using Services.Dtos.Shared.Inputs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/grade")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }
        /// <summary>
        /// Lọc danh sách lớp
        /// </summary>
        [HttpGet("filter/{page:int}/{pageSize:int}")]
        public async Task<BaseResponse<IPagedResult<GradeDto>>> FilterAsync(
            [FromRoute] int page,
            [FromRoute] int pageSize,
            [FromQuery] Guid? majorId,
            [FromQuery] Guid? headTeacherId,
            [FromQuery] Guid? schooId,
            [FromQuery] string searchKey)
        {
            var response = new BaseResponse<IPagedResult<GradeDto>>
            {
                Data = await _gradeService.FilterAsync(
                        new PagingDto(page, pageSize),
                        new GradeFilterDto()
                        {
                            FK_Major= majorId,
                            FK_HeadTeacher= headTeacherId,
                            FK_SchoolId = schooId,
                            SearchKey = searchKey
                        }
                    ),
                Status = true
            };
            return response;
        }
        /// <summary>
        /// Lấy danh sách tất cả các lớp
        /// </summary>
        [HttpGet("get-all")]
        public async Task<BaseResponse<GradeDto[]>> GetAll()
        {
            var response = new BaseResponse<GradeDto[]>
            {
                Data = await _gradeService.GetAll(),
                Status = true
            };
            return response;
        }
        /// <summary>
        /// Lấy thông tin chi tiết lớp
        /// </summary>
        [HttpGet("{id:Guid}")]
        public async Task<BaseResponse<GradeDetailDto>> GetAsync(Guid id)
        {
            var response = new BaseResponse<GradeDetailDto>
            {
                Data = await _gradeService.GetAsync(id),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Tạo mới lớp
        /// </summary>
        [HttpPost("create")]
        public async Task<BaseResponse<bool>> CreateAsync(GradeCreateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _gradeService.CreateAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Xoá lớp
        /// </summary>
        [HttpDelete("delete")]
        public async Task<BaseResponse<bool>> DeleteAsync(EntityDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _gradeService.DeleteAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Cập nhật lớp
        /// </summary>
        [HttpPut("update")]
        public async Task<BaseResponse<bool>> UpdateAsync(GradeUpdateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _gradeService.UpdateAsync(dto),
                Status = true
            };
            return response;
        }



    }
}
