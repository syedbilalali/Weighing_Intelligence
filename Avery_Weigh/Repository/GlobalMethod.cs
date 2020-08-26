using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Avery_Weigh.Repository
{
    public class GlobalMethod
    {
        static string _strPrintString;
        static int _lastTransactionNo;
        private static Object thisLockTicketNo = new object();
        private static Object thisLockTransactionNo = new object();
        private static int iCheckThreadTicketNo;
        private static int iCheckThreadTransactionNo;

        /// <summary>
        /// MaxTransactionLimit is 99999.
        /// </summary>
        const int MaxTransactionLimit = 99999;

        /// <summary>
        /// Gets or sets Last Transaction No.
        /// </summary>
        public static int LastTransactionNo
        {
            get { return _lastTransactionNo; }
            set { _lastTransactionNo = value; }
        }

        /// <summary>
        /// 0 if thread is not running and 1 if thread is running.
        /// By default its value is zero.
        /// </summary>
        public static int ThreadCheckGetTicketNo
        {
            get
            {
                return iCheckThreadTicketNo;
            }
            set
            {
                iCheckThreadTicketNo = value;
            }
        }

        /// <summary>
        /// 0 if thread is not running and 1 if thread is running.
        /// By default its value is zero.
        /// </summary>
        public static int ThreadCheckGetTransactionNo
        {
            get
            {
                return iCheckThreadTransactionNo;
            }
            set
            {
                iCheckThreadTransactionNo = value;
            }
        }

     


        public static string GetValueToApostrophy(string pValue)
        {
            if (pValue.Trim().Contains("'"))
            {
                string[] _strSeparator = { "'" };
                string[] _str = pValue.Trim().Split(_strSeparator, StringSplitOptions.None);
                StringBuilder _strBuild = new StringBuilder();
                for (int i = 0; i < _str.Length; i++)
                {
                    if (i == 0)
                    {
                        if (!(_str[i].Trim().Length == 0))
                            _strBuild.Append(_str[i]);
                    }
                    else
                    {
                        _strBuild.Append("''");
                        _strBuild.Append(_str[i]);
                    }
                }
                pValue = _strBuild.ToString();
            }
            return pValue;
        }


        #region ReportMethods
        internal static string Chr(int p_intByte)
        {


            if ((p_intByte < 0) || (p_intByte > 255))
            {

                throw new ArgumentOutOfRangeException("p_intByte", p_intByte, "The argument must be greater than 0 and less then 255");
            }


            byte[] bytBuffer = new byte[] { (byte)p_intByte };

            return Encoding.GetEncoding(1252).GetString(bytBuffer);
        }

        //internal static int Asc(string p_strChar)
        //{
        //    if ((p_strChar.Length == 0) || (p_strChar.Length > 1))
        //    {
        //        throw new ArgumentOutOfRangeException("p_strChar", p_strChar, "Must be a single character.");
        //    }
        //    char[] chrBuffer = { Convert.ToChar(p_strChar) };
        //    byte[] bytBuffer = Encoding.GetEncoding(1252).GetBytes(chrBuffer);
        //    return (int)bytBuffer[0];
        //}

        public static string Space(int n)
        {
            //try
            //{
            return String.Empty.PadLeft(n);
            //}
            //catch { }
        }

        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        public static string funRightJustify(string strInput, int intPosition, string strInsert)
        {

            int intCtr;

            for (intCtr = 0; intCtr <= ((strInsert.Length) - 1); intCtr++)
            {
                string _str = Mid(strInsert, (((strInsert.Length) - 1) - intCtr), 1);
                strInput = strInput.Insert(intPosition - intCtr, _str);
                strInput = strInput.Substring(1);

            }
            return strInput;
        }

        public static string FunRightJustifyDraft(string strInput, int intPosition, string strInsert)
        {
            StringBuilder _strBuild = new StringBuilder(strInput);

            for (int intCtr = 0; intCtr < strInsert.Length; intCtr++)
            {
                _strBuild[intPosition - 1 - intCtr] = Convert.ToChar(strInsert.Substring(strInsert.Length - 1 - intCtr, 1));
            }

            return _strBuild.ToString();
        }

        public static string CALSPACE(string _str, int LENGTH, int LEF)
        {
            int l;
            _str = _str.Trim();
            l = _str.Length;
            if (LEF == 0)
            {
                _str = Space((LENGTH - l)) + _str;
            }

            else
            {
                _str = _str + Space((LENGTH - l));
            }

            return _str;
        }


        public static string Mid(string _str, int StartIndex, int length, string ValueToInsert)
        {

            StringBuilder _strb = new StringBuilder();
            _strb.Append(_str);
            //_strb.Append(ValueToInsert, StartIndex, length);


            int _iStrIndex = 0;
            for (int i = StartIndex - 1; i < StartIndex + length; i++)
            {
                if (_iStrIndex < ValueToInsert.Length)
                {
                    _strb[i] = Convert.ToChar(ValueToInsert.Substring(_iStrIndex, 1));
                    //_strb.Insert(i, ValueToInsert.Substring(_iStrIndex, 1));
                    //_strb.Remove(79, 1);
                }
                else
                {
                    break;
                }
                _iStrIndex++;

            }

            return _strb.ToString();

        }

        public static string Mid(string _str, int StartIndex, string ValueToInsert)
        {

            StringBuilder _strb = new StringBuilder();
            _strb.Append(_str);
            //_strb.Append(ValueToInsert, StartIndex, length);


            int _iStrIndex = 0;
            for (int i = StartIndex - 1; i < StartIndex + ValueToInsert.Length - 1; i++)
            {
                _strb[i] = Convert.ToChar(ValueToInsert.Substring(_iStrIndex, 1));
                _iStrIndex++;
            }

            return _strb.ToString();

        }

        #endregion



    }
}