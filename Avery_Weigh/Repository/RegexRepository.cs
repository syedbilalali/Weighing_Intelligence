using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class RegexRepository
    {

        public bool CheckIPAddress(string ip)
        {
            bool result = true;
            result = Regex.IsMatch(ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
            return result;
        }

        public bool ValidateEmail(string email)
        {
            bool result = true;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                result = true;
            else
                result = false;
            return result;
        }

        public  bool CheckNumber(string strPhoneNumber)
        {
            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            if (strPhoneNumber != null) return Regex.IsMatch(strPhoneNumber, MatchPhoneNumberPattern);
            else return false;
        }
     
        public string ConvertDatasetToString(DataSet Ds)
        {
            string OUT = "";
            for (int t = 0; t < Ds.Tables.Count; t++)
            {
                for (int r = 0; r < Ds.Tables[t].Rows.Count; r++)
                {
                    for (int c = 0; c < Ds.Tables[t].Columns.Count; c++)
                    {
                        string s = Ds.Tables[t].Rows[r][c].ToString();
                        OUT += s;
                    }
                }
            }
            return OUT;
        }

        public DataTable Convertstring(DataTable dt)
        {
            DataTable outputTable = new DataTable();
            foreach (DataColumn col in dt.Columns)
            {
                outputTable.Columns.Add(col.ColumnName, typeof(string));

                //foreach (DataRow row in dt.Rows)
                //{
                //    outputTable.Columns.Add(row.ToString());
                //}
            }
            return outputTable;
        }

        public string Get_FormatDate(DateTime? date)
        {
            return string.Format("{0:dd/MM/yyyy}", date);
        }

        public bool Validate_Date(string input)
        {
            string[] formats = { "d/MM/yyyy", "dd/MM/yyyy","dd-MM-yyyy","MM-dd-yyyy","yyyy-MM-dd", "dddd, dd MMMM yyyy", "dddd, dd MMMM yyyy"
            ,"MM/dd/yyyy HH:mm","MM/dd/yyyy hh:mm tt"};
            var isValidFormat = DateTime.TryParseExact(input, formats, new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime parsedDate);
            if (isValidFormat)
            {
                return true;
            }
            else
            {
                return false;             
            }
        }

       
    }
}