using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class Layout
    {
		// -- Private Members --
		private readonly string m_sName;
		private readonly string m_sComment;
		// -- Public Methods --
		public Layout(QALayout l)
		{
			m_sName = l.Name;
			m_sComment = l.Comment;
		}
		public static Layout[] CreateArray(QALayout[] aLayouts)
		{
			Layout[] aResults = null;
			if (aLayouts != null)
			{
				int iSize = aLayouts.GetLength(0);
				if (iSize > 0)
				{
					aResults = new Layout[iSize];
					for (int i = 0; i < iSize; i++)
					{
						aResults[i] = new Layout(aLayouts[i]);
					}
				}
			}
			return aResults;
		}
		public static Layout FindByName(Layout[] aLayouts, string sLayoutName)
		{
			for (int i = 0; i < aLayouts.GetLength(0); i++)
			{
				if (aLayouts[i].Name.Equals(sLayoutName))
				{
					return aLayouts[i];
				}
			}
			return null;
		}
		// -- Read-only Properties --
		public string Name
		{
			get
			{
				return m_sName;
			}
		}
		public string Comment
		{
			get
			{
				return m_sComment;
			}
		}
	}
}