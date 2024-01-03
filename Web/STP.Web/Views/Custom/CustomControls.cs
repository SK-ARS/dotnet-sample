using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Custom.Views
{
    public static class CustomControls
    {
        //#region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object showDefault, object htmlAttributes)
        ///// <summary>
        ///// datepicker with default date
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="name"></param>
        ///// <param name="showDefault"></param>
        ///// <param name="htmlAttributes"></param>
        ///// <returns></returns>
        //public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object showDefault, object htmlAttributes)
        //{
        //    StringBuilder htmlString = new StringBuilder();
        //    string datetime = string.Empty;
        //    string attributes = string.Empty;
        //    if (showDefault != null && (bool)showDefault)
        //    {
        //        datetime = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    }

        //    if (htmlAttributes != null)
        //    {
        //        attributes = htmlAttributes.ToString().Replace('{', ' ').Replace('}', ' ');
        //    }

        //    htmlString.Append("<br/><input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' value='" + datetime + "' " + attributes + "/>");
        //    string html = htmlString.ToString();
        //    return new MvcHtmlString(html);
        //}
        //#endregion public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object showDefault, object htmlAttributes)

        //#region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        ///// <summary>
        ///// datepicker without default date
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="name"></param>
        ///// <param name="htmlAttributes"></param>
        ///// <returns></returns>
        //public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        //{
        //    StringBuilder htmlString = new StringBuilder();
        //    string attributes = string.Empty;            

        //    if (htmlAttributes != null)
        //    {
        //        attributes = htmlAttributes.ToString().Replace('{', ' ').Replace('}', ' ');
        //    }

        //    htmlString.Append("<br/><input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' " + attributes + "/>");
        //    string html = htmlString.ToString();
        //    return new MvcHtmlString(html);
        //}
        //#endregion public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes)


        #region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name)
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            string html = string.Empty;
            string attributes = string.Empty;
            #endregion declaration
            if (type != null)
            {
                value = getdate != null ? Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            }
            htmlString.Append("<input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' value='" + value + "'>");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            StringBuilder htmlAttrStr = new StringBuilder();

            string html = string.Empty;
            string attributes = string.Empty;
            #endregion declaration
            var format = "dd/MM/yyyy";
            if (htmlAttributes != null)
            {
                var formatInput = htmlAttributes.GetType().GetProperties().Where(x=>x.Name=="format").FirstOrDefault();
                if(formatInput != null && formatInput.GetValue(htmlAttributes, null) != null && !string.IsNullOrWhiteSpace(formatInput.GetValue(htmlAttributes, null).ToString()))
                    format = formatInput.GetValue(htmlAttributes, null).ToString();
            }
            if (type != null)
            {
                value = getdate != null ? Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString(format, CultureInfo.InvariantCulture) : string.Empty;
            }
            if (htmlAttributes != null)
            {
                var aatrData = htmlAttributes.GetType().GetProperties();
                foreach (var item in aatrData)
                {
                    var attrName = item.Name;
                    var attrValue = htmlAttributes.GetType().GetProperty(attrName).GetValue(htmlAttributes, null);
                    if (attrValue != null)
                    {
                        var attrString = attrName.ToString() + "='" + attrValue.ToString() + "'";
                        htmlAttrStr.Append(attrString);
                    }
                }
                attributes = htmlAttrStr.ToString();
            }
            htmlString.Append("<input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' value='" + value + "' " + attributes + ">");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, string customDate)
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, string customDate)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();

            string html = string.Empty;
            #endregion declaration
            if (type != null)
            {
                value = getdate != null ? Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            }
            if (string.IsNullOrEmpty(value))
            {
                value = Convert.ToDateTime(customDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            htmlString.Append("<input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' value='" + value + "'>");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            StringBuilder htmlAttrStr = new StringBuilder();


            string html = string.Empty;

            string attributes = string.Empty;
            #endregion declaration
            if (type != null)
            {
                value = getdate != null ? Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            }
            if (string.IsNullOrEmpty(value))
            {
                value = Convert.ToDateTime(customDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (htmlAttributes != null)
            {
                var aatrData = htmlAttributes.GetType().GetProperties();
                foreach (var item in aatrData)
                {
                    var attrName = item.Name;
                    var attrValue = htmlAttributes.GetType().GetProperty(attrName).GetValue(htmlAttributes, null);
                    if (attrValue != null)
                    {
                        var attrString = attrName.ToString() + "='" + attrValue.ToString() + "'";
                        htmlAttrStr.Append(attrString);
                    }
                }
                attributes = htmlAttrStr.ToString();
            }
            htmlString.Append("<input type='text' control-type='datepicker' id='" + name + "' name='" + name + "' value='" + value + "' " + attributes + ">");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region public static MvcHtmlString DatePickerMin(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        public static MvcHtmlString DatePickerMin(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            StringBuilder htmlAttrStr = new StringBuilder();


            string html = string.Empty;

            string attributes = string.Empty;
            #endregion declaration
            if (type != null)
            {

                value = getdate != null ? objProp.GetValue(obj, null).ToString() : string.Empty;
                //DateTime.ParseExact(, "dd/MM/yyyy", null).ToString; Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
            }
            if (string.IsNullOrEmpty(value))
            {

                value = Convert.ToDateTime(customDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            else
            {
                value = Convert.ToDateTime(value).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            if (htmlAttributes != null)
            {
                var aatrData = htmlAttributes.GetType().GetProperties();
                foreach (var item in aatrData)
                {
                    var attrName = item.Name;
                    var attrValue = htmlAttributes.GetType().GetProperty(attrName).GetValue(htmlAttributes, null);
                    if (attrValue != null)
                    {
                        var attrString = attrName.ToString() + "='" + attrValue.ToString() + "'";
                        htmlAttrStr.Append(attrString);
                    }
                }
                attributes = htmlAttrStr.ToString();
            }
            htmlString.Append("<input type='text' control-type='datepicker_min' id='" + name + "' name='" + name + "' value='" + value + "' " + attributes + ">");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region DateTimePicker
        public static MvcHtmlString DateTimePicker(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            StringBuilder htmlAttrStr = new StringBuilder();


            string html = string.Empty;

            string attributes = string.Empty;
            #endregion declaration
            if (type != null)
            {
                value = getdate != null ? Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture) : string.Empty;
            }
            if (string.IsNullOrEmpty(value))
            {
                value = Convert.ToDateTime(customDate).ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            }
            if (htmlAttributes != null)
            {
                var aatrData = htmlAttributes.GetType().GetProperties();
                foreach (var item in aatrData)
                {
                    var attrName = item.Name;
                    var attrValue = htmlAttributes.GetType().GetProperty(attrName).GetValue(htmlAttributes, null);
                    if (attrValue != null)
                    {
                        var attrString = attrName.ToString() + "='" + attrValue.ToString() + "'";
                        htmlAttrStr.Append(attrString);
                    }
                }
                attributes = htmlAttrStr.ToString();
            }
            htmlString.Append("<input type='text' control-type='datetimepicker' id='" + name + "' name='" + name + "' value='" + value + "' " + attributes + ">");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region DateTimePickerMin
        public static MvcHtmlString DateTimePickerMin(this HtmlHelper htmlHelper, string name, string customDate, object htmlAttributes)
        {
            #region declaration
            object obj = htmlHelper.ViewData.Model;
            Type type = htmlHelper.ViewData.Model.GetType();
            PropertyInfo objProp = type.GetProperty(name);

            var getdate = objProp != null ? objProp.GetValue(obj, null) : null;

            string value = string.Empty;
            StringBuilder htmlString = new StringBuilder();
            StringBuilder htmlAttrStr = new StringBuilder();


            string html = string.Empty;

            string attributes = string.Empty;
            #endregion declaration
            if (type != null)
            {

                value = getdate != null ? objProp.GetValue(obj, null).ToString() : string.Empty;
                //DateTime.ParseExact(, "dd/MM/yyyy", null).ToString; Convert.ToDateTime(objProp.GetValue(obj, null).ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;

            }
            if (string.IsNullOrEmpty(value))
            {

                value = Convert.ToDateTime(customDate).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            }
            if (htmlAttributes != null)
            {
                var aatrData = htmlAttributes.GetType().GetProperties();
                foreach (var item in aatrData)
                {
                    var attrName = item.Name;
                    var attrValue = htmlAttributes.GetType().GetProperty(attrName).GetValue(htmlAttributes, null);
                    if (attrValue != null)
                    {
                        var attrString = attrName.ToString() + "='" + attrValue.ToString() + "'";
                        htmlAttrStr.Append(attrString);
                    }
                }
                attributes = htmlAttrStr.ToString();
            }
            htmlString.Append("<input type='text' control-type='datetimepicker_min' id='" + name + "' name='" + name + "' value='" + value + "' " + attributes + ">");
            html = htmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion

        #region public static MvcHtmlString SOTabs(this HtmlHelper htmlHelper, int applStatus)
        public static MvcHtmlString SOTabs(this HtmlHelper htmlHelper, int applStatus, int versionStatus, bool IsVR1Applciation = false, bool reduceddetailed = false, int IsDistributed = 0)
        {
            StringBuilder objhtmlString = new StringBuilder();
            string html = string.Empty;

            string routeType = "Proposed";
            if (versionStatus == 305004 || versionStatus == 305005 || versionStatus == 305006)
            {
                routeType = "Agreed";
            }
            if (IsVR1Applciation && applStatus == 308003 && versionStatus == 305013)
            {
                versionStatus = 0;
            }
            //if WIP appl
            if (applStatus == 308001)
            {
                //general tab (edit)
                objhtmlString.Append("<div id='1' class='column t'>");
                objhtmlString.Append("<div class='card  active-card btn-edit_generaldetails' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>General</span>");
                objhtmlString.Append("</div></div>");

                if (IsVR1Applciation)
                {
                    if (!reduceddetailed)
                    {
                        // vr1 vehicle tab (edit)

                        objhtmlString.Append("<div id='3' class='column t'>");
                        objhtmlString.Append("<div class='card btn-listvr1veh' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Vehicle</span>");
                        objhtmlString.Append("</div></div>");
                    }
                }
                else
                {
                    // so vehicle tab (edit)
                    objhtmlString.Append("<div id='3' class='column t'>");
                    objhtmlString.Append("<div class='card btn-listsoveh' >");
                    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Vehicle</span>");
                    objhtmlString.Append("</div></div>");
                }

                //route tab (edit)

                objhtmlString.Append("<div id='4' class='column t'>");
                objhtmlString.Append("<div class='card btn-cloneroutes' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route</span>");
                objhtmlString.Append("</div></div>");

                if (IsVR1Applciation == true)
                {
                    if (!reduceddetailed)
                    {
                        //route assessment tab (edit)
                        objhtmlString.Append("<div id='5' class='column t'>");
                        objhtmlString.Append("<div class='card btn-applrouteassessment' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route Assessment</span>");
                        objhtmlString.Append("</div></div>");
                    }
                    //supplimentery info (edit)
                    objhtmlString.Append("<div id='6' class='column t'>");
                    objhtmlString.Append("<div class='card btn-supplementaryinfo' >");
                    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Supplementary Information</span>");
                    objhtmlString.Append("</div></div>");
                }
            }
            else if (applStatus == 308002 || applStatus == 308004 || (applStatus == 308003 && versionStatus == 0))
            {
                //if submitted appl

                //general tab (display)
                objhtmlString.Append("<div id='1' class='column t'>");
                objhtmlString.Append("<div class='card active-card btn-loadgeneraldetails' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>General</span>");
                objhtmlString.Append("</div></div>");

                objhtmlString.Append("<div id='2' class='column t'>");
                objhtmlString.Append("<div class='card btn-remove_routeanalysisaddroutepart' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Haulier Application</span>");
                objhtmlString.Append("</div></div>");
                //commenting below code to maintain both SO and VR1 application will show same tabs fixed as a part of HE 4104
                //if (IsVR1Applciation)
                //{
                //    if (!reduceddetailed)
                //    {
                //        //vehcile tab (display)
                //        objhtmlString.Append("<div id='3' class='column t'>");
                //        objhtmlString.Append("<div class='card' onclick='load_applVehicle()'>");
                //        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Vehicle</span>");
                //        objhtmlString.Append("</div></div>");
                //    }

                //    //Agreed Route tab (display)
                //    objhtmlString.Append("<div id='4' class='column t'>");
                //    objhtmlString.Append("<div class='card' onclick='AgreedRoute_AddRoutePart()'>");
                //    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route</span>");
                //    objhtmlString.Append("</div></div>");

                //    if (IsVR1Applciation && !reduceddetailed)
                //    {

                //        //Route Assessment tab (display)
                //        objhtmlString.Append("<div id='5' class='column t'>");
                //        objhtmlString.Append("<div class='card' onclick='load_routeAnalysis()'>");
                //        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route Assessment</span>");
                //        objhtmlString.Append("</div></div>");
                //    }
                //}
                //else
                //{
                //    //haulier application tab (display)
                //    objhtmlString.Append("<div id='2' class='column t'>");
                //    objhtmlString.Append("<div class='card' onclick='remove_routeanalysisAddRoutePart()'>");
                //    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Haulier Application</span>");
                //    objhtmlString.Append("</div></div>");
                //}
            }
            else
            {
                //general tab (display)

                objhtmlString.Append("<div id='1' class='column t'>");
                objhtmlString.Append("<div class='card active-card btn-loadgeneraldetails' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>General</span>");
                objhtmlString.Append("</div></div>");

                //haulier application tab (display)

                objhtmlString.Append("<div id='2' class='column t'>");
                objhtmlString.Append("<div class='card btn-remove_routeanalysisaddroutepart' >");
                objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Haulier Application</span>");
                objhtmlString.Append("</div></div>");

                if (!IsVR1Applciation)
                {
                    //vehcile tab (display)

                    objhtmlString.Append("<div id='3' class='column t'>");
                    objhtmlString.Append("<div class='card btn-load_applvehicle'  style='width: 14rem;'>");
                    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>" + routeType + " Vehicle And Route</span>");
                    objhtmlString.Append("</div></div>");

                    //Agreed Route tab (display)
                    //objhtmlString.Append("<li id='4' class='t' onclick='AgreedRoute_AddRoutePart()'>");
                    //objhtmlString.Append("<div class='tab'>");
                    //objhtmlString.Append("<span class='tab_left'></span><span class='tab_centre'>" + routeType + "  route</span><span class='tab_right'></span>");
                    //objhtmlString.Append("</div></li>");
                }

                if (IsVR1Applciation)
                {
                    if (!reduceddetailed)
                    {
                        //Route Assessment tab (display)
                        objhtmlString.Append("<div id='5' class='column t'>");
                        objhtmlString.Append("<div class='card btn-load_routeanalysis' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route Assessment</span>");
                        objhtmlString.Append("</div></div>");
                    }
                }
                else
                {
                    //Route Assessment tab (display)
                    objhtmlString.Append("<div id='5' class='column t'>");
                    objhtmlString.Append("<div class='card btn-load_routeanalysis' >");
                    objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route Assessment</span>");
                    objhtmlString.Append("</div></div>");
                }

                if (!IsVR1Applciation)
                {
                    if (IsDistributed == 0)
                    {
                        //Agreed Route tab  (display)
                        objhtmlString.Append("<div id='6' class='column t'>");
                        objhtmlString.Append("<div class='card btn-load_affectedparties' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Affected Parties</span>");
                        objhtmlString.Append("</div></div>");
                    }
                    else if (IsDistributed == 1)
                    {
                        //contacted parties tab (display)
                        objhtmlString.Append("<div id='8' class='column t'>");
                        objhtmlString.Append("<div class='card btn-showcontactedparties' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Contacted Parties</span>");
                        objhtmlString.Append("</div></div>");
                    }

                    if (versionStatus != 305002)
                    {
                        //Notification History tab (display)
                        objhtmlString.Append("<div id='7' class='column t'>");
                        objhtmlString.Append("<div class='card btn-showvehicleconfigdetail' >");
                        objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Notification History</span>");
                        objhtmlString.Append("</div></div>");
                    }
                }
            }


            //---------------------------------------------------------------------------



            html = objhtmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion public static MvcHtmlString SOTabs(this HtmlHelper htmlHelper, int applStatus)

        #region NENotification
        public static MvcHtmlString NEN_NotificationTabs(this HtmlHelper htmlHelper, int NotifStatus, bool ShowHaulApp)
        {
            StringBuilder objhtmlString = new StringBuilder();
            string html = string.Empty;

            //general tab

            objhtmlString.Append("<div id='1' class='column t'>");
            objhtmlString.Append("<div id='generalTab' class='card  active-card btn-showgeneraldetails' >");
            objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>General</span>");
            objhtmlString.Append("</div></div>");

            //vehicle tab
            //objhtmlString.Append("<li id='2' class='t' onclick='ShowVehicleTab()' >");
            //objhtmlString.Append("<div class='tab'>");
            //objhtmlString.Append("<span class='tab_left'></span><span class='tab_centre'>Vehicle</span><span class='tab_right'></span>");
            //objhtmlString.Append("</div></li>");

            //route tab
            objhtmlString.Append("<div id='3' class='column t'>");
            objhtmlString.Append("<div id='routeTab' class='card btn-plannnenotification' >");
            objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'>Route</span>");
            objhtmlString.Append("</div></div>");

            //Haulier Description 
            objhtmlString.Append("<div id='5' class='column haulimage'>");
            objhtmlString.Append("<div id='haulTab' class='card no-active-class-required btn-view_hualierdetails' >");
            objhtmlString.Append("<span class='text-normal-hyperlink tab_centre'><img src=../Content/assets/images/warning.svg id='scoll-right-btn' width='20' title='Haulier Route Description'> </span>");
            objhtmlString.Append("</div></div>");

            html = objhtmlString.ToString();
            return new MvcHtmlString(html);
        }
        #endregion
        #region NotificationTabs
        public static MvcHtmlString NotificationTabs(this HtmlHelper htmlHelper)
        {
            StringBuilder objhtmlString = new StringBuilder();
            string html = string.Empty;

            objhtmlString.Append("<div id='tab1' class='column btn-display-general-details' >");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>General</span>");
            objhtmlString.Append("</div></div>");

            //route- vehicle tab
            objhtmlString.Append("<div id='tab2' class='column btn-display-route-vehicle' >");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Route and Vehicle</span>");
            objhtmlString.Append("</div></div>");

            //route assessment tab
            objhtmlString.Append("<div id='tab3' class='column btn-display-route-assessment'>");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Route Assessment</span>");
            objhtmlString.Append("</div></div>");

            //notified parties tab
            objhtmlString.Append("<div id='tab7' class='column btn-display-parties'>");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Notified parties</span>");
            objhtmlString.Append("</div></div>");

            //transmission status tab
            objhtmlString.Append("<div id='tab4' class='column btn-diplay-transmission-status'>");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Transmission Status</span>");
            objhtmlString.Append("</div></div>");

            //collaboration tab
            objhtmlString.Append("<div id='tab5' class='column btn-display-collaboration'>");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Collaboration</span>");
            objhtmlString.Append("</div></div>");

            //notification history tab
            objhtmlString.Append("<div id='tab6' class='column btn-display-notif-history'>");
            objhtmlString.Append("<div class='card'>");
            objhtmlString.Append("<span class='text-normal-hyperlink'>Notification History</span>");
            objhtmlString.Append("</div></div>");

            html = objhtmlString.ToString();
            return new MvcHtmlString(html);
        }

        #endregion

        #region SORTTabs
        public static MvcHtmlString SORTTabs(this HtmlHelper htmlHelper, string SORTStatus, bool IsVR1Applciation = false, bool reduceddetailed = false, bool AppEdit = false, int EnterBySort = 0, bool isSubmit = true, int VersionStatus = 0, bool isCandidateVersion = false)
        {
            StringBuilder objStrBuilder = new StringBuilder();
            string html = string.Empty;

            if (SORTStatus == "Revisions")
            {
                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                //General Tab
                objStrBuilder.Append("<div id='2' class='column t'>");
                objStrBuilder.Append("<div class='card active-card btn-display-general' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Application Details</span>");
                objStrBuilder.Append("</div></div>");

                //General Tab
                if (IsVR1Applciation != true)
                {
                    //objStrBuilder.Append("<li id='3' class='t' onclick='DisplayVehroute()'>");
                    //objStrBuilder.Append("<div class='tab'>");
                    //objStrBuilder.Append("<span class='tab_left'></span><span class='tab_centre'>Route and Vehicles </span><span class='tab_right'></span>");
                    //objStrBuilder.Append("</div></li>");

                    //objStrBuilder.Append("<div id='3' class='column t'>");
                    //objStrBuilder.Append("<div class='card' onclick='DisplayVehroute()'>");
                    //objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route and Vehicles</span>");
                    //objStrBuilder.Append("</div></div>");
                }
                else if (EnterBySort == 0 && IsVR1Applciation != false)
                {
                    //if (reduceddetailed == true)
                    //{

                    ////Vehicle Tab
                    //objStrBuilder.Append("<div id='3' class='column t'>");
                    //objStrBuilder.Append("<div class='card' onclick='ShowSORTVR1Vehicle()'>");
                    //objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Vehicle</span>");
                    //objStrBuilder.Append("</div></div>");
                    //}

                    ////Route Tab
                    //objStrBuilder.Append("<div id='4' class='column t'>");
                    //objStrBuilder.Append("<div class='card' onclick='ShowSORTVR1Route()'>");
                    //objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route</span>");
                    //objStrBuilder.Append("</div></div>");
                }

                objStrBuilder.Append("<div id='9' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-history' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >History</span>");
                objStrBuilder.Append("</div></div>");
            }
            else if (SORTStatus == "ViewProj")
            {
                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card active-card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                objStrBuilder.Append("<div id='9' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-history' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>History</span>");
                objStrBuilder.Append("</div></div>");

                //objStrBuilder.Append("<div class='column'>");
                //objStrBuilder.Append("<div class='card'>");
                //objStrBuilder.Append("<span class='text-normal-hyperlink' onclick='toHaulier()'>Notes to Haulier</span>");
                //objStrBuilder.Append("</div></div>");
            }
            else if (SORTStatus == "MoveVer")
            {
                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                //Movement summary Tab
                objStrBuilder.Append("<div id='2' class='column t'>");
                objStrBuilder.Append("<div id='2MoveVer' class='card active-card btn-display-appl-summary' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Movement Summary</span>");
                objStrBuilder.Append("</div></div>");

                if (IsVR1Applciation != true)
                {
                    //General Tab
                    objStrBuilder.Append("<div id='3' class='column t'>");
                    objStrBuilder.Append("<div id='MoveVerVehicleandRoutes' class='card btn-display-vehroute' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Route and Vehicles</span>");
                    objStrBuilder.Append("</div></div>");

                    //Route assessment Tab
                    objStrBuilder.Append("<div id='5' class='column t'>");
                    objStrBuilder.Append("<div id='5MoveVerRoute' class='card btn-list-route-assessment' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Route Assessment</span>");
                    objStrBuilder.Append("</div></div>");
                }
                else if (IsVR1Applciation == true)
                {
                    //if (EnterBySort == 0)
                    // {
                    objStrBuilder.Append("<div id='3' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-show-sortvr1-vehicle' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Vehicle</span>");
                    objStrBuilder.Append("</div></div>");

                    //Route Tab
                    objStrBuilder.Append("<div id='4' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-show-sortvr1-route' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Route</span>");
                    objStrBuilder.Append("</div></div>");

                    //Route assessment Tab
                    objStrBuilder.Append("<div id='5' class='column t'>");
                    objStrBuilder.Append("<div id='5MoveVerRoute' class='card btn-load_routeAnalysisSORT' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route Assessment</span>");
                    objStrBuilder.Append("</div></div>");
                    // }
                }


                if (!IsVR1Applciation)
                {
                    //Collaboration status Tab
                    objStrBuilder.Append("<div id='6' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-display-collab-status' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Collaboration Status</span>");
                    objStrBuilder.Append("</div></div>");

                    //Transmission status Tab
                    objStrBuilder.Append("<div id='7' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-display-trans-status' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Transmission Status</span>");
                    objStrBuilder.Append("</div></div>");

                    //Note to Haulier Tab
                    /*if (VersionStatus != 305001 )
                    {
                        if (VersionStatus != 305011)
                        {
                            objStrBuilder.Append("<li id='8' class='t' onclick='toHaulier()'>");
                            objStrBuilder.Append("<div class='tab'>");
                            objStrBuilder.Append("<span class='tab_left'></span><span class='tab_centre'>Notes to Haulier</span><span class='tab_right'></span>");
                            objStrBuilder.Append("</div></li>");
                        }
                    }*/

                    objStrBuilder.Append("<div id='8' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-to-haulier' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Notes to Haulier</span>");
                    objStrBuilder.Append("</div></div>");
                }
                //History Tab
                objStrBuilder.Append("<div id='9' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-history' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>History</span>");
                objStrBuilder.Append("</div></div>");

            }
            else if (SORTStatus == "RouteVer")
            {
                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                //Candidate route review Tab
                objStrBuilder.Append("<div id='2' class='column t'>");
                objStrBuilder.Append("<div class='card'>");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Candidate Route Review</span>");
                objStrBuilder.Append("</div></div>");

                //Vehicle Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-show-so-vehicle-page' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Vehicle</span>");
                objStrBuilder.Append("</div></div>");

                //Route Tab
                objStrBuilder.Append("<div id='4' class='column t'>");
                objStrBuilder.Append("<div class='card btn-showsoroutepage' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route</span>");
                objStrBuilder.Append("</div></div>");

                //Route assessment Tab
                objStrBuilder.Append("<div id='5' class='column t'>");
                objStrBuilder.Append("<div class='card btn-showsoroutepage' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route Assessment</span>");
                objStrBuilder.Append("</div></div>");

                //Checking differences Tab
                objStrBuilder.Append("<div id='6' class='column t'>");
                objStrBuilder.Append("<div class='card btn-showcheckdiff' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Checking Differences</span>");
                objStrBuilder.Append("</div></div>");
            }
            else if (SORTStatus == "SpecOrder")
            {
                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                //Special order Tab
                objStrBuilder.Append("<div id='2' class='column t'>");
                objStrBuilder.Append("<div class='card btn-displayspecorder' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Special Order</span>");
                objStrBuilder.Append("</div></div>");
            }
            else if (SORTStatus == "CandidateRT")
            {
                isSubmit = true;

                //Project overview Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card btn-display-proj-view' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Project Overview</span>");
                objStrBuilder.Append("</div></div>");

                //General Tab
                objStrBuilder.Append("<div id='3' class='column t'>");
                objStrBuilder.Append("<div id='3general' class='card active-card btn-viewprojectgeneraldetails' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>General</span>");
                objStrBuilder.Append("</div></div>");

                //Candidate Route Tab
                objStrBuilder.Append("<div id='4' class='column t'>");
                objStrBuilder.Append("<div class='card btn-bindrouteparts' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route</span>");
                objStrBuilder.Append("</div></div>");

                //Candidate Vehicle Tab
                objStrBuilder.Append("<div id='2' class='column t'>");
                objStrBuilder.Append("<div class='card btn-show_candidatertvehicles' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Vehicles</span>");
                objStrBuilder.Append("</div></div>");
                //Route Analysis
                if (isCandidateVersion)
                {
                    objStrBuilder.Append("<div id='cand_rtanalysis' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-load_routeanalysissort' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route Assessment</span>");
                    objStrBuilder.Append("</div></div>");
                }
                //Related movements to structure
                //objStrBuilder.Append("<li id='StructRelatedMovements' class='t' onclick='showStructRelatedMovements()' style='display:none'>");
                //objStrBuilder.Append("<li id='9' class='t' onclick='showStructRelatedMovements()' style='display:none'>");
                //objStrBuilder.Append("<div class='tab'>");
                //objStrBuilder.Append("<span class='tab_left'></span><span class='tab_centre'>Related movements</span><span class='tab_right'></span>");
                //objStrBuilder.Append("</div></li>");

                //Checking difference .
                //objStrBuilder.Append("<li id='5' class='t' onclick='Cand_Cdefference_Click()'>");
                //objStrBuilder.Append("<div class='tab'>");
                //objStrBuilder.Append("<span class='tab_left'></span><span class='tab_centre'>Checking differences</span><span class='tab_right'></span>");
                //objStrBuilder.Append("</div></li>");

                objStrBuilder.Append("<div id='9' class='column t'>");
                objStrBuilder.Append("<div class='card btn-displayhistory' >");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >History</span>");
                objStrBuilder.Append("</div></div>");

            }
            else
            {
                if (AppEdit)
                {
                    //Project overview Tab
                    objStrBuilder.Append("<div id='0' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-displayprojview' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Project Overview</span>");
                    objStrBuilder.Append("</div></div>");
                }

                //Haulier Organisation Tab
                objStrBuilder.Append("<div id='1' class='column t'>");
                objStrBuilder.Append("<div class='card'>");
                objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Haulier Organisation</span>");
                objStrBuilder.Append("</div></div>");

                if (SORTStatus == "CreateSO")
                {
                    //General Tab
                    objStrBuilder.Append("<div id='2' class='column t'>");
                    objStrBuilder.Append("<div class='card'>");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >General</span>");
                    objStrBuilder.Append("</div></div>");

                    //Vehicle Tab
                    objStrBuilder.Append("<div id='3' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-sortshowsovehiclepage' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Vehicle</span>");
                    objStrBuilder.Append("</div></div>");

                    //Route Tab
                    objStrBuilder.Append("<div id='4' class='column t'>");
                    objStrBuilder.Append("<div class='card btn-sortshowsoroutepage' >");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Route</span>");
                    objStrBuilder.Append("</div></div>");
                }
                else if (SORTStatus == "CreateVR1")
                {
                    //General Tab
                    objStrBuilder.Append("<div id='4' class='column t'>");
                    objStrBuilder.Append("<div class='card'>");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >General</span>");
                    objStrBuilder.Append("</div></div>");

                    //Supplimentory Information Tab
                    objStrBuilder.Append("<div id='3' class='column t'>");
                    objStrBuilder.Append("<div class='card'>");
                    objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre' >Supplementary Information</span>");
                    objStrBuilder.Append("</div></div>");
                }
            }

            objStrBuilder.Append("<div id='10' class='column' style='display:none' >");
            objStrBuilder.Append("<div class='card btn-showstructrelatedmovements' >");
            objStrBuilder.Append("<span class='text-normal-hyperlink tab_centre'>Related Movements</span>");
            objStrBuilder.Append("</div></div>");

            html = objStrBuilder.ToString();
            return new MvcHtmlString(html);
        }
        #endregion
    }
}