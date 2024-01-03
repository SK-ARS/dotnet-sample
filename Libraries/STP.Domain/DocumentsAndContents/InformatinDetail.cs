using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace STP.Domain.DocumentsAndContents
{
    public class InformatinDetail
    {
        /// <summary>
        /// Use for requested page name.Ex. download, link, news story, info story
        /// </summary>
        public string PageName { get; set; }
        /// <summary>
        /// Identify for Details Used for  Download,link,news story and infor story. Ex.ManagedContentItemTypeEnum
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Store Portal user.
        /// </summary>
        public string PortalName { get; set; }

        /// <summary>
        /// Hot news content id
        /// </summary>
        [Display(Name = "Hot news content id")]
        public double HotNewsContentId { get; set; }

        /// <summary>
        /// Hot news name
        /// </summary>
        [Display(Name = "Hot news name")]
        public string HotNewsName { get; set; }
        /// <summary>
        /// Hot news
        /// </summary>
        [Display(Name = "Hot news")]
        public string HotNews { get; set; }
    }
}