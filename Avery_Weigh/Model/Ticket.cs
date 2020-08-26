using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Drawing;
using iTextSharp.text.pdf;
using Avery_Weigh.Repository;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Avery_Weigh.Model
{
    public class Ticket
    {
        TransactionRepository _transRepo = new TransactionRepository();
        PlantmasterRepository _plantRepo = new PlantmasterRepository();
        DataClasses1DataContext db = new DataClasses1DataContext();
        TransporterRepository _transrepo = new TransporterRepository();

        public void GetTicket(int TripId)
        {
            tblTransaction trans = _transRepo.getTransactionByTripId(TripId);
            PlantMaster _plant = _plantRepo.getplantByWeighingMachine(trans.PlantCode, trans.CompanyCode);


            DynamicFieldName  CheckdataTobePrintedMatrialClassification = db.DynamicFieldNames.FirstOrDefault(x => x.MachineId == trans.WeighbridgeId && x.PlantId == trans.PlantCode && x.FieldName == "Material Classification");

            DynamicFieldName CheckdataTobePrintedsupplier = db.DynamicFieldNames.FirstOrDefault(x => x.MachineId == trans.WeighbridgeId && x.PlantId == trans.PlantCode && x.FieldName == "Supplier/customer");
            DynamicFieldName CheckdataTobePrintedTransporter = db.DynamicFieldNames.FirstOrDefault(x => x.MachineId == trans.WeighbridgeId && x.PlantId == trans.PlantCode && x.FieldName == "Transporter");

            DynamicFieldName CheckdataTobePrintedPackingWt = db.DynamicFieldNames.FirstOrDefault(x => x.MachineId == trans.WeighbridgeId && x.PlantId == trans.PlantCode && x.FieldName == "Packing");

            tblMachineWorkingParameter tbldata = db.tblMachineWorkingParameters.FirstOrDefault(x => x.MachineId == trans.WeighbridgeId && x.PlantCode == trans.PlantCode);

            //modelDynamics.ToList()=DynamicFieldName.

            WeightMachineMaster _wtmaster= db.WeightMachineMasters.FirstOrDefault(x => x.PlantCodeId == trans.PlantCode && x.MachineId==trans.WeighbridgeId);
            string varweighingunit = _wtmaster.WeighingUnit;
            string pdfFileName = trans.TripId.ToString();
            string Filename = "";
            
            if (!string.IsNullOrEmpty(pdfFileName))
            {
                string folderpath = HttpContext.Current.Server.MapPath("~\\Pdfs\\");
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                string currentTime = System.DateTime.Now.ToString().Replace(" ", "-").Replace(":", ".");

                //EmpName = EmpName + "";
                Filename = folderpath + pdfFileName + ".pdf";

            }
            else
            {
                Filename = "Blank";
            }

            CompanyMaster _company = db.CompanyMasters.FirstOrDefault(x => x.Id == 1);
            string varLogoName = _company.CompanyLogo;
            //PdfDocument pdfDocument = new PdfDocument();
            ////Add page to the PDF document
            //PdfPage pdfPage = pdfDocument.Pages.Add();
            ////Create a new PdfGrid
            //PdfGrid pdfGrid = new PdfGrid();
            ////Add three columns
            //pdfGrid.Columns.Add(3);
            ////Add header
            //pdfGrid.Headers.Add(1);
            //PdfGridRow pdfGridHeader = pdfGrid.Headers[0];
            //pdfGridHeader.Cells[0].Value = "Employee ID";
            //pdfGridHeader.Cells[1].Value = "Employee Name";
            //pdfGridHeader.Cells[2].Value = "Salary";
            ////Add rows
            //PdfGridRow pdfGridRow = pdfGrid.Rows.Add();
            //pdfGridRow.Cells[0].Value = "E01";
            //pdfGridRow.Cells[1].Value = "Clay";
            //pdfGridRow.Cells[2].Value = "$10,000";
            ////Specify the style for the PdfGridCell
            //PdfGridCellStyle pdfGridCellStyle = new PdfGridCellStyle();
            ////Set background image
            //string logo = HttpContext.Current.Server.MapPath(".") + @"\Ticketheader.png";
            //pdfGridCellStyle.BackgroundImage = new PdfBitmap(logo);
            //pdfGridCellStyle.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            ////Apply style
            //PdfGridCell pdfGridCell = pdfGrid.Rows[0].Cells[0];
            //pdfGridCell.Style = pdfGridCellStyle;
            //pdfGrid.Rows[0].Height = 50;
            ////Set image position for the background image in the style
            //pdfGridCell.ImagePosition = PdfGridImagePosition.Stretch;
            ////Draw the PdfGrid
            //pdfGrid.Draw(pdfPage, PointF.Empty);
            ////Save the document
            //pdfDocument.Save(Filename);
            ////Close the document
            //pdfDocument.Close(true);
            
            int mywidth=0;
            int myheight=0;
            if (tbldata.TicketPaperSize == "A5" && trans.IsMultiProduct==false)
            {
                mywidth = 595;
                myheight = 450;   // 420;
            }
            else
            {
                mywidth = 595;
                myheight = 842;
            }

            var pgSize = new iTextSharp.text.Rectangle(mywidth , myheight );
            // 1 inch=72 points

            Document mydocument = new Document(pgSize);   // iTextSharp.text.PageSize.A5, 10, 10, 42, 35);

            

            iTextSharp.text.Font txtHeadingBOLD = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textNormalShort = FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textNormalShort_BOLD = FontFactory.GetFont("Times New Roman", 9, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textNormalShort22 = FontFactory.GetFont("Times New Roman", 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textNormalShort22_bold = FontFactory.GetFont("Times New Roman", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textBOld = FontFactory.GetFont("Times New Roman", 13, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textbold33 = FontFactory.GetFont("Times New Roman", 11, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font textBOld22 = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLDITALIC, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font lastlinefont = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            iTextSharp.text.Font txtfooter = FontFactory.GetFont("Times New Roman", 10, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.BLACK);
            string CompanyLogo = HttpContext.Current.Server.MapPath(".") + @"\images\companylogo\"+ varLogoName;
            string header1 = HttpContext.Current.Server.MapPath(".") + @"\images\header1.png";
            string header2 = HttpContext.Current.Server.MapPath(".") + @"\images\header2.png";
            //imgLogo.ImageUrl = "/images/companylogo/" + company.CompanyLogo;
            //added on 24-10-2019
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            var boldTableFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD);
            var NORMALFont = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);

            //end 24-09-2019
            
            //FileStream file1 = new FileStream(Filename, System.IO.FileMode.OpenOrCreate);
            //iTextSharp.text.pdf 
            var writer = PdfWriter.GetInstance(mydocument, new FileStream(Filename, FileMode.Append));
            //iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(mydocument, file1);


            mydocument.Open();
            

            //added on 24-10-2019
            PdfPTable headertable = new PdfPTable(6);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 4, 4, 4, 4, 4, 4 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable.SpacingAfter = 10;


            PdfPCell triph = new PdfPCell(new Phrase("Trip Id/Txn No. :", boldTableFont));
            triph.HorizontalAlignment = Element.ALIGN_RIGHT;
            triph.Padding = 5;
            headertable.AddCell(triph);

            PdfPCell triphvalue = new PdfPCell(new Phrase(trans.TripId.ToString(), NORMALFont));
            triphvalue.HorizontalAlignment = Element.ALIGN_LEFT;
            triphvalue.Padding = 5;
            triphvalue.Colspan = 2;
            headertable.AddCell(triphvalue);

            PdfPCell tripdatetime = new PdfPCell(new Phrase("Print Date/Time  :", boldTableFont));
            tripdatetime.HorizontalAlignment = Element.ALIGN_RIGHT;
            tripdatetime.Padding = 5;
            headertable.AddCell(tripdatetime);


            PdfPCell tripdatetimeValue = new PdfPCell(new Phrase(DateTime.Now.ToString(), NORMALFont));
            tripdatetimeValue.HorizontalAlignment = Element.ALIGN_LEFT;
            tripdatetimeValue.Padding = 5;
            tripdatetimeValue.Colspan = 2;
            headertable.AddCell(tripdatetimeValue);
            //above new 
            PdfPCell header = new PdfPCell(new Phrase("Truck No. :", boldTableFont));
            header.HorizontalAlignment = Element.ALIGN_RIGHT;
            header.Padding = 5;
            headertable.AddCell(header);

            PdfPCell header11 = new PdfPCell(new Phrase(trans.TruckNo, NORMALFont));
            header11.HorizontalAlignment = Element.ALIGN_LEFT;
            header11.Padding = 5;
            headertable.AddCell(header11);
            
            //disable because currently not required below 16-lines
            //PdfPCell header21 = new PdfPCell(new Phrase("Vehicle Class :", boldTableFont));
            //header21.HorizontalAlignment = Element.ALIGN_RIGHT;
            //header21.Padding = 5;
            //headertable.AddCell(header21);


            //PdfPCell header3 = new PdfPCell(new Phrase("Value", NORMALFont));
            //header3.HorizontalAlignment = Element.ALIGN_LEFT;
            //header3.Padding = 5;
            //headertable.AddCell(header3);

            //PdfPCell header4 = new PdfPCell(new Phrase("Reg Tare Weight :", boldTableFont));
            //header4.Padding = 5;
            //header4.HorizontalAlignment = Element.ALIGN_RIGHT;
            //headertable.AddCell(header4);


            //PdfPCell header5 = new PdfPCell(new Phrase("", NORMALFont));
            //header5.Padding = 5;
            //header5.HorizontalAlignment = Element.ALIGN_LEFT;
            //headertable.AddCell(header5);

            PdfPCell GatePassheader = new PdfPCell(new Phrase("Gate Pass No. :", boldTableFont));
            GatePassheader.HorizontalAlignment = Element.ALIGN_RIGHT;
            GatePassheader.Padding = 5;
            headertable.AddCell(GatePassheader);

            PdfPCell GatePass = new PdfPCell(new Phrase(trans.GateEntryNo, NORMALFont));
            GatePass.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable.AddCell(GatePass);

            PdfPCell DatedHead = new PdfPCell(new Phrase("Dated :", boldTableFont));
            DatedHead.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable.AddCell(DatedHead);

            PdfPCell Dated1 = new PdfPCell(new Phrase("", NORMALFont));
            Dated1.HorizontalAlignment = Element.ALIGN_LEFT;
            Dated1.Colspan = 3;
            headertable.AddCell(Dated1);

            PdfPCell Challanheader = new PdfPCell(new Phrase("Challan/Invoice No.:", boldTableFont));
            Challanheader.HorizontalAlignment = Element.ALIGN_RIGHT;
            Challanheader.Padding = 5;
            headertable.AddCell(Challanheader);

            PdfPCell Challan = new PdfPCell(new Phrase(trans.ChallanNo, NORMALFont));
            Challan.HorizontalAlignment = Element.ALIGN_LEFT;
            Challan.Padding = 5;
            headertable.AddCell(Challan);


            PdfPCell Dated2Head = new PdfPCell(new Phrase("Dated :", boldTableFont));
            Dated2Head.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable.AddCell(Dated2Head);

            PdfPCell Dated2 = new PdfPCell(new Phrase(trans.ChallanDate==null ? "" : trans.ChallanDate.Value.ToString("dd/MM/yyyy"), NORMALFont));
            Dated2.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable.AddCell(Dated2);


            PdfPCell Dated2Headd = new PdfPCell(new Phrase("Challan Weight :", boldTableFont));
            Dated2Headd.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable.AddCell(Dated2Headd);

            PdfPCell Dated2d = new PdfPCell(new Phrase(trans.ChallanWeight + " " + varweighingunit, NORMALFont));
            Dated2d.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable.AddCell(Dated2d);

            if (CheckdataTobePrintedsupplier.IsRequired == false)
            {

                PdfPCell SupplierHead = new PdfPCell(new Phrase("Supplier Name :", boldTableFont));
                SupplierHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable.AddCell(SupplierHead);

                PdfPCell Supplier = new PdfPCell(new Phrase(trans.SupplierName, NORMALFont));
                Supplier.HorizontalAlignment = Element.ALIGN_LEFT;
                Supplier.Colspan = 5;
                headertable.AddCell(Supplier);
            }

            if (CheckdataTobePrintedTransporter.IsRequired == false)
            {
                PdfPCell TransporterHead = new PdfPCell(new Phrase("Transporter Name :", boldTableFont));
                TransporterHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable.AddCell(TransporterHead);

                PdfPCell TransporterHeadValue = new PdfPCell(new Phrase(trans.TransporterName, NORMALFont));
                TransporterHeadValue.HorizontalAlignment = Element.ALIGN_LEFT;
                TransporterHeadValue.Colspan = 5;
                headertable.AddCell(TransporterHeadValue);
            }
            if (CheckdataTobePrintedMatrialClassification.IsRequired == false)
            {
                PdfPCell MatrialClassificationHead = new PdfPCell(new Phrase("Material Classification :", boldTableFont));
                MatrialClassificationHead.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable.AddCell(MatrialClassificationHead);

                PdfPCell MatrialClassificationHeadValue = new PdfPCell(new Phrase(trans.MaterialClassificationName, NORMALFont));
                MatrialClassificationHeadValue.HorizontalAlignment = Element.ALIGN_LEFT;
                MatrialClassificationHeadValue.Colspan = 5;
                headertable.AddCell(MatrialClassificationHeadValue);
            }

            PdfPCell Blank = new PdfPCell(new Phrase("", NORMALFont));
            Blank.HorizontalAlignment = Element.ALIGN_LEFT;
            Blank.Colspan = 6;
            Blank.Padding = 10;
            Blank.Border = 0;
            headertable.AddCell(Blank);

            PdfPCell Details = new PdfPCell(new Phrase("Material Details :", boldTableFont));
            Details.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.Colspan = 2;
            headertable.AddCell(Details);

            if (CheckdataTobePrintedPackingWt.IsRequired == false)
            {
                PdfPCell Quantity = new PdfPCell(new Phrase("Packing : " + trans.PackingName, boldTableFont));
                Quantity.HorizontalAlignment = Element.ALIGN_LEFT;
                Quantity.Padding = 5;
                Quantity.Colspan = 2;
                //Quantity=(Quantity.ToString() + " + trans.PackingName + ").ToString();
                headertable.AddCell(Quantity);
            }
            else
            {
                PdfPCell Quantity = new PdfPCell(new Phrase(" " , boldTableFont));
                Quantity.HorizontalAlignment = Element.ALIGN_LEFT;
                Quantity.Padding = 5;
                Quantity.Colspan = 2;
                //Quantity=(Quantity.ToString() + " + trans.PackingName + ").ToString();
                headertable.AddCell(Quantity);
            }

            //PdfPCell packingValue = new PdfPCell(new Phrase(trans.PackingName, NORMALFont));
            //Supplier.HorizontalAlignment = Element.ALIGN_LEFT;
            //Supplier.Colspan = 5;
            //headertable.AddCell(packingValue);

            if (CheckdataTobePrintedPackingWt.IsRequired == false)
            {
                PdfPCell PQuantity = new PdfPCell(new Phrase("Packing Quantity : " + trans.PackingQty.ToString() + " " + varweighingunit, boldTableFont));
                PQuantity.HorizontalAlignment = Element.ALIGN_LEFT;
                PQuantity.Colspan = 2;
                PQuantity.Padding = 5;
                headertable.AddCell(PQuantity);
            }
            else
            {
                PdfPCell PQuantity = new PdfPCell(new Phrase(" " , boldTableFont));
                PQuantity.HorizontalAlignment = Element.ALIGN_LEFT;
                PQuantity.Colspan = 2;
                PQuantity.Padding = 5;
                headertable.AddCell(PQuantity);
            }
            //PdfPCell QuantityValue = new PdfPCell(new Phrase(trans.PackingQty.ToString(), NORMALFont));
            //Supplier.HorizontalAlignment = Element.ALIGN_LEFT;
            //Supplier.Colspan = 5;
            //headertable.AddCell(QuantityValue);



            PdfPCell SNO = new PdfPCell(new Phrase("S. No. :", boldTableFont));
            SNO.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable.AddCell(SNO);

            PdfPCell PODO = new PdfPCell(new Phrase("P.O/D.O. No./Date :", boldTableFont));
            PODO.HorizontalAlignment = Element.ALIGN_LEFT;
            PODO.Padding = 5;
            headertable.AddCell(PODO);

            PdfPCell Mcode = new PdfPCell(new Phrase("Material Code :", boldTableFont));
            Mcode.HorizontalAlignment = Element.ALIGN_LEFT;
            Mcode.Padding = 5;
            headertable.AddCell(Mcode);

            PdfPCell MDesc = new PdfPCell(new Phrase("Material Description :", boldTableFont));
            MDesc.HorizontalAlignment = Element.ALIGN_LEFT;
            MDesc.Padding = 5;
            MDesc.Colspan = 2;
            headertable.AddCell(MDesc);


            PdfPCell QTY = new PdfPCell(new Phrase("QTY :", boldTableFont));
            QTY.HorizontalAlignment = Element.ALIGN_LEFT;
            QTY.Padding = 5;
            QTY.Colspan = 2;
            headertable.AddCell(QTY);

            if (trans.IsMultiProduct == false)
            {

                PdfPCell SNOV = new PdfPCell(new Phrase("1", NORMALFont));
                SNOV.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable.AddCell(SNOV);

                string varpodata = string.Empty;
                if (trans.PODate != null)
                {
                    varpodata = trans.PONo == "" ? " " : trans.PONo.ToString() + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    varpodata = trans.PONo == null ? " " : trans.PONo.ToString(); // + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                }
                try
                {
                PdfPCell PODOv = new PdfPCell(new Phrase(varpodata  , NORMALFont));
            
                PODOv.HorizontalAlignment = Element.ALIGN_LEFT;
                PODOv.Padding = 5;
                headertable.AddCell(PODOv);
                }
                catch { }
          
                PdfPCell McodeV = new PdfPCell(new Phrase(trans.MaterialCode, NORMALFont));
                McodeV.HorizontalAlignment = Element.ALIGN_LEFT;
                McodeV.Padding = 5;
                headertable.AddCell(McodeV);

                PdfPCell MDescV = new PdfPCell(new Phrase(trans.MaterialName, NORMALFont));
                MDescV.HorizontalAlignment = Element.ALIGN_LEFT;
                MDescV.Padding = 5;
                MDescV.Colspan = 2;
                headertable.AddCell(MDescV);


                PdfPCell QTYV = new PdfPCell(new Phrase(trans.NetWeight == null ? "" : trans.NetWeight.Value.ToString("0") + " " + varweighingunit, NORMALFont)); ;
                QTYV.HorizontalAlignment = Element.ALIGN_LEFT;
                QTYV.Padding = 5;
                QTYV.Colspan = 2;
                headertable.AddCell(QTYV);
                headertable.SpacingAfter = 10;
            }
            else
            {

                PdfPCell SNOV = new PdfPCell(new Phrase("1", NORMALFont));
                SNOV.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable.AddCell(SNOV);

                string varpodata = string.Empty;
                if (trans.PODate != null)
                {
                    varpodata = trans.PONo == "" ? " " : trans.PONo.ToString() + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    varpodata = trans.PONo == null ? " " : trans.PONo.ToString(); // + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                }
                try
                {
                    PdfPCell PODOv = new PdfPCell(new Phrase(varpodata, NORMALFont));

                    PODOv.HorizontalAlignment = Element.ALIGN_LEFT;
                    PODOv.Padding = 5;
                    headertable.AddCell(PODOv);
                }
                catch { }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AveryDBConnectionString"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("select * from tblTransactionMaterials where TransactionId=" + trans.TripId + "", con))
                    {
                        using (SqlDataAdapter ds = new SqlDataAdapter(cmd))
                        {
                            using (DataTable dtbl = new DataTable())
                            {
                                ds.Fill(dtbl);

                                if (dtbl.Rows.Count > 0)
                                {

                                    for (int i = 0; i < dtbl.Rows.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            PdfPCell McodeV = new PdfPCell(new Phrase(dtbl.Rows[i]["MaterialCode"].ToString(), NORMALFont));
                                            McodeV.HorizontalAlignment = Element.ALIGN_LEFT;
                                            McodeV.Padding = 5;
                                            headertable.AddCell(McodeV);

                                            PdfPCell MDescV = new PdfPCell(new Phrase(dtbl.Rows[i]["MaterialName"].ToString(), NORMALFont));
                                            MDescV.HorizontalAlignment = Element.ALIGN_LEFT;
                                            MDescV.Padding = 5;
                                            MDescV.Colspan = 2;
                                            headertable.AddCell(MDescV);


                                            PdfPCell QTYV = new PdfPCell(new Phrase(string.Format("{0:0}",dtbl.Rows[i]["Weight"]).ToString() + " " + varweighingunit, NORMALFont));
                                            QTYV.HorizontalAlignment = Element.ALIGN_LEFT;
                                            QTYV.Padding = 5;
                                            QTYV.Colspan = 2;
                                            headertable.AddCell(QTYV);
                                            headertable.SpacingAfter = 10;
                                        }
                                        else
                                        {
                                            PdfPCell SNOV1 = new PdfPCell(new Phrase((i+1).ToString(), NORMALFont));
                                            SNOV1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            headertable.AddCell(SNOV1);

                                            string varpodata1 = string.Empty;
                                            if (trans.PODate != null)
                                            {
                                                varpodata1 = trans.PONo == "" ? " " : trans.PONo.ToString() + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                varpodata1 = trans.PONo == null ? " " : trans.PONo.ToString(); // + "," + trans.PODate == null ? "" : trans.PODate.Value.ToString("dd/MM/yyyy");
                                            }
                                            try
                                            {
                                                PdfPCell PODOv = new PdfPCell(new Phrase(varpodata, NORMALFont));

                                                PODOv.HorizontalAlignment = Element.ALIGN_LEFT;
                                                PODOv.Padding = 5;
                                                headertable.AddCell(PODOv);
                                            }
                                            catch { }

                                            PdfPCell McodeV1 = new PdfPCell(new Phrase(dtbl.Rows[i]["MaterialCode"].ToString(), NORMALFont));
                                            McodeV1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            McodeV1.Padding = 5;
                                            headertable.AddCell(McodeV1);

                                            PdfPCell MDescV1 = new PdfPCell(new Phrase(dtbl.Rows[i]["MaterialName"].ToString(), NORMALFont));
                                            MDescV1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            MDescV1.Padding = 5;
                                            MDescV1.Colspan = 2;
                                            headertable.AddCell(MDescV1);


                                            PdfPCell QTYV1 = new PdfPCell(new Phrase(string.Format("{0:0}", dtbl.Rows[i]["Weight"]).ToString() + " " + varweighingunit, NORMALFont)); ;
                                            QTYV1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            QTYV1.Padding = 5;
                                            QTYV1.Colspan = 2;
                                            headertable.AddCell(QTYV1);
                                            headertable.SpacingAfter = 10;
                                        }
                                    }

                                }
                            }

                        }
                    }

                }
               

            }

            PdfPTable headertable1 = new PdfPTable(6);
            headertable1.HorizontalAlignment = 0;
            headertable1.WidthPercentage = 100;
            headertable1.SetWidths(new float[] { 4, 4, 4, 4, 4, 4 });  // then set the column's __relative__ widths
            headertable1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            //headertable1.SpacingBefore =10;
            


            PdfPCell FstWtHeader = new PdfPCell(new Phrase("1st Wt DateTime :", boldTableFont));
            FstWtHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
            FstWtHeader.Padding = 5;
            headertable1.AddCell(FstWtHeader);

            PdfPCell FstWtHeaderValue = new PdfPCell(new Phrase(trans.FirstWtDateTime.ToString(), NORMALFont));
            FstWtHeaderValue.HorizontalAlignment = Element.ALIGN_LEFT;
            FstWtHeaderValue.Padding = 5;
            FstWtHeaderValue.Colspan = 2;
            headertable1.AddCell(FstWtHeaderValue);

            if (!string.IsNullOrEmpty(trans.SecondWeight.ToString()) && trans.TransactionStatus == 2)
            {
                PdfPCell secondWtHeader = new PdfPCell(new Phrase("2nd Wt DateTime :", boldTableFont));
                secondWtHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
                secondWtHeader.Padding = 5;
                headertable1.AddCell(secondWtHeader);


                PdfPCell secondWtHeaderValue = new PdfPCell(new Phrase(trans.SecondWtDateTime.ToString(), NORMALFont));
                secondWtHeaderValue.HorizontalAlignment = Element.ALIGN_LEFT;
                secondWtHeaderValue.Padding = 5;
                secondWtHeaderValue.Colspan = 2;
                headertable1.AddCell(secondWtHeaderValue);
            }

            PdfPCell FstWtHeader1 = new PdfPCell(new Phrase("First Weight :", boldTableFont));
            FstWtHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
            FstWtHeader1.Padding = 5;
            headertable1.AddCell(FstWtHeader1);

            PdfPCell FstWtHeaderValue1 = new PdfPCell(new Phrase(trans.FirstWeight.Value.ToString("0") + " " + varweighingunit, NORMALFont));
            FstWtHeaderValue1.HorizontalAlignment = Element.ALIGN_LEFT;
            FstWtHeaderValue1.Padding = 5;
            FstWtHeaderValue1.Colspan = 2;
            headertable1.AddCell(FstWtHeaderValue1);

            if (!string.IsNullOrEmpty(trans.SecondWeight.ToString()) && trans.TransactionStatus==2)
            {

                PdfPCell secondWtHeader1 = new PdfPCell(new Phrase("Second Weight :", boldTableFont));
                secondWtHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                secondWtHeader1.Padding = 5;
                headertable1.AddCell(secondWtHeader1);


                PdfPCell secondWtHeaderValue1 = new PdfPCell(new Phrase(trans.SecondWeight.Value.ToString("0") + " " + varweighingunit, NORMALFont));
                secondWtHeaderValue1.HorizontalAlignment = Element.ALIGN_LEFT;
                secondWtHeaderValue1.Padding = 5;
                secondWtHeaderValue1.Colspan = 2;
                headertable1.AddCell(secondWtHeaderValue1);

                if (CheckdataTobePrintedPackingWt.IsRequired == false)
                {

                    PdfPCell NetWtHeader = new PdfPCell(new Phrase("Net Weight(With Packing Wt) :", boldTableFont));
                    NetWtHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
                    NetWtHeader.Padding = 5;
                    //NetWtHeader.Colspan = 2;
                    headertable1.AddCell(NetWtHeader);

                    PdfPCell NetWtHeaderValue = new PdfPCell(new Phrase(trans.NetWeight.Value.ToString("0") + "  " + varweighingunit, NORMALFont));
                    NetWtHeaderValue.HorizontalAlignment = Element.ALIGN_LEFT;
                    NetWtHeader.Padding = 5;
                    NetWtHeaderValue.Colspan = 2;
                    headertable1.AddCell(NetWtHeaderValue);

                    PdfPCell NetWtHeader1 = new PdfPCell(new Phrase("Net Weight(Without Packing Wt) :", boldTableFont));
                    NetWtHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    NetWtHeader1.Padding = 5;
                    //NetWtHeader.Colspan = 2;
                    headertable1.AddCell(NetWtHeader1);

                    var totalnetwt = string.Empty;
                    var varPackingqty = string.Empty;
                    try
                    {
                        varPackingqty = trans.PackingQty.Value.ToString();
                    }
                    catch { }
                    if (varPackingqty != "")
                    {
                        totalnetwt = (trans.NetWeight.Value - trans.PackingQty.Value).ToString("0");
                    }
                    else
                    {
                        totalnetwt = trans.NetWeight.Value.ToString("0");
                    }
                    //PdfPCell NetWtHeaderValue = new PdfPCell(new Phrase(trans.NetWeight.Value.ToString("0") + "  " + varweighingunit, NORMALFont ));
                    PdfPCell NetWtHeaderValue1 = new PdfPCell(new Phrase(totalnetwt.ToString() + "  " + varweighingunit, NORMALFont));
                    NetWtHeaderValue1.HorizontalAlignment = Element.ALIGN_LEFT;
                    NetWtHeader1.Padding = 5;
                    NetWtHeaderValue1.Colspan = 2;
                    headertable1.AddCell(NetWtHeaderValue1);
                }

                if (CheckdataTobePrintedPackingWt.IsRequired == true)
                {

                    PdfPCell NetWtHeader = new PdfPCell(new Phrase("Net Weight :", boldTableFont));
                    NetWtHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
                    NetWtHeader.Padding = 5;
                    //NetWtHeader.Colspan = 2;
                    headertable1.AddCell(NetWtHeader);

                    PdfPCell NetWtHeaderValue = new PdfPCell(new Phrase(trans.NetWeight.Value.ToString("0") + "  " + varweighingunit, NORMALFont));
                    NetWtHeaderValue.HorizontalAlignment = Element.ALIGN_LEFT;
                    NetWtHeader.Padding = 5;
                    NetWtHeaderValue.Colspan = 5;
                    headertable1.AddCell(NetWtHeaderValue);

                    //PdfPCell NetWtHeader1 = new PdfPCell(new Phrase("Net Weight(With Packing Wt) :", boldTableFont));
                    //NetWtHeader1.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //NetWtHeader1.Padding = 5;
                    ////NetWtHeader.Colspan = 2;
                    //headertable1.AddCell(NetWtHeader1);

                    //var totalnetwt = string.Empty;
                    //var varPackingqty = string.Empty;
                    //try
                    //{
                    //    varPackingqty = trans.PackingQty.Value.ToString();
                    //}
                    //catch { }
                    //if (varPackingqty != "")
                    //{
                    //    totalnetwt = (trans.NetWeight.Value - trans.PackingQty.Value).ToString("0");
                    //}
                    //else
                    //{
                    //    totalnetwt = trans.NetWeight.Value.ToString("0");
                    //}
                    ////PdfPCell NetWtHeaderValue = new PdfPCell(new Phrase(trans.NetWeight.Value.ToString("0") + "  " + varweighingunit, NORMALFont ));
                    //PdfPCell NetWtHeaderValue1 = new PdfPCell(new Phrase(totalnetwt.ToString() + "  " + varweighingunit, NORMALFont));
                    //NetWtHeaderValue1.HorizontalAlignment = Element.ALIGN_LEFT;
                    //NetWtHeader1.Padding = 5;
                    //NetWtHeaderValue1.Colspan = 2;
                    //headertable1.AddCell(NetWtHeaderValue1);
                }
            }
            //above new 

            //end new code


            PdfPTable table1 = new PdfPTable(4);
            table1.WidthPercentage = 100;
            table1.SetWidths(new float[] { 0.10f, 0.10f, 0.15f, 0.20f });
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
            try

            {
                string PlantCodeAddress = _plant.PlantName + "\n" + _plant.PlantAddress1 + "\n" + _plant.PlantAddress2;
                var p = new Paragraph(PlantCodeAddress, boldTableFont);
            
                cellheaderleft = new PdfPCell();
            
                cellheaderleft.Border = 0;
                cellheaderleft.HorizontalAlignment = Element.ALIGN_CENTER;
                cellheaderleft.AddElement(p);
                table1.AddCell(cellheaderleft);
            }
            catch { }
            //new table added on 28-09-2019


            // for signature added

           

            PdfPTable table9 = new PdfPTable(4);

            PdfPCell cell = new PdfPCell(new Phrase("Header spanning 3 columns"));

            cell.Colspan = 4;

            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.Border = 0;
           
            table9.AddCell(cell);

            table9.AddCell("Col 1 Row 1");

            table9.AddCell("Col 2 Row 1");

            table9.AddCell("Col 3 Row 1");
            table9.AddCell("Col 4 Row 1");

            table9.AddCell("Col 1 Row 2");

            table9.AddCell("Col 2 Row 2");

            table9.AddCell("Col 3 Row 2");
            table9.AddCell("Col 4 Row 2");
            table9.DefaultCell.Border.Equals(false);
            
            //document.Add(table9);

            //end new table for testing 



            PdfPTable table2 = new PdfPTable(4);
            table2.WidthPercentage = 100;
            table2.SetWidths(new float[] { 0.15f, 0.15f, 0.15f, 0.18f });
            cellheaderleft = new PdfPCell();
            cellheaderleft.Colspan = 3;
            cellheaderleft.Border = 0;
            table2.AddCell(cellheaderleft);
            string varTktCaption = string.Empty;
            if (trans.SecondWeight  == null ||  trans.TransactionStatus == 1)
            {
                varTktCaption = "FIRST WEIGHING TICKET";
            }
            else if(trans.PRINT_TICKET == "N")
            {
                varTktCaption = "               TICKET";
            }
            else
            {
                varTktCaption = "     DUPLICATE TICKET";
            }
            var _ticket = new Paragraph(varTktCaption , textNormalShort22_bold);
            cellheaderleft = new PdfPCell();
            cellheaderleft.BorderWidthTop = 0;
            cellheaderleft.BorderWidthLeft = 0;
            cellheaderleft.BorderWidthRight = 0;
            cellheaderleft.BorderWidthBottom = 2;
            cellheaderleft.PaddingBottom = 10;
            cellheaderleft.BorderColorBottom = BaseColor.RED;
            cellheaderleft.AddElement(_ticket);
            table2.AddCell(cellheaderleft);

            //PdfPTable table3 = new PdfPTable(6);
            //table3.DefaultCell.HorizontalAlignment = 2;
            //table3.WidthPercentage = 100;
            //table3.SetWidths(new float[] { 0.15f, 0.15f, 0.15f, 0.15f, 0.15f, 0.15f });
            PdfPCell cellbody = new PdfPCell();
            //cellbody.HorizontalAlignment = Element.ALIGN_RIGHT;
            //var _tripID = new Paragraph("   Trip ID/Txn. No.:", boldTableFont);
            //cellbody.Padding = 5;
            //cellbody.AddElement(_tripID);
            //cellbody.Border = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var tripID = new Paragraph(trans.TripId.ToString(), textNormalShort_BOLD);
            //cellbody.Padding = 5;
            //cellbody.AddElement(tripID);
            //cellbody.Border = 0;
            //cellbody.Colspan = 2;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _dateTime = new Paragraph("Date/Time", textNormalShort_BOLD);
            //cellbody.AddElement(_dateTime);
            //cellbody.Border = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var dateTime = new Paragraph(DateTime.Now.ToString(), textNormalShort_BOLD);
            //cellbody.AddElement(dateTime);
            //cellbody.Padding = 5;

            //cellbody.Border = 0;
            //cellbody.Colspan = 2;
            //table3.AddCell(cellbody);
            //next row
            //cellbody = new PdfPCell();
            //cellbody.HorizontalAlignment = Element.ALIGN_RIGHT;
            //cellbody.Padding = 5;
            //var _truckNo = new Paragraph("Truck No :", textNormalShort22_bold);
            //cellbody.AddElement(_truckNo);
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0.5f;
            //cellbody.BorderWidthRight = 0;
            ////cellbody.HorizontalAlignment =2;
            //table3.DefaultCell.HorizontalAlignment = 2;


            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var truckNo = new Paragraph(trans.TruckNo, textNormalShort_BOLD);
            //cellbody.AddElement(truckNo);
            //cellbody.Padding = 4;
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _vehicleClass = new Paragraph("Vehicle Class", textNormalShort_BOLD);
            //cellbody.AddElement(_vehicleClass);
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            ////var VEHICLECLASS = new Paragraph(trans.v, textNormalShort_BOLD);
            ////cellbody.AddElement(VEHICLECLASS);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _regTareWeight = new Paragraph("Reg. Tare Weight", textNormalShort_BOLD);
            //cellbody.AddElement(_regTareWeight);
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0.5f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0.5f;
            //table3.AddCell(cellbody);
            ////next Row
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _gatePassno = new Paragraph("Gate Pass No.", textNormalShort_BOLD);
            //cellbody.AddElement(_gatePassno);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0.5f;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var gatepassno = new Paragraph(trans.GateEntryNo, textNormalShort_BOLD);
            //cellbody.AddElement(gatepassno);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _gatepassdated = new Paragraph("Dated", textNormalShort_BOLD);
            //cellbody.AddElement(_gatepassdated);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _blank = new Paragraph("", textNormalShort_BOLD);
            //cellbody.AddElement(_blank);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0.5f;
            //table3.AddCell(cellbody);
            ////next row
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _challanInvoice = new Paragraph("Challan/Invoice", textNormalShort_BOLD);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0.5f;
            //cellbody.BorderWidthRight = 0;
            //cellbody.AddElement(_challanInvoice);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var challanno = new Paragraph(trans.ChallanNo, textNormalShort_BOLD);
            //cellbody.AddElement(challanno);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();

            //cellbody.Padding = 5;
            //var _challanInvoicedated = new Paragraph("Dated", textNormalShort_BOLD);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //cellbody.AddElement(_challanInvoicedated);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var challandate = new Paragraph(trans.ChallanDate.ToString(), textNormalShort_BOLD);
            //cellbody.AddElement(challandate);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //var _challanweight = new Paragraph("Challan Weight", textNormalShort_BOLD);
            //cellbody.AddElement(_challanweight);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var challanwt = new Paragraph(trans.ChallanWeight.ToString(), textNormalShort_BOLD);
            //cellbody.AddElement(challanwt);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0.5f;
            //table3.AddCell(cellbody);
            ////next row
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0.5f;
            //cellbody.BorderWidthRight = 0;
            //var _supplier = new Paragraph("Supplier/Customer", textNormalShort_BOLD);
            //cellbody.AddElement(_supplier);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var suppcustomer = new Paragraph(trans.SupplierName, textNormalShort_BOLD);
            //cellbody.AddElement(suppcustomer);
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();

            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //_blank = new Paragraph("", textNormalShort_BOLD);
            //cellbody.AddElement(_blank);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //_blank = new Paragraph("", textNormalShort_BOLD);
            //cellbody.AddElement(_blank);
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0.5f;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //cellbody.Padding = 5;
            //var _Transporter = new Paragraph("Transporter", textNormalShort_BOLD);
            //cellbody.AddElement(_Transporter);
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0.5f;
            //cellbody.BorderWidthRight = 0;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();
            //var transporter = new Paragraph(trans.TransporterName, textNormalShort_BOLD);
            //cellbody.AddElement(transporter);
            //cellbody.Padding = 5;
            //cellbody.Colspan = 5;
            //cellbody.BorderWidthTop = 0;
            //cellbody.BorderWidthBottom = 0.5f;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0.5f;
            //table3.AddCell(cellbody);
            //cellbody = new PdfPCell();

            //cellbody.Padding = 5;
            //cellbody.Colspan = 6;
            //cellbody.Border = 0;
            //cellbody.MinimumHeight = 20;
            //table3.AddCell(cellbody);

            PdfPTable table4 = new PdfPTable(6);
            table4.WidthPercentage = 100;
            table4.SetWidths(new float[] { 0.15f, 0.15f, 0.15f, 0.15f, 0.15f, 0.15f });
            cellbody = new PdfPCell();
            cellbody.Colspan = 2;
            var _materialDetails = new Paragraph("Materials Details", textNormalShort_BOLD);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.AddElement(_materialDetails);
            table4.AddCell(cellbody);
            var _packing = new Paragraph("Packing", textNormalShort_BOLD);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            cellbody.AddElement(_packing);
            table4.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            table4.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            var _packingQty = new Paragraph("Packing Qty", textNormalShort_BOLD);
            cellbody.AddElement(_packingQty);
            table4.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0.5f;
            cellbody.Padding = 5;
            table4.AddCell(cellbody);

            PdfPTable table5 = new PdfPTable(5);
            table5.WidthPercentage = 100;
            table5.SetWidths(new float[] { 0.05f, 0.15f, 0.15f, 0.25f, 0.10f });
            cellbody = new PdfPCell();
            var _SNo = new Paragraph("S.No.", textNormalShort_BOLD);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.AddElement(_SNo);
            table5.AddCell(cellbody);
            var _PODO = new Paragraph("P.O./D.O. No./Date", textNormalShort_BOLD);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            cellbody.AddElement(_PODO);
            table5.AddCell(cellbody);
            var _matcode = new Paragraph("Material Code", textNormalShort_BOLD);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            cellbody.AddElement(_matcode);
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            var _matdesc = new Paragraph("Material Description", textNormalShort_BOLD);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.AddElement(_matdesc);
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            var _Qty = new Paragraph("Qty", textNormalShort_BOLD);
            cellbody.AddElement(_Qty);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0.5f;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            //for(int i=0;i<3;i++)
            //for (int i = 0; i < 0; i++)
            //{
            cellbody = new PdfPCell();
            _SNo = new Paragraph("1", textNormalShort_BOLD);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.AddElement(_SNo);
            table5.AddCell(cellbody);
            _PODO = new Paragraph(" ", textNormalShort_BOLD);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            cellbody.AddElement(_PODO);
            table5.AddCell(cellbody);
            //_matcode = new Paragraph("Material Code", textNormalShort_BOLD);
            cellbody = new PdfPCell();
            var MATCODE = new Paragraph(trans.MaterialCode, textNormalShort_BOLD);
            cellbody.AddElement(MATCODE);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            //cellbody.AddElement(_matcode);
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            //_matdesc = new Paragraph("Material Description", textNormalShort_BOLD);
            var matdesc = new Paragraph(trans.MaterialName, textNormalShort_BOLD);
            cellbody.AddElement(matdesc);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            //cellbody.AddElement(_matdesc);
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            // _Qty = new Paragraph("Qty", textNormalShort_BOLD);
            // cellbody.AddElement(_Qty);
            var varnetwt = new Paragraph(trans.NetWeight.ToString(), textNormalShort_BOLD);
            cellbody.AddElement(varnetwt);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0.5f;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            //}
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            cellbody.Padding = 5;
            table5.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            cellbody.Colspan = 5;
            cellbody.Border = 0;
            cellbody.MinimumHeight = 20;
            table5.AddCell(cellbody);


            PdfPTable table6 = new PdfPTable(4);
            table6.WidthPercentage = 100;
            table6.SetWidths(new float[] { 0.15f, 0.15f, 0.15f, 0.15f });
            //next row
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var _FT = new Paragraph("First Weight Date & Time", textNormalShort_BOLD);
            cellbody.AddElement(_FT);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            var firstwtdttime = new Paragraph(trans.FirstWtDateTime.ToString(), textNormalShort_BOLD);
            cellbody.AddElement(firstwtdttime);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var _ST = new Paragraph("Second Weight Date & Time", textNormalShort_BOLD);
            cellbody.AddElement(_ST);
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            var Secondwtdttime = new Paragraph(trans.SecondWtDateTime.ToString(), textNormalShort_BOLD);
            cellbody.AddElement(Secondwtdttime);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0.5f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0.5f;
            table6.AddCell(cellbody);

            //next row
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var _FWT = new Paragraph("First Weight", textNormalShort_BOLD);
            cellbody.AddElement(_FWT);
            cellbody.BorderWidthTop = 0f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            var firstwt = new Paragraph(trans.FirstWeight.ToString(), textNormalShort_BOLD);
            cellbody.AddElement(firstwt);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0f;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var _SWT = new Paragraph("Second Weight", textNormalShort_BOLD);
            cellbody.AddElement(_SWT);
            cellbody.BorderWidthTop = 0f;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0;
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            var secondwt = new Paragraph(trans.SecondWeight.ToString(), textNormalShort_BOLD);
            cellbody.AddElement(secondwt);
            cellbody.Padding = 5;
            cellbody.BorderWidthTop = 0;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0.5f;
            table6.AddCell(cellbody);
            //next row
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var NW = new Paragraph("Net Weight (kg) ", textNormalShort_BOLD);
            cellbody.AddElement(NW);
            cellbody.BorderWidthTop = 0;
            //cellbody.Colspan = 4;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0.5f;
            cellbody.BorderWidthRight = 0;
            
            table6.AddCell(cellbody);
            cellbody = new PdfPCell();
            cellbody.Padding = 5;
            var netwt = new Paragraph(trans.NetWeight.ToString(), textNormalShort_BOLD);
            //var varstr = NW + "  " + netwt;
            cellbody.AddElement(netwt);
            cellbody.BorderWidthTop = 0;
            cellbody.Colspan = 4;
            cellbody.BorderWidthBottom = 0;
            cellbody.BorderWidthLeft = 0;
            cellbody.BorderWidthRight = 0.5f;
            //cellbody.BorderWidthTop = 0f;
            //cellbody.BorderWidthBottom = 0;
            //cellbody.BorderWidthLeft = 0;
            //cellbody.BorderWidthRight = 0f;

            table6.AddCell(cellbody);
            
            PdfPTable table7 = new PdfPTable(2);
            table7.WidthPercentage = 100;
            table7.SetWidths(new float[] { 0.15f, 0.15f });
            cellbody = new PdfPCell();
            if (trans.FrontImage1 != null)
            {
                byte[] bytImageDataBytes = (byte[])(trans.FrontImage1).ToArray();
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bytImageDataBytes);
                //img.ScaleToFit(105F, 120F);
                cellbody.AddElement(img);


                cellbody.Padding = 5;
                cellbody.BorderWidthTop = 0.5f;
                cellbody.BorderWidthBottom = 0.5f;
                cellbody.BorderWidthLeft = 0.5f;
                cellbody.BorderWidthRight = 0.5f;
                cellbody.MinimumHeight = 100;
                table7.AddCell(cellbody);
            }
            cellbody = new PdfPCell();
            if (trans.BackImage1 != null)
            {
                byte[] bytImageDataBytes1 = (byte[])(trans.BackImage1).ToArray();
                iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(bytImageDataBytes1);
                //img1.ScaleToFit(105F, 120F);

                cellbody.AddElement(img1);
                cellbody.Padding = 5;
                cellbody.BorderWidthTop = 0.5f;
                cellbody.BorderWidthBottom = 0.5f;
                cellbody.BorderWidthLeft = 0.5f;
                cellbody.BorderWidthRight = 0.5f;
                cellbody.MinimumHeight = 100;
            }
            table7.AddCell(cellbody);
            if (!string.IsNullOrEmpty(trans.SecondWeight.ToString()))
            {
                cellbody = new PdfPCell();
                if (trans.FrontImage2 !=null)
                {
                    byte[] bytImageDataBytes2 = (byte[])(trans.FrontImage2).ToArray();
                    iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(bytImageDataBytes2);

                    //img2.ScaleToFit(105F, 120F);
                    cellbody.AddElement(img2);
                    cellbody.Padding = 5;
                    cellbody.BorderWidthTop = 0.5f;
                    cellbody.BorderWidthBottom = 0.5f;
                    cellbody.BorderWidthLeft = 0.5f;
                    cellbody.BorderWidthRight = 0.5f;
                    cellbody.MinimumHeight = 100;
                    table7.AddCell(cellbody);
                }
                cellbody = new PdfPCell();
                if (trans.BackImage2 != null)
                {
                    byte[] bytImageDataBytes3 = (byte[])(trans.BackImage2).ToArray();
                    iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance(bytImageDataBytes3);
                    //img.ScaleToFit(105F, 120F);
                    cellbody.AddElement(img3);
                    cellbody.Padding = 5;
                    cellbody.BorderWidthTop = 0.5f;
                    cellbody.BorderWidthBottom = 0.5f;
                    cellbody.BorderWidthLeft = 0.5f;
                    cellbody.BorderWidthRight = 0.5f;
                    cellbody.MinimumHeight = 100;
                }
                table7.AddCell(cellbody);
            }
            cellbody = new PdfPCell();
            if (trans.FrontImage1 != null)
            {
                byte[] bytImageDataBytes4 = (byte[])(trans.FrontImage1).ToArray();
                iTextSharp.text.Image img4 = iTextSharp.text.Image.GetInstance(bytImageDataBytes4);
                //img.ScaleToFit(105F, 120F);
                cellbody.AddElement(img4);
                cellbody.Padding = 5;
                cellbody.BorderWidthTop = 0.5f;
                cellbody.BorderWidthBottom = 0.5f;
                cellbody.BorderWidthLeft = 0.5f;
                cellbody.BorderWidthRight = 0.5f;
                cellbody.MinimumHeight = 100;
            }
            table7.AddCell(cellbody);
            if (!string.IsNullOrEmpty(trans.SecondWeight.ToString()))
            {
                cellbody = new PdfPCell();
                if (trans.FrontImage2 != null)
                {
                    byte[] bytImageDataBytes5 = (byte[])(trans.FrontImage2).ToArray();
                    iTextSharp.text.Image img5 = iTextSharp.text.Image.GetInstance(bytImageDataBytes5);
                    //img.ScaleToFit(105F, 120F);
                    cellbody.AddElement(img5);
                    cellbody.Padding = 5;
                    cellbody.BorderWidthTop = 0.5f;
                    cellbody.BorderWidthBottom = 0.5f;
                    cellbody.BorderWidthLeft = 0.5f;
                    cellbody.BorderWidthRight = 0.5f;
                    cellbody.MinimumHeight = 100;
                }
                table7.AddCell(cellbody);
            }


            //added on 24-10-2019
            PdfPTable headertable13 = new PdfPTable(2);
            headertable13.HorizontalAlignment = 0;
            headertable13.WidthPercentage = 100;
            //headertable13.SetWidths(new float[] { 4, 4, 4, 4, 4, 4 });  // then set the column's __relative__ widths
            headertable13.SetWidths(new float[] { 4, 4 });
            //headertable13.SetWidths(new float[] { 0.10f, 0.10f, 0.15f, 0.20f });
            headertable13.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable13.SpacingBefore = 10;
            headertable13.SpacingAfter = 10;



            PdfPCell triph1 = new PdfPCell(new Phrase("Authorized Signature", boldTableFont));
            triph1.HorizontalAlignment = Element.ALIGN_LEFT;
            triph1.Padding = 5;
            triph1.Border = 0;
            headertable13.AddCell(triph1);

            PdfPCell triph12 = new PdfPCell(new Phrase("WB Operator Signature", boldTableFont));
            triph12.HorizontalAlignment = Element.ALIGN_RIGHT;
            triph12.Padding = 5;
            triph12.Border = 0;
            headertable13.AddCell(triph12);

            // end 


            mydocument.Add(table1);

            mydocument.Add(table2);
            // document.Add(table9);
            //document.Add(table3);
            //document.Add(table4);
            //document.Add(table5);
            //document.Add(table6);
            mydocument.Add(headertable);
            mydocument.Add(headertable1);
            mydocument.Add(table7);

            mydocument.Add(headertable13);

            mydocument.Close();


        }
    }
}