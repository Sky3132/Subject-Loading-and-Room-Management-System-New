-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER PROCEDURE sp_GetSubjectsWithDepartment
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

