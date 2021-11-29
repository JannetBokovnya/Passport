using System;
using System.ComponentModel;
using Passpot.Controls;
using Services.ServiceReference;


namespace Passpot.Business
{
    //интерфейс INotifyPropertyChanged используется для валидации

    public class TextModel : INotifyPropertyChanged
    {
        private string _value;
        private string _title;


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

        public TextModel(FieldMetaDataItem metaData, ControlAttributeItem attrOneControl, string value)
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
                Validate(value);
                OnPropertyChanged("Value");
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
            IsValide = true;

            if (!IsValidate)
            {
                return;
            }

            int TypeId ;
            TypeId = MetaData.TYPEVALIDATION;
            switch (MetaData.TYPEVALIDATION)
            {
                case 3:
                    {
                        double valDouble;
                        if (double.TryParse(checkedValue, out valDouble))
                        {
                            if (valDouble.ToString().Equals(checkedValue))
                            {
                                break;
                            }
                        }
                        IsValide = false;
                        throw new Exception("Допустимы только  числа");
                    }
            }
        }
    }
}
