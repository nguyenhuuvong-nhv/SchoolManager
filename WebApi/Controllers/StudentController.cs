using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Dtos.Respose;
using Services.Dtos.Shared;
using Services.Dtos.Shared.Inputs;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        /// <summary>
        /// Lọc danh sách sinh viên
        /// </summary>
        [HttpGet("filter/{page:int}/{pageSize:int}")]
        public async Task<BaseResponse<IPagedResult<StudentDto>>> FilterAsync(
            [FromRoute] int page,
            [FromRoute] int pageSize,
            [FromQuery] Guid? schooId,
            [FromQuery] Guid? gradeId,
            [FromQuery] string searchKey)
        {
            var response = new BaseResponse<IPagedResult<StudentDto>>
            {
                Data = await _studentService.FilterAsync(
                        new PagingDto(page, pageSize),
                        new StudentFilterDto()
                        {
                            FK_GradeId = gradeId,
                            FK_SchoolId = schooId,
                            SearchKey = searchKey
                        }
                    ),
                Status = true
            };
            return response;
        }
        /// <summary>
        /// Lấy danh sách tất cả các sinh viên
        /// </summary>
        [HttpGet("get-all")]
        public async Task<BaseResponse<StudentDto[]>> GetAll()
        {
            var response = new BaseResponse<StudentDto[]>
            {
                Data = await _studentService.GetAll(),
                Status = true
            };
            return response;
        }
        /// <summary>
        /// Lấy thông tin chi tiết sinh viên
        /// </summary>
        [HttpGet("{id:Guid}")]
        public async Task<BaseResponse<StudentDetailDto>> GetAsync(Guid id)
        {
            var response = new BaseResponse<StudentDetailDto>
            {
                Data = await _studentService.GetAsync(id),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Tạo mới sinh viên
        /// </summary>
        [HttpPost("create")]
        public async Task<BaseResponse<bool>> CreateAsync([FromForm] StudentCreateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _studentService.CreateAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Xoá sinh viên
        /// </summary>
        [HttpDelete("delete")]
        public async Task<BaseResponse<bool>> DeleteAsync(EntityDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _studentService.DeleteAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Cập nhật sinh viên
        /// </summary>
        [HttpPut("update")]
        public async Task<BaseResponse<bool>> UpdateAsync([FromForm] StudentUpdateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _studentService.UpdateAsync(dto),
                Status = true
            };
            return response;
        }

    }
}
