using CrystalDecisions.CrystalReports.Engine;
using Distributor.Dao._DbContext;
using Distributor.POCO;
using Distributor.Service.Interfaces;
using Management_Distributor.ExceptionHandler;
using Management_Distributor.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Management_Distributor.Controllers
{
    [Authorize(Roles = "staff")]
    public class ReportController : Controller
    {
        IOrderService orderService = null;
        IOrderDetailService detailService = null;
        IEmployeeService employeeService = null;
        IDistributorService distributorService = null;
        public ReportController(IOrderService _orderService, IOrderDetailService _detailService, IEmployeeService _employeeService, IDistributorService _distributorService)
        {
            this.orderService = _orderService;
            this.detailService = _detailService;
            this.employeeService = _employeeService;
            this.distributorService = _distributorService;
        }
        public ActionResult PrintInvoice(int? orderId)
        {
            if (orderId == null)
                return null;
            int _orderId = Convert.ToInt32(orderId);
            Order order = orderService.FindById(_orderId);
            if (order == null)
                return null;

            // get distributor info
            _Distributor distributor = distributorService.GetOne(order.DistributorId);

            // get cashier info
            Employee employee = employeeService.FindById(order.EmployeeId);

            // get order detail info
            //List<OrderDetail> detail = detailService.FindByOrderId(order.OrderId);
            

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/InvoiceCrystal.rpt")));


            //CrystalReportViewer crysReportViewer = new CrystalReportViewer();

            //InvoiceCrystal report = new InvoiceCrystal();

            // set list order detail  info


            using (ManagementDistributorDbContext dbContext = new ManagementDistributorDbContext())
            {
               
                rd.SetDataSource(dbContext.OrderDetails.Select(od => new
                {
                    od.OrderId,
                    od.OrderDetailId,
                    od.ActualQuantity,
                    od.Price,
                    od.ProductId,
                    od.Product.ProductName,

                }
                ).ToList());
            }
            //rd.SetDataSource(detail);

            // set order generic info
            //rd.SetParameterValue("OrderDate", order.OrderDate);
            rd.SetParameterValue("OrderId", order.OrderId);

            rd.SetParameterValue("OrderToTalAmount", order.ToTalAmount);

            rd.SetParameterValue("InvoiceDate", DateTime.Now);
            rd.SetParameterValue("InvoiceId", 1);
            //rd.SetParameterValue("DueDate", DateTime.Now); // 

            // Set Distributor Info
            rd.SetParameterValue("DistributorName", distributor.DistributorName);
            rd.SetParameterValue("DistributorAddress", distributor.DistributorAddress);

            // set Cashier info
            rd.SetParameterValue("EmployeeName", employee.EmpName);


            //crysReportViewer.ReporSource = report;

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);


                return File(stream, "application/pdf", "Invoice.pdf");
            }
            catch(Exception ex)
            {
                string path = @"C:/PrintInvoice.Exception.txt";
                ExceptionProofer.LogToFile(path, ex);
                return null;
            }

           
        }

        
       
    }
}