using Newtonsoft.Json;
using Social.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Social.Helpers
{
    public class Support
    {

        protected static readonly YeauplatoContext context = new YeauplatoContext();
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly string DefaultImage = "~/assets/images/demo/shop/products/600x900/p10.jpg";
        public static readonly int Base = Alphabet.Length;
        public Support() { }
        public static string Key { get; set; } = "J#$_A;.<";
        public static int GetID()
        {
            var date = DateTime.UtcNow;
            var year = date.Year;
            var day = date.Day;
            var month = date.Month;
            var hour = date.Hour;
            var min = date.Minute;
            var sec = date.Second;
            var mill = date.Millisecond;
            var newId = year + month + day + hour + min + sec + mill;
            return newId;
        }
        public static string Encode(int i)
        {
            if (i == 0) return Alphabet[0].ToString();
            var s = new StringBuilder();// string.Empty;
            while (i > 0)
            {
                s.Insert(0, Alphabet[i % Base]);
                i = i / Base;
            }
            return s.ToString();
        }
        public static int Decode(string s)
        {
            var i = 0;
            foreach (var c in s)
            {
                i = (i * Base) + Alphabet.IndexOf(c);
            }
            return i;
        }
        public enum MessageType { ERROR, SUCCESS, INFO, WARNING }
        public static string ShowMessage(string message, MessageType type)
        {
            var _type = "";
            switch (type)
            {
                case MessageType.ERROR:
                    _type = "danger";
                    break;
                case MessageType.SUCCESS:
                    _type = MessageType.SUCCESS.ToString().ToLower();
                    break;
                case MessageType.INFO:
                    _type = MessageType.INFO.ToString().ToLower();
                    break;
                case MessageType.WARNING:
                    _type = MessageType.WARNING.ToString().ToLower();
                    break;
                default:
                    break;
            }
            var data = $@"
            <div class='alert alert-{_type} margin-bottom-30'>
	            <button type='button' class='close' data-dismiss='alert'>
		            <span aria-hidden='true'>×</span>
		            <span class='sr-only'>Close</span>
	            </button>
	            {message}
            </div>";
            return data;
        }
        public static string GetMD5(string text)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public static string GetRelativetime(DateTime date)
        {
            string result = string.Empty;
            var timeSpan = DateTime.UtcNow.Subtract(date);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    string.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    string.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    string.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    string.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    string.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }
            return result;
        }
        public static string GetArticleImage(Article article)
        {
            return "";
        }
        public static (string message, bool check) CheckPassword(string password)
        {
            var reg = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            if (!reg.Match(password).Success)
            {
                return (ShowMessage("Password must contain a minimum of eight characters, at least one letter and one number", MessageType.ERROR), false);
            }
            else { return ("", true); }
        }
        public static (string message, bool check) CheckEmail(string email)
        {
            var email_format = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
            var reg = new Regex(email_format);
            if (!reg.Match(email).Success)
            {
                return (ShowMessage("The email is wrong!", MessageType.ERROR), false);
            }
            else { return ("", true); }
        }
    }
    public class Encryption
    {
        public static string Decrypt(string text, string _key)
        {
            text = text.Replace("~", "=").Replace("$", "+").Replace("€", "/");
            MemoryStream memStream = null;
            try
            {
                byte[] key = { };
                byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
                string encryptKey = _key; // MUST be 8 characters
                key = Encoding.UTF8.GetBytes(encryptKey);
                byte[] byteInput = new byte[text.Length];
                byteInput = Convert.FromBase64String(text);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                memStream = new MemoryStream();
                ICryptoTransform transform = provider.CreateDecryptor(key, IV);
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(byteInput, 0, byteInput.Length);
                cryptoStream.FlushFinalBlock();
            }
            catch (Exception)
            {
                return "";
            }

            Encoding encoding1 = Encoding.UTF8;
            return encoding1.GetString(memStream.ToArray());
        }
        public static string Encrypt(string text, string _key)
        {
            MemoryStream memStream = null;
            try
            {
                byte[] key = { };
                byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
                string encryptKey = _key; // MUST be 8 characters
                key = Encoding.UTF8.GetBytes(encryptKey);
                byte[] byteInput = Encoding.UTF8.GetBytes(text);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                memStream = new MemoryStream();
                ICryptoTransform transform = provider.CreateEncryptor(key, IV);
                CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(byteInput, 0, byteInput.Length);
                cryptoStream.FlushFinalBlock();

            }
            catch (Exception)
            {
                return "";
            }
            return Convert.ToBase64String(memStream.ToArray()).Replace("=", "~").Replace("+", "$").Replace("/", "€");
        }
    }
    public class Posting
    {
        public string Heading { get; set; }
        public string Category { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
    public class Message
    {
        public string _Message { get; set; }
    }
    public class Register
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime Time { get; set; }
        public string Email { get; set; }
    }
    public static class ExtensionMethods
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return String.Join("&", properties.ToArray());
        }
        public static string ToJson(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public static T FromJson<T>(this string obj)
        {
            var json = JsonConvert.DeserializeObject<T>(obj);
            return json;
        }
        public static string ToEncryptJson(this object obj, string key)
        {
            var json = JsonConvert.SerializeObject(obj);
            var encryptedData = Encryption.Encrypt(json, key);
            return encryptedData;
        }
        public static T ToDecryptedJsonObj<T>(this string obj, string key)
        {
            var decryptedData = Encryption.Decrypt(obj, key);
            var json = JsonConvert.DeserializeObject<T>(decryptedData);
            return json;
        }
    }
}