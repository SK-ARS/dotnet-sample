using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class LicensedSet
    {
		// -- Public Constants --
		public enum WarningLevels
		{
			None = LicenceWarningLevel.None,
			DataExpiring = LicenceWarningLevel.DataExpiring,
			LicenceExpiring = LicenceWarningLevel.LicenceExpiring,
			ClicksLow = LicenceWarningLevel.ClicksLow,
			Evaluation = LicenceWarningLevel.Evaluation,
			NoClicks = LicenceWarningLevel.NoClicks,
			DataExpired = LicenceWarningLevel.DataExpired,
			EvalLicenceExpired = LicenceWarningLevel.EvalLicenceExpired,
			FullLicenceExpired = LicenceWarningLevel.FullLicenceExpired,
			LicenceNotFound = LicenceWarningLevel.LicenceNotFound,
			DataUnreadable = LicenceWarningLevel.DataUnreadable
		}
		// -- Private Members --
		private readonly string m_sID;
		private readonly string m_sDescription;
		private readonly string m_sCopyright;
		private readonly string m_sVersion;
		private readonly string m_sBaseCountry;
		private readonly string m_sStatus;
		private readonly string m_sServer;
		private readonly WarningLevels m_eWarningLevel;
		private readonly int m_iDaysLeft;  // non-negative
		private readonly int m_iDataDaysLeft;        // non-negative
		private readonly int m_iLicenceDaysLeft; // non-negative
		// -- Public Methods --
		public LicensedSet(QALicensedSet s)
		{
			m_sID = s.ID;
			m_sDescription = s.Description;
			m_sCopyright = s.Copyright;
			m_sVersion = s.Version;
			m_sBaseCountry = s.BaseCountry;
			m_sStatus = s.Status;
			m_sServer = s.Server;
			m_eWarningLevel = (WarningLevels)s.WarningLevel;
			m_iDaysLeft = System.Convert.ToInt32(s.DaysLeft);
			m_iDataDaysLeft = System.Convert.ToInt32(s.DataDaysLeft);
			m_iLicenceDaysLeft = System.Convert.ToInt32(s.LicenceDaysLeft);
		}
		public static LicensedSet[] createArray(QALicenceInfo info)
		{
			LicensedSet[] aResults = null;
			QALicensedSet[] aInfo = info.LicensedSet;
			if (aInfo != null)
			{
				int iSize = aInfo.GetLength(0);
				if (iSize > 0)
				{
					aResults = new LicensedSet[iSize];
					for (int i = 0; i < iSize; i++)
					{
						aResults[i] = new LicensedSet(aInfo[i]);
					}
				}
			}
			return aResults;
		}
		public static LicensedSet[] createArray(QADataMapDetail info)
		{
			LicensedSet[] aResults = null;
			QALicensedSet[] aInfo = info.LicensedSet;

			if (aInfo != null && aInfo.Length > 0)
			{
				aResults = new LicensedSet[aInfo.Length];
				
				for (int i = 0; i < aInfo.Length; ++i)
				{
					aResults[i] = new LicensedSet(aInfo[i]);
				}
			}

			return aResults;
		}
		// -- Read-only Properties --
		public string ID
		{
			get
			{
				return m_sID;
			}
		}
		public string Description
		{
			get
			{
				return m_sDescription;
			}
		}
		public string Copyright
		{
			get
			{
				return m_sCopyright;
			}
		}
		public string Version
		{
			get
			{
				return m_sVersion;
			}
		}
		public string BaseCountry
		{
			get
			{
				return m_sBaseCountry;
			}
		}
		public string Status
		{
			get
			{
				return m_sStatus;
			}
		}
		public string Server
		{
			get
			{
				return m_sServer;
			}
		}
		public WarningLevels WarningLevel
		{
			get
			{
				return m_eWarningLevel;
			}
		}
		public int DaysLeft
		{
			get
			{
				return m_iDaysLeft;
			}
		}
		public int DataDaysLeft
		{
			get
			{
				return m_iDataDaysLeft;
			}
		}
		public int LicenceDaysLeft
		{
			get
			{
				return m_iLicenceDaysLeft;
			}
		}
	}
}