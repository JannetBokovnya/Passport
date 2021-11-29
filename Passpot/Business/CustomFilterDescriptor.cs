using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Passpot.Business
{
    //public class CustomFilterDescriptor : FilterDescriptorBase
    //{
        public class CustomFilterDescriptor : FilterDescriptorBase
        {
            private readonly CompositeFilterDescriptor compositeFilterDesriptor;
            private static readonly ConstantExpression TrueExpression = System.Linq.Expressions.Expression.Constant(true);
            private string filterValue;

            public CustomFilterDescriptor(IEnumerable<GridViewDataColumn> columns)
            {
                this.compositeFilterDesriptor = new CompositeFilterDescriptor();
                this.compositeFilterDesriptor.LogicalOperator = FilterCompositionLogicalOperator.Or;

                foreach (GridViewDataColumn column in columns)
                {
                    this.compositeFilterDesriptor.FilterDescriptors.Add(this.CreateFilterForColumn(column));
                }
            }

            public string FilterValue
            {
                get
                {
                    return this.filterValue;
                }
                set
                {
                    if (this.filterValue != value)
                    {
                        this.filterValue = value;
                        this.UpdateCompositeFilterValues();
                        this.OnPropertyChanged("FilterValue");
                    }
                }
            }

            protected override System.Linq.Expressions.Expression CreateFilterExpression(ParameterExpression parameterExpression)
            {
                if (string.IsNullOrEmpty(this.FilterValue))
                {
                    return TrueExpression;
                }
                try
                {
                    return this.compositeFilterDesriptor.CreateFilterExpression(parameterExpression);
                }
                catch
                {
                }

                return TrueExpression;
            }

            private IFilterDescriptor CreateFilterForColumn(GridViewDataColumn column)
            {
                FilterOperator filterOperator = GetFilterOperatorForType(column.DataType);
                FilterDescriptor descriptor = new FilterDescriptor(column.UniqueName, filterOperator, this.filterValue);
                descriptor.MemberType = column.DataType;

                return descriptor;
            }

            private static FilterOperator GetFilterOperatorForType(Type dataType)
            {
                return dataType == typeof(string) ? FilterOperator.Contains : FilterOperator.IsEqualTo;
            }

            private void UpdateCompositeFilterValues()
            {
                foreach (FilterDescriptor descriptor in this.compositeFilterDesriptor.FilterDescriptors)
                {
                    object convertedValue = DefaultValue(descriptor.MemberType);

                    try
                    {
                        convertedValue = Convert.ChangeType(this.FilterValue, descriptor.MemberType, CultureInfo.CurrentCulture);
                    }
                    catch
                    {
                    }

                    if (descriptor.MemberType.IsAssignableFrom(typeof(DateTime)))
                    {
                        DateTime date;
                        if (DateTime.TryParse(this.FilterValue, out date))
                        {
                            convertedValue = date;
                        }

                    }

                    descriptor.Value = convertedValue;
                }
            }

            private static object DefaultValue(Type type)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }
        }

        //private readonly CompositeFilterDescriptor compositeFilterDesriptor;
        //private static readonly ConstantExpression TrueExpression = System.Linq.Expressions.Expression.Constant(true);
        //private string filterValue;

        //public CustomFilterDescriptor(IEnumerable<Telerik.Windows.Controls.GridViewColumn> columns)
        //{
        //    this.compositeFilterDesriptor = new CompositeFilterDescriptor();
        //    this.compositeFilterDesriptor.LogicalOperator = FilterCompositionLogicalOperator.Or;

        //    foreach (GridViewDataColumn column in columns)
        //    {
        //        this.compositeFilterDesriptor.FilterDescriptors.Add(this.CreateFilterForColumn(column));
        //    }
        //}

        //public string FilterValue
        //{
        //    get
        //    {
        //        return this.filterValue;
        //    }
        //    set
        //    {
        //        if (this.filterValue != value)
        //        {
        //            this.filterValue = value;
        //            this.UpdateCompositeFilterValues();
        //            this.OnPropertyChanged("FilterValue");
        //        }
        //    }
        //}

        //protected override System.Linq.Expressions.Expression CreateFilterExpression(ParameterExpression parameterExpression)
        //{
        //    if (string.IsNullOrEmpty(this.FilterValue))
        //    {
        //        return TrueExpression;
        //    }
        //    try
        //    {
        //        return this.compositeFilterDesriptor.CreateFilterExpression(parameterExpression);
        //    }
        //    catch
        //    {
        //    }

        //    return TrueExpression;
        //}

        //private IFilterDescriptor CreateFilterForColumn(GridViewDataColumn column)
        //{
        //    FilterOperator filterOperator = GetFilterOperatorForType(column.DataType);
        //    FilterDescriptor descriptor = new FilterDescriptor(column.UniqueName, filterOperator, this.filterValue);
        //    descriptor.MemberType = column.DataType;

        //    return descriptor;
        //}

        //private static FilterOperator GetFilterOperatorForType(Type dataType)
        //{
        //    return dataType == typeof(string) ? FilterOperator.Contains : FilterOperator.IsEqualTo;
        //}

        //private void UpdateCompositeFilterValues()
        //{
        //    foreach (FilterDescriptor descriptor in this.compositeFilterDesriptor.FilterDescriptors)
        //    {
        //        object convertedValue = DefaultValue(descriptor.MemberType);

        //        try
        //        {
        //            convertedValue = Convert.ChangeType(this.FilterValue, descriptor.MemberType, CultureInfo.CurrentCulture);
        //        }
        //        catch
        //        {
        //            convertedValue = Telerik.Windows.Data.FilterDescriptor.UnsetValue;
        //        }

        //        if (descriptor.MemberType.IsAssignableFrom(typeof(DateTime)))
        //        {
        //            DateTime date;
        //            if (DateTime.TryParse(this.FilterValue, out date))
        //            {
        //                convertedValue = date;
        //            }
        //        }

        //        descriptor.Value = convertedValue;
        //    }
        //}

        //private static object DefaultValue(Type type)
        //{
        //    if (type.IsValueType)
        //    {
        //        return Activator.CreateInstance(type);
        //    }

        //    return null;
        //}


        //private readonly CompositeFilterDescriptor compositeFilterDesriptor;
        //private static readonly ConstantExpression TrueExpression = System.Linq.Expressions.Expression.Constant(true);
        //private string filterValue;

        //public CustomFilterDescriptor(IEnumerable<Telerik.Windows.Controls.GridViewColumn> columns)
        //{
        //    this.compositeFilterDesriptor = new CompositeFilterDescriptor();
        //    this.compositeFilterDesriptor.LogicalOperator = FilterCompositionLogicalOperator.Or;

        //    foreach (GridViewDataColumn column in columns)
        //    {
        //        if (column.IsVisible)
        //        {
        //            this.compositeFilterDesriptor.FilterDescriptors.Add(this.CreateFilterForColumn(column));     
        //        }
               
               
        //    }
        //}

        //public string FilterValue
        //{
        //    get
        //    {
        //        return this.filterValue;
        //    }
        //    set
        //    {
        //        if (this.filterValue != value)
        //        {
        //            this.filterValue = value;
        //            this.UpdateCompositeFilterValues();
        //            this.OnPropertyChanged("FilterValue");
        //        }
        //    }
        //}

        //protected override System.Linq.Expressions.Expression CreateFilterExpression(ParameterExpression parameterExpression)
        //{
        //    if (string.IsNullOrEmpty(this.FilterValue))
        //    {
        //        return TrueExpression;
        //    }
        //    try
        //    {
        //        return this.compositeFilterDesriptor.CreateFilterExpression(parameterExpression);
        //    }
        //    catch
        //    {
        //    }

        //    return TrueExpression;
        //}

        //private IFilterDescriptor CreateFilterForColumn(GridViewDataColumn column)
        //{
        //    FilterOperator filterOperator = GetFilterOperatorForType(column.DataType);
        //    FilterDescriptor descriptor = new FilterDescriptor(column.UniqueName, filterOperator, this.filterValue);
        //    descriptor.MemberType = column.DataType;

        //    return descriptor;
        //}

        //private static FilterOperator GetFilterOperatorForType(Type dataType)
        //{
        //    return dataType == typeof(string) ? FilterOperator.Contains : FilterOperator.IsEqualTo;
        //}

        //private void UpdateCompositeFilterValues()
        //{
        //    foreach (FilterDescriptor descriptor in this.compositeFilterDesriptor.FilterDescriptors)
        //    {
        //        object convertedValue = DefaultValue(descriptor.MemberType);

        //        try
        //        {
        //            convertedValue = Convert.ChangeType(this.FilterValue, descriptor.MemberType, CultureInfo.CurrentCulture);
        //        }
        //        catch
        //        {
        //        }

        //        if (descriptor.MemberType.IsAssignableFrom(typeof(DateTime)))
        //        {
        //            DateTime date;
        //            if (DateTime.TryParse(this.FilterValue, out date))
        //            {
        //                convertedValue = date;
        //            }

        //        }

        //        descriptor.Value = convertedValue;
        //    }
        //}

        //private static object DefaultValue(Type type)
        //{
        //    if (type.IsValueType)
        //    {
        //        return Activator.CreateInstance(type);
        //    }

        //    return null;
        //}
    //}
}
