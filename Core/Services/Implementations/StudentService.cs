using AutoMapper;
using Common.Constants;
using Common.Enums;
using Common.Exceptions;
using Dapper;
using Data.Entity;
using Entities.UnitOfWork;
using MicroOrm.Dapper.Repositories;
using Microsoft.AspNetCore.Http;
using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Dtos.Shared;
using Services.Dtos.Shared.Inputs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class StudentService : IStudentService
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private IDapperRepository<Student> studentRepository => _unitOfWork.GetRepository<Student>();

        private IDapperRepository<Grade> gradeRepository => _unitOfWork.GetRepository<Grade>();

        public StudentService(
           IUnitOfWork unitOfWork,
           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> CreateAsync(StudentCreateDto dto)
        {
            if (dto.Avatar != null)
            {
                ValidateImage(dto.Avatar);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", dto.Avatar.FileName);
                var stream = new FileStream(path, FileMode.Create);
                dto.Avatar.CopyToAsync(stream);
            }
            var student = _mapper.Map<Student>(dto);
            student.Id = Guid.NewGuid();
            student.CreatedAt = DateTime.Now;
            student.Status = StudentStatus.Active.ToString();

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await studentRepository.InsertAsync(student, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<bool> DeleteAsync(EntityDto dto)
        {
            var student = await studentRepository.FindByIdAsync(dto.Id);
            if (student == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);

            student.ModifiedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await studentRepository.DeleteAsync(student, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<IPagedResult<StudentDto>> FilterAsync(PagingDto paging, StudentFilterDto filter)
        {
            int totalRow = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SearchKey", filter.SearchKey, DbType.String);
            parameters.Add("@GradeId", filter.FK_GradeId, DbType.Guid);
            parameters.Add("@SchoolId", filter.FK_SchoolId, DbType.Guid);
            parameters.Add("@PageNumber", paging.PageNumber, DbType.Int32);
            parameters.Add("@PageSize", paging.PageSize, DbType.Int32);
            parameters.Add("@TotalRow", totalRow, DbType.Int32, ParameterDirection.Output);

            var results = (await gradeRepository.Connection.QueryAsync<Student>(
                StoredProcedureName.FILTER_STUDENT,
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            var pagedResult = new PagedResult<StudentDto>()
            {
                TotalCount = totalRow,
                PageSize = paging.PageSize,
                TotalPages = (int)Math.Ceiling(totalRow / (double)paging.PageSize),
                PageIndex = paging.PageNumber,
                Items = _mapper.Map<StudentDto[]>(results)
            };
            return pagedResult;
        }

        public async Task<StudentDto[]> GetAll()
        {
            return _mapper.Map<StudentDto[]>(await studentRepository.FindAllAsync());
        }

        public async Task<bool> UpdateAsync(StudentUpdateDto dto)
        {
            var student = await studentRepository.FindByIdAsync(dto.Id);

            if (student == null)
                throw new BusinessException("OBJECT NOT FOUND", ErrorCode.OBJECT_NOT_FOUND);
            if(student.Avatar != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/",student.Avatar);
                File.Delete(path);
            }
            if (dto.Avatar != null)
            {
                ValidateImage(dto.Avatar);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", dto.Avatar.FileName);
                var stream = new FileStream(path, FileMode.Create);
                dto.Avatar.CopyToAsync(stream);
            }
            student = _mapper.Map(dto, student);
            student.ModifiedAt = DateTime.Now;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                await studentRepository.UpdateAsync(student, transaction);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<StudentDetailDto> GetAsync(Guid id)
        {
            var student = (await studentRepository.Connection.QueryAsync<Student, Grade, School, Student>(
           StoredProcedureName.GET_STUDENT_BY_ID,
           (st, g, sc) =>
           {
               if (g != null)
               {
                   if (sc != null)
                       g.School = sc;
                   st.Grade = g;
               }

               return st;
           },
           new { Id = id },
           splitOn: "Id",
           commandType: CommandType.StoredProcedure)).FirstOrDefault();
           return _mapper.Map<StudentDetailDto>(student);
        }
        private void ValidateImage(IFormFile file)
        {
            string[] contentType = new[] { "image/jpg", "image/jpeg", "image/pjpeg", "image/x-png", "image/png" };
            long maxSize = 10 * 1024 * 1024; //Max 10 MB
            if (!contentType.Contains(file.ContentType.ToLower()))
            {
                throw new BusinessException("Invalid format file!", ErrorCode.INVALID_FORMAT_FILE);
            }
            if (file.Length > maxSize)
            {
                throw new BusinessException("Invalid size file!", ErrorCode.INVALID_SIZE_FILE);
            }
        }
    }
}
