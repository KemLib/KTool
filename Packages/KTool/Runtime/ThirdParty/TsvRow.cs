using System.Collections.Generic;
using System.Text;

namespace KTool.ThirdParty
{
    public class TsvRow
    {
        #region Properties
        public const char CHAR_CELL_END = '\t';

        private List<string> cells;

        public int Count => cells.Count;
        public string this[int index] => cells[index];
        #endregion

        #region Construction
        public TsvRow()
        {
            cells = new List<string>();
        }
        public TsvRow(List<string> cells)
        {
            this.cells = cells;
        }
        public TsvRow(string[] cells)
        {
            this.cells = new List<string>(cells);
        }
        #endregion

        #region Method

        #endregion

        #region Convert
        public static TsvRow Convert_StringToTsv(string data)
        {
            string[] cells = data.Split(CHAR_CELL_END);
            return new TsvRow(cells);
        }
        public static string Convert_TsvToString(TsvRow tsvRow)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int maxIndex = tsvRow.Count - 1;
            for (int i = 0; i < tsvRow.Count; i++)
            {
                stringBuilder.Append(tsvRow[i]);
                if (i < maxIndex)
                    stringBuilder.Append(CHAR_CELL_END);
            }
            return stringBuilder.ToString();
        }
        public static void Convert_TsvToString(TsvRow tsvRow, StringBuilder stringBuilder)
        {
            int maxIndex = tsvRow.Count - 1;
            for (int i = 0; i < tsvRow.Count; i++)
            {
                stringBuilder.Append(tsvRow[i]);
                if (i < maxIndex)
                    stringBuilder.Append(CHAR_CELL_END);
            }
        }
        #endregion
    }
}
