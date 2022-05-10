using AutoMapper;
using Common.Enums;
using Common.Extensions;
using Data.Entity;
using Services.Dtos.Dbo;
using Services.Dtos.Dbo.Input;
using Services.Helpers;

namespace Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            EntityToDtoMappingConfiguration();
            DtoToEntityMappingConfiguration();
        }

        public void EntityToDtoMappingConfiguration()
        {
            CreateMap<School, SchoolDto>();

            CreateMap<Grade, GradeDto>();
            CreateMap<Grade, GradeDetailDto>();

            CreateMap<Student, StudentDto>()
                .ForMember(dto => dto.Birthday, entity => entity.MapFrom(prop => prop.Birthday.ToSecondsTimestamp()))
                .ForMember(dto => dto.Status, entity => entity.MapFrom(prop => prop.Status.ToDictionaryItemDto<StudentStatus>()))
                .ForMember(dto => dto.Gender, entity => entity.MapFrom(prop => prop.Gender.ToDictionaryItemDto<Gender>()));

            CreateMap<Student, StudentDetailDto>()
                .ForMember(dto => dto.Birthday, entity => entity.MapFrom(prop => prop.Birthday.ToSecondsTimestamp()))
                .ForMember(dto => dto.Status, entity => entity.MapFrom(prop => prop.Status.ToDictionaryItemDto<StudentStatus>()))
                .ForMember(dto => dto.Gender, entity => entity.MapFrom(prop => prop.Gender.ToDictionaryItemDto<Gender>()));
        }

        public void DtoToEntityMappingConfiguration()
        {
            CreateMap<SchoolCreateDto, School>();
            CreateMap<SchoolUpdateDto, School>();

            CreateMap<GradeCreateDto, Grade>();
            CreateMap<GradeUpdateDto, Grade>();

            CreateMap<StudentCreateDto, Student>()
                .ForMember(prop => prop.Birthday, entity => entity.MapFrom(dto => dto.Birthday.FromUnixTimeStamp()))
                .ForMember(prop => prop.Avatar, entity => entity.MapFrom(dto => dto.Avatar.FileName))
                .ForMember(dto => dto.Gender, entity => entity.MapFrom(prop => prop.Gender.Value));
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(prop => prop.Birthday, entity => entity.MapFrom(dto => dto.Birthday.FromUnixTimeStamp()))
                .ForMember(prop => prop.Avatar, entity => entity.MapFrom(dto => dto.Avatar.FileName))
                .ForMember(dto => dto.Status, entity => entity.MapFrom(prop => prop.Status.Value))
                .ForMember(dto => dto.Gender, entity => entity.MapFrom(prop => prop.Gender.Value));
        }
    }
}
