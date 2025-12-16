CREATE OR ALTER PROCEDURE DeleteSubjectOffering
    @offeringId INT         
AS
BEGIN
    SET NOCOUNT ON; 

    DELETE FROM tblsubjectOffering
    WHERE 
        offeringId = @offeringId;
END
GO