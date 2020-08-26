using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Services;
using Advantech.Adam;

namespace Avery_Weigh
{
    /// <summary>
    /// Summary description for adam
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class adam : System.Web.Services.WebService
    {
        private bool m_bStart;
        private AdamSocket adamModbus;
        private Adam6000Type m_Adam6000Type;
        private string m_szIP;
        private int m_iPort;
        private int m_iDoTotal, m_iDiTotal, m_iCount;

        bool varSA = false, varSB = false, varSC = false;

        [WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        public void adamconnect(string varIPAddress, string varPortNo)
        {

            try
            {
                m_bStart = false;

                m_szIP = varIPAddress;   // GlobalVariable.strDIOIPAddress_glb;
                m_iPort = Convert.ToInt32(varPortNo); // GlobalVariable.strDIOPortNumber_glb);
                adamModbus = new AdamSocket();
                adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP
                m_Adam6000Type = Adam6000Type.Adam6066;                                         //InitAdam6066();
                int iDI = 12, iDO = 2;
                m_iDoTotal = iDO;
                m_iDiTotal = iDI;
                if (adamModbus.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
                {
                    // panelDIO.Enabled = true;
                    m_iCount = 0; // reset the reading counter
                    //timer1.Enabled = true; // enable timer
                    m_bStart = true; // starting flag
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Adam is not connected. Please check.');", true);
                }
            }
            catch { }

        }

        [WebMethod]
        public void RefreshDIO(string varIPAddress, string varPortNo)
        {

            try
            {
                m_bStart = false;

                m_szIP = varIPAddress ;   // GlobalVariable.strDIOIPAddress_glb;
                m_iPort = Convert.ToInt32(varPortNo); // GlobalVariable.strDIOPortNumber_glb);
                adamModbus = new AdamSocket();
                adamModbus.SetTimeout(1000, 1000, 1000); // set timeout for TCP
                m_Adam6000Type = Adam6000Type.Adam6066;                                         //InitAdam6066();
                int iDI = 12, iDO = 2;
                m_iDoTotal = iDO;
                m_iDiTotal = iDI;
                if (adamModbus.Connect(m_szIP, ProtocolType.Tcp, m_iPort))
                {
                    // panelDIO.Enabled = true;
                    m_iCount = 0; // reset the reading counter
                    //timer1.Enabled = true; // enable timer
                    m_bStart = true; // starting flag
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('Adam is not connected. Please check.');", true);
                }
            }
            catch { }


            int iDiStart = 1, iDoStart = 17;
            int iChTotal;
            bool[] bDiData, bDoData, bData;

            //bool IsFirstWeight = false


            if (adamModbus.Modbus().ReadCoilStatus(iDiStart, m_iDiTotal, out bDiData) &&
                adamModbus.Modbus().ReadCoilStatus(iDoStart, m_iDoTotal, out bDoData))
            {
                iChTotal = m_iDiTotal + m_iDoTotal;
                bData = new bool[iChTotal];
                Array.Copy(bDiData, 0, bData, 0, m_iDiTotal);
                Array.Copy(bDoData, 0, bData, m_iDiTotal, m_iDoTotal);

                if (bData[0].ToString() == "True")
                {
                    //btnSA.BackColor = System.Drawing.Color.Red;
                    varSA = false;
                    Session["SENSORSA"] = "0";
                }
                else if (bData[0].ToString() == "False")
                {
                    //btnSA.BackColor = System.Drawing.Color.GreenYellow;
                    varSA = true;
                    Session["SENSORSA"] = "1";
                }
                else if (bData[0].ToString() == "Fail")
                {

                }

                if (bData[1].ToString() == "True")
                {
                    //btnSB.BackColor = System.Drawing.Color.Red;
                    varSB = false;
                    Session["SENSORSB"] = "0";
                }
                else if (bData[1].ToString() == "False")
                {
                    //btnSB.BackColor = System.Drawing.Color.GreenYellow;
                    varSB = true;
                    Session["SENSORSB"] = "1";
                }
                else if (bData[1].ToString() == "Fail")
                {

                }


                if (bData[2].ToString() == "True")
                {
                    //btnSC.BackColor = System.Drawing.Color.Red;
                    varSC = false;
                    Session["SENSORSC"] = "0";
                }
                else if (bData[2].ToString() == "False")
                {
                    //btnSC.BackColor = System.Drawing.Color.GreenYellow;
                    varSC = true;
                    Session["SENSORSC"] = "1";
                }
                else if (bData[1].ToString() == "Fail")
                {

                }

            }

            //string varReturnData = varSA.ToString() + "," + varSB.ToString() + "," + varSC.ToString();

            //return varReturnData;


        }
    }
}
