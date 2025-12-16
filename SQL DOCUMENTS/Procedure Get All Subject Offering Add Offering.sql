
-- reading only--
CREATE or ALTER PROCEDURE GetAllSubjectOfferings
AS
BEGIN
    SELECT 
         tso.offeringID,
         tso.Semester,
         tso.subjectId,
         tso.SchoolYear,
         ts.Code,
         ts.Title
    FROM tblsubjectOffering tso
    INNER JOIN tblsubject ts ON tso.subjectId = ts.subjectId;
END
GO
