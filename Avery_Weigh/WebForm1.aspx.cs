using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Avery_Weigh
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sourceURL = "rtsp://192.168.1.250:554/VideoInput/1/mpeg4/1";
            string login = "admin";
            string password = "admin123";
            byte[] buffer = new byte[10000000];
            int read, total = 0;
            // create HTTP request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sourceURL);
            // set login and password
            req.Credentials = new NetworkCredential(login, password);
            // get response
            WebResponse resp = req.GetResponse();
            // get response stream
            Stream stream = resp.GetResponseStream();


            // read data from stream
            while ((read = stream.Read(buffer, total, 1000)) != 0)
            {
                total += read;
            }
            //stream.Close();
            //resp.Close();

            Bitmap bmp1 = (Bitmap)Bitmap.FromStream(
                          new MemoryStream(buffer, 0, total)); //Error: Parametor not valid


            
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}