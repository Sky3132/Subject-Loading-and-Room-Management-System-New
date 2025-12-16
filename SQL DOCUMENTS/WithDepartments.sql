CREATE OR ALTER PROCEDURE GetSubjectsWithDepartment
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT
        d.DepartmentID,
        d.DepartmentName,
        p.ProgramID,
        p.ProgramName,
        s.subjectId,
        s.Code,
        s.Title,
        s.LectureUnits,
        s.LaboratoryUnits
    FROM tblDepartment d
   FULL OUTER JOIN tblProgram p
        ON d.ProgramID = p.ProgramID
    FULL OUTER JOIN tblsubject s
        ON s.DepartmentID = d.DepartmentID;
END
GO

