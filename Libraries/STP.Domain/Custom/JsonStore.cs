using STP.Common.Logger;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace STP.Domain.Custom
{
    public static class JsonStore
    {
        public static void StoreInputJsonFile(string inputJson, long id, string name, bool isJsonFile = false)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["JsonServerPath"];
                if (ConfigurationManager.AppSettings["Envrironment"] == "Debug")
                    filePath = AppDomain.CurrentDomain.RelativeSearchPath;
                string courrentfilePath = string.Empty;
                CreateNewDirectory(filePath, ref courrentfilePath);
                CreateNewFile(id, ref name, ref courrentfilePath);

                if(!isJsonFile)
                    File.WriteAllBytes(courrentfilePath, StringExtraction.ZipAndBlob(inputJson)); 
                else
                    File.WriteAllText(courrentfilePath, inputJson);
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + $"StoreInputJsonAsZipFile Exception: {ex}");
            }
        }
        private static void CreateNewFile(long id, ref string name, ref string courrentfilePath)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            string filename = $"{id}_{name}_{ DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}.gzip";
            courrentfilePath = Path.Combine(courrentfilePath, filename);
        }
        private static void CreateNewDirectory(string filePath, ref string courrentfilePath)
        {
            string currentYear = $"{DateTime.Now.Year}";
            courrentfilePath = Path.Combine(filePath, currentYear);

            bool isCurrentExist = Directory.Exists(courrentfilePath);
            if (!isCurrentExist)
                Directory.CreateDirectory(courrentfilePath);
        }
#pragma warning disable S1144 // Unused private types or members should be removed
        private static void DeletePreviousDirectory(string filePath)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
            string previousYear = $"{DateTime.Now.Year - 2}";
            string previousfilePath = Path.Combine(filePath, previousYear);
            bool isPreviousExists = Directory.Exists(previousfilePath);
            if (isPreviousExists)
                Directory.Delete(previousfilePath, true);
        }
    }
}
