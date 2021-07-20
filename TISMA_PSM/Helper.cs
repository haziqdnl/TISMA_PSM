using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;

namespace TISMA_PSM
{
    /**
     * This is a Helper Class.
     * 
     * All non-SQL or global methods that are used repeatedly is implemented in this Class.
     * 
     */
    public class Helper
    {
        /**
         * To check whether given string is a valid address format
         * - contain alphanumeric
         * - allow #.,-
         * - allow whitespace
         */
        public static bool CheckPatternIsMultiLineText(string text)
        {
            return Regex.IsMatch(text, @"^[()'#./0-9a-zA-Z\s,-]+$");
        }

        /**
         * To check whether given string is an email format
         * Must match with this RegEx
         */
        public static bool CheckPatternIsEmail(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /**
         * To check whether given string is a IC No format
         *  - 12 digit
         */
        public static bool CheckPatternIsIcNo(string icNo)
        {
            return Regex.IsMatch(icNo, @"^([A-Z]|[0-9]){12}$");
        }


        /**
        * To check whether given string is a valid name format
        * Must match with this RegEx
        */
        public static bool CheckPatternIsName(string name)
        {
            return Regex.IsMatch(name, @"(^[A-Za-z'@/-]{1,16})([ ]{0,1})([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?([A-Za-z'@/-]{1,16})?([ ]{0,1})?");
        }

        /**
         * To check whether given string is a new password format
         *  - min 8 character
         *  - max 70 character
         *  - 1 upper case letter
         *  - 1 special character
         *  - 1 number
         */
        public static bool CheckPatternIsNewPassword(string newPassword)
        {
            return Regex.IsMatch(newPassword, @"^.*(?=.{8,70})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=_]).*$");
        }

        /**
         * To check whether given string is a passport format; ex: (A50715102)
         *  - start with a capital letter
         *  - min 8 digit
         *  - max 9 digit
         */
        public static bool CheckPatternIsPassport(string passport)
        {
            return Regex.IsMatch(passport, @"^([A-Z]|[0-9]){8,12}$");
        }

        /**
         * To check whether given string is a password format
         *  - contain alphanumeric and [ !*@#$%^&+=_ ] only
         *  - min 8 character
         *  - max 70 character
         */
        public static bool CheckPatternIsPassword(string password)
        {
            return Regex.IsMatch(password, @"^([a-z]|[A-Z]|[0-9]|[!*@#$%^&+=_]){8,70}$");
        }

        /**
         * To check whether given string is a valid Malaysia phone format; ex: (0123456789)
         *  - contain numbers only
         *  - start with 0 followed by 1
         *  - rest of number min 8, max 10
         */
        public static bool CheckPatternIsPhoneNo(string phoneNo)
        {
            return Regex.IsMatch(phoneNo, @"^[0]{1}[1]{1}[0-9]{8,10}$");
        }

        /**
         * To check whether given string is username format
         *  - contain alphanumeric and [ ._- ] only
         *  - min 3 character
         *  - max 30 character
         */
        public static bool CheckPatternIsUsername(string username)
        {
            return Regex.IsMatch(username, @"^([a-z]|[A-Z]|[0-9]|[._-]){1,30}$");
        }

        /**
         * To convert num into 3 digit string
         * Type: string
         */
        public static string ConvertNumString(int num)
        {
            string str;
            if (num <= 9)
                str = "00" + num.ToString();
            else if (num >= 10 && num <= 99)
                str = "0" + num.ToString();
            else
                str = num.ToString();
            return str;
        }

        /**
         * To decrypt URL param
         * Type: string
         */
        public static string DecryptURL(string encryptedURL)
        {
            try 
            {
                //- Key
                string EncryptionKey = "3NCRYPTTH1SURLP4R4M";

                //- Decryption logic
                encryptedURL = encryptedURL.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(encryptedURL);
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
                        encryptedURL = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
            return encryptedURL;
        }

        /**
         * To encrypt URL param
         * Type: string
         */
        public static string EncryptURL(string url)
        {
            string encrypted = "";
            try
            {
                //- Key
                string EncryptionKey = "3NCRYPTTH1SURLP4R4M";

                //- Encryption logic
                byte[] clearBytes = Encoding.Unicode.GetBytes(url);
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
         * Generate token
         * Type: string
         */
        public static string GenerateToken()
        {
            var allChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var resultToken = new string(
               Enumerable.Repeat(allChar, 64)
               .Select(token => token[random.Next(token.Length)]).ToArray());
            return resultToken.ToString();
        }

        /**
         * Return connection string value of 'tismaDB'
         * Type: string
         */
        public static string GetConnectionStringTismaDB()
        {
            return ConfigurationManager.ConnectionStrings["tismaDBConnectionString"].ConnectionString;
        }

        /**
         * Return connection string value of 'utmhr'
         * Type: string
         */
        public static string GetConnectionStringUtmHr()
        {
            return ConfigurationManager.ConnectionStrings["utmhrConnectionString"].ConnectionString;
        }

        /**
         * Return connection string value of 'utmacad'
         * Type: string
         */
        public static string GetConnectionStringUtmAcad()
        {
            return ConfigurationManager.ConnectionStrings["utmacadConnectionString"].ConnectionString;
        }

        /**
         * Return URL of Password Reset page
         * Type: string
         */
        public static string GetUrleMC()
        {
            string domainUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string pageUrl = "/e-MC.aspx?token=";
            return (domainUrl + pageUrl);
        }

        /**
         * Return URL of Password Reset page
         * Type: string
         */
        public static string GetUrlResetPassword()
        {
            string domainUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            string pageUrl = "/Reset-Password.aspx?token=";
            return (domainUrl + pageUrl);
        }

        /**
         * Generate random salt and hash a password
         * Type: string[]
         */
        public static string[] Hashing(string password)
        {
            string salt = Crypto.GenerateSalt();
            string hashed = Crypto.HashPassword(salt + password);
            string[] encryption = { hashed, salt };
            return encryption;
        }

        /**
         * To identify the status type: UTM-ACAD / UTM-HR
         * Type: string
         */
        public static string IdentifyStatus(string status)
        {
            if (status.Equals("utmacad"))
                return "UTM-ACAD";
            else if (status.Equals("utmhr"))
                return "UTM-HR";
            else
                return "";
        }

        /**
         * Send Email API
         * Type: void
         */
        public static void SendEmail(string subject, string body, string receiver)
        {
            try
            {
                //- System's email credentials (username & password)
                System.Net.NetworkCredential net = new System.Net.NetworkCredential
                {
                    UserName = "psmtisma@gmail.com",
                    Password = "psmtisma2021"
                };

                //- Email logic
                using (MailMessage mailMsg = new MailMessage())
                {
                    //- Content
                    mailMsg.From = new MailAddress("psmtisma@gmail.com");
                    mailMsg.To.Add(receiver);
                    mailMsg.Subject = subject;
                    mailMsg.Body = body;
                    mailMsg.IsBodyHtml = true;

                    //- SMTP
                    using (SmtpClient smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        UseDefaultCredentials = true,
                        Credentials = net
                    })
                    {
                        smtp.Send(mailMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
        }

        /**
         * Send SMS API
         * Type: string
         */
        public static string SendSMS(string recepient, string message)
        {
            string smsHubEmail = "haziqdnl73@gmail.com";
            string smsHubKey = "54346b6c7b5551395c91e8fe13e1638e";
            string uri = "https://www.smshubs.net/api/sendsms.php?email=" + smsHubEmail + "&key=" + smsHubKey + "&recipient=6" + recepient + "&message=" + message;
            string sentResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                    {
                        string resultMsg = responseReader.ReadToEnd();

                        int startIndex = 0;
                        int lastIndex = resultMsg.Length;

                        if (lastIndex > 0)
                            sentResult = resultMsg.Substring(startIndex, lastIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                //- Display exception message
                Debug.WriteLine("<<< ERROR : " + ex.ToString());
            }
            return sentResult;
        }

        /**
         * Printing 'SqlException' message
         * Type: void
         */
        public static void SqlExceptionMsg(SqlException ex)
        {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorMessages.Append("Index #" + i + "\n" +
                "Message: " + ex.Errors[i].Message + "\n" +
                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                "Source: " + ex.Errors[i].Source + "\n" +
                "Procedure: " + ex.Errors[i].Procedure + "\n");
            }
            Debug.WriteLine(errorMessages.ToString());
        }
    }
}