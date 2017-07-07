using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNet.Atmcs.Uscmcp.Bll
{
    public class PrintColumns : List<PrintColumn>
    {

         
    }
    public class PrintColumn
    {

        public PrintColumn(string colName, int colId)
        {
            ColumnName = colName;
            ColumnId = colId;
        }
        string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        int columnId;

        public int ColumnId
        {
            get { return columnId; }
            set { columnId = value; }
        }
    }
}
