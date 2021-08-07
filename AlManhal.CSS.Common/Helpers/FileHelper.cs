using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class FileHelper
    {
        /// <summary>
        /// Gets the MIME type corresponding to the extension of the specified file name.
        /// </summary>
        /// <param name="fileName">The file name to determine the MIME type for.</param>
        /// <returns>The MIME type corresponding to the extension of the specified file name, if found; otherwise, null.</returns>
        public static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (String.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);

            if (registryKey == null)
            {
                return null;
            }

            var value = registryKey.GetValue("Content Type") as string;

            return String.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
