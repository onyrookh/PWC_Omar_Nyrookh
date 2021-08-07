using PWC.Common.Helpers;
using Cyotek.GhostScript;
using Cyotek.GhostScript.PdfConversion;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PWC.Common.Utilities
{
    public static class PdfUtils
    {
        #region Variables

        private static ILogger log = ApplicationLogging.CreateLogger("PdfUtils");

        #endregion

        #region Constants

        private const string PDF_CONTENT_FOLDER = @"\\192.168.0.8\TPS\Files\UploadedFiles\EbraryFiles\";
        private const string PDF_CONTENT_TYPE = "application/pdf";
        private const string HTTP_HEADER_DISPOSITION = "Content-Disposition";
        private const string HTTP_HEADER_DISPOSITION_VAL = "attachment; filename={0}";
        private const string MSG_FILE_NOT_EXIST = "This file doesn't exist.";
        private const string MSG_UNEXPECTED_ERROR = "Unexpected error has been occurred, Please try again or contact your System Administrator.";
        private const string PDF_WATERMARK_TEXT = "PDFWaterMarkText";
        private const string PDF_IMAGE_PATH = "\\Assets\\Images\\PDFWatermarke.png";
        private const string PDF_ARABIC_FONT = "c:\\windows\\fonts\\arialuni.ttf";

        #endregion
        public static Bitmap[] ConvertPdfToImages(string srcPdfFilePath, out float aspectRatio)
        {
            aspectRatio = 1;
            Pdf2ImageSettings settings;
            settings = new Pdf2ImageSettings();
            settings.AntiAliasMode = AntiAliasMode.High;
            settings.Dpi = 150;
            settings.GridFitMode = GridFitMode.Topological;
            // settings.PaperSize = PaperSize.A4;
            //settings.ImageFormat = ImageFormat.Unknown;
            settings.TrimMode = PdfTrimMode.CropBox;

            Bitmap[] images = new Pdf2Image(srcPdfFilePath).GetImages();


            if (images.Length > 0)
            {
                int originalWidth = images[0].Width;
                int originalHeight = images[0].Height;
                aspectRatio = (float)originalHeight / (float)originalWidth;
            }


            return images;
        }

        #region Methods

        /// <summary>
        /// Extracts the listed page numbers from a PDF into a new PDF stream
        /// </summary>
        /// <param name="sourcePdfPath">Full path to source pdf</param>
        /// <param name="pagesToExtract">List of page numbers (1 based) to be extracted</param>
        /// <returns>Bytes array that represents the extracted pages stream</returns>

        public static byte[] ExtractPages(PdfReader reader, List<int> pagesToExtract)
        {
            if (reader != null && pagesToExtract != null && pagesToExtract.Count > 0)
            {
                // create new pdf of pages in the extractPages list
                Document document = new Document(PageSize.LETTER);
                Document.Compress = true;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                    document.Open();
                    document.AddDocListener(writer);
                    int documentPagesCount = reader.NumberOfPages;
                    foreach (int p in pagesToExtract)
                    {
                        if (p > 0 && p <= documentPagesCount)
                        {
                            document.SetPageSize(reader.GetPageSize(p));
                            document.NewPage();
                            PdfContentByte cb = writer.DirectContent;
                            PdfImportedPage pageImport = writer.GetImportedPage(reader, p);
                            int rot = reader.GetPageRotation(p);
                            float pageHeight = reader.GetPageSizeWithRotation(p).Height;
                            float pageWidth = reader.GetPageSizeWithRotation(p).Width;
                            if (rot == 90)
                            {
                                cb.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, 0, pageHeight);
                            }
                            else if (rot == 180)
                            {
                                cb.AddTemplate(pageImport, -1f, 0, 0, -1f, pageWidth, pageHeight);
                            }
                            else if (rot == 270)
                            {
                                cb.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, pageHeight, 0);
                            }
                            else
                            {
                                cb.AddTemplate(pageImport, 1.0F, 0, 0, 1.0F, 0, 0);
                            }
                        }
                        else if (p == 0)
                        {
                            byte[] arrPages = new byte[0];
                            return arrPages;
                        }
                        else
                        {
                            throw new Exception("Invalid page index: " + p);
                        }
                    }
                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Extracts the listed page numbers from a PDF into a new PDF stream with print order and waterMark and footer
        /// </summary>
        /// <param name="sourcePdfPath">Full path to source pdf</param>
        /// <param name="pagesToExtract">List of page numbers (1 based) to be extracted</param>
        /// <returns>Bytes Array of the printable pdf</returns>
        public static byte[] ExtractPagesForPrint(PdfReader oReader, List<int> pagesToExtract, string footerText, bool isArabic, string filePath = "")
        {
            if (oReader != null && pagesToExtract != null && pagesToExtract.Count > 0)
            {
                // create new pdf of pages in the extractPages list
                Document oDocument = new Document();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter oWriter = PdfWriter.GetInstance(oDocument, memoryStream);
                    oDocument.Open();
                    oDocument.AddDocListener(oWriter);
                    int documentPagesCount = oReader.NumberOfPages;

                    int PDFWriterDirection = PdfWriter.RUN_DIRECTION_LTR;

                    iTextSharp.text.Image oHeaderImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(PDF_IMAGE_PATH));
                    oHeaderImage.ScaleAbsoluteWidth(135);
                    oHeaderImage.ScaleAbsoluteHeight(50);

                    foreach (int p in pagesToExtract)
                    {
                        if (p > 0 && p <= documentPagesCount)
                        {
                            oDocument.SetPageSize(oReader.GetPageSize(p));
                            oDocument.NewPage();

                            Bitmap oPdf2Image = new Pdf2Image(filePath).GetImage(p, documentPagesCount);

                            System.Drawing.Image image = oPdf2Image;

                            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);

                            pdfImage.ScaleAbsoluteWidth(oDocument.PageSize.Width);
                            pdfImage.ScaleAbsoluteHeight(oDocument.PageSize.Height);
                            pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;

                            iTextSharp.text.Rectangle oPageRectangle = oReader.GetPageSizeWithRotation(p);
                            oHeaderImage.SetAbsolutePosition(oPageRectangle.Width - 148, oPageRectangle.Height - 58);
                            pdfImage.SetAbsolutePosition(0, 0);

                            oDocument.Add(pdfImage);
                            oDocument.Add(oHeaderImage);


                            BaseFont oBaseFont = BaseFont.CreateFont(PDF_ARABIC_FONT, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                            iTextSharp.text.Font oFont = new iTextSharp.text.Font(oBaseFont, 8);//, iTextSharp.text.Font.NORMAL, new BaseColor(108, 207, 246, 1));
                            oFont.Color = new BaseColor(108, 207, 246);

                            string[] phrases = footerText != null ? footerText.Split('\n') : null;

                            PdfPTable oPdfPTable = new PdfPTable(1);
                            if (phrases != null)
                            {
                                foreach (string phraseText in phrases)
                                {
                                    Phrase oPhrase = new Phrase(phraseText, oFont);
                                    oPdfPTable.RunDirection = PDFWriterDirection;
                                    Chunk ochunk = new Chunk(phraseText, oFont);
                                    Paragraph oParagraph = new Paragraph(ochunk);
                                    oParagraph.Alignment = Element.ALIGN_BOTTOM;
                                    PdfPCell cell = new PdfPCell(oParagraph);
                                    cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                                    cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    cell.Padding = 0;
                                    oPdfPTable.AddCell(cell);
                                }
                            }

                            PdfPTable oPdfTable = new PdfPTable(1);
                            PdfPCell PdfCell = new PdfPCell(oPdfPTable);
                            PdfCell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                            PdfCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            PdfCell.MinimumHeight = oDocument.PageSize.Height - 73;
                            PdfCell.Padding = 0;
                            PdfCell.PaddingLeft = -20;
                            PdfCell.PaddingBottom = -20;

                            oPdfTable.AddCell(PdfCell);
                            oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            oDocument.Add(oPdfTable);

                            PdfGState oGraphicsState = new PdfGState();
                            oGraphicsState.FillOpacity = 0.6f;
                        }
                        else
                        {
                            throw new Exception("Invalid page index: " + p);
                        }
                    }

                    PdfAction oJsAction = PdfAction.JavaScript("this.print({bUI:true, nStart:" + pagesToExtract.First() + ", nEnd:" + (pagesToExtract.Last()) + ", bSilent: true});\r", oWriter);
                    oWriter.AddJavaScript(oJsAction);
                    oDocument.Close();

                    return memoryStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Extracts the listed page numbers from a PDF into a new PDF stream with print order and waterMark and footer
        /// </summary>
        /// <param name="sourcePdfPath">Full path to source pdf</param>
        /// <param name="pagesToExtract">List of page numbers (1 based) to be extracted</param>
        /// <returns>Bytes Array of the printable pdf</returns>
        public static byte[] ExtractPDFPagesForDownload(PdfReader oReader, List<int> pagesToExtract, string footerText, bool isArabic, string fileName = "")
        {
            try
            {
                if (oReader != null && pagesToExtract != null && pagesToExtract.Count > 0)
                {

                    // create new pdf of pages in the extractPages list
                    Document oDocument = new Document();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter oWriter = PdfWriter.GetInstance(oDocument, memoryStream);
                        oDocument.Open();
                        oDocument.AddDocListener(oWriter);
                        int documentPagesCount = oReader.NumberOfPages;

                        int PDFWriterDirection = PdfWriter.RUN_DIRECTION_LTR;

                        iTextSharp.text.Image oHeaderImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(PDF_IMAGE_PATH));
                        oHeaderImage.ScaleAbsoluteWidth(135);
                        oHeaderImage.ScaleAbsoluteHeight(50);

                        foreach (int p in pagesToExtract)
                        {
                            if (p > 0 && p <= documentPagesCount)
                            {
                                int PageIndex = 1;
                                bool CreateImage = false;
                                string PDFPath = Path.Combine(Path.GetDirectoryName(fileName), "shredded", "pdf", Path.GetFileNameWithoutExtension(fileName) + "-" + p);
                                string imagePath = Path.Combine(Path.GetDirectoryName(fileName), "shredded", "images", Path.GetFileNameWithoutExtension(fileName) + "-" + p);
                                if (!File.Exists(PDFPath + Path.GetExtension(fileName)) || !File.Exists(imagePath + ".jpg"))
                                {
                                    PageIndex = p;
                                    PDFPath = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName));
                                    CreateImage = true;
                                }

                                PdfReader oPdfReader = new PdfReader(PDFPath + Path.GetExtension(fileName));

                                oDocument.SetPageSize(oPdfReader.GetPageSize(PageIndex));
                                oDocument.NewPage();
                                System.Drawing.Image image;

                                if (CreateImage)
                                {
                                    Bitmap oPdf2Image = new Pdf2Image(PDFPath + Path.GetExtension(fileName)).GetImage(PageIndex, oPdfReader.NumberOfPages);
                                    image = oPdf2Image;

                                }
                                else
                                {
                                    image = System.Drawing.Image.FromFile(imagePath + ".jpg");
                                }


                                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Jpeg);

                                pdfImage.ScaleAbsoluteWidth(oDocument.PageSize.Width);
                                pdfImage.ScaleAbsoluteHeight(oDocument.PageSize.Height);
                                pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;

                                iTextSharp.text.Rectangle oPageRectangle = oPdfReader.GetPageSizeWithRotation(1);
                                oHeaderImage.SetAbsolutePosition(oPageRectangle.Width - 148, oPageRectangle.Height - 58);
                                pdfImage.SetAbsolutePosition(0, 0);

                                oDocument.Add(pdfImage);
                                oDocument.Add(oHeaderImage);

                                BaseFont oBaseFont = BaseFont.CreateFont(PDF_ARABIC_FONT, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                                iTextSharp.text.Font oFont = new iTextSharp.text.Font(oBaseFont, 8);//, iTextSharp.text.Font.NORMAL, new BaseColor(108, 207, 246, 1));
                                oFont.Color = new BaseColor(108, 207, 246);

                                string[] phrases = footerText != null ? footerText.Split('\n') : null;

                                PdfPTable oPdfPTable = new PdfPTable(1);
                                if (phrases != null)
                                {
                                    foreach (string phraseText in phrases)
                                    {
                                        Phrase oPhrase = new Phrase(phraseText, oFont);
                                        oPdfPTable.RunDirection = PDFWriterDirection;
                                        Chunk ochunk = new Chunk(phraseText, oFont);
                                        Paragraph oParagraph = new Paragraph(ochunk);
                                        oParagraph.Alignment = Element.ALIGN_BOTTOM;
                                        PdfPCell cell = new PdfPCell(oParagraph);
                                        cell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        cell.Padding = 0;
                                        oPdfPTable.AddCell(cell);
                                    }
                                }

                                PdfPTable oPdfTable = new PdfPTable(1);
                                PdfPCell PdfCell = new PdfPCell(oPdfPTable);
                                PdfCell.VerticalAlignment = PdfPCell.ALIGN_BOTTOM;
                                PdfCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PdfCell.MinimumHeight = oDocument.PageSize.Height - 73;
                                PdfCell.Padding = 0;
                                PdfCell.PaddingLeft = -20;
                                PdfCell.PaddingBottom = -20;

                                oPdfTable.AddCell(PdfCell);

                                oPdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                                oDocument.Add(oPdfTable);

                                PdfGState oGraphicsState = new PdfGState();
                                oGraphicsState.FillOpacity = 0.6f;
                            }
                            else
                            {
                                throw new Exception("Invalid page index: " + p);
                            }
                        }
                        oDocument.Close();

                        return memoryStream.ToArray();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on ExtractPDFPagesForDownload Method: ");
                return null;
            }
        }


        /// <summary>
        /// Takes the list of bookmarks in Acrobat format and returns a string contains JSON array of the bookmarks
        /// </summary>
        /// <param name="list">List of bookmarks in Acrobat format</param>
        /// <returns>String contains JSON array of the bookmarks</returns>
        /// 
        public static string GenrateBookmarksObject(IList<Dictionary<string, object>> list)
        {
            //StringBuilder bookmarks = new StringBuilder();
            List<object> bookmarkList = new List<object>();
            object oBookmark = null;
            if (list != null && list.Count > 0)
            {
                list = list.Where(l => l.Keys.Any(k => k == "Title") && l.Keys.Any(k => k == "Page")).ToList();

                if (list != null && list.Count > 0)
                {
                    //bookmarks.Append("{bookmarks:[");
                    IList<Dictionary<string, object>> kids = null;
                    foreach (Dictionary<string, object> map in list)
                    {
                        oBookmark = new
                        {
                            Page = GetBookmarkPage(map["Page"].ToString()),
                            Title = map["Title"].ToString()
                        };
                        bookmarkList.Add(oBookmark);

                        if (kids != null)
                        {
                            GenrateBookmarksObject(kids);
                        }
                    }
                    //bookmarks.Remove(bookmarks.Length - 1, 1).Append("]}");
                }
            }

            return JsonConvert.SerializeObject(bookmarkList);
        }

        /// <summary>
        /// Gets the page number of an Acrobat format bookmark
        /// </summary>
        /// <param name="pageCode">Acrobat format page code</param>
        /// <returns>Page number (1 based)</returns>
        private static string GetBookmarkPage(string pageCode)
        {
            return pageCode.Remove(pageCode.IndexOf(' '));
        }

        /// <summary>
        /// Flushes the PDF file which has a specified MLB & ORG
        /// </summary>
        /// <param name="filePath">ORG</param>
        /// <param name="fileName">The name of the downloaded file</param>
        public static byte[] DownloadPDF(string filePath, string fileName, string UserName, string customerName, bool isArabic, int startPage = 0, int endPage = 0, string footerText = "")
        {
            HttpResponse oResponse = HttpContext.Current.Response;
            //bool downloadSucceded = false;
            byte[] content = null;
            oResponse.Clear();
            try
            {
                if (filePath != null)
                {
                    if (File.Exists(filePath))
                    {
                        PdfReader pdfReader = new PdfReader(filePath);
                        int EndPage = endPage > 0 ? endPage : startPage;
                        List<int> pages = new List<int>();
                        for (int i = startPage; i <= endPage; i++)
                        {
                            pages.Add(i);
                        }

                        using (MemoryStream outputStream = new MemoryStream(ExtractPDFPagesForDownload(pdfReader, pages, footerText, isArabic, filePath)))
                        {
                            int numberOfPages = pdfReader.NumberOfPages;
                            PdfStamper pdfStamper = new PdfStamper(pdfReader, outputStream);

                            if (startPage <= 0)
                            {
                                startPage = 1;
                            }

                            if (endPage <= 0)
                            {
                                endPage = pdfReader.NumberOfPages;
                            }

                            for (int pageIndex = startPage; pageIndex <= endPage; pageIndex++)
                            {
                                //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                                iTextSharp.text.Rectangle pageRectangle = pdfReader.GetPageSizeWithRotation(pageIndex);

                                PdfContentByte pdfData = pdfStamper.GetOverContent(pageIndex);

                                PdfContentByte pdfImageData = pdfStamper.GetOverContent(pageIndex);
                                PdfGState graphicsState = new PdfGState();
                                graphicsState.FillOpacity = 0.6f;
                                //set graphics state to pdfcontentbyte
                                pdfImageData.SetGState(graphicsState);
                            }
                            //close stamper and output filestream
                            //used using instead
                            //pdfStamper.Close();
                            content = outputStream.ToArray();
                            pdfReader.Close();
                            outputStream.Close();
                            return content;
                        }
                    }
                    else
                    {
                        oResponse.Write(MSG_FILE_NOT_EXIST);
                        //downloadSucceded = false;
                    }
                }
                else
                {
                    oResponse.Write(MSG_FILE_NOT_EXIST);
                    //downloadSucceded = false;
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on DownloadPDF Method: ");
            }
            return content;
        }

        public static byte[] ExtractPagesForImages(PdfReader oReader, List<int> pagesToExtract, string footerText, bool isArabic, bool isDownload = false)
        {
            if (oReader != null && pagesToExtract != null && pagesToExtract.Count > 0)
            {
                // create new pdf of pages in the extractPages list
                Document oDocument = new Document();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter oWriter = PdfWriter.GetInstance(oDocument, memoryStream);
                    oDocument.Open();
                    oDocument.AddDocListener(oWriter);
                    int documentPagesCount = oReader.NumberOfPages;

                    int PDFWriterDirection = PdfWriter.RUN_DIRECTION_LTR;
                    int PDFLang = 0;
                    var startingPoint = 15;
                    var alignment = Element.ALIGN_LEFT;


                    iTextSharp.text.Image oHeaderImage = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(PDF_IMAGE_PATH));
                    oHeaderImage.ScaleAbsoluteWidth(135);
                    oHeaderImage.ScaleAbsoluteHeight(50);

                    foreach (int p in pagesToExtract)
                    {
                        if (p > 0 && p <= documentPagesCount)
                        {
                            oDocument.SetPageSize(oReader.GetPageSize(p));
                            oDocument.NewPage();

                            PdfContentByte oPdfContentByte = oWriter.DirectContent;
                            PdfImportedPage pageImport = oWriter.GetImportedPage(oReader, p);
                            int rot = oReader.GetPageRotation(p);
                            if (rot == 90 || rot == 270)
                            {
                                oPdfContentByte.AddTemplate(pageImport, 0, -1.0F, 1.0F, 0, 0, oReader.GetPageSizeWithRotation(p).Height);
                            }
                            else
                            {
                                oPdfContentByte.AddTemplate(pageImport, 1.0F, 0, 0, 1.0F, 0, 0);
                            }

                            iTextSharp.text.Rectangle oPageRectangle = oReader.GetPageSizeWithRotation(p);
                            oHeaderImage.SetAbsolutePosition(oPageRectangle.Width - 148, oPageRectangle.Height - 58);

                            BaseFont oBaseFont = BaseFont.CreateFont(PDF_ARABIC_FONT, BaseFont.IDENTITY_H, true);
                            iTextSharp.text.Font oFont = new iTextSharp.text.Font(oBaseFont, 8, iTextSharp.text.Font.NORMAL, new BaseColor(108, 207, 246, 1));
                            int floatY = 62;
                            string[] phrases = footerText != null ? footerText.Split('\n') : null;
                            if (phrases != null)
                            {
                                foreach (string phraseText in phrases)
                                {
                                    ColumnText.ShowTextAligned(oPdfContentByte, alignment, new Phrase(phraseText, oFont), startingPoint, floatY, 0, PDFWriterDirection, PDFLang);
                                    floatY -= 9;
                                }
                            }

                            PdfGState oGraphicsState = new PdfGState();
                            oGraphicsState.FillOpacity = 0.6f;

                            oPdfContentByte.SetGState(oGraphicsState);
                            oPdfContentByte.AddImage(oHeaderImage);
                        }
                        else
                        {
                            throw new Exception("Invalid page index: " + p);
                        }
                    }

                    if (!isDownload)
                    {
                        PdfAction oJsAction = PdfAction.JavaScript("this.print({bUI:true, nStart:" + pagesToExtract.First() + ", nEnd:" + (pagesToExtract.Last()) + ", bSilent: true});\r", oWriter);
                        oWriter.AddJavaScript(oJsAction);
                    }
                    oDocument.Close();

                    return memoryStream.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
