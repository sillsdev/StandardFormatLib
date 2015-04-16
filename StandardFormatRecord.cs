using System;
using System.Collections;

namespace StandardFormatLib
{
	/// <summary>
	/// 
	/// </summary>
	public class StandardFormatRecord 
	{
		private ArrayList m_Fields;		//Array of Standard Format Fields

		public StandardFormatRecord()
		{
			m_Fields = new ArrayList();
		}

		public StandardFormatRecord(string strRecord)
		{
			int nBeg = 0;
			int nEnd = 0;
			string strField = "";
			StandardFormatField sff;
			m_Fields = new ArrayList();

			nBeg = strRecord.IndexOf(StandardFormatFile.kBackSlash, nBeg);
			do
			{
				nEnd = strRecord.IndexOf(StandardFormatFile.kBackSlash, nBeg+1) - 1;
				if (nEnd < 0)
					nEnd = strRecord.Length;
				strField = strRecord.Substring(nBeg,nEnd-nBeg);
				sff = new StandardFormatField(strField);
				m_Fields.Add(sff);
				nBeg = nEnd +1;;
			}
			while (nBeg < strRecord.Length);
			m_Fields.TrimToSize();
		}

		public string GetFieldContents(string strFieldMarker)
		{
			string strFieldContents = "";
			StandardFormatField sff;

			for (int i =0; i<m_Fields.Count; i++)
			{
				sff = (StandardFormatField) m_Fields[i];
				if ( strFieldMarker == sff.GetFieldMarker() )
				{
					strFieldContents = sff.GetFieldContents();
					break;
				}
			}
			return strFieldContents;
		}

		public StandardFormatField GetField(int n)
		{
			return (StandardFormatField) m_Fields[n];
		}

		public void AddField(StandardFormatField sff)
		{
			m_Fields.Add(sff);
		}

		public void DelField(int n)
		{
			m_Fields.RemoveAt(n);
		}

		public int Count()
		{
			if (m_Fields == null)
				return 0;
			else return m_Fields.Count;
		}
	}
}
