namespace catalog.viewer.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

    internal static class xmlUtils
    {
        internal static void Element2Member(this XElement element, object record, CultureInfo cInfo)
        {
            PropertyInfo pi = record.GetType().GetProperty(element.Name.LocalName);
            if (pi == null)
            {
                Console.Error.WriteLine("invalid propname: {0}, ignored", element.Name.LocalName);
                return;
            }

            Type type = pi.PropertyType;
            if (type.IsGenericType)
            {
                type = type.GetGenericArguments()[0];
            }
            switch (type.FullName)
            {
                case "System.String":
                    pi.SetValue(record, element.Value, null);
                    break;
                case "System.Boolean":
                    {
                        Boolean value;
                        if (Boolean.TryParse(element.Value, out value) && pi.CanWrite)
                            pi.SetValue(record, value, null);
                        else
                        {
                            pi.SetValue(record, null, null);
                        }
                        break;
                    }
                case "System.Int32":
                    {
                        Int32 value = Int32.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Int16":
                    {
                        Int16 value = Int16.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Byte":
                    {
                        Byte value = Byte.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Decimal":
                    {
                        Decimal value = Decimal.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Single":
                    {
                        float value = float.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Double":
                    {
                        double value = double.Parse(element.Value, cInfo.NumberFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.DateTime":
                    {
                        DateTime value = DateTime.Parse(element.Value, cInfo.DateTimeFormat);
                        if (pi.CanWrite)
                            pi.SetValue(record, value, null);
                        break;
                    }
                case "System.Xml.Linq.XElement":
                    if (pi.CanWrite)
                        pi.SetValue(record, element, null);
                    break;
                default:
                    {
                        break;
                    }
            }
        }

        internal static void Properties2Attributes(this XElement elem, object obj)
        {
            List<PropertyInfo> props = (from prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        where !prop.GetGetMethod().IsVirtual && !prop.GetGetMethod().IsAbstract
                                        && prop.CanRead
                                        orderby prop.Name
                                        select prop).ToList();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj, new object[0]);
                if (value == null)
                    continue;
                string _value = value.ToString();
                if (string.IsNullOrWhiteSpace(_value))
                    continue;
                elem.SetAttributeValue(prop.Name, _value);
            }
       }
        internal static XElement Properties2Elements(this object obj)
        {
            XElement root = new XElement(obj.GetType().Name);
            List<PropertyInfo> props = (from prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        where prop.CanRead
                                        //                                        && !prop.GetCustomAttributes(typeof(IgnoreConvertAttribute), false).Any()
                                        orderby prop.Name
                                        select prop).ToList();

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj, new object[0]);
                if (value == null)
                    continue;
                if (prop.Name == "id")
                {
                    root.SetAttributeValue("id", value.ToString());
                    continue;
                }
                Type type = value.GetType();
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        type = System.Nullable.GetUnderlyingType(type);
                    else if (type.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
                    {
                        Type _type = type.GetGenericArguments()[0];
                        foreach (var item in value as IEnumerable)
                        {
                            root.Add(Properties2Elements(item));
                        }
                    }
                }
                switch (type.FullName)
                {
                    case "System.String":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Boolean":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Int16":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Int32":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Decimal":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Single":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Double":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.Byte":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                    case "System.DateTime":
                        {
                            string _value = value.ToString();
                            if (string.IsNullOrWhiteSpace(_value))
                                continue;
                            root.Add(new XElement(prop.Name, _value));
                        }
                        break;
                }
            }
            return root;
        }
    }
}
