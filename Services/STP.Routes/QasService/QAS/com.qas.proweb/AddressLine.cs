using STP.Routes.QasService.QAS.com.qas.proweb.soap;

namespace STP.Routes.QasService.QAS.com.qas.proweb
{
    public class AddressLine
    {
		// -- Public Constants --
		public enum Types
		{
			None = LineContentType.None,
			Address = LineContentType.Address,
			Name = LineContentType.Name,
			Ancillary = LineContentType.Ancillary,
			DataPlus = LineContentType.DataPlus
		}
		// -- Private Members --
		private readonly string m_sLabel;
		private readonly string m_sLine;
		private readonly Types m_eLineType;
		private readonly bool m_bIsTruncated;
		private readonly bool m_bIsOverflow;
		private readonly DataplusGroup[] m_atDataplusGroups;
		// -- Public Methods --
		public AddressLine(AddressLineType t)
		{
			m_sLabel = t.Label;
			m_sLine = t.Line;
			m_eLineType = (Types)t.LineContent;
			m_bIsTruncated = t.Truncated;
			m_bIsOverflow = t.Overflow;

			if (t.DataplusGroup != null)
			{
				m_atDataplusGroups = new DataplusGroup[t.DataplusGroup.Length];

				for (int i = 0; i < t.DataplusGroup.Length; i++)
				{
					DataplusGroup tGroup = new DataplusGroup(t.DataplusGroup[i]);

					m_atDataplusGroups[i] = tGroup;
				}
			}
		}
		// -- Read-only Properties --
		public string Label
		{
			get
			{
				return m_sLabel;
			}
		}
		public string Line
		{
			get
			{
				return m_sLine;
			}
		}
		public DataplusGroup[] DataplusGroups
		{
			get
			{
				return m_atDataplusGroups;
			}
		}
		public Types LineType
		{
			get
			{
				return m_eLineType;
			}
		}
		public bool IsTruncated
		{
			get
			{
				return m_bIsTruncated;
			}
		}
		public bool IsOverflow
		{
			get
			{
				return m_bIsOverflow;
			}
		}

	}
	public class DataplusGroup
	{
		// -- Private Members --

		private readonly string m_sGroupName;
		private readonly string[] m_asItems;
		public DataplusGroup(DataplusGroupType t)
		{
			m_sGroupName = t.GroupName;
			m_asItems = t.DataplusGroupItem;
		}
		// -- Read-only Properties --
		public string Name
		{
			get
			{
				return m_sGroupName;
			}
		}
		public string[] Items
		{
			get
			{
				return m_asItems;
			}
		}
	}
}