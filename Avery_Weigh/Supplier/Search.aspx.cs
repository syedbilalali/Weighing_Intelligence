using Avery_Weigh.Repository;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;

namespace Avery_Weigh.Supplier
{
    public partial class Search : System.Web.UI.Page
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SupplierRepository _supplierrepo = new SupplierRepository();
        SystemLogRepository logRepo = new SystemLogRepository();
        protected void Page_Load(object sender, EventArgs e)
        {

        }          
    }
}