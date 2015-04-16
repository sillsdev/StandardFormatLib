using System;

namespace StandardFormatLib
{
	/// <summary>
	/// Standard Format Field
	/// </summary>
	public class StandardFormatField
	{
		private string m_FieldMarker = "";
		private string m_FieldContents = "";

		public StandardFormatField(string strField)
		{
			if ( (strField.Length > 1)  && (strField.Substring(0,1) == StandardFormatFile.kBackSlash) )
			{
				int nLen = strField.Length;
				string str = " \t";
				char [] chr = str.ToCharArray();
				int index = strField.IndexOfAny(chr);
				if (index < 0)
					index = nLen-1;
				if (index < nLen)
				{
					m_FieldMarker = strField.Substring(1, index).Trim();
					m_FieldContents = strField.Substring(index+1,nLen-index-1).Trim();
				}
			}
		}

		public StandardFormatField(string strFieldMarker, string strFieldContents)
		{
			m_FieldMarker = strFieldMarker;
			m_FieldContents = strFieldContents;
		}

		public string GetFieldMarker()
		{
			return m_FieldMarker;
		}

		public string GetFieldContents()
		{
			return m_FieldContents;
		}

	}
}
