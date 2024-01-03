using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class CanSearch
    {
		// -- Private Members --
		private readonly bool m_bOk;
		private readonly string m_sErrorMessage;
		private readonly int m_iError;

		// -- Public Methods --
		public CanSearch(QASearchOk tResult)
		{
			m_bOk = tResult.IsOk;

			if (tResult.ErrorCode != null)
			{
				m_iError = System.Convert.ToInt32(tResult.ErrorCode);
			}
			if (tResult.ErrorMessage != null)
			{
				m_sErrorMessage = tResult.ErrorMessage + " [" + m_iError + "]";
			}
		}
		// -- Read-only Properties --
		public bool IsOk
		{
			get
			{
				return m_bOk;
			}
		}
		public string ErrorMessage
		{
			get
			{
				return m_sErrorMessage;
			}
		}
	}
}