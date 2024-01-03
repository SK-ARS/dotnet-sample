#region namespaces
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
#endregion

namespace STP.Web.Content.UIHelpers.Movements
{
    public static class MovementInbox
    {
        #region public methods

        #region public static MvcHtmlString HaulierInbox(this HtmlHelper htmlHelper,IEnumerable<STP.Domain.Movements.MovementsList> objMovementlist)
        public static MvcHtmlString HaulierInbox(this HtmlHelper htmlHelper,IEnumerable<STP.Domain.MovementsAndNotifications.Movements.MovementsList> objMovementlist)
        {
            string html = string.Empty;
            html = SOMovementInbox(objMovementlist);
            return new MvcHtmlString(html);
        }
        #endregion public static MvcHtmlString HaulierInbox(this HtmlHelper htmlHelper,IEnumerable<STP.Domain.MovementsAndNotifications.Movements.MovementsList> objMovementlist)

        #region public static MvcHtmlString SoMovementHeader(this HtmlHelper htmlhelper, bool isListForSO)
        public static MvcHtmlString SoMovementHeader(this HtmlHelper htmlhelper, bool isListForSO)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            string html = string.Empty;
            htmlBuilder.Append("<thead><tr>");
            if (!isListForSO)
            {
                htmlBuilder.Append("<th class='headgrad'>"+Resources.Resource.Type+"</th>");
            }
            htmlBuilder.Append("<th class='headgrad' style='width:140px'>" + Resources.Resource.Status + "</th>");
            htmlBuilder.Append("<th class='headgrad'>" + Resources.Resource.ESDALRef + "</th>");
            htmlBuilder.Append("<th class='headgrad'>" + Resources.Resource.From + "</th>");
            htmlBuilder.Append("<th class='headgrad'>" + Resources.Resource.To + "</th>");
            if (!isListForSO)
            {
                htmlBuilder.Append("<th class='headgrad' style='width:100px'>" + Resources.Resource.DateFrom + "</th>");
                htmlBuilder.Append("<th class='headgrad' style='width:100px'>" + Resources.Resource.DateTo + "</th>");
            }
            htmlBuilder.Append("<th class='headgrad'>" + Resources.Resource.MyRef + "</th>");
            htmlBuilder.Append("</tr></thead>");

            html = htmlBuilder.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #endregion

        #region private methods
        private static string SOMovementInbox(IEnumerable<STP.Domain.MovementsAndNotifications.Movements.MovementsList> objMovementlist)
        {
            StringBuilder htmlString = new StringBuilder();
            string html = string.Empty;
            foreach (var item in objMovementlist)
            {

            }            
            html = htmlString.ToString();
            return html;
        }
        #endregion

    }
}