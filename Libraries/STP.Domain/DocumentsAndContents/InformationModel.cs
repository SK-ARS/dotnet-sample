using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace STP.Domain.DocumentsAndContents
{
    public class InformationModel : IValidatableObject
    {

        
        [Display(Name = "Content Id")]
        public double ContentId { get; set; }

        [Display(Name = "Content Type")]
        public int ContentType { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        [StringLength(400, ErrorMessage = "Maximum 400 characters")]
        public string Name { get; set; } 

        [Display(Name = "Priority")]
        public double Priority { get; set; }

        [Display(Name = "Priority List")]
        public List<SelectListItem> PriorityList { get; set; }

        [Display(Name = "Suppressed")]
        public short Suppressed { get; set; } 

        [Display(Name = "Suppressed Name")]
        public string SuppressedName { get; set; } 

        [Display(Name = "Suppressed Name List")]
        public List<SelectListItem> SuppressedList { get; set; }

        [Display(Name = "Retracted")]
        public short Retracted { get; set; }

        [Display(Name = "Current Version Id")]
        public double CurrentVerssionID { get; set; }

        [Display(Name = "Next Version Id")]
        public double NextVersionId { get; set; }

        [Display(Name = "Download Type")]
        public Int32 DownloadType { get; set; }

        [Display(Name = "Download Type Name")]
        public string DownloadTpyeName { get; set; }

        [Display(Name = "Download Type List")]
        public List<SelectListItem> DownloadTypeList { get; set; }

        
        [Display(Name = "File Id")]
        public double FileId { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "File Content")]
        public byte[] FileContent { get; set; }

        [Display(Name = "Name Type")]
        public string MimeType { get; set; }

        
        [Display(Name = "Version Id")]
        public double VersionId { get; set; }

        [Display(Name = "Headline")]
        [Required(ErrorMessage = "Headline is required")]
        [StringLength(400, ErrorMessage = "Maximum 400 characters")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; }

        [Display(Name = "Next File Id")]
        public double NextFileId { get; set; } 

        [Display(Name = "Link URL")]
        [StringLength(255)]
        //[RegularExpression(@"(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?", ErrorMessage = "Please enter a valid URL")]
        public string LinkUrl { get; set; }

        
        [Display(Name = "Portal Type")]
        public double PortalType { get; set; }

        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

       
        public Int32 Code { get; set; }
        public short Type { get; set; }
        public string EnumValuesName { get; set; }

        public byte[] FileContentDb { get; set; } 

        [Display(Name = "Content Type Name")]
        public string ContentTypeName { get; set; }

        [Display(Name = "Content Type Name")]
        public string ContentTypeNameEnum { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        /// <summary>
        /// Combine key value for PRIORITY field
        /// </summary>
        /// 
        public string PriorityName { get; set; }
        /// <summary>
        /// Below properties use for search criteria
        /// </summary>
        public string SearchType { get; set; } 
        public string SearchName { get; set; }

        /// <summary>
        /// Used to store total record count
        /// </summary>
        [Display(Name = "Total Record Count")]
        public decimal TotalRecordCount { get; set; }

        /// <summary>
        /// User name. ex.hauliers portal, cm admin portal
        /// </summary>
        //[Required(ErrorMessage = "Portal is required")]
        [Display(Name = "Portal Name")]
        [DataType(DataType.Text)]
        public string PortalName { get; set; }

        /// <summary>
        /// ,seperate Id of Portal name. ex.hauliers portal, cm admin portal, 
        /// </summary>
        [Display(Name = "Portal Id")]
        public string PortalId { get; set; }


        /// <summary>
        /// Below Property used for CheckListBox 
        /// </summary>
        [Required(ErrorMessage = "Portal is required")]
        public List<CheckBoxList> PortalList { get; set; }

        [Display(Name = "Video file name")]
        public string VideoFileName { get; set; }

        [Display(Name = "Video file size")]
        public double VideoFileSize { get; set; }

        public string MSWordOption { get; set; }

        public string PDFOption { get; set; }

        public string VideoOption { get; set; }
        public List<WebContentFile> AssociatedFiles { get; set; }
        /// <summary>
        /// Validatation message.
        /// </summary>
        /// <param name="Validate"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (PortalList.Count > 0)
            {
                var selectedPortal = PortalList.Find(p => p.IsSelected);
                if (selectedPortal == null)
                {
                    var property = new[] { "PortalList" };
                    yield return new ValidationResult("Please select portal.", property);
                }
            }
            else
            {
                var selectedPortal = PortalList.Find(p => p.IsSelected);
                if (selectedPortal == null)
                {
                    var property = new[] { "PortalList" };
                    yield return new ValidationResult("Please insert portal.", property);
                }
            }

        }

        /// <summary>
        /// Hot news content id
        /// </summary>
        [Display(Name = "Hot news content id")]
        public double HotNewsContentId { get; set; }

        /// <summary>
        /// Hot news name
        [Display(Name = "Hot news name")]
        /// </summary>
        public string HotNewsName { get; set; }

        public decimal FileCount { get; set; }
    }

    public class NewsNotificationModel
    {
        public double NewsId { get; set; }
        public bool IsRead { get; set; }
        public DateTime UploadedDateTime { get; set; }
    }
    /// <summary>
    /// property for checkbox list
    /// </summary>
    public class CheckBoxList
    {
        public int CheckBoxId { get; set; }
        public string CheckBoxName { get; set; }
        public bool IsSelected { get; set; }
    }

    /// <summary>
    /// Used during filtering of the model
    /// </summary>
    public class InformationModelFilter
    {
        public string SearchColumn { get; set; }  
        public string SearchValue { get; set; }  
    }
    /// <summary>
    /// Property for Web content file table.
    /// </summary>
    public class WebContentFile
    {
        /// <summary>
        /// Content Id FK of WEB_CONTENT
        /// </summary>
        public double ContentID { get; set; }
        /// <summary>
        /// Version Id of File uploaded
        /// </summary>
        public double VersionID { get; set; }
        /// <summary>
        /// Custome generated file id
        /// </summary>
        public double FileID { get; set; }
        /// <summary>
        /// File name of uploaded file
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File body of uploaded file.
        /// </summary>
        public byte[] FileContentUpload { get; set; }
        /// <summary>
        /// File type of uploaded file.
        /// </summary>
        public string MimeTypeUpload { get; set; }
    }

    public class LatestNews
    {
        public double ContentId { get; set; }
        public string Title { get; set; }
        public DateTime UploadedDateTime { get; set; }
    }

}