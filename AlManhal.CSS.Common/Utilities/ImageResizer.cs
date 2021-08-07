using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace PWC.GlobalComponent.Utils
{
    public static class ImageResizer
    {
        #region Constants

        private const string JPEG_IMAGE_QUALITY = "Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.";
        private const string IMAGE_EXTENSION = "image/jpeg";
        #endregion 

        #region Variables
        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        private static Dictionary<string, ImageCodecInfo> encoders = null;

        /// <summary>
        /// A quick lookup for getting image encoders
        /// </summary>
        public static Dictionary<string, ImageCodecInfo> Encoders
        {
            //get accessor that creates the dictionary on demand
            get
            {
                //if the quick lookup isn't initialised, initialise it
                if (encoders == null)
                {
                    encoders = new Dictionary<string, ImageCodecInfo>();
                }

                //if there are no codecs, try loading them
                if (encoders.Count == 0)
                {
                    //get all the codecs
                    foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageEncoders())
                    {
                        //add each codec to the quick lookup
                        encoders.Add(codec.MimeType.ToLower(), codec);
                    }
                }

                //return the lookup
                return encoders;
            }
        }

        #endregion

        #region Methods
        // Saves the image to specific location, save location includes filename
        public static void saveImageToLocation(System.Drawing.Image theImage, string saveLocation)
        {
            // Strip the file from the end of the dir
            string saveFolder = Path.GetDirectoryName(saveLocation);
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            // Save to disk
            theImage.Save(saveLocation);
        }

        // Resizes the image and saves it to disk.  Save as property is full path including file extension
        public static void resizeImageAndSave(Image ImageToResize, int newWidth, int maxHeight, bool onlyResizeIfWider, string thumbnailSaveAs, int quality = 100)
        {
            Image thumbnail = resizeImage(ImageToResize, newWidth, maxHeight, onlyResizeIfWider);
            SaveJpeg(thumbnailSaveAs, thumbnail, quality);
        }
        // Overload if filepath is passed in
        public static void resizeImageAndSave(string imageLocation, int newWidth, int maxHeight, bool onlyResizeIfWider, string thumbnailSaveAs, int quality = 100)
        {
            Image loadedImage = Image.FromFile(imageLocation);
            Image thumbnail = resizeImage(loadedImage, newWidth, maxHeight, onlyResizeIfWider);

            SaveJpeg(thumbnailSaveAs, thumbnail, quality);
            //saveImageToLocation(thumbnail, thumbnailSaveAs);
        }

        // Returns the thumbnail image when an image object is passed in
        public static Image resizeImage(Image ImageToResize, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            // Prevent using images internal thumbnail
            ImageToResize.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            ImageToResize.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            // Set new width if in bounds
            if (onlyResizeIfWider)
            {
                if (ImageToResize.Width <= newWidth)
                {
                    newWidth = ImageToResize.Width;
                }
            }

            // Calculate new height
            int newHeight = ImageToResize.Height * newWidth / ImageToResize.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = ImageToResize.Width * maxHeight / ImageToResize.Height;
                newHeight = maxHeight;
            }

            //a holder for the result
            Bitmap result = new Bitmap(newWidth, newHeight);
            //set the resolutions the same to avoid cropping due to resolution differences
            result.SetResolution(ImageToResize.HorizontalResolution, ImageToResize.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(ImageToResize, 0, 0, result.Width, result.Height);
            }

            //// Clear handle to original file so that we can overwrite it if necessary
            ImageToResize.Dispose();

            return result;
        }

        // Overload if file path is passed in instead
        public static Image resizeImage(string imageLocation, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            Image loadedImage = Image.FromFile(imageLocation);
            return resizeImage(loadedImage, newWidth, maxHeight, onlyResizeIfWider);
        }

        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path">Path to which the image would be saved.</param> 
        /// <param name="quality">An integer from 0 to 100, with 100 being the 
        /// highest quality</param> 
        /// <exception cref="ArgumentOutOfRangeException">
        /// An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            //ensure the quality is within the correct range
            if ((quality < 0) || (quality > 100))
            {
                //create the error message
                string error = string.Format(JPEG_IMAGE_QUALITY, quality);
                //throw a helpful exception
                throw new ArgumentOutOfRangeException(error);
            }

            //create an encoder parameter for the image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            //get the jpeg codec
            ImageCodecInfo jpegCodec = GetEncoderInfo(IMAGE_EXTENSION);

            //create a collection of all parameters that we will pass to the encoder
            EncoderParameters encoderParams = new EncoderParameters(1);
            //set the quality parameter for the codec
            encoderParams.Param[0] = qualityParam;
            //save the image using the codec and the parameters
            image.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //do a case insensitive search for the mime type
            string lookupKey = mimeType.ToLower();

            //the codec to return, default to null
            ImageCodecInfo foundCodec = null;

            //if we have the encoder, get it to return
            if (Encoders.ContainsKey(lookupKey))
            {
                //pull the codec from the lookup
                foundCodec = Encoders[lookupKey];
            }

            return foundCodec;
        }

        #endregion
    }
}
