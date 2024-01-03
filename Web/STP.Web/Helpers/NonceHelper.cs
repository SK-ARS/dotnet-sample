using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace STP.Web.Helpers
{
    public static class NonceHelper
    {
        private static string nonceValue;
        private static Queue<string> nonceQueue = new Queue<string>();

        public static string ScriptNonce()
        {
            var owinContext = HttpContext.Current.GetOwinContext();
            nonceValue = owinContext.Get<string>("ScriptNonce").ToString();
            nonceQueue.Enqueue(nonceValue);
            return nonceValue;
        }

        public static Queue<string> NonceQueue()
        {
            return nonceQueue;
        }

        public static void ClearNonceQueue()
        {
            nonceQueue.Clear();
        }
    }
}