using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using STP.Web.General;
using STP.Web.Helpers;
using STP.Common.Logger;
using STP.Domain.SecurityAndUsers;

public class URIHandler : IHttpModule
{
    #region IHttpModule members
    public void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
        context.BeginRequest += new EventHandler(context_BeginRequest);
    }
    #endregion

    private const string PARAMETER_NAME = "vR5bYkhK1Vg3ivnTgSYBA!";
    private const string JSAppParam = "AImq7w2TM6YIugpo6idvg=";
    private const string CSAppParam = "B7vy6imTleYsMr6Nlv7VQ=";

    private void context_BeginRequest(object sender, EventArgs e)
    {
        HttpContext context = HttpContext.Current;
        string query = ExtractQuery(context.Request.RawUrl);

        try
        {
            if (context.Request.RawUrl.Contains("?" + JSAppParam))
            {
                string path = GetVirtualPath();
                string rawQuery = query.Replace(JSAppParam, string.Empty);
                var decryptedQuery = AES.DecryptStringAES(rawQuery);

                string encryptedQuery = EncryptionUtility.Encrypt(decryptedQuery);
                context.Response.Redirect(path + encryptedQuery);
            }
            else if (context.Request.RawUrl.Contains("?" + CSAppParam))
            {
                string path = GetVirtualPath();
                string rawQuery = context.Server.UrlDecode(query.Replace(CSAppParam, string.Empty));
                if (path == null)
                {
                    Uri uriResult;
                    bool result = Uri.TryCreate(rawQuery, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                    if (result)
                    {
                        context.Response.Redirect("/");
                    }
                }
                context.Response.Redirect(path + rawQuery);
            }
            else if (context.Request.RawUrl.Contains("?returnUrl"))
            {
                string url = context.Request.RawUrl.Substring(context.Request.RawUrl.IndexOf('=') + 1);

                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                {
                    context.Response.Redirect("/");
                }
            }
            else if (context.Request.RawUrl.Contains("?"))
            {
                string path = GetVirtualPath();
                if (query.StartsWith(PARAMETER_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    // Decrypts the query string and rewrites the path.
                    string rawQuery = query.Replace(PARAMETER_NAME, string.Empty);
                    string decryptedQuery = EncryptionUtility.Decrypt(rawQuery);
                    path = path == null ? "Users/Login" : path;
                    if(path== "Users/Login")
                    {
                        Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("URL Redirect exception line 88: {0}/{1}", path, decryptedQuery));
                    }
                    context.RewritePath(path, string.Empty, decryptedQuery);
                }
                else if (context.Request.HttpMethod == "GET")
                {
                    // Encrypt the query string and redirects to the encrypted URL.
                    // Remove if you don't want all query strings to be encrypted automatically.
                    string encryptedQuery = EncryptionUtility.Encrypt(query);
                    context.Response.Redirect(path + encryptedQuery);
                }
            }
            else if (context.Request.RawUrl.Contains("undefined"))
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("URL Redirect exception line 102: {0}", context.Request.RawUrl));
                context.Response.Redirect("/Error/UnauthorizedAccess");
            }
        }
        catch (Exception ex)
        {
            if (!ex.Message.Equals("Thread was being aborted."))
            {
                Logger.GetInstance().LogMessage(Log_Priority.INFORMATIONAL, string.Format("URL Redirect exception line 110: {0} --- Exception {1}", context.Request.RawUrl, ex.Message.ToString()));
                context.Response.Redirect("/Error/NotFound");
            }
        }
    }

    /// <summary>
    /// Parses the current URL and extracts the virtual path without query string.
    /// </summary>
    /// <returns>The virtual path of the current URL.</returns>
    private static string GetVirtualPath()
    {
        string path = HttpContext.Current.Request.RawUrl;
        path = path.Substring(0, path.IndexOf("?"));
        path = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        return path;
    }

    /// <summary>
    /// Parses a URL and returns the query string.
    /// </summary>
    /// <param name="url">The URL to parse.</param>
    /// <returns>The query string without the question mark.</returns>
    private static string ExtractQuery(string url)
    {
        int index = url.IndexOf("?") + 1;
        return url.Substring(index);
    }
}