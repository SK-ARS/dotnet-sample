using System;
using System.ComponentModel;

namespace STP.Common.Enums
{
    /// <summary>
    ///   Identifies constant rules for searching purpose in marketting section
    ///   <para>Announcment =9996</para>
    ///   <para>ContactUs = 9997</para>
    ///   <para>BannerImages = 9998</para>
    ///   <para>DocumentManagement = 9999</para>
    /// </summary>
    [CLSCompliant(false)]
    public enum DocumentType
    {
        [Description("Document")] Document = 1,
        [Description("Application")] Application = 2,
        [Description("Term Sheet")] TermSheet = 3,
    }
}