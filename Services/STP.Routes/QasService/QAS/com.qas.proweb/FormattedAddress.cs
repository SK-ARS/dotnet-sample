using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class FormattedAddress
    {
		// -- Private Members --
		private readonly AddressLine[] m_aAddressLines;
		private readonly bool m_bIsOverflow;
		private readonly bool m_bIsTruncated;
        // -- Public Methods --
        public FormattedAddress(QAAddressType t)
		{
			m_bIsOverflow = t.Overflow;
			m_bIsTruncated = t.Truncated;
			DPVStatus = t.DPVStatus;

			AddressLineType[] aLines = t.AddressLine;
			// We must have lines in an address so aLines should never be null
			int iSize = aLines.GetLength(0);
			if (iSize > 0)
			{
				m_aAddressLines = new AddressLine[iSize];
				for (int i = 0; i < iSize; i++)
				{
					m_aAddressLines[i] = new AddressLine(aLines[i]);
				}
			}
		}
		// -- Read-only Properties --
		public AddressLine[] AddressLines
		{
			get
			{
				return m_aAddressLines;
			}
		}
		public int Length
		{
			get
			{
				return (m_aAddressLines != null ? m_aAddressLines.Length : 0);
			}
		}
		public bool IsOverflow
		{
			get
			{
				return m_bIsOverflow;
			}
		}
		public bool IsTruncated
		{
			get
			{
				return m_bIsTruncated;
			}
		}
        public DPVStatusType DPVStatus { get; set; }
    }
}