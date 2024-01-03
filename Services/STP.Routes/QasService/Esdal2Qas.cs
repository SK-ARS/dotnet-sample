using STP.Common.Logger;
using STP.Domain.Routes.QAS;
using STP.Routes.QasService.QAS.com.qas.proweb;
using STP.Routes.QasService.QAS.com.qas.prowebintegration;
using System;
using System.Collections.Generic;

namespace STP.Routes.QasService
{
    public static class Esdal2Qas
    {
        public static bool Search(string searchKeyword, ref List<AddrDetails> addrDetails)
        {
            try
            {
                char[] delimiterChars = { ',', ';', '+' };
                QuickAddress searchService = Global.NewQuickAddress();
                searchService.Engine = QuickAddress.EngineTypes.Singleline;
                searchService.SetFlatten(true);

                string[] keywords = searchKeyword.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

                SearchResult res = searchService.Search("GBR", keywords, PromptSet.Types.OneLine, null);

                if (res.Picklist.Total <= 0)
                {
                    return false;
                }

                if (res.Picklist.Items.Length == 1 && res.Picklist.Items[0].Score <= 0)
                {
                    return false;
                }

                for (int i = 0; i < res.Picklist.Items.Length; i++)
                {
                    bool isIrisPost = (res.Picklist.Items[i].Postcode != null || res.Picklist.Items[i].Postcode != "") && res.Picklist.Items[i].Postcode.StartsWith("BT");//Remove Iris Post code(Staring with 'BT') from list
                    if (res.Picklist.Items[i].Score > 0 && !isIrisPost)
                    {
                        AddrDetails addr = new AddrDetails();

                        if (res.Picklist.Items[i].Postcode != null)
                        {
                            res.Picklist.Items[i].Postcode = PostcodeValidation(res.Picklist.Items[i].Postcode);//avoid occurance of invalid postcode 
                        }
                        addr.AddressLine = res.Picklist.Items[i].Text + ", " + res.Picklist.Items[i].Postcode;
                        addr.Moniker = res.Picklist.Items[i].Moniker;
                        addrDetails.Add(addr);
                    }
                }
                return true;//addrDetails.Count > 0 ? true : false;l
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Address Search,  Exception: " + ex​​​​);
                return false;
            }
        }
        //Code added by Nithin on 6/26/15 
        public static string PostcodeValidation(string Postcode)
        {
            string result;
            result = Postcode;
            try
            {
                int index1 = Postcode.IndexOf(' ');
                if (index1 >= 0) //check whether there exist white space
                {
                    int index2 = Postcode.IndexOf(' ', index1 + 1);
                    if (index2 >= 0)//check whether there exist more than one white spaces
                    {
                        result = Postcode.Remove(index2);//only one space is applicable in postcode other wise remove all other charecters after second space
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Postcode Validation,  Exception: " + ex​​​​); 
                return result;//if ther any execption, actual postcode is need to return
            }
        }
        public static AddrDetails GetAddress(string moniker)
        {
            try
            {
                AddrDetails addr = new AddrDetails();
                addr.Northing = 0;
                addr.Easting = 0;

                QuickAddress searchService = Global.NewQuickAddress();
                searchService.Engine = QuickAddress.EngineTypes.Singleline;
                searchService.SetFlatten(true);

                FormattedAddress fa = searchService.GetFormattedAddress(moniker, "GBRCodepoint");

                for (int i = 0; i < fa.AddressLines.Length; i++)
                {
                    if (fa.AddressLines[i].Label == "Code-Point Northing")
                    {
                        addr.Northing = Convert.ToInt64(fa.AddressLines[i].Line);
                    }

                    if (fa.AddressLines[i].Label == "Code-Point Easting")
                    {
                        addr.Easting = Convert.ToInt64(fa.AddressLines[i].Line);
                    }

                    if (addr.Northing != 0 && addr.Easting != 0)
                    {
                        return addr;
                    }
                }
                return addr;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().LogMessage(Log_Priority.FATAL_ERROR, Logger.LogInstance + @" - Unable to Get Address,  Exception: " + ex​​​​);
                return null;
            }
        }
    }
}