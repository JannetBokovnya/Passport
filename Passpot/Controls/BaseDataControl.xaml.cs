using System.Collections.Generic;
using System.Windows.Browser;
using System.Windows.Controls;
using Passpot.Business;
using System;
using System.Globalization;
using Passpot.Controls.Priv;
using Passpot.Model;
using Services.ServiceReference;

namespace Passpot.Controls
{
    public partial class BaseDataControl : UserControl
    {
        public BaseDataControl()
        {
            InitializeComponent();
        }

        private readonly List<Control> _controls = new List<Control>();

        #region Enums

        enum ControlType
        {
            Text = 1,
            CheckBox = 2,
            DataPicker = 3,
            ComboBox = 4,
            Label = 6,
            Number = 7,
            PrivControl = 9,
            DataTimePicker = 10,
            HashControl = 11,
            //для горизонтальных связей
            TextAndButton = 5,
            Grid = 8,
            LinkButton = 12,
            TextBlock = 13,
            LinkControlInVisible = 14 //- нужно для админдоступа - невидимый контрол но данные передаются

        }

        #endregion Enums

        //валидация
        public bool Validate()
        {
            bool hasErrors = false;
            // для проверки смотрим пока на один контрол
            foreach (var control in Controls)
            {
                var a = control as NumberControl;
                if (a != null)
                {
                    if (!a.Model.IsValide)
                    {
                        hasErrors = true;
                        break;
                    }
                }
            }

            return (hasErrors == false);

        }
        public List<Control> Controls
        {
            get { return _controls; }
        }
        public static BaseDataControl Create(PassportDetailModel passportModel, bool editable)
        {
            if (editable)
            {
                return GetEditableDataEditControl(passportModel);
            }
            return GetNotEditableDataEditControl(passportModel);
        }

        private static BaseDataControl GetEditableDataEditControl(PassportDetailModel passportModel)
        {
            passportModel.MainModel.Indicator(true);
            //название связи(если объект связан...)
            passportModel.MainModel.Report("создаем GetEditableDataEditControl - начало");
            string nameConnect1 = " Будем передавать уже полное название вычесленное в базе";

            var c = new BaseDataControl();

            for (int i = 0; i < passportModel.MetaEditDataList.Count; i++)
            {
                FieldMetaDataItem metaData = passportModel.MetaEditDataList[i];
                string valueStr = "";
                if (passportModel.PassportEditData != null)
                {

                    if ((metaData.FLDNAME != "") && (metaData.TYPECONTROL != 9) && (metaData.TYPECONTROL != 3))
                    {

                        valueStr = passportModel.PassportEditData[metaData.FLDNAME.ToUpper()];
                    }
                    else
                    {

                        if (metaData.TYPECONTROL == 3)
                        {
                            DateTime dateValue;
                            if (!String.IsNullOrEmpty(passportModel.PassportEditData[metaData.FLDNAME.ToUpper()]))
                            {

                                CultureInfo culture = new CultureInfo("ru-RU");
                                DateTimeStyles styles = DateTimeStyles.None;

                                if (DateTime.TryParse(passportModel.PassportEditData[metaData.FLDNAME.ToUpper()], culture, styles, out dateValue))
                                {
                                    //valueStr = dateValue.ToString("dd.MM.yyyy");
                                    valueStr = dateValue.ToString();
                                }
                                else
                                {
                                    valueStr = passportModel.PassportEditData[metaData.FLDNAME.ToUpper()];
                                }
                            }
                            else
                            {
                                valueStr = passportModel.PassportEditData[metaData.FLDNAME.ToUpper()];
                            }
                        }
                    }
                }
                if (metaData.IS_VISIBLE == 1)
                {
                    switch (metaData.TYPECONTROL)
                    {
                        case (int)ControlType.Text:
                            {

                                for (int j = 0; j < passportModel.ControlAttributeListAll.Count; j++)
                                {
                                    ControlAttributeItem attrAllContr = passportModel.ControlAttributeListAll[j];
                                    switch (attrAllContr.TYPECONTROL)
                                    {
                                        case 1:
                                            {
                                                Control tc = TextControl.CreateControl(metaData, attrAllContr, valueStr);
                                                c.passportHolder.Children.Add(tc);
                                                c.Controls.Add(tc);
                                                break;
                                            }
                                    }
                                }

                                break;
                            }

                        case (int)ControlType.Number:
                            {

                                for (int j = 0; j < passportModel.ControlAttributeListAll.Count; j++)
                                {
                                    ControlAttributeItem attrAllContr = passportModel.ControlAttributeListAll[j];
                                    switch (attrAllContr.TYPECONTROL)
                                    {
                                        case 7:
                                            {
                                                Control tc = NumberControl.CreateControl(metaData, attrAllContr, valueStr);
                                                c.passportHolder.Children.Add(tc);
                                                c.Controls.Add(tc);
                                                break;
                                            }
                                    }
                                }

                                break;
                            }

                        case (int)ControlType.CheckBox:
                            {
                                Control tc = CheckBoxControl.CreateControl(metaData, valueStr);
                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }
                        case (int)ControlType.DataPicker:
                            {
                                Control tc = DataPickerControl.CreateControl(metaData, valueStr);
                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }
                        case (int)ControlType.DataTimePicker:
                            {
                                Control tc = DataTimePickerControl.CreateControl(metaData, valueStr);
                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }

                        case (int)ControlType.HashControl:
                            {
                                Control tc = HashControl.CreateControl(metaData, passportModel, valueStr);
                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }
                        case (int)ControlType.ComboBox:
                            {
                                Control tc = ComboBoxControl.CreateControl(metaData, passportModel.DictDataList, passportModel, valueStr);



                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }
                        case (int)ControlType.TextAndButton:
                            {

                                for (int j = 0; j < passportModel.ControlAttributeListAll.Count; j++)
                                {
                                    ControlAttributeItem attrAllContr = passportModel.ControlAttributeListAll[j];
                                    switch (attrAllContr.TYPECONTROL)
                                    {
                                        case 5:
                                            {
                                                //Control tc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, true);
                                                //Control tc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, true);
                                                Control tc = TextBoxAndButton.CreateControl(metaData, attrAllContr, passportModel, true);
                                                c.passportHolder.Children.Add(tc);
                                                c.Controls.Add(tc);

                                                break;
                                            }
                                    }
                                }
                                break;

                            }


                        case (int)ControlType.Label:
                            {

                                for (int j = 0; j < passportModel.ControlAttributeListAll.Count; j++)
                                {
                                    ControlAttributeItem attrAllContr = passportModel.ControlAttributeListAll[j];
                                    switch (attrAllContr.TYPECONTROL)
                                    {
                                        case 6:
                                            {
                                                Control tc = TextLabelControl.CreateControl(metaData, valueStr);
                                                c.passportHolder.Children.Add(tc);
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                        #region контролы для горизонтальных связей
                        case (int)ControlType.Grid:
                            {

                                if (passportModel.PassportKey == "0")
                                {
                                    //грид
                                    // Control dlc = GridControl.CreateControl(metaData, passportModel, false);
                                    //новый контрол связи
                                    Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, false);
                                    //Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }
                                else
                                {
                                    //Control dlc = GridControl.CreateControl(metaData, passportModel, true);
                                    Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, true);
                                    //Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, true);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }


                                break;

                            }

                        case (int)ControlType.LinkButton:
                            {

                                if (passportModel.PassportKey == "0")
                                {

                                    Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }
                                else
                                {

                                    Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, true);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }


                                break;

                            }

                        case (int)ControlType.TextBlock:
                            {

                                if (passportModel.PassportKey == "0")
                                {

                                    //новый контрол связи
                                    Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, false);
                                    //Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }
                                else
                                {

                                    Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, true);

                                    c.passportHolder.Children.Add(dlc);
                                    c.Controls.Add(dlc);
                                }


                                break;

                            }


                        #endregion

                        case (int)ControlType.PrivControl:
                            {
                                if (passportModel.PassportKey == "0")
                                {
                                    // Control pc = PrivControl.CreateControl(passportModel, false);
                                    Control pc = PrivNewControlTexBlock.CreateControl(metaData, passportModel, false);
                                    c.passportHolder.Children.Add(pc);
                                    c.Controls.Add(pc);
                                }
                                else
                                {
                                    // Control pc = PrivControl.CreateControl(passportModel, true);
                                    Control pc = PrivNewControlTexBlock.CreateControl(metaData, passportModel, true);
                                    c.passportHolder.Children.Add(pc);
                                    c.Controls.Add(pc);
                                }


                                break;

                            }

                        case (int)ControlType.LinkControlInVisible:
                            {
                                Control tc = LinkControlInVisible.CreateControl(metaData, passportModel);
                                c.passportHolder.Children.Add(tc);
                                c.Controls.Add(tc);
                                break;
                            }
                    }
                }
            }

            passportModel.MainModel.Report("создаем GetEditableDataEditControl - создался");
            passportModel.MainModel.Indicator(false);
            return c;
        }

        //создание контрола для просмотра
        private static BaseDataControl GetNotEditableDataEditControl(PassportDetailModel passportModel)
        {
            passportModel.MainModel.Indicator(true);
            passportModel.MainModel.Report("создаем GetNotEditableDataEditControl - начало");
            //название связи(если объект связан...)
            string nameConnect = " Будем передавать уже полное название вычесленное в базе";

            var c = new BaseDataControl();

            for (int i = 0; i < passportModel.MetaDataList.Count; i++)
            {
                FieldMetaDataItem meta = passportModel.MetaDataList[i];
                string valueStr = "";
                if (passportModel.PassportData != null)
                {

                    if ((meta.FLDNAME != "") && (meta.TYPECONTROL != 9) && (meta.TYPECONTROL != 3) && (meta.TYPECONTROL != 13))
                    {
                        valueStr = passportModel.PassportData[meta.FLDNAME.ToUpper()];
                    }
                    else
                    {
                        if (meta.TYPECONTROL == 3)
                        {
                            CultureInfo culture = new CultureInfo("ru-RU");
                            DateTimeStyles styles = DateTimeStyles.None;

                            DateTime dateValue;
                            if (DateTime.TryParse(passportModel.PassportData[meta.FLDNAME.ToUpper()], culture, styles, out dateValue))
                            {
                                //valueStr = dateValue.ToString("dd.MM.yyyy"); 
                                valueStr = dateValue.ToShortDateString();
                            }
                            else
                            {
                                valueStr = passportModel.PassportData[meta.FLDNAME.ToUpper()];
                            }
                        }
                    }
                }

                FieldMetaDataItem metaData = passportModel.MetaDataList[i];

                if (metaData.IS_VISIBLE == 1)
                {
                    {
                        for (int j = 0; j < passportModel.ControlAttributeListAll.Count; j++)
                        {
                            ControlAttributeItem attrAllContr = passportModel.ControlAttributeListAll[j];
                            switch (attrAllContr.TYPECONTROL)
                            {

                                case 5:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.TextAndButton)
                                        {
                                            // Control tcb = TextBlockControl.CreateTextBlockControl(metaData, passportModel, false);
                                            //Control tcb = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);
                                            Control tcb = TextBoxAndButton.CreateControl(metaData, attrAllContr, passportModel, false);
                                            c.passportHolder.Children.Add(tcb);
                                            c.Controls.Add(tcb);
                                        }
                                        break;
                                    }
                                case 6:
                                    {
                                        // если комбобокс то вычисляем из коллекции словарей нужное значение(не ключ! а значение!!! и передаем его)
                                        if (meta.TYPECONTROL == (int)ControlType.ComboBox)
                                        {
                                            Control tc1 = TextLabelControl.CreateControl(metaData, valueStr);
                                            c.passportHolder.Children.Add(tc1);
                                            break;
                                        }
                                        if (meta.TYPECONTROL == (int)ControlType.CheckBox)
                                        {
                                            Control cb = CheckBoxControl.CreateControl(metaData, valueStr);
                                            cb.IsEnabled = false;
                                            c.passportHolder.Children.Add(cb);
                                            c.Controls.Add(cb);
                                            break;
                                        }

                                        if (meta.TYPECONTROL == (int)ControlType.Grid)
                                        {
                                            break;
                                        }
                                        if (meta.TYPECONTROL == (int)ControlType.TextAndButton)
                                        {
                                            break;
                                        }
                                        if (meta.TYPECONTROL == (int)ControlType.PrivControl)
                                        {
                                            break;
                                        }
                                        if (meta.TYPECONTROL == (int)ControlType.TextBlock)
                                        {
                                            break;
                                        }
                                        if (meta.TYPECONTROL == (int)ControlType.LinkButton)
                                        {
                                            break;
                                        }

                                        if (meta.TYPECONTROL == (int)ControlType.LinkControlInVisible)
                                        {
                                            break;
                                        }
                                        Control tc = TextLabelControl.CreateControl(metaData, valueStr);
                                        c.passportHolder.Children.Add(tc);
                                        break;
                                    }

                                case 8:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.Grid)
                                        {
                                            //контрол типа грид не нужен (пока)
                                            //Control dlc = GridControl.CreateControl(metaData, passportModel, false);

                                            //c.passportHolder.Children.Add(dlc);
                                           // c.Controls.Add(dlc);

                                            //насильно тип контролов новых для связей
                                             Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, false);
                                            //Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);
                                              c.passportHolder.Children.Add(dlc);
                                             c.Controls.Add(dlc);
                                        }
                                        break;
                                    }
                                case 12:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.LinkButton)
                                        {
                                            Control dlc = TextBlockAsButtonControl.CreateTextBlockControl(metaData, passportModel, false);
                                            c.passportHolder.Children.Add(dlc);
                                            c.Controls.Add(dlc);
                                        }
                                        break;
                                    }
                                case 13:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.TextBlock)
                                        {
                                            Control dlc = TextBlockControl.CreateTextBlockControl(metaData, passportModel, false);
                                            c.passportHolder.Children.Add(dlc);
                                            c.Controls.Add(dlc);
                                        }
                                        break;
                                    }
                                case 14:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.LinkControlInVisible)
                                        {
                                            Control dlc = LinkControlInVisible.CreateControl(metaData, passportModel);
                                            c.passportHolder.Children.Add(dlc);
                                            c.Controls.Add(dlc);
                                        }
                                        break;
                                    }
                                case 9:
                                    {
                                        if (meta.TYPECONTROL == (int)ControlType.PrivControl)
                                        {
                                            //Control pc = PrivControl.CreateControl(passportModel, false);
                                            //привязка для нового контрола!
                                            Control pc = PrivNewControlTexBlock.CreateControl(metaData, passportModel, false);
                                            c.passportHolder.Children.Add(pc);
                                            c.Controls.Add(pc);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            passportModel.MainModel.Report("создаем GetNotEditableDataEditControl - создался");
            passportModel.MainModel.Indicator(false);
            return c;
        }

    }
}
