using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class PicklistItem
    {
		// -- Private Members --
		private readonly string m_sMoniker;
		private readonly string m_sText;
        private readonly int m_iScore;
		private readonly string m_sPartialAddress;
		private readonly bool m_bFullAddress;
		private readonly bool m_bMultiples;
		private readonly bool m_bCanStep;
		private readonly bool m_bAliasMatch;
		private readonly bool m_bPostcodeRecode;
		private readonly bool m_bCrossBorderMatch;
		private readonly bool m_bDummyPOBox;
		private readonly bool m_bName;
		private readonly bool m_bInformation;
		private readonly bool m_bWarnInformation;
		private readonly bool m_bIncompleteAddress;
		private readonly bool m_bUnresolvableRange;
		private readonly bool m_bPhantomPrimaryPoint;
		private readonly bool m_bSubsidiaryData;
		private readonly bool m_bExtendedData;
		private readonly bool m_bEnhancedData;
		// -- Public Methods --
		public PicklistItem(PicklistEntryType tItem)
		{
			m_sText = tItem.Picklist;
			Postcode = tItem.Postcode;
			m_iScore = System.Convert.ToInt32(tItem.Score);
			m_sMoniker = tItem.Moniker;
			m_sPartialAddress = tItem.PartialAddress;
			// Flags
			m_bFullAddress = tItem.FullAddress;
			m_bMultiples = tItem.Multiples;
			m_bCanStep = tItem.CanStep;
			m_bAliasMatch = tItem.AliasMatch;
			m_bPostcodeRecode = tItem.PostcodeRecoded;
			m_bCrossBorderMatch = tItem.CrossBorderMatch;
			m_bDummyPOBox = tItem.DummyPOBox;
			m_bName = tItem.Name;
			m_bInformation = tItem.Information;
			m_bWarnInformation = tItem.WarnInformation;
			m_bIncompleteAddress = tItem.IncompleteAddr;
			m_bUnresolvableRange = tItem.UnresolvableRange;
			m_bPhantomPrimaryPoint = tItem.PhantomPrimaryPoint;
			m_bSubsidiaryData = tItem.SubsidiaryData;
			m_bExtendedData = tItem.ExtendedData;
			m_bEnhancedData = tItem.EnhancedData;
		}
		// -- Read-only Properties --
		public string Moniker
		{
			get
			{
				return m_sMoniker;
			}
		}
		public string Text
		{
			get
			{
				return m_sText;
			}
		}
        public string Postcode { get; set; }
        public int Score
		{
			get
			{
				return m_iScore;
			}
		}
		public string ScoreAsString
		{
			get
			{
				if (Score > 0)
				{
					return Score.ToString() + "%";
				}
				else
				{
					return "";
				}
			}
		}
		public string PartialAddress
		{
			get
			{
				return m_sPartialAddress;
			}
		}

		// -- Read-only Property Flags --
		public bool IsFullAddress
		{
			get
			{
				return m_bFullAddress;
			}
		}
		public bool IsMultipleAddresses
		{
			get
			{
				return m_bMultiples;
			}
		}
		public bool CanStep
		{
			get
			{
				return m_bCanStep;
			}
		}
		public bool IsAliasMatch
		{
			get
			{
				return m_bAliasMatch;
			}
		}
		public bool IsPostcodeRecoded
		{
			get
			{
				return m_bPostcodeRecode;
			}
		}
		public bool IsIncompleteAddress
		{
			get
			{
				return m_bIncompleteAddress;
			}
		}
		public bool IsUnresolvableRange
		{
			get
			{
				return m_bUnresolvableRange;
			}
		}
		public bool IsPhantomPrimaryPoint
		{
			get
			{
				return m_bPhantomPrimaryPoint;
			}
		}
		public bool IsCrossBorderMatch
		{
			get
			{
				return m_bCrossBorderMatch;
			}
		}
		public bool IsDummyPOBox
		{
			get
			{
				return m_bDummyPOBox;
			}
		}
		public bool IsName
		{
			get
			{
				return m_bName;
			}
		}
		public bool IsInformation
		{
			get
			{
				return m_bInformation;
			}
		}
		public bool IsWarnInformation
		{
			get
			{
				return m_bWarnInformation;
			}
		}
		public bool IsSubsidiaryData
		{
			get
			{
				return m_bSubsidiaryData;
			}
		}
		public bool IsExtendedData
		{
			get
			{
				return m_bExtendedData;
			}
		}
		public bool IsEnhancedData
		{
			get
			{
				return m_bEnhancedData;
			}
		}
	}
}