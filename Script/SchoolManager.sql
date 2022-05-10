create database SchoolManager
go
use SchoolManager
go
create table Schools (
	Id uniqueidentifier NOT NULL,
	IsDeleted bit NULL,
	CreatedAt datetime NULL,
	CreatedBy uniqueidentifier NULL,
	ModifiedAt datetime NULL,
	ModifiedBy uniqueidentifier NULL,
	Name nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Code nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Address nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Hotline nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Email nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_School PRIMARY KEY (Id)
);
go
create table Grades (
	Id uniqueidentifier NOT NULL,
	IsDeleted bit NULL,
	CreatedAt datetime NULL,
	CreatedBy uniqueidentifier NULL,
	ModifiedAt datetime NULL,
	ModifiedBy uniqueidentifier NULL,
	Code nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	FK_Major uniqueidentifier NULL ,
	FK_HeadTeacher uniqueidentifier NULL ,
	FK_SchoolId uniqueidentifier NULL 
		FOREIGN KEY REFERENCES Schools(Id),
	CONSTRAINT PK_Grade PRIMARY KEY (Id)
);
go
create table Students (
	Id uniqueidentifier NOT NULL,
	IsDeleted bit NULL,
	CreatedAt datetime NULL,
	CreatedBy uniqueidentifier NULL,
	ModifiedAt datetime NULL,
	ModifiedBy uniqueidentifier NULL,
	StudentCode nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Name nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Birthday datetime NULL,
	Gender nvarchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Address nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Phone nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IdCard nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	BankAccount nvarchar(20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Email nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Avatar nvarchar(512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Status nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	FK_GradeId uniqueidentifier NULL 
		FOREIGN KEY REFERENCES Grades(Id),
	CONSTRAINT PK_Student PRIMARY KEY (Id)
);
go
CREATE TABLE dbo.ConfigTexts (
	Id uniqueidentifier NOT NULL,
	IsDeleted bit NULL,
	ConfigGroup nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ConfigKey nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ConfigValue nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DisplayText nvarchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ConfigOrder int NULL,
	CONSTRAINT PK_ConfigTexts PRIMARY KEY (Id)
);
INSERT INTO dbo.ConfigTexts
VALUES('3dac7042-a17a-45ab-a32e-9b52f452c54f', 0, 'StudentGender', 'StudentGenderMale', 'Male', N'Nam', 1),
	('6ad28328-1578-47c6-a5a3-5b1628e7d4b2', 0, 'StudentGender', 'StudentGenderFemale', 'Female', N'Nữ', 2),
	('72541b11-105c-41ef-9ec5-e76999313224', 0, 'StudentStatus', 'StudentStatusGraduated', 'Graduated', N'Đã tốt nghiệp', 1),
	('8ea8a0b3-47f8-44bc-b779-78f2b4e78212', 0, 'StudentStatus', 'StudentStatusRetention', 'Retention', N'Bảo lưu', 2),
	('f43b0c39-c6dc-4857-a751-9f5c6aa747e2', 0, 'StudentStatus', 'StudentStatusActive', 'Active', N'Hoạt động', 3),
	('810442b0-06f1-4e04-aeff-58b5c28cf715', 0, 'StudentStatus', 'StudentStatusDisabled', 'Disabled', N'Không hoạt động', 4)


CREATE PROCEDURE dbo.API_Schools_Filter
	@SearchKey NVARCHAR(255) = NULL,
	@PageNumber INT,  
	@PageSize INT,
	@TotalRow INT OUT
AS
BEGIN
	
	SELECT
		s.Id
	INTO #FilterData
	FROM dbo.Schools s
	WHERE s.IsDeleted = 0
		AND (@SearchKey IS NULL 
				OR s.Name LIKE N'%' + @SearchKey + '%'
				OR s.Code LIKE N'%' + @SearchKey + '%')
	
	SELECT @TotalRow = COUNT(1) FROM #FilterData  po

	SELECT s.*
	FROM #FilterData (NOLOCK) f
	INNER JOIN dbo.Schools s ON f.Id = s.Id
	ORDER BY s.Code
	OFFSET @PageNumber * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END


CREATE PROCEDURE dbo.API_Grades_Filter
	@SearchKey NVARCHAR(255) = NULL,
	@MajorId UNIQUEIDENTIFIER = NULL,
	@HeadTeacherId UNIQUEIDENTIFIER = NULL,
	@SchoolId UNIQUEIDENTIFIER = NULL,
	@PageNumber INT,  
	@PageSize INT,
	@TotalRow INT OUT
AS
BEGIN
	
	SELECT
		g.Id
	INTO #FilterData
	FROM dbo.Grades g
	WHERE g.IsDeleted = 0
		AND (@SearchKey IS NULL 
				OR g.Code LIKE N'%' + @SearchKey + '%')
		AND (@MajorId IS NULL OR @MajorId = g.FK_Major)
		AND (@HeadTeacherId IS NULL OR @HeadTeacherId = g.FK_HeadTeacher)
		AND (@SchoolId IS NULL OR @SchoolId = g.FK_SchoolId)

	
	SELECT @TotalRow = COUNT(1) FROM #FilterData  po

	SELECT g.*
	FROM #FilterData (NOLOCK) f
	INNER JOIN dbo.Grades g ON f.Id = g.Id
	ORDER BY g.Code
	OFFSET @PageNumber * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

CREATE PROCEDURE dbo.API_Grade_GetById
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	
	SELECT
		 g.*,sc.*
	FROM dbo.Grades g
	LEFT JOIN dbo.Schools (NOLOCK) sc ON sc.Id = g.FK_SchoolId AND sc.IsDeleted = 0
	WHERE g.IsDeleted = 0
		AND g.Id= @Id
END 

CREATE PROCEDURE dbo.API_Student_GetById
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	
	SELECT
		st.*, g.*,sc.*
	FROM dbo.Students st
	LEFT JOIN dbo.Grades (NOLOCK) g ON g.Id = st.FK_GradeId AND g.IsDeleted = 0
	LEFT JOIN dbo.Schools (NOLOCK) sc ON sc.Id = g.FK_SchoolId AND sc.IsDeleted = 0
	WHERE st.IsDeleted = 0
		AND st.Id= @Id
END 

CREATE PROCEDURE dbo.API_Students_Filter
	@SearchKey NVARCHAR(255) = NULL,
	@SchoolId UNIQUEIDENTIFIER = NULL,
	@GradeId UNIQUEIDENTIFIER = NULL,
	@PageNumber INT,  
	@PageSize INT,
	@TotalRow INT OUT
AS
BEGIN
	
	SELECT
		st.Id
	INTO #FilterData
	FROM dbo.Students st
	LEFT JOIN dbo.Grades (NOLOCK) g ON g.Id = st.FK_GradeId AND g.IsDeleted = 0
	WHERE st.IsDeleted = 0
		AND (@SearchKey IS NULL 
				OR st.Name LIKE N'%' + @SearchKey + '%'
				OR st.StudentCode LIKE N'%' + @SearchKey + '%'
				OR st.Email LIKE N'%' + @SearchKey + '%')
		AND (@GradeId IS NULL OR @GradeId = st.FK_GradeId)
		AND (@SchoolId IS NULL OR @SchoolId = g.FK_SchoolId)
	
	SELECT @TotalRow = COUNT(1) FROM #FilterData  po

	SELECT st.*
	FROM #FilterData (NOLOCK) f
	INNER JOIN dbo.Students st ON f.Id = st.Id
	ORDER BY st.StudentCode
	OFFSET @PageNumber * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END 