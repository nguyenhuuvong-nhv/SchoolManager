using AutoMapper;
using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Dapper;
using Data.Entity;
using Entities.UnitOfWork;
using MicroOrm.Dapper.Repositories;
using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Dtos.Shared;
using Services.Dtos.Shared.Inputs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class SchoolService : ISchoolService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private IDapperRepository<School> schoolRepository => _unitOfWork.GetRepository<School>();

        private IDapperRepository<Grade> classRepository => _unitOfWork.GetRepository<Grade>();

        public SchoolService(
           IUnitOfWork unitOfWork,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SchoolDto[]> GetAll()
        {
            return _mapper.Map<SchoolDto[]>(await schoolRepository.FindAllAsync());
        }

        public async Task<IPagedResult<SchoolDto>> FilterAsync(PagingDto paging, SchoolFilterDto filter)
        {
            int totalRow = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", filter.SearchKey, DbType.String);
            parameters.Add("@PageNumber", paging.PageNumber, DbType.Int32);
            parameters.Add("@PageSize", paging.PageSize, DbType.Int32);
            parameters.Add("@TotalRow", totalRow, DbType.Int32, ParameterDirection.Output);

            var results = (await schoolRepository.Connection.QueryAsync<School>(
                StoredProcedureName.FILTER_SCHOOL,
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            var pagedResult = new Dtos.Shared.PagedResult<SchoolDto>()
            {
                TotalCount = totalRow,
                PageSize = paging.PageSize,
                TotalPages = (int)Math.Ceiling(totalRow / (double)paging.PageSize),
                PageIndex = paging.PageNumber,
                Items = _mapper.Map<SchoolDto[]>(results)
            };
            return pagedResult;
        }

        public async Task<bool> CreateAsync(SchoolCreateDto dto)
        {
            var school = _mapper.Map<School>(dto);
            school.Id = Guid.NewGuid();
            school.CreatedAt = DateTime.Now;
            
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await schoolRepository.InsertAsync(school, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<bool> UpdateAsync(SchoolUpdateDto dto)
        {
            var school = await schoolRepository.FindByIdAsync(dto.Id);

            if (school == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);

            school = _mapper.Map(dto, school);
            school.ModifiedAt = DateTime.Now;
            
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await schoolRepository.UpdateAsync(school, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<bool> DeleteAsync(EntityDto dto)
        {
            var school = await schoolRepository.FindByIdAsync(dto.Id);
            if (school == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);

            if (await classRepository.CountAsync(cl => cl.FK_SchoolId == school.Id) > 0)
                throw new BusinessException("School has been used", ErrorCode.SCHOOL_HAS_BEEN_USED);

            school.ModifiedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await schoolRepository.DeleteAsync(school, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }
    }
}
