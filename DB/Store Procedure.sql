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
CREATE PROCEDURE AddToTismaPublic
(
	-- Table patient
	@pIcNo NVARCHAR(12),
	@pAccNo NVARCHAR(20),
	@pPassport NVARCHAR(15),
	@pTelNo NVARCHAR(15),
	@pEmail NVARCHAR(50),
	@pName NVARCHAR(max),
	@Dob DATE,
	@pAge INT,
	@pGender NVARCHAR(10),
	@pMarital NVARCHAR(10),
	@pReligion NVARCHAR(10),
	@pRace NVARCHAR(20),
	@pNationality NVARCHAR(50),
	@pAddress TEXT,
	@pDesignation NVARCHAR(50),
	@pCategory NVARCHAR(10),
	@pSession NVARCHAR(10),
	@pBranch NVARCHAR(10),
	@pRemarks TEXT,

	-- Table patient_staff
	@pPublicStat BIT
)
AS
	INSERT 
		INTO patient (p_ic_no, p_account_no, p_passport_no, p_tel_no, p_email, p_name, p_dob, p_age, p_gender, p_marital_stat, p_religion, p_race, p_nationality, p_address, p_designation, p_category, p_session, p_branch, p_remarks) 
		VALUES (@pIcNo, @pAccNo, @pPassport, @pTelNo, @pEmail, @pName, @Dob, @pAge, @pGender, @pMarital, @pReligion, @pRace, @pNationality, @pAddress, @pDesignation, @pCategory, @pSession, @pBranch, @pRemarks)

	INSERT 
		INTO patient_public(public_stat, fk_ic_no) 
		VALUES (@pPublicStat, @pIcNo)
RETURN
