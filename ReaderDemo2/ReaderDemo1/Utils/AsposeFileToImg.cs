using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ReaderDemo.Utils
{
    public class AsposeFileToImg
    {
        private static string imageDirectoryPath = "";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="currentNo">当前页</param>
        /// <param name="pageSize">每页大小</param>
        public static void FileToImg(string filePath,int currentNo,int pageSize)
        {
            if (File.Exists(filePath))
            {
                FileInfo pdfFi = new FileInfo(filePath);
                string filename = pdfFi.Name.Replace(pdfFi.Extension, "");
                imageDirectoryPath = System.IO.Path.Combine(pdfFi.DirectoryName, filename);
                if (!Directory.Exists(imageDirectoryPath))
                {
                    Directory.CreateDirectory(imageDirectoryPath);
                }
                string a = Path.GetExtension(filePath).ToLower();
                if (a == ".pdf")
                {
                    PdfToImg(filePath,currentNo,pageSize);
                }
                else
                {
                    if (a == ".doc" || a == ".docx")
                    {
                        DocToImg(filePath, currentNo, pageSize);
                    }
                    else
                    {
                        if (a == ".ppt")
                        {
                            PptToImg(filePath, currentNo, pageSize);
                        }
                        else if (a == ".pptx")
                        {
                            PptxToImg(filePath, currentNo, pageSize);
                        }
                        else
                        {
                            if (a == ".xls" || a == ".xlsx")
                            {
                                XlsToImg(filePath, currentNo, pageSize);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="currentNo">当前页</param>
        /// <param name="pageSize">每页大小</param>
        private static void PdfToImg(string filePath, int currentNo, int pageSize)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            //FileStream docStream = new FileStream(filePath, FileMode.Open);
            Aspose.Pdf.Document pdf = new Aspose.Pdf.Document(filePath);
            //pdf.Pages.Count 总页数
            int num = ((currentNo - 1) * pageSize) + 1;
            if (pdf.Pages.Count >= (num + pageSize))
            {
                for (int i = num; i < num + pageSize; i++)
                {
                    PdfToImgMethod(filename,i,pdf);
                }
            }
            if (pdf.Pages.Count < (num + pageSize) && num < pdf.Pages.Count)
            {
                for (int i = num; i < pdf.Pages.Count + 1; i++)
                {
                    PdfToImgMethod(filename,i,pdf);
                }
            }
        }
        /// <summary>
        /// PdfToImgMethod
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="i"></param>
        /// <param name="pdf"></param>
        private static void PdfToImgMethod(string filename, int i, Aspose.Pdf.Document pdf)
        {
            try
            {
                string imgpath = imageDirectoryPath + "/" + filename + "_" + i + ".jpeg";
                if (!File.Exists(imgpath))
                {
                    using (FileStream imageStream = new FileStream(imgpath, FileMode.Create))
                    {
                        Aspose.Pdf.Devices.Resolution resolution = new Aspose.Pdf.Devices.Resolution(90); // 像素
                        Aspose.Pdf.Devices.JpegDevice jpegDevice = new Aspose.Pdf.Devices.JpegDevice(resolution, 80); // 图片质量

                        jpegDevice.Process(pdf.Pages[i], imageStream);
                        imageStream.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// 获取pdf总数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int getPDFCount(string filePath)
        {
            //FileStream docStream = new FileStream(filePath, FileMode.Open);
            var pdf = new Aspose.Pdf.Document(filePath);
            int pdfCount = pdf.Pages.Count;
            return pdfCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="currentNo">当前页</param>
        /// <param name="pageSize">每页大小</param>
        private static void DocToImg(string filePath, int currentNo, int pageSize)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            var words = new Aspose.Words.Document(filePath);

            Aspose.Words.Saving.ImageSaveOptions imageSaveOptions =
                   new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Jpeg);
            imageSaveOptions.Resolution = 90; // 默认128

            int pageindex = 0;
            int pagecount = words.PageCount;
            int num = ((currentNo - 1) * pageSize) + 1;
            if (pagecount >= (num + pageSize))
            {
                for (int i = num; i < num + pageSize; i++)
                {
                    pageindex = DocToImgMethod(filename, words, imageSaveOptions, pageindex, i);
                }
            }
            if (pagecount < (num + pageSize) && num < pagecount)
            {
                for (int i = num; i < pagecount + 1; i++)
                {
                    pageindex = DocToImgMethod(filename, words, imageSaveOptions, pageindex, i);
                }
            }
        }
        /// <summary>
        /// DocToImgMethod
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="words"></param>
        /// <param name="imageSaveOptions"></param>
        /// <param name="pageindex"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int DocToImgMethod(string filename, Aspose.Words.Document words, Aspose.Words.Saving.ImageSaveOptions imageSaveOptions, int pageindex, int i)
        {
            try
            {
                string imgpath = imageDirectoryPath + "/" + filename + "_" + i + ".jpeg";
                if (!File.Exists(imgpath))
                {
                    imageSaveOptions.PageIndex = pageindex;
                    words.Save(imgpath, imageSaveOptions);
                    pageindex++;
                }
            }
            catch (Exception)
            {

            }
            return pageindex;
        }
        /// <summary>
        ///获取Doc页总数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int getDocCount(string filePath) 
        {
            var words = new Aspose.Words.Document(filePath);
            Aspose.Words.Saving.ImageSaveOptions imageSaveOptions =
                   new Aspose.Words.Saving.ImageSaveOptions(Aspose.Words.SaveFormat.Jpeg);
            imageSaveOptions.Resolution = 90; // 默认128
            int pagecount = words.PageCount;
            return pagecount;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="filePath">文件路径</param>
       /// <param name="currentNo">当前页</param>
       /// <param name="pageSize">每页大小</param>
        private static void PptToImg(string filePath, int currentNo, int pageSize)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            var ppt = new Aspose.Slides.Presentation(filePath);
            string imgpath = imageDirectoryPath + "/" + filename + ".pdf";
            if (!File.Exists(imgpath))
            {
                ppt.Save(imgpath, Aspose.Slides.Export.SaveFormat.Pdf);
            }
            PdfToImg(imgpath, currentNo, pageSize);
        }
        private static void PptxToImg(string filePath, int currentNo, int pageSize)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            var ppt = new Aspose.Slides.Pptx.PresentationEx(filePath);
            string imgpath = imageDirectoryPath + "/" + filename + ".pdf";
            if (!File.Exists(imgpath))
            {
                ppt.Save(imgpath, Aspose.Slides.Export.SaveFormat.Pdf);
            }
            PdfToImg(imgpath,currentNo,pageSize);
        }
        /// <summary>
        /// 获取ppt页总数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int getPptCount(string filePath) 
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            var ppt = new Aspose.Slides.Pptx.PresentationEx(filePath);
            string imgpath = imageDirectoryPath + "/" + filename + ".pdf";
            if (!File.Exists(imgpath))
            {
                ppt.Save(imgpath, Aspose.Slides.Export.SaveFormat.Pdf);
            }
            int pptCount = getPDFCount(filePath);
            return 0;
        }
        public static int getPptxCount(string filePath)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            var ppt = new Aspose.Slides.Pptx.PresentationEx(filePath);
            string imgpath = imageDirectoryPath + "/" + filename + ".pdf";
            if (!File.Exists(imgpath))
            {
                ppt.Save(imgpath, Aspose.Slides.Export.SaveFormat.Pdf);
            }
            int pptxCount = getPDFCount(filePath);
            return pptxCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="currentNo">当前页</param>
        /// <param name="pageSize">每页大小</param>
        private static void XlsToImg(string filePath, int currentNo, int pageSize)
        {
            string filename = Path.GetFileNameWithoutExtension(filePath).ToLower();
            Workbook book = new Workbook(filePath);
            //创建一个图表选项的对象
            Aspose.Cells.Rendering.ImageOrPrintOptions imgOptions = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            imgOptions.ImageFormat = ImageFormat.Jpeg;
            int count = book.Worksheets.Count;
            int num = ((currentNo - 1) * pageSize) + 1;
            if (count >= (num + pageSize))
            {
                for (int i = num; i < (num + pageSize); i++)
                {
                    XlsToImgMothed(filename, book, imgOptions, i);
                }
            }
            if (count < (num + pageSize) && num < count)
            {
                for (int i = num; i < count + 1; i++)
                {
                    XlsToImgMothed(filename, book, imgOptions, i);
                }
            }
        }

        private static void XlsToImgMothed(string filename, Workbook book, Aspose.Cells.Rendering.ImageOrPrintOptions imgOptions, int i)
        {
            //获取一张工作表
            Worksheet sheet = book.Worksheets[i];
            //创建一个纸张底色渲染对象
            Aspose.Cells.Rendering.SheetRender sr = new Aspose.Cells.Rendering.SheetRender(sheet, imgOptions);
            for (int j = 0; j < sr.PageCount; j++)
            {
                string imgpath = imageDirectoryPath + "/" + filename + "_" + i + "_" + j + "_.jpeg";
                if (!File.Exists(imgpath))
                {
                    sr.ToImage(j, imgpath);
                }
            }
        }
        /// <summary>
        /// 获取Xls页总数
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int getXlsCount(string filePath)
        {
            Workbook book = new Workbook(filePath);
            //创建一个图表选项的对象
            Aspose.Cells.Rendering.ImageOrPrintOptions imgOptions = new Aspose.Cells.Rendering.ImageOrPrintOptions();
            imgOptions.ImageFormat = ImageFormat.Jpeg;
            int xlsCount = book.Worksheets.Count;
            return xlsCount;
        }
    }
}