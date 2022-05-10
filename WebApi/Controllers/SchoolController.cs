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

namespace StudentManager.Controllers
{
    [Route("api/school")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        /// <summary>
        /// Lọc danh sách trường
        /// </summary>
        [HttpGet("filter/{page:int}/{pageSize:int}")]
        public async Task<BaseResponse<IPagedResult<SchoolDto>>> FilterAsync(
            [FromRoute] int page,
            [FromRoute] int pageSize,
            [FromQuery] string searchKey)
        {
            var response = new BaseResponse<IPagedResult<SchoolDto>>
            {
                Data = await _schoolService.FilterAsync(
                        new PagingDto(page, pageSize),
                        new SchoolFilterDto()
                        {
                            SearchKey = searchKey
                        }
                    ),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Lấy danh sách tất cả các trường
        /// </summary>
        [HttpGet("get-all")]
        public async Task<BaseResponse<SchoolDto[]>> GetAll()
        {
            var response = new BaseResponse<SchoolDto[]>
            {
                Data = await _schoolService.GetAll(),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Tạo mới trường
        /// </summary>
        [HttpPost("create")]
        public async Task<BaseResponse<bool>> CreateAsync(SchoolCreateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _schoolService.CreateAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Cập nhật trường
        /// </summary>
        [HttpPut("update")]
        public async Task<BaseResponse<bool>> UpdateAsync(SchoolUpdateDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _schoolService.UpdateAsync(dto),
                Status = true
            };
            return response;
        }

        /// <summary>
        /// Xoá trường
        /// </summary>
        [HttpDelete("delete")]
        public async Task<BaseResponse<bool>> DeleteAsync(EntityDto dto)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _schoolService.DeleteAsync(dto),
                Status = true
            };
            return response;
        }
    }
}
