using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class PromptLine
    {
		// -- Private Members --
		private readonly string m_sPrompt;
		private readonly string m_sExample;
		private readonly int m_iSuggestedInputLength; // positive integer
		// -- Public Methods --
		public PromptLine(STP.Routes.QasService.QAS.com.qas.proweb.soap.PromptLine t)
		{
			m_sPrompt = t.Prompt;
			m_sExample = t.Example;
			m_iSuggestedInputLength = System.Convert.ToInt32(t.SuggestedInputLength);
		}
		// -- Read-only Properties --
		public string Prompt
		{
			get
			{
				return m_sPrompt;
			}
		}
		public string Example
		{
			get
			{
				return m_sExample;
			}
		}
		public int SuggestedInputLength
		{
			get
			{
				return m_iSuggestedInputLength;
			}
		}
	}
}