namespace TISMA_PSM
{
    /**
     * Model class for Patient 
     */
    public class ModelPatient
    {
        //- 'patient'
        public string pIcNo;
        public string pAccountNo;
        public string pPassportNo;
        public string pTelNo;
        public string pEmail;
        public string pName;
        public string pDob;
        public int pAge;
        public string pGender;
        public string pMaritalStat;
        public string pReligion;
        public string pRace;
        public string pNationality;
        public string pAddress;
        public string pDesignation;
        public string pCategory;
        public string pSession;
        public string pBranch;
        public string pRemarks;

        //- 'patient_student'
        public bool utmAcadStat;
        public string pMatricNo;
        public string pFaculty;
        public string pCourse;
        public int pSemester;

        //- 'patient_staff'
        public bool utmHrStat;
        public string pStaffId;
        public string pDepartment;

        //- 'patient_public'
        public bool publicStat;
    }
}