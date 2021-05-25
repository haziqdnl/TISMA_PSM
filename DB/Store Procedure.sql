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
-- Author:		Haziq Danial
-- Create date: 11 May 2021
-- Description:	
-- =============================================
CREATE PROCEDURE CheckIsStaffMO
	@IcNo NVARCHAR(12)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT fk_ic_no FROM pku_medical_officer WHERE fk_ic_no = @IcNo)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END
END