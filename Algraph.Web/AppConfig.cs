using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Algraph.Web
{
    public class AppConfig
    {
        /// <summary>
        /// 获取站点名称。
        /// </summary>
        public static string SiteName
        {
            get { return ConfigurationManager.AppSettings["SiteName"]; }
        }
    }
}