using System;
using System.ComponentModel;
using System.Globalization;
using Media.Resources;
using Services.ServiceReference;


namespace Passpot.Business
{
    public class NumberModel : INotifyPropertyChanged
    {
        private string _value;
        private string _title;
        private string separator = string.Empty;

        enum ValidatorType
        {
            onlyNumberValue = 1,
            pozitivNumber = 2,
            negativNumber = 3,
            fractionalPozitivNumber = 4,
            fractionalNegativNumber = 5,
            nullValue = 6,
            symbolValue = 7,
            maxLengthSymbolValue = 8,
            minLengthSymbolValue = 9,
        }




        //1 только числовые значения
        //2 только целые положительные 
        //3 отрицательные целые
        //4 дробные положительные
        //5 дробные отрицательные 
        //6 пустое значение
        //7 символьные значения
        //8 мах длина символьных значений
        //9 мин длина символ значений


        #region Ctor

        public NumberModel(FieldMetaDataItem metaData, ControlAttributeItem attrOneControl, string value)
        {
            OldValue = value;
            MetaData = metaData;
            Value = value;
            Title = metaData.TITUL;
            AttrOneControl = attrOneControl;
            IsValide = true;
        }

        #endregion


        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if ((_value == ProjectResources.PrivKmATxt)||
                    (_value == ProjectResources.Priv2)||
                    (_value == ProjectResources.PrivKmATxt)||
                    (_value ==ProjectResources.PrivKmBTxt)||
                    (_value ==ProjectResources.PrivDistTxt)||
                    (_value ==ProjectResources.PrivShirotATxt)||
                    (_value ==ProjectResources.PrivDolgATxt)||
                    (_value ==ProjectResources.PrivShirotBTxt)||
                    (_value ==ProjectResources.PrivDolgBTxt)||
                    (_value ==ProjectResources.PrivShirot2)||
                    (_value ==ProjectResources.PrivDolg2)
                    )
                {
                   
                }
                else
                {
                    separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                    string checkValue;
                    checkValue = value.Replace(".", separator);
                    checkValue = checkValue.Replace(",", separator);
                    checkValue = checkValue.Replace(" ", "");
                    _value = checkValue;
                    Validate(_value);
                    OnPropertyChanged("Value");  
                }
       
               
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public string OldValue { get; private set; }

        public FieldMetaDataItem MetaData { get; private set; }

        public ControlAttributeItem AttrOneControl { get; private set; }

        public bool IsValidate { get; set; }

        //валидность контрола, который "вытягиваем " наверх, при сохранении пасопрта
        public bool IsValide { get; private set; }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void Validate(string checkedValue)
        {
            string priv2 = ProjectResources.Priv2.Replace(".", separator);
            priv2 = priv2.Replace(",", separator); 
            priv2 = priv2.Replace(" ", "");


            string privKmATxt = ProjectResources.PrivKmATxt.Replace(".", separator);
            privKmATxt = privKmATxt.Replace(",", separator);
            privKmATxt = privKmATxt.Replace(" ", "");

            string privKmBTxt = ProjectResources.PrivKmBTxt.Replace(".", separator);
            privKmBTxt = privKmBTxt.Replace(",", separator);
            privKmBTxt = privKmBTxt.Replace(" ", "");

            string privDistTxt =  ProjectResources.PrivDistTxt.Replace(".", separator);
            privDistTxt = privDistTxt.Replace(",", separator);
            privDistTxt = privDistTxt.Replace(" ", "");

            string pPrivShirotATxt = ProjectResources.PrivShirotATxt.Replace(".", separator);
            pPrivShirotATxt = pPrivShirotATxt.Replace(",", separator);
            pPrivShirotATxt = pPrivShirotATxt.Replace(" ", "");

            string privDolgATxt = ProjectResources.PrivDolgATxt.Replace(".", separator);
            privDolgATxt = privDolgATxt.Replace(",", separator);
            privDolgATxt = privDolgATxt.Replace(" ", "");

            string privShirotBTxt = ProjectResources.PrivShirotBTxt.Replace(".", separator);
            privShirotBTxt = privShirotBTxt.Replace(",", separator);
            privShirotBTxt = privShirotBTxt.Replace(" ", "");

            string privDolgBTxt = ProjectResources.PrivDolgBTxt.Replace(".", separator);
            privDolgBTxt = privDolgBTxt.Replace(",", separator);
            privDolgBTxt = privDolgBTxt.Replace(" ", "");

            string privShirot2 = ProjectResources.PrivShirot2.Replace(".", separator);
            privShirot2 = privShirot2.Replace(",", separator);
            privShirot2 = privShirot2.Replace(" ", "");

            string privDolg2 = ProjectResources.PrivDolg2.Replace(".", separator);
            privDolg2 = privDolg2.Replace(",", separator);
            privDolg2 = privDolg2.Replace(" ", "");





            IsValide = true;
            if ((!IsValidate) || 
                (checkedValue == priv2) ||
                (checkedValue == privKmBTxt) ||
                (checkedValue == privDistTxt) ||
                (checkedValue == pPrivShirotATxt) ||
                (checkedValue == privDolgATxt) ||
                (checkedValue == privShirotBTxt) ||
                (checkedValue == privDolgBTxt) ||
                (checkedValue == privShirot2) ||
                (checkedValue == privDolg2)||
                (checkedValue == privKmATxt))
            {
                return;
            }
          
            int TypeId;
            //TypeId = MetaData.TYPEVALIDATION;
            TypeId = 1;
            //switch (MetaData.TYPEVALIDATION)
            switch (TypeId)
            {
                case (int)ValidatorType.pozitivNumber:
                    {
                        int valInt;
                        if (int.TryParse(checkedValue, out valInt))
                        {
                            if ((valInt >= 0) && (valInt.ToString().Equals(checkedValue)))
                            {
                                break;
                            }
                        }
                        IsValide = false;
                        throw new Exception("Допустимы только целые положительные числа");
                    }

                case (int)ValidatorType.onlyNumberValue:
                    {
                        double valDouble;
                        if (checkedValue == "") break;
                        //Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                        //separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

                        //checkedValue = checkedValue.Replace(".", separator);
                        //checkedValue = checkedValue.Replace(",", separator);


                        if (double.TryParse(checkedValue, out valDouble))
                        {
                            break;

                            //if (valDouble.ToString().Equals(checkedValue))
                            //{
                            //    break;
                            //}
                        }
                        IsValide = false;

                        throw new Exception(ProjectResources.NumberExeption); //"Допустимы только числа"
                    }
                case (int)ValidatorType.negativNumber:
                    {
                        int valInt;
                       
                            if (int.TryParse(checkedValue, out valInt))
                            {
                                if ((valInt <= 0) && (valInt.ToString().Equals(checkedValue)))
                                {
                                    break;
                                }
                            }     
                       
                       
                        IsValide = false;
                        throw new Exception("Допустимы только целые отрицательные числа");
                    }
            }
        }
    }
}

//String Source = "0,05".Replace(',', separator);
//if (separator.ToString() == ",")
//{
//    checkedValue = checkedValue.Replace(".", ",");
//}
//else
//{
//    if (separator.ToString() == ".")
//    {
//        checkedValue = checkedValue.Replace(",", ".");
//    }
//}

