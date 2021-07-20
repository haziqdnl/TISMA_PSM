namespace TISMA_PSM
{
    /**
     * Model class for e-MC 
     */
    public class ModelEMC
    {
        public string serialNo;
        public string urlHashed;
        public string urlSalt;
        public byte[] qrCode;
        public string password;
        public string dateFrom;
        public string dateTo;
        public int period;
        public string timeCreated;
        public string dateCreated;
        public string comment;
        public string pIcNo;
        public string sIcNo;
        public int clinicalId;
    }
}