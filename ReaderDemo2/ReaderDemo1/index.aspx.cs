using ReaderDemo.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ReaderDemo1
{
    public partial class index : System.Web.UI.Page
    {
        public static string pageTate = string.Empty;
        
        /// <summary>
        /// 项目路径
        /// </summary>
        private static string projectPath = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static index() {
            projectPath = System.Web.HttpRuntime.AppDomainAppPath.ToString();
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 获取文档总页数
            pageTate = AsposeFileToImg.getPDFCount(projectPath+"imgs/aaa.pdf").ToString();

        }
        /// <summary>
        /// 获取指定页的内容
        /// </summary>
        /// <param name="stratPage">当前页</param>
        /// <param name="page">每页数</param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static List<Dictionary<string, object>> FindPage(int stratPage, int page)
        {
            // 指定 Dictionary List集合存储每次查询的图片相关信息
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            int pdfCount = AsposeFileToImg.getPDFCount(projectPath + "imgs/aaa.pdf");
            // 分解相应页数的文档图片
            AsposeFileToImg.FileToImg(projectPath + "imgs/aaa.pdf", stratPage, page);
            // 获取文件名
            string filename = Path.GetFileNameWithoutExtension(projectPath + "‪imgs/aaa.pdf").ToLower();
            // 判断文件夹是否存在
            //if (Directory.Exists(projectPath + "imgs/" + filename))
            //{
            //    DirectoryInfo dir = new DirectoryInfo(projectPath + "imgs/" + filename);
            //    FileInfo[] fiList = dir.GetFiles();
            //    Console.WriteLine(fiList);
            //}
            int num = ((stratPage - 1) * page) + 1;
            // 判断当前获取的内容是否超过了文档页数大小
            if (pdfCount >= (num + page))
            {
                for (int i = num; i < num + page; i++)
                {
                    productData(list, i, filename);
                }
            }
            // 页数超过文档页进行处理
            if (pdfCount < (num + page) && num < pdfCount)
            {
                for (int i = num; i < pdfCount+1; i++)
                {
                    productData(list, i, filename);
                }
            }
            return list;
        }
        
        /// <summary>
        /// 制造数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> productData(List<Dictionary<string, object>> list, int i, string filename)
        {
            string img = System.Configuration.ConfigurationManager.AppSettings["WebImgPath"].ToString();
            string imgPath = img + "imgs/" + filename + "/" + filename + "_" + i + ".jpeg";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("cnt", i);
            dic.Add("path", imgPath);
            list.Add(dic);

            return list;
        }
    }
}