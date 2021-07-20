namespace TISMA_PSM
{
    /**
     * Model class for Staff 
     */
    public class ModelStaff
    {
        //- 'pku_staff'
        public string sIcNo;
        public string sPassportNo;
        public string sAccountNo;
        public string sUsername;
        public string sPasswordEncrypted;
        public string sPasswordSalt;
        public string sStaffId;
        public string sTelNo;
        public string sEmail;
        public string sName;
        public string sDob;
        public int sAge;
        public string sGender;
        public string sMaritalStat;
        public string sReligion;
        public string sRace;
        public string sNationality;
        public string sAddress;
        public string sDesignation;
        public string sDepartment;
        public string sSession;
        public string sCategory;
        public string sBranch;

        //- 'pku_admin'
        public bool adminRole;

        //- 'pku_medical_officer'
        public bool moRole;

        //- 'pku_receptionist'
        public bool receptionistRole;
    }
}