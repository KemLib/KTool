using System.Collections.Generic;
using System.Text;

namespace KTool.ThirdParty
{
    public class TsvTable
    {
        #region Properties
        public const char CHAR_ROW_END = '\n';

        private List<TsvRow> rows;

        public int Count => rows.Count;
        public TsvRow this[int index] => rows[index];
        #endregion

        #region Construction
        public TsvTable()
        {
            rows = new List<TsvRow>();
        }
        public TsvTable(List<TsvRow> rows)
        {
            this.rows = rows;
        }
        public TsvTable(TsvRow[] rows)
        {
            this.rows = new List<TsvRow>(rows);
        }
        #endregion

        #region Colume
        public int Colume_GetCount()
        {
            return this[0].Count;
        }
        public string Colume_GetValue(int index)
        {
            return this[0][index];
        }
        public int Colume_GetIndex(string valueColume)
        {
            int columeCount = Colume_GetCount();
            for (int i = 0; i < columeCount; i++)
                if (Colume_GetValue(i) == valueColume)
                    return i;
            return -1;
        }
        public string Colume_GetRowValue(int indexColume, int indexRow)
        {
            return this[indexRow][indexColume];
        }
        public string Colume_GetRowValue(string valueColume, int indexRow)
        {
            int indexColume = Colume_GetIndex(valueColume);
            return Colume_GetRowValue(indexColume, indexRow);
        }
        #endregion

        #region Row
        public bool Row_Find(int indexColume, string rowValue, out TsvRow tsvRow)
        {
            for (int i = 0; i < Count; i++)
                if (this[i][indexColume] == rowValue)
                {
                    tsvRow = this[i];
                    return true;
                }
            tsvRow = null;
            return false;
        }
        public bool Row_Find(string valueColume, string rowValue, out TsvRow tsvRow)
        {
            int indexColume = Colume_GetIndex(valueColume);
            return Row_Find(indexColume, rowValue, out tsvRow);
        }
        #endregion

        #region Convert
        public static TsvTable Convert_StringToTsv(string data)
        {
            List<TsvRow> rows = new List<TsvRow>();
            string[] dataRows = data.Split(CHAR_ROW_END);
            foreach (string dataRow in dataRows)
            {
                if (string.IsNullOrEmpty(dataRow))
                    continue;
                TsvRow row = TsvRow.Convert_StringToTsv(dataRow);
                rows.Add(row);
            }
            return new TsvTable(rows);
        }
        public static string Convert_TsvToString(TsvTable tsvTable)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int maxIndex = tsvTable.Count - 1;
            for (int i = 0; i < tsvTable.Count; i++)
            {
                TsvRow.Convert_TsvToString(tsvTable[i]);
                if (i < maxIndex)
                    stringBuilder.Append(CHAR_ROW_END);
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}
