using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class PromptSet
    {
		// -- Public Constants --
		public enum Types
		{
			OneLine = PromptSetType.OneLine,
			Default = PromptSetType.Default,
			Generic = PromptSetType.Generic,
			Optimal = PromptSetType.Optimal,
			Alternate = PromptSetType.Alternate,
			Alternate2 = PromptSetType.Alternate2,
			Alternate3 = PromptSetType.Alternate3
		}

		// -- Private Members --
		private readonly bool m_bDynamic;
		private readonly PromptLine[] m_aLines;

		// -- Public Methods --
		public PromptSet(QAPromptSet tPromptSet)
		{
			m_bDynamic = tPromptSet.Dynamic;

			m_aLines = null;
			if (tPromptSet.Line != null)
			{
				int iSize = tPromptSet.Line.GetLength(0);
				if (iSize > 0)
				{
					m_aLines = new PromptLine[iSize];
					for (int i = 0; i < iSize; i++)
					{
						m_aLines[i] = new PromptLine(tPromptSet.Line[i]);
					}
				}
			}
		}
		public String[] GetLinePrompts()
		{
			int iSize = m_aLines.GetLength(0);
			String[] asResults = new String[iSize];
			for (int i = 0; i < iSize; i++)
			{
				asResults[i] = m_aLines[i].Prompt;
			}
			return asResults;
		}
		public bool IsDynamic
		{
			get
			{
				return m_bDynamic;
			}
		}
		public PromptLine[] Lines
		{
			get
			{
				return m_aLines;
			}
		}
	}
}