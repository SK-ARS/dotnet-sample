using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class Picklist
    {
		// -- Private Members --
		private readonly string m_sMoniker;
		private readonly PicklistItem[] m_aItems;
		private readonly string m_sPrompt;
		private readonly int m_iTotal;
		private readonly bool m_bAutoStepinSafe;
		private readonly bool m_bAutoStepinPastClose;
		private readonly bool m_bAutoFormatSafe;
		private readonly bool m_bAutoFormatPastClose;
		private readonly bool m_bLargePotential;
		private readonly bool m_bMaxMatches;
		private readonly bool m_bMoreOtherMatches;
		private readonly bool m_bOverThreshold;
		private readonly bool m_bTimeout;
		// -- Public Methods --
		public Picklist(QAPicklistType p)
		{
			m_iTotal = System.Convert.ToInt32(p.Total);
			m_sMoniker = p.FullPicklistMoniker;
			m_sPrompt = p.Prompt;
			m_bAutoStepinSafe = p.AutoStepinSafe;
			m_bAutoStepinPastClose = p.AutoStepinPastClose;
			m_bAutoFormatSafe = p.AutoFormatSafe;
			m_bAutoFormatPastClose = p.AutoFormatPastClose;
			m_bLargePotential = p.LargePotential;
			m_bMaxMatches = p.MaxMatches;
			m_bMoreOtherMatches = p.MoreOtherMatches;
			m_bOverThreshold = p.OverThreshold;
			m_bTimeout = p.Timeout;
			// Convert the lines in the picklist
			m_aItems = null;
			PicklistEntryType[] aItems = p.PicklistEntry;
			// Check for null as we can have an empty picklist
			if (aItems != null)
			{
				int iSize = aItems.GetLength(0);
				if (iSize > 0)
				{
					m_aItems = new PicklistItem[iSize];
					for (int i = 0; i < iSize; i++)
					{
						m_aItems[i] = new PicklistItem(aItems[i]);
					}
				}
			}
		}
		// -- Read-only Properties --
		public string Moniker
		{
			get
			{
				return m_sMoniker;
			}
		}
		public PicklistItem[] Items
		{
			get
			{
				return m_aItems;
			}
		}
		public int Length
		{
			get
			{
				return (m_aItems != null ? m_aItems.Length : 0);
			}
		}
		public string Prompt
		{
			get
			{
				return m_sPrompt;
			}
		}
		public int Total
		{
			get
			{
				return m_iTotal;
			}
		}
		// -- Read-only Property Flags --
		public bool IsAutoStepinSafe
		{
			get
			{
				return m_bAutoStepinSafe;
			}
		}
		public bool IsAutoStepinPastClose
		{
			get
			{
				return m_bAutoStepinPastClose;
			}
		}
		public bool IsAutoStepinSingle
		{
			get
			{
				return Length == 1
					&& Items[0].CanStep
					&& !Items[0].IsInformation;
			}
		}
		public bool IsAutoFormatSafe
		{
			get
			{
				return m_bAutoFormatSafe;
			}
		}
		public bool IsAutoFormatPastClose
		{
			get
			{
				return m_bAutoFormatPastClose;
			}
		}
		public bool IsAutoFormatSingle
		{
			get
			{
				return Length == 1
					&& Items[0].IsFullAddress
					&& !Items[0].IsInformation;
			}
		}
		public bool IsLargePotential
		{
			get
			{
				return m_bLargePotential;
			}
		}
		public bool IsMaxMatches
		{
			get
			{
				return m_bMaxMatches;
			}
		}
		public bool AreMoreMatches
		{
			get
			{
				return m_bMoreOtherMatches;
			}
		}
		public bool IsOverThreshold
		{
			get
			{
				return m_bOverThreshold;
			}
		}
		public bool IsTimeout
		{
			get
			{
				return m_bTimeout;
			}
		}
	}
}