using System;
using System.IO;
using System.Collections;
using System.Text;
using GenLib;

namespace StandardFormatLib
{
	/// <summary>
	/// Standard Format File
	/// </summary>
	public class StandardFormatFile
	{
		private ArrayList m_Records;
		private string m_FileName = "";

		public const string kBackSlash = "\\";

		public StandardFormatFile ()
		{
			m_Records = new ArrayList();
            m_FileName = "";
        }

		public string FileName
		{
			get {return m_FileName;}
			set {m_FileName = value;}
		}

		public int Count()
		{
			if (m_Records == null)
				return 0;
			else return m_Records.Count;
		}

		public bool LoadFile(string strFileName, string strFirstRecordMarker)
		{
			bool fReturn = false;
			m_Records = null;
			m_FileName = strFileName;
			strFirstRecordMarker = StandardFormatFile.kBackSlash
				+ strFirstRecordMarker + Constants.Space;		//need to add for backslash and ending space
			StandardFormatRecord sfr;

            if (File.Exists(strFileName))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(strFileName))
                    {
                        string strRecord = "";
                        string strLine = "";
                        int nLenFRM = strFirstRecordMarker.Length;
                        bool fMoreLines = true;
                        bool fFoundFirstRecord = false;


                        // look for first record
                        strLine = sr.ReadLine();
                        if (strLine == null)
                            fMoreLines = false;

                        while (fMoreLines && !fFoundFirstRecord) //find start of first record
                        {
                            if (strLine != "")		//skip empty lines
                            {
                                if (strLine.StartsWith(strFirstRecordMarker))
                                    fFoundFirstRecord = true;		//found it
                                else
                                {
                                    strLine = sr.ReadLine();
                                    if (strLine == null)
                                        fMoreLines = false;			//no more lines to read
                                }
                            }
                            else
                            {
                                strLine = sr.ReadLine();
                                if (strLine == null)
                                    fMoreLines = false;
                            }
                        }					//end search for first record

                        if (fFoundFirstRecord)
                        {
                            strRecord = strLine;
                            m_Records = new ArrayList();
                            while ((strLine = sr.ReadLine()) != null)	//next line
                            {
                                if (strLine != "")			//skip empty lines
                                {
                                    if (strLine.StartsWith(strFirstRecordMarker))
                                    {
                                        if (strRecord != "")
                                        {
                                            sfr = new StandardFormatRecord(strRecord);
                                            m_Records.Add(sfr);
                                        }
                                        strRecord = strLine;
                                    }
                                    else strRecord += Environment.NewLine + strLine;
                                }
                            }
                            if (strRecord != "")
                            {
                                sfr = new StandardFormatRecord(strRecord);
                                m_Records.Add(sfr);
                            }
                            m_Records.TrimToSize();
                        }
                        //else no first record
                    }
                    // end using

                }
                catch 
                {
                    fReturn = false;
                }
            }
			// else file does not exist
			if ( (m_Records != null) && (m_Records.Count > 0) )
				fReturn = true;
			return fReturn;
		}

		public void SaveFile()
		{
			string strFileName = m_FileName;
			SaveFile(strFileName);
		}

		public void SaveFile(string strFileName)
		{
			if (strFileName == "") return;
            StandardFormatRecord sfr = null;
			StandardFormatField sff = null;
			string strLine = "";

			if ( File.Exists(strFileName) )
				File.Delete(strFileName);
			
			StreamWriter sw = File.CreateText(strFileName);
			int nRecs = this.Count();
			for (int i = 0; i < nRecs; i++)
			{
				sfr = this.GetRecord(i);
				int nFlds = sfr.Count();
				for (int j = 0; j < nFlds; j++)
				{
					sff = sfr.GetField(j);
					strLine = StandardFormatFile.kBackSlash + sff.GetFieldMarker() + " " + sff.GetFieldContents();
					sw.WriteLine(strLine);
				}
				sw.WriteLine("");	//write blank line between records
			}
			sw.Close();
		}

		public ArrayList RetrieveAll()
		{
			return m_Records;
		}

		public StandardFormatRecord GetRecord(int nRec)
		// nRec - nth record
		{
			StandardFormatRecord sfr = null;
			if ( nRec < m_Records.Count)
			{
				sfr = (StandardFormatRecord) m_Records[nRec];
			}
			return sfr;
		}

		public int AddRecord(StandardFormatRecord sfr)
		{
			m_Records.Add(sfr);
            return m_Records.Count - 1;
		}

        public void InsertRecord(int ndx, StandardFormatRecord sfr)
        {
            m_Records.Insert(ndx, sfr);
            return;
        }

		public void DelRecord(int nRec)
		{
			m_Records.RemoveAt(nRec);
		}

		public string GetField(int nRec, string strFM )
		//	nRec  - nth record
		//	strFM - Field marker
		{
			StandardFormatRecord sfr = null;
			string strField = "";
			if ( nRec < m_Records.Count)
			{
				sfr = (StandardFormatRecord) m_Records[nRec];
				strField = sfr.GetFieldContents(strFM);
			}
			return strField;
		}

	}
}

