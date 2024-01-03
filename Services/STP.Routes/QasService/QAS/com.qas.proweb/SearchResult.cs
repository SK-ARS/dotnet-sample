using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class SearchResult
    {
		// -- Public Constants --
		public enum VerificationLevels
		{
			// No verified matches found, or not application
			None = VerifyLevelType.None,
			// High confidence match found (address returned)
			Verified = VerifyLevelType.Verified,
			// Single match found, but user confirmation is recommended (address returned)
			InteractionRequired = VerifyLevelType.InteractionRequired,
			// Address was verified to premises level only (picklist returned)
			PremisesPartial = VerifyLevelType.PremisesPartial,
			// Address was verified to street level only (picklist returned)
			StreetPartial = VerifyLevelType.StreetPartial,
			// Address was verified to multiple addresses (picklist returned)
			Multiple = VerifyLevelType.Multiple,
			// Address was verified to place level (global data only)
			VerifiedPlace = VerifyLevelType.VerifiedPlace,
			// Address was verified to street level (global data only)
			VerifiedStreet = VerifyLevelType.VerifiedStreet
		}
		
		// -- Private Members --
		private readonly FormattedAddress m_Address;
		private readonly Picklist m_Picklist;
		private readonly VerificationLevels m_eVerifyLevel;
		
		// -- Public Methods --
		public SearchResult(QASearchResult sr)
		{
			QAAddressType address = sr.QAAddress;
			if (address != null)
			{
				m_Address = new FormattedAddress(address);
			}

			QAPicklistType picklist = sr.QAPicklist;
			if (picklist != null)
			{
				m_Picklist = new Picklist(picklist);
			}

			m_eVerifyLevel = (VerificationLevels)sr.VerifyLevel;
		}

		// -- Read-only Properties --
		public FormattedAddress Address
		{
			get
			{
				return m_Address;
			}
		}
		public Picklist Picklist
		{
			get
			{
				return m_Picklist;
			}
		}
		public VerificationLevels VerifyLevel
		{
			get
			{
				return m_eVerifyLevel;
			}
		}
	}
}