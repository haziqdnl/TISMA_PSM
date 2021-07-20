using QRCoder;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static TISMA_PSM.Helper;

namespace TISMA_PSM
{
    /**
     * This is a Controller Class for e-MC
     * 
     * All SQL and public methods related only to e-MC model 
     * that are used repeatedly are implemented in this Class.
     * 
     */
    public class ControllerEMC
    {
        /**
         * Validate if e-MC has generated
         * Type: bool 
         */
        public static bool CheckEMCGenerated(string icNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckEMCGenerated", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@IcNo", icNo.Trim());
                        status = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return status;
        }

        /**
         * Validate if Serial No. already exist
         * Type: bool 
         */
        public static bool CheckEMCHashedUrlNotExist(string hashedUrl)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT url_hashed FROM emc WHERE url_hashed = '" + hashedUrl + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                                status = false;
                            else
                                status = true;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return status;
        }

        /**
         * Validate if Serial No. already exist
         * Type: bool 
         */
        public static bool CheckSerialNotExist(string _serialNo)
        {
            bool status = false;
            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Search Query
                    using (SqlCommand cmd = new SqlCommand("CheckSerialNotExist", con) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@SerialNo", _serialNo.Trim());
                        status = Convert.ToBoolean(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return status;
        }

        /**
         * To decrypt the e-MC password
         * Type: string
         */
        public static string DecryptEMCPassword(string encryption)
        {
            try
            {
                //- Key
                string EncryptionKey = "3NCRYPTP@55W0RD3MC";

                //- Decryption logic
                encryption = encryption.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(encryption);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        encryption = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
            return encryption;
        }

        /**
         * To encrypt the e-MC password
         * Type: string
         */
        public static string EncryptEMCPassword(string password)
        {
            string encrypted = "";
            try
            {
                //- Key
                string EncryptionKey = "3NCRYPTP@55W0RD3MC";

                //- Encryption logic
                byte[] clearBytes = Encoding.Unicode.GetBytes(password);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        encrypted = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
            return encrypted;
        }

        /**
         * To generate password for e-MC
         * Type: string
         */
        public static string[] GeneratePasswordEMC()
        {
            var allChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultPwd = new string(Enumerable.Repeat(allChar, 8).Select(token => token[random.Next(token.Length)]).ToArray());

            string password = resultPwd.ToString();
            string encryptedPassword = EncryptEMCPassword(password);
            string[] generatedPassword = { password, encryptedPassword };

            return generatedPassword;
        }

        /**
         * To generate QR code for e-MC.
         * Type: string
         */
        public static byte[] GenerateQRCode(string urlParam)
        {
            string content = GetUrleMC() + urlParam;
            byte[] byteImage = null;

            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                using (System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image())
                {
                    imgQRCode.Height = 150;
                    imgQRCode.Width = 150;
                }
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byteImage = new byte[ms.ToArray().Length];
                        byteImage = ms.ToArray();
                        //imgQRCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage); This will be use for displaying the QR Code in e-MC
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
            return byteImage;
        }

        /**
         * To generate serial no. for e-MC
         * Type: string
         */
        public static string GenerateSerialNo()
        {
            //- Step 1: Get today's date
            DateTime dateCreated = DateTime.Now;

            //- Step 2: Set today date pattern
            string today = dateCreated.ToString("yy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            today = today.Remove(5, 1); // yyyy/MM_dd
            today = today.Remove(2, 1); // yyyy_MMdd

            //- Step 3: Append today date pattern to another string pattern
            string _serialNo;
            _serialNo = "PKUeMC" + today;

            //- Step 4: Generate and append a string no. to _serialNo
            //-- Step 4.1: Produce temp SerialNo and initialize a string no.
            int n = 1;
            string strNum = ConvertNumString(n);
            string tempSerialNo = _serialNo + strNum;

            //-- Step 4.2: Check is Serial No pattern not exist?
            while (CheckSerialNotExist(tempSerialNo).Equals(false))
            {
                //-- If exist (the CheckIsQueueNotExist return false)
                n += 1;
                strNum = ConvertNumString(n);
                tempSerialNo = _serialNo + strNum;
            }
            _serialNo = tempSerialNo;
            return _serialNo;
        }

        /**
         * Get e-MC info from DB as a ModelEMC object
         * Type: ModelEMC
         */
        public static ModelEMC GetEMCInfoByIcNo(string icNo)
        {
            //- Object e-MC model
            ModelEMC emc = new ModelEMC();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM emc WHERE fk_p_ic_no = '" + icNo + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            DateTime.TryParse(sdr["date_from"].ToString(), out DateTime dateFrom);
                            DateTime.TryParse(sdr["date_to"].ToString(), out DateTime dateTo);
                            DateTime.TryParse(sdr["time_created"].ToString(), out DateTime timeCreated);
                            DateTime.TryParse(sdr["date_created"].ToString(), out DateTime dateCreated);

                            emc.serialNo = sdr["serial_no"].ToString();
                            emc.urlHashed = sdr["url_hashed"].ToString();
                            emc.password = sdr["emc_password"].ToString();
                            emc.dateFrom = dateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateTo = dateTo.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.period = int.Parse(sdr["emc_period"].ToString());
                            emc.timeCreated = timeCreated.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateCreated = dateCreated.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            emc.comment = sdr["comment"].ToString();
                            emc.pIcNo = sdr["fk_p_ic_no"].ToString();
                            emc.sIcNo = sdr["fk_s_ic_no"].ToString();
                            emc.clinicalId = int.Parse(sdr["fk_clinical_id"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return emc;
        }

        /**
         * Get e-MC info from DB as a ModelEMC object by using url as param
         * Type: ModelEMC
         */
        public static ModelEMC GetEMCInfoByUrl(string url)
        {
            //- Object e-MC model
            ModelEMC emc = new ModelEMC();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM emc WHERE url_hashed = '" + url + "'", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            DateTime.TryParse(sdr["date_from"].ToString(), out DateTime dateFrom);
                            DateTime.TryParse(sdr["date_to"].ToString(), out DateTime dateTo);
                            DateTime.TryParse(sdr["time_created"].ToString(), out DateTime timeCreated);
                            DateTime.TryParse(sdr["date_created"].ToString(), out DateTime dateCreated);

                            emc.serialNo = sdr["serial_no"].ToString();
                            emc.urlHashed = sdr["url_hashed"].ToString();
                            emc.password = sdr["emc_password"].ToString();
                            emc.dateFrom = dateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateTo = dateTo.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.period = int.Parse(sdr["emc_period"].ToString());
                            emc.timeCreated = timeCreated.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateCreated = dateCreated.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            emc.comment = sdr["comment"].ToString();
                            emc.pIcNo = sdr["fk_p_ic_no"].ToString();
                            emc.sIcNo = sdr["fk_s_ic_no"].ToString();
                            emc.clinicalId = int.Parse(sdr["fk_clinical_id"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return emc;
        }

        /**
         * Get e-MC info from DB as a ModelEMC object by using url as param
         * Type: ModelEMC
         */
        public static ModelEMC GetEMCInfoForTodayByIcNo(string icNo)
        {
            //- Object e-MC model
            ModelEMC emc = new ModelEMC();

            //- DB Exception/Error handling
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionStringTismaDB()))
                {
                    con.Open();
                    //- Get Query
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM emc WHERE fk_p_ic_no = '" + icNo + "' AND CONVERT(DATE, date_created) = CONVERT(DATE, GETDATE()) ORDER BY date_created DESC", con))
                    {
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            //- Parse data
                            sdr.Read();

                            DateTime.TryParse(sdr["date_from"].ToString(), out DateTime dateFrom);
                            DateTime.TryParse(sdr["date_to"].ToString(), out DateTime dateTo);
                            DateTime.TryParse(sdr["time_created"].ToString(), out DateTime timeCreated);
                            DateTime.TryParse(sdr["date_created"].ToString(), out DateTime dateCreated);

                            emc.serialNo = sdr["serial_no"].ToString();
                            emc.urlHashed = sdr["url_hashed"].ToString();
                            emc.password = sdr["emc_password"].ToString();
                            emc.dateFrom = dateFrom.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateTo = dateTo.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            emc.period = int.Parse(sdr["emc_period"].ToString());
                            emc.timeCreated = timeCreated.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                            emc.dateCreated = dateCreated.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            emc.comment = sdr["comment"].ToString();
                            emc.pIcNo = sdr["fk_p_ic_no"].ToString();
                            emc.sIcNo = sdr["fk_s_ic_no"].ToString();
                            emc.clinicalId = int.Parse(sdr["fk_clinical_id"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //- Display handling-error message
                SqlExceptionMsg(ex);
            }
            return emc;
        }
    }
}