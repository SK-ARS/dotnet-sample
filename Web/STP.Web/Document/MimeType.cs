using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace STP.Web.Document
{
    public class MimeType
    {
        public bool GetMimeType(IEnumerable<HttpPostedFileBase> associatedFileUpload)
        {
            bool valid = false;
            int count = associatedFileUpload.Count();

            for (int i = 0; i < count; i++)
            {
                if (associatedFileUpload.ElementAt(i) != null)
                {
                    string fileName = associatedFileUpload.ElementAt(i).FileName;
                    string ext = fileName.Contains(".") ? Path.GetExtension(fileName).ToLower() : "." + fileName;
                    string contentType = associatedFileUpload.ElementAt(i).ContentType;

                    if (contentType == "application/msword" || contentType == "application/pdf" || contentType == "video/mpeg" || ext == ".html")
                        valid = true;
                }
                else
                {
                    valid = true;
                }
            }

            return valid;
        }

        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        public byte ValidateFile(HttpPostedFileBase associatedFileUpload, List<string> contentTypeList, List<string> extensionList)
        {
            byte valid = 0;
            string fileName = associatedFileUpload.FileName;
            string ext = Path.GetExtension(fileName).ToLower();
            string contentType = associatedFileUpload.ContentType.ToLower();

            if (!contentTypeList.Any(x => x.ToLower() == contentType))
                valid = 1;

            if (!extensionList.Any(x => x.ToLower() == ext))
                valid = 2;

            var result = Regex.Match(fileName, @"\.(?=.*\.)").Success;
            if (result)
                valid = 3;

            return valid;
        }
    }
}


