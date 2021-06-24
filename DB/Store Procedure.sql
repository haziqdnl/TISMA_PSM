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
CREATE PROCEDURE CheckEMCGenerated
	@IcNo NVARCHAR(12)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT TOP 1 * FROM emc WHERE fk_p_ic_no = @IcNo AND CONVERT(DATE, date_from) <= CONVERT(DATE, GETDATE()) AND CONVERT(DATE, date_to) >= CONVERT(DATE, GETDATE()) ORDER BY date_created DESC)
	BEGIN
		SELECT 'TRUE'
	END
	ELSE
	BEGIN
		SELECT 'FALSE'
	END
END