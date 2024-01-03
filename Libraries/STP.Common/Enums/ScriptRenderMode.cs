using System;

namespace STP.Common.Enums
{
    /// <summary>
    ///   Determines how scripts are included into the page
    /// </summary>
    [CLSCompliant(false)]
    public enum ScriptRenderMode
    {
        /// <summary>
        ///   Inherits the setting from the control or from the ClientScript.DefaultScriptRenderMode
        /// </summary>
        Inherit,
        /// <summary>
        ///   Renders the script include at the location of the control
        /// </summary>
        Inline,
        /// <summary>
        ///   Renders the script include into the bottom of the header of the page
        /// </summary>
        Header,
        /// <summary>
        ///   Renders the script include into the top of the header of the page
        /// </summary>
        HeaderTop,
        /// <summary>
        ///   Uses ClientScript or ScriptManager to embed the script include to
        ///   provide standard ASP.NET style rendering in the HTML body.
        /// </summary>
        Script,
        /// <summary>
        ///   Renders script at the bottom of the page before the last Page.Controls
        ///   literal control. Note this may result in unexpected behavior 
        ///   if /body and /html are not the last thing in the markup page.
        /// </summary>
        BottomOfPage
    }
}