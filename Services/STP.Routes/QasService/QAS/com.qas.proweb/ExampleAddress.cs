using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class ExampleAddress
    {
		// -- Private Members --
		private readonly string m_sComment;
		private readonly FormattedAddress m_Address;
		// -- Public Methods --
		public ExampleAddress(QAExampleAddress a)
		{
			m_sComment = a.Comment;
			m_Address = new FormattedAddress(a.Address);
		}
		public static ExampleAddress[] createArray(QAExampleAddress[] aAddresses)
		{
			ExampleAddress[] aResults = null;
			if (aAddresses != null)
			{
				int iSize = aAddresses.GetLength(0);
				if (iSize > 0)
				{
					aResults = new ExampleAddress[iSize];
					for (int i = 0; i < iSize; i++)
					{
						aResults[i] = new ExampleAddress(aAddresses[i]);
					}
				}
			}
			return aResults;
		}
		// -- Read-only Properties --
		public string Comment
		{
			get
			{
				return m_sComment;
			}
		}
		public AddressLine[] AddressLines
		{
			get
			{
				return m_Address.AddressLines;
			}
		}
	}
}