namespace PWC.Common.Utilities
{
    public class IPUtils
    {
        #region Methods

        public static long? ConvertIPToInteger(string ip)
        {
            long? result = 0;
            if (!string.IsNullOrWhiteSpace(ip))
            {
                string[] octets = ip.Split('.');
                if (octets != null && octets.Length == 4)
                {
                    long a, b, c, d;
                    a = int.Parse(octets[0]);
                    b = int.Parse(octets[1]);
                    c = int.Parse(octets[2]);
                    d = int.Parse(octets[3]);
                    result = (a * 16777216) + (b * 65536) + (c * 256) + (d);
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }

            return result;
        }

        public static string GetClientIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            // Code snippet user to handle HAProxy
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        #endregion
    }
}
