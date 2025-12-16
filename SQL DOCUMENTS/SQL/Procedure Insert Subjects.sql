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
CREATE or alter PROCEDURE InsertSubjects 
    @Code int,
	@Title nvarchar(50),
	@Units int,
	@Department nvarchar(50),
	@Program nvarchar(50)
AS
BEGIN

	insert into tblsubject (Code, Title, Units, Department, Program)
	values (@Code, @Title, @Units, @Department, @Program);
END
GO              
