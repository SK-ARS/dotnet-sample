using STP.Applications.Interface;
using STP.Applications.Persistance;
using STP.Domain.NonESDAL;
using System.Diagnostics;

namespace STP.Applications.Providers
{
    public sealed class NEApplicationProvider : INEApplication
    {
        #region ApplicationProvider Singleton

        private NEApplicationProvider()
        {
        }
        public static NEApplicationProvider Instance
        {
            [DebuggerStepThrough]
            get
            {
                return Nested.instance;
            }
        }

        /// <summary>
        /// Not to be called while using logic
        /// </summary>
        internal static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly NEApplicationProvider instance = new NEApplicationProvider();
        }
        #endregion

        public long SaveNEApplication(NEAppGeneralDetails generalDetails)
        {
            return NEApplicationDao.SaveNEApplication(generalDetails);
        }
        public string SubmitNEApplication(long appRevId, bool isVr1)
        {
            return NEApplicationDao.SubmitNEApplication(appRevId, isVr1);
        }
        public string GetNEApplicationStatus(string ESDALReferenceNumber)
        {
            return NEApplicationDao.GetNEApplicationStatus(ESDALReferenceNumber);
        }
    }
}