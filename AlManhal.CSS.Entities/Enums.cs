using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities
{
    public class Enums
    {
        public enum AppLanguages : int
        {
            [Description("en-US")]
            English = 1,
            [Description("ar-JO")]
            Arabic = 2,
            [Description("tr-TR")]
            Turkish = 3,
            [Description("fr-FR")]
            French = 4
        }

        public enum LanguageCodes : int
        {
            [Description("English")]
            en = 1,
            [Description("Arabic")]
            ar = 2,
            [Description("Turkish")]
            tr = 3,
            [Description("French")]
            fr = 4
        }

        public enum SessionVariablesKeys : int
        {
            [Description("UserInfo")]
            UserInfo = 1,
        }

        public enum UserType
        {
            Admin = 1,
            Customer = 2,
        }

        public enum ComplaintStatus
        {
            [Description("Resolved")]
            Resolved = 1,
            [Description("Pending")]
            Pending = 2,
            [Description("Dismissed")]
            Dismissed = 3
        }


        public enum Issuer
        {
            DC = 1,
            CSS = 2
        }

        #region Helpers

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        #endregion
    }
}
