using STP.Routes.QasService.QAS.com.qas.proweb.soap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class Dataset : IComparable
    {
		// -- Private Members --
		private readonly string m_sID;
		private readonly string m_sName;
		// -- Public Methods --
		public Dataset()
		{
		}
		public Dataset(QADataSet d)
		{
			m_sID = d.ID;
			m_sName = d.Name;
		}
		// Construct from name & id
		public Dataset(string sID, string sName)
		{
			m_sID = sID;
			m_sName = sName;
		}
		// Implement IComparable interface
		public int CompareTo(object obj)
		{
			if (!(obj is Dataset))
			{
				throw new ArgumentException("Object is not a Dataset");
			}
			else
			{
				Dataset dset = (Dataset)obj;

				return Name.CompareTo(dset.Name);
			}
		}
		public static Dataset[] CreateArray(QADataSet[] aDatasets)
		{
			Dataset[] aResults = null;
			if (aDatasets != null)
			{
				int iSize = aDatasets.GetLength(0);
				if (iSize > 0)
				{
					aResults = new Dataset[iSize];
					for (int i = 0; i < iSize; i++)
					{
						aResults[i] = new Dataset(aDatasets[i]);
					}
				}
			}
			return aResults;
		}
		public static Dataset FindByID(Dataset[] aDatasets, string sDataID)
		{
			for (int i = 0; i < aDatasets.GetLength(0); i++)
			{
				if (aDatasets[i].ID.Equals(sDataID))
				{
					return aDatasets[i];
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
		public string ID
		{
			get
			{
				return m_sID;
			}
		}
	}
}