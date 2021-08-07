using PWC.Common.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace System
{
    public static class FileUtils
    {
        private static ILogger log = ApplicationLogging.CreateLogger("FileUtils");

        #region Constants

        private const string STRING_FORMAT_0_1 = "{0}{1}";
        private const string STRING_FORMAT_0_1_2 = "{0}\\{1}\\{2}";
        private const string UPLOAD_FILE_ERROR = "Error in file uploading : ";

        #endregion

        #region Methods

        /// <summary>
        /// get the uploaded file folder path 
        /// </summary>
        /// <param name="classKey"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetUploadFolderPath(string classKey, string resourceKey)
        {
            return String.Format(STRING_FORMAT_0_1, HttpContext.Current.Request.PhysicalApplicationPath.ToString(), HttpContext.GetGlobalResourceObject(classKey, resourceKey).ToString());
        }

        /// <summary>
        ///  upload file to Folder
        /// </summary>
        /// <param name="fuControl"></param>
        /// <param name="folderName"></param>
        /// <param name="classKey"></param>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string UploadFile(HttpPostedFileBase fuControl, string folderName, out string errorMsg)
        {
            string filePath;

            try
            {
                filePath = string.Format(STRING_FORMAT_0_1_2, HttpContext.Current.Request.PhysicalApplicationPath.ToString(), folderName, fuControl.FileName);
                fuControl.SaveAs(filePath);
                errorMsg = string.Empty;
            }
            catch (Exception ex)
            {
                errorMsg = string.Format(STRING_FORMAT_0_1, UPLOAD_FILE_ERROR, ex.Message);
                filePath = null;
            }

            return filePath;
        }

        /// <summary>
        /// delete file physically 
        /// </summary>
        /// <param name="path"></param>
        public static void DeletePhysicalFile(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    File.Delete(path);
                }
            }
            catch (IOException ex)
            {
                log.LogTrace(ex, "Exception on DeletePhysicalFile Method: ");
            }

        }
    
        #endregion
    }
}
