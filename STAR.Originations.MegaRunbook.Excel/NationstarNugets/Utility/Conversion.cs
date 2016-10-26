// Decompiled with JetBrains decompiler
// Type: STAR.Framework.Utility.Conversion
// Assembly: STAR.Framework.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09c4f98813f6b683
// MVID: C0759794-9AB4-40FB-8B71-6E464E708E5C
// Assembly location: C:\Git\Repos\CapitalMarkets\Solutions\packages\STAR.Framework.Utility.1.0.0\lib\net40\STAR.Framework.Utility.dll

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace STAR.Framework.Utility
{
    public static class Conversion
    {
        private static object thisLock = new object();

        public static string GetObjectInfo(object value)
        {
            string str = string.Empty;
            return value != null ? value.ToString() : "NULL";
        }

        /// <summary>
        /// Returns 'DBNull.Value' if the 'obj' parameter is null or not within Min and Max values
        /// for the type (for example DateTime.MinValue or DateTime.MaxValue). Otherwise, just
        /// returns the 'obj' parameter.
        /// </summary>
        /// <param name="value">The obj.</param>
        /// <returns></returns>
        public static object GetDbNullValue(object value)
        {
            object obj = value;
            if (value == null)
                obj = (object)DBNull.Value;
            else if (Type.GetTypeCode(value.GetType()) == TypeCode.DateTime)
            {
                DateTime dateTime = Conversion.GetDateTime(value);
                if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                    obj = (object)DBNull.Value;
            }
            return obj;
        }

        /// <summary>
        /// Returns 'null' if the 'obj' parameter is null or not within Min and Max values
        /// for the type (for example DateTime.MinValue or DateTime.MaxValue). Otherwise, just
        /// returns the 'obj' parameter.
        /// </summary>
        /// <param name="value">The obj.</param>
        /// <returns></returns>
        public static object GetNullableValue(object value)
        {
            object obj = value;
            if (value == null)
            {
                obj = (object)null;
            }
            else
            {
                switch (Type.GetTypeCode(value.GetType()))
                {
                    case TypeCode.Int32:
                        switch ((long)Conversion.GetInt(value))
                        {
                            case (long)int.MinValue:
                            case (long)int.MaxValue:
                                obj = (object)null;
                                break;
                        }
                        break;
                    case TypeCode.Decimal:
                        Decimal @decimal = Conversion.GetDecimal(value);
                        if (@decimal == new Decimal(-1, -1, -1, true, (byte)0) || @decimal == new Decimal(-1, -1, -1, false, (byte)0))
                        {
                            obj = (object)null;
                            break;
                        }
                        break;
                    case TypeCode.DateTime:
                        DateTime dateTime = Conversion.GetDateTime(value);
                        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                        {
                            obj = (object)null;
                            break;
                        }
                        break;
                }
            }
            return obj;
        }

        /// <summary>
        /// Returns 'null' if the 'guid' parameter is Guid.Empty. Otherwise, just returns the 'guid' parameter.
        /// </summary>
        /// <param name="value">The guid.</param>
        /// <returns></returns>
        public static object GetDbNullableGuid(Guid value)
        {
            object obj = (object)value;
            if (value == Guid.Empty)
                obj = (object)DBNull.Value;
            return obj;
        }

        /// <summary>
        /// If the 'value' parameter is NULL then the method
        /// returns 'defaultValue'. Otherwise return the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <remarks>
        /// Essentially a shortcut to something along the lines of:
        /// 
        ///     MyNonNull = MyNull.HasValue ? MyNull.Value : 0
        /// 
        /// </remarks>
        public static U ToNonNull<T, U>(T value, U defaultValue)
        {
            U u = defaultValue;
            if ((object)value != null)
            {
                try
                {
                    Type nullableType = typeof(T);
                    Type conversionType = typeof(U);
                    if (nullableType.IsGenericType)
                        Nullable.GetUnderlyingType(nullableType);
                    u = (U)Convert.ChangeType((object)value, conversionType, (IFormatProvider)CultureInfo.CurrentCulture);
                }
                catch (InvalidCastException ex)
                {
                }
            }
            return u;
        }

        /// <summary>
        /// Parses the 'value' parameter to an Enumeration without exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Returns the parsed enumeration or default(T) if the conversion fails.</returns>
        public static T ParseEnum<T>(string value)
        {
            T defaultValue = default(T);
            return Conversion.ParseEnum<T>(value, defaultValue, true, false);
        }

        /// <summary>
        /// Parses the 'value' parameter to an Enumeration and uses 'default(T)'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>
        /// Returns the parsed enumeration or default(T) if the conversion fails and the
        /// 'throwException' parameter is false.
        /// </returns>
        public static T ParseEnum<T>(string value, bool throwException)
        {
            T defaultValue = default(T);
            return Conversion.ParseEnum<T>(value, defaultValue, true, throwException);
        }

        /// <summary>
        /// Parses the 'value' parameter to an Enumeration and uses the 'defaultValue' parameter if
        /// conversion fails and the 'throwException' is false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>
        /// Returns the parsed enumeration or the 'defaultValue' parameter if the conversion fails
        /// and the 'throwException' parameter is false.
        /// </returns>
        public static T ParseEnum<T>(string value, T defaultValue, bool throwException)
        {
            return Conversion.ParseEnum<T>(value, defaultValue, true, throwException);
        }

        /// <summary>
        /// Parses the 'value' parameter to an Enumeration and uses the 'defaultValue' parameter if
        /// conversion fails and the 'throwException' is false.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>
        /// Returns the parsed enumeration or the 'defaultValue' parameter if the conversion fails
        /// and the 'throwException' parameter is false.
        /// </returns>
        public static T ParseEnum<T>(string value, T defaultValue, bool ignoreCase, bool throwException)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                    return (T)Enum.Parse(typeof(T), value, ignoreCase);
                if (!throwException)
                    return defaultValue;
                string str = ignoreCase ? "(The conversion is NOT case-sensitive)" : "(The conversion is case-sensitive)";
                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the value '{0}' to a '{1}' {2}.", (object)value, (object)typeof(T).Name, (object)str));
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    string str = ignoreCase ? "(The conversion is NOT case-sensitive)" : "(The conversion is case-sensitive)";
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the value '{0}' to a '{1}' {2}. {3}", (object)value, (object)typeof(T).Name, (object)str, (object)ex.Message));
                }
                return defaultValue;
            }
        }

        public static bool CanConvert<T>(T value)
        {
            bool flag = false;
            try
            {
                Type type = typeof(T);
                if (type.IsGenericType)
                    type = Nullable.GetUnderlyingType(type);
                Convert.ChangeType((object)value, type, (IFormatProvider)CultureInfo.CurrentCulture);
                flag = true;
            }
            catch (ArgumentNullException ex)
            {
            }
            return flag;
        }

        /// <summary>
        /// Attempts to convert 'valueToConvert' to an Integer (System.Int32). (0 by default)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.Int32 value of the 'valueToConvert' parameter or '0' if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns '0' by default.
        /// </remarks>
        public static int GetInt(object valueToConvert)
        {
            int defaultValue = 0;
            return Conversion.GetValue<int>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'valueToConvert' to a Long (System.Int64). (0 by default)
        /// </summary>
        /// <param name="valueToConvert">The value.</param>
        /// <returns>A System.Int64 value of the 'value' parameter or 'defaultValue' if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns '0' by default.
        /// </remarks>
        public static long GetLong(object valueToConvert)
        {
            long defaultValue = 0;
            return Conversion.GetValue<long>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to an System.Decimal. (0m by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.Decimal value of the 'value' parameter or 'defaultValue' if the conversion fails.</returns>
        /// <remarks>Does not throw an exception if the conversion fails - returns 0 by default.</remarks>
        public static Decimal GetDecimal(object valueToConvert)
        {
            Decimal defaultValue = new Decimal();
            return Conversion.GetValue<Decimal>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to a float (System.Single). (0f by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.Single value (float) of the object 'value' or 0, by default, if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 0 by default.
        /// </remarks>
        public static float GetFloat(object valueToConvert)
        {
            float defaultValue = 0.0f;
            return Conversion.GetValue<float>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to a double (System.Double). (0d by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.Double value of the 'value' parameter or 0, by default, if the conversion fails.</returns>
        /// <remarks>Does not throw an exception if the conversion fails - returns 0 by default.</remarks>
        public static double GetDouble(object valueToConvert)
        {
            double defaultValue = 0.0;
            return Conversion.GetValue<double>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to a Boolean. (false by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.Boolean value of the 'value' parameter or FALSE, by default, if the conversion fails.</returns>
        /// <remarks>Does not throw an exception if the conversion fails - returns FALSE by default.</remarks>
        public static bool GetBoolean(object valueToConvert)
        {
            return Conversion.GetValue<bool>(valueToConvert, false, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to a DateTime. (DateTime.MinValue by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A System.DateTime value of the 'value' parameter or DateTime.MinValue, by default, if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns DateTime.MinValue by default.
        /// </remarks>
        public static DateTime GetDateTime(object valueToConvert)
        {
            return Conversion.GetValue<DateTime>(valueToConvert, DateTime.MinValue, false);
        }

        /// <summary>
        /// Attempts to convert 'value' to a String. (String.Empty by default)
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>A String value of the 'value' parameter or String.Empty, by default, if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns String.Empty by default.
        /// </remarks>
        public static string GetString(object valueToConvert)
        {
            return Conversion.GetValue<string>(valueToConvert, string.Empty, false);
        }

        /// <summary>
        /// Safely gets the value of the 'valueToConvert' parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <returns>
        /// If 'valueToConvert' cannot be converted, 'defaultValue' is returned instead,
        /// unless 'throwException' is true.
        /// </returns>
        [Obsolete("This does not work and needs to be documented!", true)]
        public static T GetValue<T>(object valueToConvert)
        {
            return Conversion.GetValue<T>(valueToConvert, default(T), false);
        }

        /// <summary>
        /// Safely gets the value of the 'valueToConvert' parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// If 'valueToConvert' cannot be converted, 'defaultValue' is returned instead,
        /// unless 'throwException' is true.
        /// </returns>
        public static T GetValue<T>(object valueToConvert, T defaultValue)
        {
            return Conversion.GetValue<T>(valueToConvert, defaultValue, false);
        }

        /// <summary>
        /// Safely gets the value of the 'valueToConvert' parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>
        /// If 'valueToConvert' cannot be converted, 'defaultValue' is returned instead,
        /// unless 'throwException' is true.
        /// </returns>
        public static T GetValue<T>(object valueToConvert, T defaultValue, bool throwException)
        {
            T obj = defaultValue;
            try
            {
                Type type = typeof(T);
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && valueToConvert == null)
                        return default(T);
                    Nullable.GetUnderlyingType(type);
                }
                else if (valueToConvert != null)
                {
                    if (valueToConvert is string)
                    {
                        if (!string.IsNullOrEmpty(valueToConvert.ToString()))
                            obj = (T)Convert.ChangeType(valueToConvert, type, (IFormatProvider)CultureInfo.CurrentCulture);
                    }
                    else
                        obj = (T)Convert.ChangeType(valueToConvert, type, (IFormatProvider)CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    string str = string.Empty;
                    if ((object)defaultValue != null)
                        str = defaultValue.GetType().Name;
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert value to type <{0}>.", new object[1] { (object)str }), ex);
                }
            }
            return obj;
        }

        /// <summary>
        /// Attempts to convert the 'value' parameter to a Boolean. 'Y' or 'Yes' = true. 'N' or 'No' = false.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A System.Boolean value of the 'value' parameter or FALSE, by default, if the conversion fails.</returns>
        /// <remarks>Does not throw an exception if the conversion fails - returns FALSE by default.</remarks>
        public static bool GetYesNoBoolean(object value)
        {
            return Conversion.GetYesNoBoolean(value, false, false);
        }

        /// <summary>
        /// Attempts to convert the 'value' parameter to a Boolean. 'Y' or 'Yes' = true. 'N' or 'No' = false.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>A System.Boolean value of the 'value' parameter or FALSE, by default, if the conversion fails.</returns>
        /// <remarks>Does not throw an exception if the conversion fails.</remarks>
        public static bool GetYesNoBoolean(object value, bool defaultValue)
        {
            return Conversion.GetYesNoBoolean(value, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert the 'value' parameter to a Boolean. 'Y' or 'Yes' = true. 'N' or 'No' = false.
        /// </summary>
        /// <param name="textValue">The text String to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <param name="throwException">If false, don't handle any exceptions and don't return a default value. Caller must handle exception.</param>
        /// <returns>A System.Boolean value of the 'value' parameter or FALSE, by default, if the conversion fails.</returns>
        /// <remarks>
        /// Trims the 'textValue' paramenter, converts it to upper case, then takes the first character
        /// and compares it to 'Y' or 'N'.
        /// If the 'throwException' parameter is TRUE then attempt to parse the 'textValue' parameter.
        /// Otherwise, return the 'defaultValue' if the 'textValue' parameter is null or empty.
        /// </remarks>
        public static bool GetYesNoBoolean(object value, bool defaultValue, bool throwException)
        {
            bool flag = defaultValue;
            try
            {
                string @string = Conversion.GetString(value);
                if (!string.IsNullOrEmpty(@string))
                {
                    if (!string.IsNullOrEmpty(@string.Trim()))
                    {
                        switch (@string.ToUpper(CultureInfo.CurrentCulture)[0])
                        {
                            case 'Y':
                                flag = true;
                                break;
                            case 'N':
                                flag = false;
                                break;
                            default:
                                if (throwException)
                                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'textValue' provided, '{0}', could not be converted to a Boolean. It does not equal 'yes' or 'no'.", new object[1]
                                    {
                    (object) STAR.Framework.Utility.Text.GetStringInfo(@string)
                                    }));
                                break;
                        }
                    }
                    else if (throwException)
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'textValue' provided '{0}' could not be converted to a Boolean. It is null or empty when trimmed.", new object[1]
                        {
              (object) STAR.Framework.Utility.Text.GetStringInfo(@string)
                        }));
                }
                else if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'textValue' provided '{0}' could not be converted to a Boolean. It is null or empty.", new object[1]
                    {
            (object) STAR.Framework.Utility.Text.GetStringInfo(@string)
                    }));
            }
            catch (Exception ex)
            {
                if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'textValue' provided '{0}', could not be converted to a Boolean. {1}", new object[2]
                    {
            (object) STAR.Framework.Utility.Text.GetStringInfo(Conversion.GetString(value)),
            (object) ex.Message
                    }), ex);
            }
            return flag;
        }

        /// <summary>
        /// Returns the text 'Y' or 'N' based on the 'yesNo' parameter.
        /// </summary>
        /// <param name="yesNo">if set to <c>true</c> [yes no].</param>
        /// <returns></returns>
        public static string GetYesNoText(bool yesNo)
        {
            return Conversion.GetYesNoText(yesNo, true);
        }

        /// <summary>
        /// Returns the text 'Y' or 'N' if the 'isAbbreviated' is TRUE, otherwise
        /// returns the text 'Yes' or 'No'.
        /// </summary>
        /// <param name="yesNo">if set to <c>true</c> [yes no].</param>
        /// <param name="isAbbreviated">if set to <c>true</c> [is abbreviated].</param>
        /// <returns></returns>
        public static string GetYesNoText(bool yesNo, bool isAbbreviated)
        {
            string str = string.Empty;
            return !isAbbreviated ? (!yesNo ? "No" : "Yes") : (!yesNo ? "N" : "Y");
        }

        /// <summary>
        /// Attempts to convert 'numericValue' to a Boolean. '1' = true. '0' = false.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <returns>FALSE if 'numericValue' is Zero. TRUE if 'numericValue' is One.</returns>
        /// <remarks>Does not throw an exception if the conversion fails - returns FALSE by default.</remarks>
        public static bool GetNumericBoolean(int value)
        {
            return Conversion.GetNumericBoolean(value, false, false);
        }

        /// <summary>
        /// Attempts to convert 'numericValue' to a Boolean. '1' = true. '0' = false.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>FALSE if 'numericValue' is Zero. TRUE if 'numericValue' is One.</returns>
        /// <remarks>Does not throw an exception if the conversion fails.</remarks>
        public static bool GetNumericBoolean(int value, bool defaultValue)
        {
            return Conversion.GetNumericBoolean(value, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'numericValue' to a Boolean. '1' = true. '0' = false.
        /// </summary>
        /// <param name="value">The numeric value to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <param name="throwException">If false, don't handle any exceptions and don't return a default value. Caller must handle exception.</param>
        /// <returns>FALSE if 'numericValue' is Zero. TRUE if 'numericValue' is One.</returns>
        /// <remarks>
        /// If the 'throwException' parameter is TRUE then throw an exception if the 'numericValue' is
        /// not Zero or One. Otherwise, return the 'defaultValue' if the 'numericValue' parameter is
        /// not Zero or One.
        /// </remarks>
        public static bool GetNumericBoolean(int value, bool defaultValue, bool throwException)
        {
            bool flag = defaultValue;
            if (value == 0)
                flag = false;
            else if (value == 1)
                flag = true;
            else if (throwException)
                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'numericValue' provided, '{0}', is not a zero or a one.", new object[1]
                {
          (object) value
                }));
            return flag;
        }

        /// <summary>
        /// Attempts to convert 'guidText' to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guidText">The string representation of the Guid to convert.</param>
        /// <returns>A System.Guid value of the 'guidText' parameter or 'Guid.Empty' if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(string guidText)
        {
            Guid defaultValue = Guid.Empty;
            return Conversion.GetGuid(guidText, defaultValue);
        }

        /// <summary>
        /// Attempts to convert 'guidText' to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guidText">The string representation of the Guid to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// A System.Guid value of the 'guidText' parameter or 'Guid.Empty' if the conversion fails.
        /// </returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(string guidText, Guid defaultValue)
        {
            return Conversion.GetGuid(guidText, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert 'guidText' to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guidText">The string representation of the Guid to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns>
        /// A System.Guid value of the 'guidText' parameter or 'Guid.Empty' if the conversion fails.
        /// </returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(string guidText, Guid defaultValue, bool throwExceptions)
        {
            Guid guid = defaultValue;
            try
            {
                if (!string.IsNullOrEmpty(guidText))
                    guid = new Guid(guidText);
                else
                    throw new ArgumentNullException("guidText", string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'guidText' parameter passed into the 'GetGuid' method is '{0}' and this is not allowed.", new object[1]
                    {
            (object) STAR.Framework.Utility.Text.GetStringInfo(guidText)
                    }));
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the string value '{0}' to a Guid.", new object[1]
                    {
            (object) STAR.Framework.Utility.Text.GetStringInfo(guidText)
                    }), ex);
            }
            return guid;
        }

        /// <summary>
        /// Attempts to convert the 'guid' object to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guid">The string representation of the Guid to convert.</param>
        /// <returns>A System.Guid value of the 'guid' parameter or 'Guid.Empty' if the conversion fails.</returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(object guid)
        {
            Guid defaultValue = Guid.Empty;
            return Conversion.GetGuid(guid, defaultValue);
        }

        /// <summary>
        /// Attempts to convert the 'guid' object to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guid">The string representation of the Guid to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// A System.Guid value of the 'guid' parameter or 'Guid.Empty' if the conversion fails.
        /// </returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(object guid, Guid defaultValue)
        {
            return Conversion.GetGuid(guid, defaultValue, false);
        }

        /// <summary>
        /// Attempts to convert the 'guid' object to a Guid. (Guid.Empty by default)
        /// </summary>
        /// <param name="guid">The string representation of the Guid to convert.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns>
        /// A System.Guid value of the 'guid' parameter or 'Guid.Empty' if the conversion fails.
        /// </returns>
        /// <remarks>
        /// Does not throw an exception if the conversion fails - returns 'Guid.Empty' by default.
        /// </remarks>
        public static Guid GetGuid(object guid, Guid defaultValue, bool throwExceptions)
        {
            Guid guid1 = defaultValue;
            try
            {
                if (guid != null)
                    guid1 = (Guid)guid;
                else
                    throw new ArgumentNullException("guid", string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The 'guid' parameter passed into the 'GetGuid' method is '{0}' and this is not allowed.", new object[1]
                    {
            (object) Conversion.GetObjectInfo(guid)
                    }));
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the string value '{0}' to a Guid.", new object[1]
                    {
            (object) Conversion.GetObjectInfo(guid)
                    }), ex);
            }
            return guid1;
        }

        public static T?[] ToNullableArray<T>(T[] array) where T : struct
        {
            T?[] nullableArray = (T?[])null;
            if (array != null)
            {
                nullableArray = new T?[array.Length];
                for (int index = 0; index < array.Length; ++index)
                    nullableArray[index] = new T?(array[index]);
            }
            return nullableArray;
        }

        /// <summary>
        /// Converts an object to the type specified in T.
        /// Serializes the object parameter to Xml and then attemptes to deserialize
        /// the Xml string into the type represented by T. Uses ASCII Xml serialization encoding.
        /// 
        /// Returns true if the conversion was a success.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryCast<T>(object source, out T value)
        {
            return Conversion.TryCast<T>(source, false, out value);
        }

        /// <summary>
        /// Converts an object to the type specified in T.
        /// Serializes the object parameter to Xml and then attemptes to deserialize
        /// the Xml string into the type represented by T.
        /// 
        /// Returns true if the conversion was a success.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="encoding">Xml serialization encoding</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryCast<T>(object source, System.Text.Encoding encoding, out T value)
        {
            return Conversion.TryCast<T>(source, false, encoding, out value);
        }

        /// <summary>
        /// Converts an object to the type specified in T.
        /// Serializes the object parameter to Xml and then attemptes to deserialize
        /// the Xml string into the type represented by T. Uses ASCII Xml serialization encoding.
        /// 
        /// Returns true if the conversion was a success.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="throwExceptions">When true the conversion will raise exceptions on failure.</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryCast<T>(object source, bool throwExceptions, out T value)
        {
            return Conversion.TryCast<T>(source, throwExceptions, System.Text.Encoding.ASCII, out value);
        }

        /// <summary>
        /// Converts an object to the type specified in T.
        /// Serializes the object parameter to Xml and then attemptes to deserialize
        /// the Xml string into the type represented by T.
        /// 
        /// Returns true if the conversion was a success.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="throwExceptions">When true the conversion will raise exceptions on failure.</param>
        /// <param name="encoding">Xml serialization encoding</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryCast<T>(object source, bool throwExceptions, System.Text.Encoding encoding, out T value)
        {
            bool flag = false;
            value = default(T);
            try
            {
                value = (T)STAR.Framework.Utility.Serialization.DeserializeToObject(STAR.Framework.Utility.Serialization.SerializeToXml(source, encoding, throwExceptions), typeof(T), encoding, throwExceptions);
                flag = true;
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not convert object '{0}'.", new object[1]
                    {
            (object) ex.Message
                    }), ex);
            }
            return flag;
        }

        public static DateTime? GetNullableDateTime(DateTime dateTime)
        {
            return Conversion.GetNullableValue((object)dateTime) as DateTime?;
        }

        public static bool IsEmpty(this DateTime dateTime)
        {
            bool flag = false;
            if (dateTime <= DateTime.MinValue || dateTime >= DateTime.MaxValue)
                flag = true;
            return flag;
        }

        /// <summary>Returns the Date for the PREVIOUS Day of the week.</summary>
        /// <returns></returns>
        /// <remarks>
        /// As an example, Manager and Cost Center changes need to set the date to the nearest Sunday in the past.
        /// Although the actual date of change may be any day of the week by HR/Payroll rules it needs to
        /// be set to the Sunday before the date of change.
        /// </remarks>
        public static DateTime GetPreviousDayOfWeek(DayOfWeek dayOfWeek)
        {
            int num = DateTime.Now.DayOfWeek - dayOfWeek;
            if (num < 0)
                num += 7;
            return DateTime.Now.AddDays((double)(-1 * num)).Date;
        }

        /// <summary>
        /// Safely converts Linq xml element and attribute values into value types.
        /// </summary>
        /// <remarks>
        /// 'Simple' methods attempt to convert an object and don't throw exceptions if the conversion
        /// fails; the 'default' DotNet value is returned instead. For example:
        /// 
        ///     int i = LinqXml.GetIntElement(xElement, "Name of an Element where the value is not a number");
        /// 
        /// ...will return 'null' without an Exception.
        /// 
        /// Various overloads allow you to explicitly set the 'default' value if conversion fails and/or
        /// determine if an exception should be raised. This exception would then need to be handled
        /// by the caller.
        /// </remarks>
        public static class LinqXml
        {
            /// <summary>
            /// Attempts to get the attribute string value of the 'xElement' parameter that
            /// matches the 'name' parameter.
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <param name="defaultValue">The default value to return if the attribute doesn't exist or an exception occurs.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception]</param>
            ///  if conversion fails.
            ///             <returns>A string value or 'defaultValue' if the conversion fails.</returns>
            public static string GetStringElement(XElement xElement, string name, string defaultValue, bool throwException)
            {
                string str = defaultValue;
                if (xElement != null)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (xElement.Element((XName)name) != null)
                        {
                            if (xElement.Element((XName)name).Value != null)
                                str = xElement.Element((XName)name).Value.Trim();
                            else if (throwException)
                                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' exists, but does not have a value. If a value is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                        }
                        else if (throwException)
                            throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' does not exist. Ensure the element indicated by the 'name' parameter exists and/or is spelled correctly in the xml file. The name is case-sensitive. If an element is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                    }
                    else if (throwException)
                        throw new Conversion.ConversionException("Could not get the element because the 'name' provided is null or empty.");
                }
                else if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not get element '{0}' because the xElement provided is null.", new object[1] { (object)name }));
                return str;
            }

            /// <summary>
            /// Attempts to get the string value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter.
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <param name="defaultValue">The default value to return if the attribute doesn't exist or an exception occurs.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception]</param>
            ///  if conversion fails.
            ///             <returns>A string value or 'defaultValue' if the attribute doesn't exist or an exception occurs.</returns>
            public static string GetStringAttribute(XElement xElement, string name, string defaultValue, bool throwException)
            {
                string str = defaultValue;
                if (xElement != null)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (xElement.Attribute((XName)name) != null)
                        {
                            if (xElement.Attribute((XName)name).Value != null)
                                str = xElement.Attribute((XName)name).Value.Trim();
                            else if (throwException)
                                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' exists, but does not have a value. If a value is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                        }
                        else if (throwException)
                            throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' does not exist. Ensure the attribute indicated by the 'name' parameter exists and/or is spelled correctly in the xml file. The name is case-sensitive. If an attribute is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                    }
                    else if (throwException)
                        throw new Conversion.ConversionException("Could not get the attribute because the 'name' provided is null or empty.");
                }
                else if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not get attribute '{0}' because the xElement provided is null.", new object[1] { (object)name }));
                return str;
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A System.Int32 or '0', by default, if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails - returns 0 by default.</remarks>
            public static int GetIntElement(XElement xElement, string name)
            {
                int defaultValue = 0;
                return Conversion.LinqXml.GetElementValue<int>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a Long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A Long (System.Int64) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static long GetLongElement(XElement xElement, string name)
            {
                long defaultValue = 0;
                return Conversion.LinqXml.GetElementValue<long>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A System.Decimal value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static Decimal GetDecimalElement(XElement xElement, string name)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.LinqXml.GetElementValue<Decimal>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A float (System.Single) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static float GetFloatElement(XElement xElement, string name)
            {
                float defaultValue = 0.0f;
                return Conversion.LinqXml.GetElementValue<float>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A System.Double value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static double GetDoubleElement(XElement xElement, string name)
            {
                double defaultValue = 0.0;
                return Conversion.LinqXml.GetElementValue<double>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A System.Boolean value or 'false' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static bool GetBooleanElement(XElement xElement, string name)
            {
                bool defaultValue = false;
                return Conversion.LinqXml.GetElementValue<bool>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="xElement">The xElement containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xElement' parameter.</param>
            /// <returns>A System.DateTime value or 'DateTime.MinValue' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static DateTime GetDateTimeElement(XElement xElement, string name)
            {
                DateTime defaultValue = DateTime.MinValue;
                return Conversion.LinqXml.GetElementValue<DateTime>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.Int32 value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static int GetIntAttribute(XElement xElement, string name)
            {
                int defaultValue = 0;
                return Conversion.LinqXml.GetAttributeValue<int>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a Long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A Long (System.Int64) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static long GetLongAttribute(XElement xElement, string name)
            {
                long defaultValue = 0;
                return Conversion.LinqXml.GetAttributeValue<long>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.Decimal value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static Decimal GetDecimalAttribute(XElement xElement, string name)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.LinqXml.GetAttributeValue<Decimal>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.Single value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static float GetFloatAttribute(XElement xElement, string name)
            {
                float defaultValue = 0.0f;
                return Conversion.LinqXml.GetAttributeValue<float>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.Double value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static double GetDoubleAttribute(XElement xElement, string name)
            {
                double defaultValue = 0.0;
                return Conversion.LinqXml.GetAttributeValue<double>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.Boolean.
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.Boolean value or 'false' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static bool GetBooleanAttribute(XElement xElement, string name)
            {
                bool defaultValue = false;
                return Conversion.LinqXml.GetAttributeValue<bool>(xElement, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xElement' parameter that
            /// matches the 'name' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="xElement">The xElement containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xElement' parameter.</param>
            /// <returns>A System.DateTime value or 'DateTime.MinValue' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static DateTime GetDateTimeAttribute(XElement xElement, string name)
            {
                DateTime defaultValue = DateTime.MinValue;
                return Conversion.LinqXml.GetAttributeValue<DateTime>(xElement, name, defaultValue, false);
            }

            [Obsolete("This does not work and needs to be documented!", true)]
            public static T GetElementValue<T>(XElement xElement, string name)
            {
                return Conversion.LinqXml.GetElementValue<T>(xElement, name, default(T), false);
            }

            public static T GetElementValue<T>(XElement xElement, string name, T defaultValue)
            {
                return Conversion.LinqXml.GetElementValue<T>(xElement, name, defaultValue, false);
            }

            public static T GetElementValue<T>(XElement xElement, string name, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                string stringElement = Conversion.LinqXml.GetStringElement(xElement, name, defaultValue.ToString(), throwException);
                try
                {
                    obj = Conversion.GetValue<T>((object)stringElement, defaultValue, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        name = string.IsNullOrEmpty(name) ? string.Empty : name;
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' could not be converted. {1}", new object[2]
                        {
              (object) name,
              (object) ex.Message
                        }), ex);
                    }
                }
                return obj;
            }

            [Obsolete("This does not work and needs to be documented!", true)]
            public static T GetAttributeValue<T>(XElement xElement, string name)
            {
                return Conversion.LinqXml.GetAttributeValue<T>(xElement, name, default(T), false);
            }

            public static T GetAttributeValue<T>(XElement xElement, string name, T defaultValue)
            {
                return Conversion.LinqXml.GetAttributeValue<T>(xElement, name, defaultValue, false);
            }

            public static T GetAttributeValue<T>(XElement xElement, string name, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                string stringAttribute = Conversion.LinqXml.GetStringAttribute(xElement, name, defaultValue.ToString(), throwException);
                try
                {
                    obj = Conversion.GetValue<T>((object)stringAttribute, defaultValue, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        name = string.IsNullOrEmpty(name) ? string.Empty : name;
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' could not be converted. {1}", new object[2]
                        {
              (object) name,
              (object) ex.Message
                        }), ex);
                    }
                }
                return obj;
            }
        }

        /// <summary>
        /// Conversion methods which accept a 'key' to a configuration file's 'appSettings'
        /// section.
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static int GetInt(string key)
            {
                int defaultValue = 0;
                return Conversion.Configuration.GetValue<int>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static int GetInt(string key, int defaultValue)
            {
                return Conversion.Configuration.GetValue<int>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Int32.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static int GetInt(string key, int defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<int>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static long GetLong(string key)
            {
                long defaultValue = 0;
                return Conversion.Configuration.GetValue<long>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static long GetLong(string key, long defaultValue)
            {
                return Conversion.Configuration.GetValue<long>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static long GetLong(string key, long defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<long>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static Decimal GetDecimal(string key)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.Configuration.GetValue<Decimal>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static Decimal GetDecimal(string key, Decimal defaultValue)
            {
                return Conversion.Configuration.GetValue<Decimal>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static Decimal GetDecimal(string key, Decimal defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<Decimal>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static float GetFloat(string key)
            {
                float defaultValue = 0.0f;
                return Conversion.Configuration.GetValue<float>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static float GetFloat(string key, float defaultValue)
            {
                return Conversion.Configuration.GetValue<float>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static float GetFloat(string key, float defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<float>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static double GetDouble(string key)
            {
                double defaultValue = 0.0;
                return Conversion.Configuration.GetValue<double>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static double GetDouble(string key, double defaultValue)
            {
                return Conversion.Configuration.GetValue<double>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static double GetDouble(string key, double defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<double>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public static bool GetBoolean(string key)
            {
                return Conversion.Configuration.GetValue<bool>(key, false, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns></returns>
            public static bool GetBoolean(string key, bool defaultValue)
            {
                return Conversion.Configuration.GetValue<bool>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static bool GetBoolean(string key, bool defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<bool>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="columnIndex">Index of the column to convert.</param>
            /// <returns></returns>
            public static DateTime GetDateTime(string key)
            {
                return Conversion.Configuration.GetValue<DateTime>(key, DateTime.MinValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static DateTime GetDateTime(string key, DateTime defaultValue)
            {
                return Conversion.Configuration.GetValue<DateTime>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static DateTime GetDateTime(string key, DateTime defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<DateTime>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a String. (String.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="columnIndex">Index of the column to convert.</param>
            /// <returns></returns>
            public static string GetString(string key)
            {
                return Conversion.Configuration.GetValue<string>(key, string.Empty, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a String. (String.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static string GetString(string key, string defaultValue)
            {
                return Conversion.Configuration.GetValue<string>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a String. (String.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static string GetString(string key, string defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<string>(key, defaultValue, throwException);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a Guid. (Guid.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="columnIndex">Index of the column to convert.</param>
            /// <returns></returns>
            public static Guid GetGuid(string key)
            {
                return Conversion.Configuration.GetValue<Guid>(key, Guid.Empty, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a Guid. (Guid.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static Guid GetGuid(string key, Guid defaultValue)
            {
                return Conversion.Configuration.GetValue<Guid>(key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the String representation of the 'appSettings' value indicated by
            /// the 'key' parameter to a Guid. (Guid.Empty by default)
            /// </summary>
            /// <param name="key">A 'key' in the 'appSettings' section of a configuration file.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static Guid GetGuid(string key, Guid defaultValue, bool throwException)
            {
                return Conversion.Configuration.GetValue<Guid>(key, defaultValue, throwException);
            }

            [Obsolete("This does not work and needs to be documented!", true)]
            public static T GetValue<T>(string key)
            {
                return Conversion.Configuration.GetValue<T>(key, default(T), false);
            }

            public static T GetValue<T>(string key, T defaultValue)
            {
                return Conversion.Configuration.GetValue<T>(key, defaultValue, false);
            }

            public static T GetValue<T>(string key, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                    obj = Conversion.GetValue<T>((object)ConfigurationManager.AppSettings[key], defaultValue, throwException);
                else if (throwException)
                {
                    string str = string.Empty;
                    if ((object)defaultValue != null)
                        str = defaultValue.GetType().Name;
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the value of key ['{0}'] to a value of type <{1}>. Key ['{0}'] does not exist.", new object[2]
                    {
            (object) key,
            (object) str
                    }));
                }
                return obj;
            }
        }

        /// <summary>
        /// Safely converts xml node element and attribute values into value types.
        /// </summary>
        /// <remarks>
        /// 'Simple' methods attempt to convert an object and don't throw exceptions if the conversion
        /// fails; the 'default' DotNet value is returned instead. For example:
        /// 
        ///     int i = Xml.GetIntElement(Element, "Name of an Element where the value is not a number");
        /// 
        /// ...will return 'null' without an Exception.
        /// 
        /// Various overloads allow you to explicitly set the 'default' value if conversion fails and/or
        /// determine if an exception should be raised. This exception would then need to be handled
        /// by the caller.
        /// </remarks>
        public static class XmlNode
        {
            /// <summary>
            /// Attempts to get the attribute string value of the 'xmlNode' parameter that
            /// matches the 'name' parameter.
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <param name="defaultValue">The default value to return if the attribute doesn't exist or an exception occurs.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception]</param>
            ///  if conversion fails.
            ///             <returns>A string value or 'defaultValue' if the conversion fails.</returns>
            public static string GetStringElement(System.Xml.XmlNode xmlNode, string name, string defaultValue, bool throwException)
            {
                string str = defaultValue;
                if (xmlNode != null)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (xmlNode[name] != null)
                        {
                            if (xmlNode[name].InnerText != null)
                                str = xmlNode[name].InnerText.Trim();
                            else if (throwException)
                                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' exists, but does not have a value. If a value is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                        }
                        else if (throwException)
                            throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' does not exist. Ensure the element indicated by the 'name' parameter exists and/or is spelled correctly in the xml file. The name is case-sensitive. If an element is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                    }
                    else if (throwException)
                        throw new Conversion.ConversionException("Could not get the element because the 'name' provided is null or empty.");
                }
                else if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not get element '{0}' because the xmlNode provided is null.", new object[1] { (object)name }));
                return str;
            }

            /// <summary>
            /// Attempts to get the string value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter.
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <param name="defaultValue">The default value to return if the attribute doesn't exist or an exception occurs.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception]</param>
            ///  if conversion fails.
            ///             <returns>A string value or 'defaultValue' if the attribute doesn't exist or an exception occurs.</returns>
            public static string GetStringAttribute(System.Xml.XmlNode xmlNode, string name, string defaultValue, bool throwException)
            {
                string str = defaultValue;
                if (xmlNode != null)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (xmlNode.Attributes[name] != null)
                        {
                            if (xmlNode.Attributes[name].Value != null)
                                str = xmlNode.Attributes[name].Value.Trim();
                            else if (throwException)
                                throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' exists, but does not have a value. If a value is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                        }
                        else if (throwException)
                            throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' does not exist. Ensure the attribute indicated by the 'name' parameter exists and/or is spelled correctly in the xml file. The name is case-sensitive. If an attribute is not required, set 'throwException' to false and provide a 'defaultValue'.", new object[1] { (object)name }));
                    }
                    else if (throwException)
                        throw new Conversion.ConversionException("Could not get the attribute because the 'name' provided is null or empty.");
                }
                else if (throwException)
                    throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Could not get attribute '{0}' because the xmlNode provided is null.", new object[1] { (object)name }));
                return str;
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A System.Int32 or '0', by default, if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails - returns 0 by default.</remarks>
            public static int GetIntElement(System.Xml.XmlNode xmlNode, string name)
            {
                int defaultValue = 0;
                return Conversion.XmlNode.GetElementValue<int>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a Long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A Long (System.Int64) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static long GetLongElement(System.Xml.XmlNode xmlNode, string name)
            {
                long defaultValue = 0;
                return Conversion.XmlNode.GetElementValue<long>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A System.Decimal value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static Decimal GetDecimalElement(System.Xml.XmlNode xmlNode, string name)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.XmlNode.GetElementValue<Decimal>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A float (System.Single) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static float GetFloatElement(System.Xml.XmlNode xmlNode, string name)
            {
                float defaultValue = 0.0f;
                return Conversion.XmlNode.GetElementValue<float>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A System.Double value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static double GetDoubleElement(System.Xml.XmlNode xmlNode, string name)
            {
                double defaultValue = 0.0;
                return Conversion.XmlNode.GetElementValue<double>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A System.Boolean value or 'false' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static bool GetBooleanElement(System.Xml.XmlNode xmlNode, string name)
            {
                bool defaultValue = false;
                return Conversion.XmlNode.GetElementValue<bool>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of a child element of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing a child element with the 'name' parameter.</param>
            /// <param name="name">The name of the child element of the 'xmlNode' parameter.</param>
            /// <returns>A System.DateTime value or 'DateTime.MinValue' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static DateTime GetDateTimeElement(System.Xml.XmlNode xmlNode, string name)
            {
                DateTime defaultValue = DateTime.MinValue;
                return Conversion.XmlNode.GetElementValue<DateTime>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.Int32 value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static int GetIntAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                int defaultValue = 0;
                return Conversion.XmlNode.GetAttributeValue<int>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a Long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A Long (System.Int64) value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static long GetLongAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                long defaultValue = 0;
                return Conversion.XmlNode.GetAttributeValue<long>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.Decimal value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static Decimal GetDecimalAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.XmlNode.GetAttributeValue<Decimal>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.Single value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static float GetFloatAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                float defaultValue = 0.0f;
                return Conversion.XmlNode.GetAttributeValue<float>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.Double value or '0' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static double GetDoubleAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                double defaultValue = 0.0;
                return Conversion.XmlNode.GetAttributeValue<double>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.Boolean.
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.Boolean value or 'false' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static bool GetBooleanAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                bool defaultValue = false;
                return Conversion.XmlNode.GetAttributeValue<bool>(xmlNode, name, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the attribute value of the 'xmlNode' parameter that
            /// matches the 'name' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="xmlNode">The xmlNode containing an attribute with the 'name' parameter.</param>
            /// <param name="name">The name of an attribte of the 'xmlNode' parameter.</param>
            /// <returns>A System.DateTime value or 'DateTime.MinValue' if the conversion fails.</returns>
            /// <remarks>Does not throw an exception if the conversion fails.</remarks>
            public static DateTime GetDateTimeAttribute(System.Xml.XmlNode xmlNode, string name)
            {
                DateTime defaultValue = DateTime.MinValue;
                return Conversion.XmlNode.GetAttributeValue<DateTime>(xmlNode, name, defaultValue, false);
            }

            [Obsolete("This does not work and needs to be documented!", true)]
            public static T GetElementValue<T>(System.Xml.XmlNode xmlNode, string name)
            {
                return Conversion.XmlNode.GetElementValue<T>(xmlNode, name, default(T), false);
            }

            public static T GetElementValue<T>(System.Xml.XmlNode xmlNode, string name, T defaultValue)
            {
                return Conversion.XmlNode.GetElementValue<T>(xmlNode, name, defaultValue, false);
            }

            public static T GetElementValue<T>(System.Xml.XmlNode xmlNode, string name, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                string stringElement = Conversion.XmlNode.GetStringElement(xmlNode, name, defaultValue.ToString(), throwException);
                try
                {
                    obj = Conversion.GetValue<T>((object)stringElement, defaultValue, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        name = string.IsNullOrEmpty(name) ? string.Empty : name;
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The element '{0}' could not be converted. {1}", new object[2]
                        {
              (object) name,
              (object) ex.Message
                        }), ex);
                    }
                }
                return obj;
            }

            [Obsolete("This does not work and needs to be documented!", true)]
            public static T GetAttributeValue<T>(System.Xml.XmlNode xmlNode, string name)
            {
                return Conversion.XmlNode.GetAttributeValue<T>(xmlNode, name, default(T), false);
            }

            public static T GetAttributeValue<T>(System.Xml.XmlNode xmlNode, string name, T defaultValue)
            {
                return Conversion.XmlNode.GetAttributeValue<T>(xmlNode, name, defaultValue, false);
            }

            public static T GetAttributeValue<T>(System.Xml.XmlNode xmlNode, string name, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                string stringAttribute = Conversion.XmlNode.GetStringAttribute(xmlNode, name, defaultValue.ToString(), throwException);
                try
                {
                    obj = Conversion.GetValue<T>((object)stringAttribute, defaultValue, throwException);
                }
                catch (Exception ex)
                {
                    if (throwException)
                    {
                        name = string.IsNullOrEmpty(name) ? string.Empty : name;
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "The attribute '{0}' could not be converted. {1}", new object[2]
                        {
              (object) name,
              (object) ex.Message
                        }), ex);
                    }
                }
                return obj;
            }
        }

        public static class Hashtable
        {
            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a String value. (String.Empty by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static string GetString(System.Collections.Hashtable hashtable, string key)
            {
                string defaultValue = string.Empty;
                return Conversion.Hashtable.GetValue<string>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a String value. (String.Empty by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static string GetString(System.Collections.Hashtable hashtable, string key, string defaultValue)
            {
                return Conversion.Hashtable.GetValue<string>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a String value. (String.Empty by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static string GetString(System.Collections.Hashtable hashtable, string key, string defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<string>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Byte. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static byte GetByte(System.Collections.Hashtable hashtable, string key)
            {
                byte defaultValue = 0;
                return Conversion.Hashtable.GetValue<byte>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Byte. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static byte GetByte(System.Collections.Hashtable hashtable, string key, byte defaultValue)
            {
                return Conversion.Hashtable.GetValue<byte>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Byte. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static byte GetByte(System.Collections.Hashtable hashtable, string key, byte defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<byte>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static int GetInt(System.Collections.Hashtable hashtable, string key)
            {
                int defaultValue = 0;
                return Conversion.Hashtable.GetValue<int>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static int GetInt(System.Collections.Hashtable hashtable, string key, int defaultValue)
            {
                return Conversion.Hashtable.GetValue<int>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int32. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static int GetInt(System.Collections.Hashtable hashtable, string key, int defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<int>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a long (System.Int64). (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static long GetLong(System.Collections.Hashtable hashtable, string key)
            {
                long defaultValue = 0;
                return Conversion.Hashtable.GetValue<long>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int64. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static long GetLong(System.Collections.Hashtable hashtable, string key, long defaultValue)
            {
                return Conversion.Hashtable.GetValue<long>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int64. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static long GetLong(System.Collections.Hashtable hashtable, string key, long defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<long>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Decimal. (0m by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static Decimal GetDecimal(System.Collections.Hashtable hashtable, string key)
            {
                Decimal defaultValue = new Decimal();
                return Conversion.Hashtable.GetValue<Decimal>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int64. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static Decimal GetDecimal(System.Collections.Hashtable hashtable, string key, Decimal defaultValue)
            {
                return Conversion.Hashtable.GetValue<Decimal>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Int64. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static Decimal GetDecimal(System.Collections.Hashtable hashtable, string key, Decimal defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<Decimal>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a float (System.Single). (0f by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static float GetFloat(System.Collections.Hashtable hashtable, string key)
            {
                float defaultValue = 0.0f;
                return Conversion.Hashtable.GetValue<float>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Single. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static float GetFloat(System.Collections.Hashtable hashtable, string key, float defaultValue)
            {
                return Conversion.Hashtable.GetValue<float>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Single. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static float GetFloat(System.Collections.Hashtable hashtable, string key, float defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<float>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Double. (0d by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static double GetDouble(System.Collections.Hashtable hashtable, string key)
            {
                double defaultValue = 0.0;
                return Conversion.Hashtable.GetValue<double>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Double. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static double GetDouble(System.Collections.Hashtable hashtable, string key, double defaultValue)
            {
                return Conversion.Hashtable.GetValue<double>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Double. (0 by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static double GetDouble(System.Collections.Hashtable hashtable, string key, double defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<double>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Boolean. (false by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static bool GetBoolean(System.Collections.Hashtable hashtable, string key)
            {
                return Conversion.Hashtable.GetValue<bool>(hashtable, key, false, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Boolean.
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <returns></returns>
            public static bool GetBoolean(System.Collections.Hashtable hashtable, string key, bool defaultValue)
            {
                return Conversion.Hashtable.GetValue<bool>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.Boolean.
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static bool GetBoolean(System.Collections.Hashtable hashtable, string key, bool defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<bool>(hashtable, key, defaultValue, throwExceptions);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.DateTime. (DateTime.MinValue by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static DateTime GetDateTime(System.Collections.Hashtable hashtable, string key)
            {
                return Conversion.Hashtable.GetValue<DateTime>(hashtable, key, DateTime.MinValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.DateTime.
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static DateTime GetDateTime(System.Collections.Hashtable hashtable, string key, DateTime defaultValue)
            {
                return Conversion.Hashtable.GetValue<DateTime>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a System.DateTime.
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value.</param>
            /// <returns></returns>
            public static DateTime GetDateTime(System.Collections.Hashtable hashtable, string key, DateTime defaultValue, bool throwExceptions)
            {
                return Conversion.Hashtable.GetValue<DateTime>(hashtable, key, defaultValue, throwExceptions);
            }

            public static T GetValue<T>(System.Collections.Hashtable hashtable, string key, T defaultValue)
            {
                return Conversion.Hashtable.GetValue<T>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a value of type [T].
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="defaultValue">The default value to return if the conversion fails.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception] if conversion fails.</param>
            /// <returns></returns>
            public static T GetValue<T>(System.Collections.Hashtable hashtable, string key, T defaultValue, bool throwException)
            {
                T obj = defaultValue;
                if (hashtable != null)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        if (hashtable.ContainsKey((object)key))
                        {
                            if (hashtable[(object)key] != null)
                                obj = Conversion.GetValue<T>(hashtable[(object)key], defaultValue, throwException);
                        }
                        else if (throwException)
                        {
                            string str = string.Empty;
                            if ((object)defaultValue != null)
                                str = defaultValue.GetType().Name;
                            throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the value of the element accessed by the ['{0}'] key to a value of type <{1}>. The key ['{0}'] does not exist.", new object[2]
                            {
                (object) key,
                (object) str
                            }));
                        }
                    }
                    else if (throwException)
                    {
                        string str = string.Empty;
                        if ((object)defaultValue != null)
                            str = defaultValue.GetType().Name;
                        throw new Conversion.ConversionException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Cannot convert the element for the System.Collections.Hashtable to a value of type <{0}>. A key was not provided.", new object[1] { (object)str }));
                    }
                }
                return obj;
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a Guid value. (Guid.Empty by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <returns></returns>
            public static Guid GetGuid(System.Collections.Hashtable hashtable, string key)
            {
                Guid defaultValue = Guid.Empty;
                return Conversion.Hashtable.GetValue<Guid>(hashtable, key, defaultValue, false);
            }

            /// <summary>
            /// Attempts to convert the value of the element in the 'hashtable' parameter indicated by
            /// the 'key' parameter to a Guid value. (Guid.Empty by default)
            /// </summary>
            /// <param name="hashtable">The Hashtable containing the element value to convert.</param>
            /// <param name="key">The key used to access the Hashtable element to convert.</param>
            /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
            /// <returns></returns>
            public static Guid GetGuid(System.Collections.Hashtable hashtable, string key, bool throwExceptions)
            {
                Guid defaultValue = Guid.Empty;
                return Conversion.Hashtable.GetValue<Guid>(hashtable, key, defaultValue, throwExceptions);
            }
        }

        /// <summary>
        /// Numeric conversion methods which strip any non-numeric characters from
        /// the text which needs to be converted. These methods may be useful for
        /// converting phone numbers, social security numbers, or currency in
        /// text form to numeric value types. For example, converting the string
        /// '$100,000.00' to 100000.
        /// 
        /// You may, for example, need these methods when accepting user text input from
        /// a web site or windows form.
        /// </summary>
        public static class Strip
        {
            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter, removes any ending
            /// decimal places, for example '.00', and attempts to converts the 'text' to a
            /// System.Int32. Does not throw exceptions and returns zero by default if the
            /// conversion fails.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <returns></returns>
            public static int GetInt(string text)
            {
                return Conversion.Strip.GetInt(text, false);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter, removes any ending
            /// decimal places, for example '.00', and attempts to converts the 'text' to a
            /// System.Int32. Returns zero by default if the conversion fails unless the
            /// 'throwException' parameter is 'true'.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static int GetInt(string text, bool throwException)
            {
                int defaultValue = 0;
                string str = string.Empty;
                try
                {
                    str = Conversion.Strip.StripNonNumericForDecimals(text);
                    if (string.IsNullOrEmpty(text))
                        throw new ArgumentException("The 'text' parameter is null or empty and therefore cannot be converted into an integer.", "text");
                    if (Conversion.Strip.ValidateSingleDecimal(text, throwException))
                    {
                        int length = str.LastIndexOf('.');
                        if (length > 0)
                            str = str.Substring(0, length);
                    }
                }
                catch (Exception ex)
                {
                    if (throwException)
                        throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "An unexpected exception occurred attempting to convert the 'text' parameter to an integer: {0}", new object[1]
                        {
              (object) ex.Message
                        }), ex);
                }
                return Conversion.GetValue<int>((object)str, defaultValue, throwException);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter, removes any ending
            /// decimal places, for example '.00', and attempts to converts the 'text' to a
            /// System.Int64. Does not throw exceptions and returns zero by default if the
            /// conversion fails.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <returns></returns>
            public static long GetLong(string text)
            {
                return Conversion.Strip.GetLong(text, false);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter, removes any ending
            /// decimal places, for example '.00', and attempts to converts the 'text' to a
            /// System.Int64. Returns zero by default if the conversion fails unless the
            /// 'throwException' parameter is 'true'.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static long GetLong(string text, bool throwException)
            {
                long defaultValue = 0;
                string str = string.Empty;
                try
                {
                    str = Conversion.Strip.StripNonNumericForDecimals(text);
                    if (string.IsNullOrEmpty(text))
                        throw new ArgumentException("The 'text' parameter is null or empty and therefore cannot be converted into an integer.", "text");
                    if (Conversion.Strip.ValidateSingleDecimal(text, throwException))
                    {
                        int length = str.LastIndexOf('.');
                        if (length > 0)
                            str = str.Substring(0, length);
                    }
                }
                catch (Exception ex)
                {
                    if (throwException)
                        throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "An unexpected exception occurred attempting to convert the 'text' parameter to an integer: {0}", new object[1]
                        {
              (object) ex.Message
                        }), ex);
                }
                return Conversion.GetValue<long>((object)str, defaultValue, throwException);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter (excluding the
            /// decimal character), and attempts to converts the 'text' to a
            /// System.Decimal. Returns zero by default if the conversion fails.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <returns></returns>
            public static Decimal GetDecimal(string text)
            {
                return Conversion.Strip.GetDecimal(text, false);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter (excluding the
            /// decimal character), and attempts to converts the 'text' to a
            /// System.Decimal. Returns zero by default if the conversion fails unless the
            /// 'throwException' parameter is 'true'.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static Decimal GetDecimal(string text, bool throwException)
            {
                Decimal num = new Decimal();
                if (Conversion.Strip.ValidateSingleDecimal(text, throwException))
                    num = Conversion.GetDecimal((object)Conversion.Strip.StripNonNumericForDecimals(text));
                return num;
            }

            public static float GetFloat(string text)
            {
                return Conversion.Strip.GetFloat(text, false);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter, removes any ending
            /// decimal places, for example '.00', and attempts to converts the 'text' to a
            /// System.Float. Returns zero by default if the conversion fails unless the
            /// 'throwException' parameter is 'true'.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static float GetFloat(string text, bool throwException)
            {
                float num = 0.0f;
                if (Conversion.Strip.ValidateSingleDecimal(text, throwException))
                    num = Conversion.GetFloat((object)Conversion.Strip.StripNonNumericForDecimals(text));
                return num;
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter (excluding the
            /// decimal character), and attempts to converts the 'text' to a
            /// System.Double. Returns zero by default if the conversion fails.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <returns></returns>
            public static double GetDouble(string text)
            {
                return Conversion.Strip.GetDouble(text, false);
            }

            /// <summary>
            /// Strips any non-numeric characters from the 'text' parameter (excluding the
            /// decimal character), and attempts to converts the 'text' to a
            /// System.Double. Returns zero by default if the conversion fails unless the
            /// 'throwException' parameter is 'true'.
            /// </summary>
            /// <param name="text">The text to convert.</param>
            /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
            /// <returns></returns>
            public static double GetDouble(string text, bool throwException)
            {
                double num = 0.0;
                if (Conversion.Strip.ValidateSingleDecimal(text, throwException))
                    num = Conversion.GetDouble((object)Conversion.Strip.StripNonNumericForDecimals(text));
                return num;
            }

            /// <summary>
            /// Strips the 'text' parameter of any non-numeric characters.
            /// CAUTION: Will strip out any decimal point as well!
            /// </summary>
            /// <param name="text">The text to strip.</param>
            /// <returns></returns>
            /// <remarks>
            /// This method is useful, for example, when handling text such as
            /// phone numbers, social security numbers, or US zip codes. This
            /// method will strip out anything non-numeric, including the '.'
            /// character, so if you're dealing with currency or decimal
            /// numbers, use the StripNonNumericForDecimals() method instead.
            /// </remarks>
            public static string StripNonNumeric(string text)
            {
                return Regex.Replace(text, "\\D", string.Empty);
            }

            /// <summary>
            /// Strips the 'text' parameter of an non-numeric characters.
            /// Does NOT strip out the '.' character so the resulting text
            /// may be converted to a decimal or currency.
            /// </summary>
            /// <param name="text">The text to strip.</param>
            /// <returns></returns>
            /// <remarks>
            /// This method is useful, for example, when handling text such as
            /// decimal numbers or currency. Use the StripNonNumeric() method
            /// to strip out ALL non-numeric characters. For example, phone numbers,
            /// social security numbers, or US zip codes.
            /// </remarks>
            public static string StripNonNumericForDecimals(string text)
            {
                return Regex.Replace(text, "[^0-9\\.]", string.Empty);
            }

            /// <summary>
            /// Validates that the text parameter contains, at most, a single '.' character.
            /// If text contains more than a single decimal character, and an attempt is made
            /// to parse the text, the conversion will fail.
            /// </summary>
            /// <param name="text">The text.</param>
            /// <returns></returns>
            /// <remarks>
            /// As an example, there may be the need to convert text to an integer - for example:
            /// 
            ///     $1,000.00
            /// 
            /// To convert the text to an integer, the '$' and ',' characters must be stripped.
            /// If the '.' is stripped, then the text will be inadvertently converted to '100000'
            /// not '1000' so it's important that the decimal is not stripped.
            /// 
            /// However, when converting to an integer the decimal places must be truncated. To
            /// ensure a valid conversion, there can't be more than one '.' character. For
            /// example, the following would be invalid:
            /// 
            ///     $1,000.000.00
            /// 
            /// This method validates that there is at most, one decimal character.
            /// 
            /// See the 'StripInt()' and 'StripDecimal()' methods which call this method.
            /// </remarks>
            public static bool ValidateSingleDecimal(string text, bool throwException)
            {
                bool flag = true;
                try
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        MatchCollection matchCollection = Regex.Matches(text, "[\\.]");
                        if (matchCollection.Count > 1)
                        {
                            flag = false;
                            if (throwException)
                                throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "After stripping out all non-numeric characters, the 'text' parameter ('{0}') contains '{1}' decimal points which is not allowed. The 'text' parameter may only contain, at most, one decimal point.", new object[2]
                                {
                  (object) text,
                  (object) matchCollection.Count
                                }), "text");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (throwException)
                        throw new ArgumentException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "An unexpected exception occurred: {0}", new object[1]
                        {
              (object) ex.Message
                        }), "text");
                }
                return flag;
            }

            /// <summary>
            /// Strips off the ending '.00' from the 'text' parameter. Useful for converting
            /// the 'text' parameter to an integer or long. If the ending decimal is not
            /// stripped off, conversion would fail.
            /// </summary>
            /// <param name="text">The text to truncate.</param>
            /// <returns></returns>
            /// <remarks>
            /// For example, attempting to convert the following string to an integer would fail:
            /// 
            ///     '100.00'
            /// 
            /// This method removes the ending '.00' and returns:
            /// 
            ///     '100'
            /// 
            /// </remarks>
            public static string TruncateDecimalText(string text)
            {
                string str = text;
                if (!string.IsNullOrEmpty(text))
                {
                    int length = str.LastIndexOf('.');
                    if (length > 0)
                        str = str.Substring(0, length);
                }
                return str;
            }
        }

        [Serializable]
        public class ConversionException : Exception
        {
            public ConversionException()
            {
            }

            public ConversionException(string message)
              : base(message)
            {
            }

            public ConversionException(string message, Exception innerException)
              : base(message, innerException)
            {
            }

            protected ConversionException(SerializationInfo info, StreamingContext context)
              : base(info, context)
            {
            }
        }
    }
}
