using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace System
{
    public class DataValidationUtils
    {
        #region Methods

        public static object CheckInputValue(char? Char)
        {

            if (Char.HasValue)
            {
                return Char;
            }
            else
            {

                return (object)DBNull.Value;
            }

        }

        public static object CheckInputValue(string str)
        {

            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return (object)DBNull.Value;
            }

        }

        public static object CheckInputValue(int? integer)
        {
            if (integer.HasValue)
            {
                if (integer.Value == -1)
                {
                    return (object)DBNull.Value;
                }
                else
                {
                    return integer;
                }
            }
            else
            {
                return (object)DBNull.Value;
            }
        }

        public static object CheckInputValue(short? shortnum)
        {

            if (shortnum.HasValue)
            {
                if (shortnum.Value == -1)
                {
                    return (object)DBNull.Value;
                }
                else
                {
                    return shortnum;
                }
            }
            else
            {
                return (object)DBNull.Value;
            }
        }

        public static object CheckInputValue(bool? BooleanValue)
        {

            if (BooleanValue.HasValue)
            {
                return BooleanValue;
            }
            else
            {
                return (object)DBNull.Value;
            }

        }

        public static object CheckInputValue(double? dbl)
        {
            if (dbl.HasValue)
            {

                if (dbl.Value == -1 || dbl.Value == -1.5)
                {
                    return (object)DBNull.Value;
                }
                else
                {
                    return dbl;
                }

            }
            else
            {
                return (object)DBNull.Value;
            }
        }

        public static object CheckInputValue(decimal? Dec)
        {

            if (Dec.HasValue)
            {
                if (Dec.Value == -1)
                {
                    return (object)DBNull.Value;
                }
                else
                {
                    return Dec;
                }

            }
            else
            {
                return (object)DBNull.Value;
            }

        }

        public static object CheckInputValue(float? flo)
        {

            if (flo.HasValue)
            {

                if (flo.Value == -1 || flo.Value == -1.5)
                {
                    return (object)DBNull.Value;
                }
                else
                {
                    return flo;
                }

            }
            else
            {
                return (object)DBNull.Value;
            }

        }

        public static object CheckInputValue(DateTime? Date)
        {

            if (Date.HasValue)
            {
                if (Date.Value.Date.Year != 1900)
                {
                    return Date;
                }
                else
                {
                    return (object)DBNull.Value;
                }
            }
            else
            {
                return (object)DBNull.Value;
            }


        }

        public static object CheckInputValue(TimeSpan? Time)
        {
            if (Time.HasValue)
            {
                return Time;
            }
            else
            {
                return (object)DBNull.Value;
            }


        }

        public static bool IsValidEmail(string email)
        {
            string emailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex regex = new Regex(emailPattern);
            return regex.IsMatch(email);
        }

        public static bool IsValidNonSpecialCharsText(string text)
        {
            string nonSpecialCharsPattern = @"^([a-zA-Z0-9.]+@){0,1}([a-zA-Z0-9.])+$";
            Regex regex = new Regex(nonSpecialCharsPattern);
            return regex.IsMatch(text);
        }

        #endregion
    }
}
