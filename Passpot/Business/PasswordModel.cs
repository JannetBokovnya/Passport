using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Services.ServiceReference;
using System.Security.Cryptography;

namespace Passpot.Business
{
    //интерфейс INotifyPropertyChanged используется для валидации

    public class PasswordModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public PasswordModel(FieldMetaDataItem metaData, Dictionary<string, string> setValue)
        {

            MetaData = metaData;
            foreach (KeyValuePair<string, string> pair in setValue)
            {
                ReapidValue = pair.Value;
                Value = pair.Key;
            }


            IsValide = true;
        }

        public FieldMetaDataItem MetaData { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void ValidateProperty(string propertyName, object value)
        {
            Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = propertyName });
        }


        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Value"));
                ValidateProperty("Value", value);
                //Validate(value, ReapidValue);
                // OnPropertyChanged("Value");
            }
        }

        public string Hashp = "";


        public bool IsValidate { get; set; }

        public string ReapidValue
        {
            get { return _reapidValue; }
            set
            {
                _reapidValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReapidValue"));
            }
        }

        //валидность контрола, который "вытягиваем " наверх, при сохранении пасопрта
        public bool IsValide { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", _value, _reapidValue);
        }

        public  ValidationResult CheckPasswordConfirmation(string value, ValidationContext context)
        {


            if (string.CompareOrdinal(_reapidValue, value) != 0)
                return new ValidationResult("Password confirmation not equal to password.");

            return ValidationResult.Success;
        }

       
        //public FieldMetaDataItem MetaData { get; private set; }

        #region INotifyPropertyChanged Members
        
        private string _value;
        private string _reapidValue;
        private ServiceDataClient _serviceDataClient;

        protected ServiceDataClient ServiceDataClient
        {
            get
            {
                if (_serviceDataClient == null)
                    _serviceDataClient = new ServiceDataClient();
                return _serviceDataClient;
            }
        }

        string errors = "";

        public string Error
        {
            get { return errors; }
        }

        public string this[string columnName]
        {
           

            get
            {
                IsValide = true;
                string result = null;
                //if (columnName == "ReapidValue")
                //{
                    if (ReapidValue != Value)
                    {
                        IsValide = false;
                        result = "пароль не совпадает!";
                       // throw new Exception("пароль не совпадает!");
                    }
                 
                //}
                //else
                //{
                     //if (columnName == "Value")
                     //{
                     //    if (ReapidValue != Value)
                     //    {
                     //        IsValide = false;
                     //        //result = "пароль не совпадает!";

                     //        //throw new Exception("пароль не совпадает!");
                     //    }
                     //}
               // }
                return result;
            }
        }

        //private void OnPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        #endregion

        private void Validate(string valuePassword, string reapideValiePassword)
        {
            IsValide = true;

            if (!IsValidate)
            {
                return;
            }

            if (valuePassword != reapideValiePassword)
            {
                IsValide = false;
                throw new Exception("пароль не совпадает!");
            }
            else
            {
                //ServiceDataClient.SetHashPasswordCompleted += ServiceDataClient_SetHashPasswordCompleted;
                // ServiceDataClient.SetHashPasswordAsync(valuePassword); 
            }
        }
        void ServiceDataClient_SetHashPasswordCompleted(object sender, SetHashPasswordCompletedEventArgs e)
        {
            ServiceDataClient.SetHashPasswordCompleted -= ServiceDataClient_SetHashPasswordCompleted;
            if (e.Result.IsValid)
            {
                Hashp = e.Result.CurrentUser_OnLog_result.HashPassword_OnLogBase;
            }
            else
            {

            }
        }

       
    }
}


//public string CreateMD5Hash(string input)
//{
//    // Create a new instance of the MD5CryptoServiceProvider object.
//    System.Security.Cryptography.MD5 md5Hasher = MD5.Create();

//    // Convert the input string to a byte array and compute the hash.
//    byte[] data = md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));

//    // Create a new Stringbuilder to collect the bytes
//    // and create a string.
//    StringBuilder sBuilder = new StringBuilder();

//    // Loop through each byte of the hashed data 
//    // and format each one as a hexadecimal string.
//    for (int i = 0; i < data.Length; i++)
//    {
//        sBuilder.Append(data[i].ToString("x2"));
//    }

//    // Return the hexadecimal string.
//    return sBuilder.ToString();
//}

//public static string Hashing(string p_cValue)
//{
//    string l_res = string.Empty;
//    try
//    {
//        SHA1 hash = System.Security.Cryptography.SHA1.Create();
//        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
//        byte[] combined = encoder.GetBytes(p_cValue);
//        hash.ComputeHash(combined);
//        l_res = Convert.ToBase64String(hash.Hash);


//    }
//    catch (Exception e)
//    {

//        throw;
//    }
//    return l_res;
//}

//public static string Hashing(string p_cValue)
//{
//    string l_res = string.Empty;
//    try
//    {
//        //System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
//        //System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
//        //byte[] combined = encoder.GetBytes(p_cValue);
//        //hash.ComputeHash(combined);
//        //l_res = Convert.ToBase64String(hash.Hash);


//    }
//    catch (Exception e)
//    {

//        throw;
//    }
//    return l_res;
//}

//public static byte[] GetHash(string str)
//{
//    return GetHash(Encoding.UTF8.GetBytes(str));
//}

//public static byte[] GetHash(byte[] bytes)
//{
//    using (var sha = SHA1.Create())
//    {
//        var hash = sha.ComputeHash(sha.ComputeHash(bytes));
//        return hash;
//    }
//}

//public static String ComputeSHA1(string textToHash)
//{
//    SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
//    byte[] byteV = System.Text.Encoding.UTF8.GetBytes(textToHash);
//    byte[] byteH = SHA1.ComputeHash(byteV);
//    SHA1.Clear();
//    return Convert.ToBase64String(byteH);
//} 


//public static string Hashing(string p_cValue)
//{
//    string l_res = string.Empty;
//    try
//    {

//        System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
//        System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
//        byte[] combined = encoder.GetBytes(p_cValue);
//        hash.ComputeHash(combined);
//        l_res = Convert.ToBase64String(hash.Hash);
//    }
//    catch (Exception e)
//    {

//        throw;
//    }
//    return l_res;
//}