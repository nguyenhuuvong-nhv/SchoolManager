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
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private IDapperRepository<Student> studentRepository => _unitOfWork.GetRepository<Student>();

        private IDapperRepository<Grade> gradeRepository => _unitOfWork.GetRepository<Grade>();

        public GradeService(
           IUnitOfWork unitOfWork,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(GradeCreateDto dto)
        {
            var grade = _mapper.Map<Grade>(dto);
            grade.Id = Guid.NewGuid();
            grade.CreatedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await gradeRepository.InsertAsync(grade, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<bool> DeleteAsync(EntityDto dto)
        {
            var grade = await gradeRepository.FindByIdAsync(dto.Id);
            if (grade == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);

            if (await studentRepository.CountAsync(st => st.FK_GradeId == grade.Id) > 0)
                throw new BusinessException("GRADE HAS BEEN USED", ErrorCode.GRADE_HAS_BEEN_USED);

            grade.ModifiedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await gradeRepository.DeleteAsync(grade, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<IPagedResult<GradeDto>> FilterAsync(PagingDto paging, GradeFilterDto filter)
        {
            int totalRow = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", filter.SearchKey, DbType.String);
            parameters.Add("@MajorId", filter.FK_Major, DbType.Guid);
            parameters.Add("@HeadTeacherId", filter.FK_HeadTeacher, DbType.Guid);
            parameters.Add("@SchoolId", filter.FK_SchoolId, DbType.Guid);
            parameters.Add("@PageNumber", paging.PageNumber, DbType.Int32);
            parameters.Add("@PageSize", paging.PageSize, DbType.Int32);
            parameters.Add("@TotalRow", totalRow, DbType.Int32, ParameterDirection.Output);

            var results = (await gradeRepository.Connection.QueryAsync<Grade>(
                StoredProcedureName.FILTER_GRADE,
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            var pagedResult = new PagedResult<GradeDto>()
            {
                TotalCount = totalRow,
                PageSize = paging.PageSize,
                TotalPages = (int)Math.Ceiling(totalRow / (double)paging.PageSize),
                PageIndex = paging.PageNumber,
                Items = _mapper.Map<GradeDto[]>(results)
            };
            return pagedResult;
        }

        public async Task<GradeDto[]> GetAll()
        {
            return _mapper.Map<GradeDto[]>(await gradeRepository.FindAllAsync());
        }

        public async Task<bool> UpdateAsync(GradeUpdateDto dto)
        {
            var grade = await gradeRepository.FindByIdAsync(dto.Id);

            if (grade == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);

            grade = _mapper.Map(dto, grade);
            grade.ModifiedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await gradeRepository.UpdateAsync(grade, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<GradeDetailDto> GetAsync(Guid id)
        {
            var grade = (await studentRepository.Connection.QueryAsync<Grade, School, Grade>(
           StoredProcedureName.GET_GRADE_BY_ID,
           (g, sc) =>
           {
               if (sc != null)
               {
                   g.School = sc;
               }

               return g;
           },
           new { Id = id },
           splitOn: "Id",
           commandType: CommandType.StoredProcedure)).FirstOrDefault();
            return _mapper.Map<GradeDetailDto>(grade);
        }
    }
}
