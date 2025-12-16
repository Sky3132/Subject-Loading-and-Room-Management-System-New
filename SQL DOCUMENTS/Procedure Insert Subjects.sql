CREATE or alter PROCEDURE InsertSubjects 
    @Code int,
	@Title nvarchar(50),
	@Department nvarchar(50),
	@Program nvarchar(50),
	@LectureUnits int,
	@LaboratoryUnits int,
	@DepartmentID int
AS
BEGIN
	insert into tblsubject (Code, Title, DepartmentID)
	values (@Code, @Title, @DepartmentID);


	insert into tblDepartmentAndProgram(Department)
	values (@Department, @Program, @LectureUnits, @LaboratoryUnits);
END
GO              
