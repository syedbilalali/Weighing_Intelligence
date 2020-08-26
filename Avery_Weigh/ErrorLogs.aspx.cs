using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Avery_Weigh.Model;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.html;
using Avery_Weigh.Repository;

namespace Avery_Weigh
{
    public partial class ErrorLogs : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


            }
        }

        private void Filldata()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    //                  using (SqlCommand cmd = new SqlCommand(@"SELECT *
                    //FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)", con))
                    //                  {
                    
                        using (SqlCommand cmd = new SqlCommand("sp_ErrorLogs", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(txtfrom.Text).ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(txtTo.Text).ToString("yyyy-MM-dd"));
                            //cmd.Parameters.AddWithValue("@Option", "DateWisePendingTransaction");

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                using (DataTable dt = new DataTable())
                                {
                                    da.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        rptList.DataSource = dt;
                                        rptList.DataBind();
                                    }
                                    else
                                    {
                                        rptList.DataSource = null;
                                        rptList.DataBind();
                                    }

                                }

                            }
                        }
                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.success('" + ex + "');", true);
            }

        }



        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Label l = e.Item.FindControl("lblid") as Label;
            //if (l != null)
            //{
            //    l.Text = e.Item.ItemIndex + 1 + "";
            //}
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Filldata();
        }
        static string tripid;
        protected void checkRecord_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            RepeaterItem item = (RepeaterItem)chk.NamingContainer;

            Label lblTripId = (Label)item.FindControl("lblTripId");
            tripid = lblTripId.Text;
        }

        PlantmasterRepository _plantRepo = new PlantmasterRepository();
        protected void linkPrint_Click(object sender, EventArgs e)
        {
            //string strTripId = tripid;
            //if (strTripId == "0")
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr", "toastr.error('No Records');", true);
            //else
            //{
            //    if (File.Exists(Server.MapPath("~/pdfs/" + strTripId + ".pdf")))
            //    {
            //        File.Delete(Server.MapPath("~/pdfs/" + strTripId + ".pdf"));
            //    }

            //    Ticket ts = new Ticket();
            //    ts.GetTicket(Convert.ToInt32(strTripId));
            //    //lnkPrint.OnClientClick = "target='_blank'";
            //    //Response.Redirect("~/pdfs/" + strTripId + ".pdf");
            //    //lnkPrint.Attributes.Add("href",String.Format("/pdfs/" + strTripId + ".pdf"));
            //    //lnkPrint.Attributes.Add("target","_blank");
            //    var varurl = "/pdfs/" + strTripId + ".pdf";
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + varurl + "','_newtab');", true);

            //    //new line added for duplicate print

            //    tblTransaction _trantkt = db.tblTransactions.FirstOrDefault(x => x.TripId == Convert.ToInt32(strTripId) && x.WeighbridgeId == Session["WBID"].ToString());

            //    if (_trantkt != null && _trantkt.SecondWeight != null)
            //    {
            //        // _trantkt.print_ticket = "Y";
            //        _trantkt.PRINT_TICKET = "Y";
            //        db.SubmitChanges();
            //    }

            //    //end of duplicate
            //}

            PlantMaster _plant = _plantRepo.getplantByWeighingMachine(Session["PlantID"].ToString(), "com1");
            //BaseFont bfR = iTextSharp.text.pdf.BaseFont.CreateFont(BaseFont.TIMES_ROMAN, iTextSharp.text.pdf.BaseFont.CP1257, iTextSharp.text.pdf.BaseFont.EMBEDDED);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            StyleSheet style1 = new StyleSheet();
            style1.LoadTagStyle(HtmlTags.TABLE, HtmlTags.FONTSIZE, "8px");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=ErrorLogReports.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddStyleAttribute("font-size", "8pt");
            // rptList.DataBind();
            Panel1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
            // pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            string CompanyLogo = HttpContext.Current.Server.MapPath(".") + @"\images\companylogo\logo.jpg";
            string header1 = HttpContext.Current.Server.MapPath(".") + @"\images\header1.png";
            string header2 = HttpContext.Current.Server.MapPath(".") + @"\images\header2.png";
            //imgLogo.ImageUrl = "/images/companylogo/" + company.CompanyLogo;
            //added on 24-10-2019
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            var boldTableFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
            var NORMALFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);

            PdfPTable table1 = new PdfPTable(4);
            table1.WidthPercentage = 100;
            table1.SetWidths(new float[] { 0.10f, 0.13f, 0.15f, 0.20f });
            //First Row
            PdfPCell cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(header1);
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            image.ScaleAbsolute(230f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(image);
            table1.AddCell(cellheaderleft);
            PdfPCell cellheaderright = new PdfPCell();
            cellheaderright.Border = 0;
            cellheaderright.Rowspan = 2;
            iTextSharp.text.Image image2 = iTextSharp.text.Image.GetInstance(header2);
            image2.ScaleAbsolute(190f, 500f);
            image2.Alignment = Element.HEADER;
            cellheaderright.AddElement(image2);
            table1.AddCell(cellheaderright);

            //2nd Row

            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            iTextSharp.text.Image logoImage = iTextSharp.text.Image.GetInstance(CompanyLogo);
            image.ScaleAbsolute(250f, 200f);
            image.Alignment = Element.HEADER;
            cellheaderleft.AddElement(logoImage);
            table1.AddCell(cellheaderleft);
            cellheaderleft = new PdfPCell();
            cellheaderleft.Border = 0;
            table1.AddCell(cellheaderleft);

            string PlantCodeAddress = _plant.PlantName + "\n" + _plant.PlantAddress1 + "\n" + _plant.PlantAddress2;
            var p = new Paragraph(PlantCodeAddress, boldTableFont);
            cellheaderleft = new PdfPCell();

            cellheaderleft.Border = 0;
            cellheaderleft.HorizontalAlignment = Element.ALIGN_CENTER;
            cellheaderleft.AddElement(p);
            table1.AddCell(cellheaderleft);


            PdfPCell Reportname = new PdfPCell(new Phrase("Error Logs Report", boldTableFont));
            Reportname.HorizontalAlignment = Element.ALIGN_LEFT;
            Reportname.Padding = 5;
            Reportname.Colspan = 2;
            table1.AddCell(Reportname);



            PdfPCell tripdatetime = new PdfPCell(new Phrase("Print Date/Time  :", boldTableFont));
            tripdatetime.HorizontalAlignment = Element.ALIGN_RIGHT;



            table1.AddCell(tripdatetime);

            PdfPCell tripdatetimeValue = new PdfPCell(new Phrase(DateTime.Now.ToString(), NORMALFont));
            tripdatetimeValue.HorizontalAlignment = Element.ALIGN_LEFT;


            table1.AddCell(tripdatetimeValue);

            pdfDoc.Add(table1);



            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
    }
}