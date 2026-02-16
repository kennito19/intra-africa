using API_Gateway.Common.orders;
using BarcodeStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using API_Gateway.Models.Dto;
using PdfSharp.Pdf.Filters;
using PdfSharpCore.Pdf.IO;
using API_Gateway.Helper;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]

    public class GenerateInvoiceController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        

        public GenerateInvoiceController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor )
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
        }
        
        [HttpPost("GenerateInvoice")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public async Task<ActionResult> GenerateInvoice(string Packageid)
        {
            List<byte[]> pdfList = new List<byte[]>();

            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GerOrderInvoiceDetails getorder = new GerOrderInvoiceDetails(_configuration, _httpContext, userID);
            var res = getorder.GetInvoice(Packageid);

            var data = new PdfDocument();
            string htmlContent = getorder.InvoiceHtmlContent(res);

            PdfGenerator.AddPdfPages(data, htmlContent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                data.Save(ms);
                //response = ms.ToArray();
                pdfList.Add(ms.ToArray());
            }

            byte[] combinedPdf = CombinePdfs(pdfList);

            string fileName = "OrderInvoice_" + res.InvoiceNo + ".pdf";

            return File(combinedPdf, "application/pdf", fileName);

            //return File(response, "application/pdf", fileName);

        }

        [HttpPost("GenerateShippingLabel")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public async Task<ActionResult> GenerateShippingLabel(string Packageid)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GerOrderInvoiceDetails getorder = new GerOrderInvoiceDetails(_configuration, _httpContext, userID);
            List<ShippingLabelDto> ShippingLabelList = getorder.GetShippingLabel(Packageid);
            List<byte[]> pdfList = new List<byte[]>();

            int count = 0;
            foreach (var res in ShippingLabelList)
            {
                var data = new PdfDocument();
                count = count + 1;

                string htmlContent = getorder.ShippingHtmlContent(res, count);
                PdfGenerator.AddPdfPages(data, htmlContent, PageSize.A6);

                // Save the PDF data for the current label to the list
                using (MemoryStream ms = new MemoryStream())
                {
                    data.Save(ms);
                    pdfList.Add(ms.ToArray());
                }
            }
            // Combine the individual PDFs into a single PDF file
            byte[] combinedPdf = CombinePdfs(pdfList);

            // Generate a unique filename (e.g., using the AWB number from the first label)
            string fileName = "OrderShippingLabels_" + ShippingLabelList[0].AwbNo + ".pdf";

            // Return the combined PDF file as a response
            return File(combinedPdf, "application/pdf", fileName);

            //byte[]? response = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    data.Save(ms);
            //    response = ms.ToArray();
            //}
            //string fileName = "OrderShippingLable_" + ShippingLabelList[0].AwbNo + ".pdf";
            //return File(response, "application/pdf", fileName);

        }


        [HttpPost("GenerateCustomerInvoice")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public async Task<ActionResult> GenerateCustomerInvoice(string orderNo)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            GerOrderInvoiceDetails getorder = new GerOrderInvoiceDetails(_configuration, _httpContext, userID);
            List<InvoiceDetails> InvoiceDetailsList = getorder.GetInvoiceByOrderNo(orderNo);
            if (InvoiceDetailsList.Count() > 0)
            {

                List<byte[]> pdfList = new List<byte[]>();

                int count = 0;
                foreach (var res in InvoiceDetailsList)
                {
                    var data = new PdfDocument();
                    count = count + 1;

                    string htmlContent = getorder.InvoiceHtmlContent(res);
                    PdfGenerator.AddPdfPages(data, htmlContent, PageSize.A4);

                    // Save the PDF data for the current label to the list
                    using (MemoryStream ms = new MemoryStream())
                    {
                        data.Save(ms);
                        pdfList.Add(ms.ToArray());
                    }
                }
                // Combine the individual PDFs into a single PDF file
                byte[] combinedPdf = CombinePdfs(pdfList);

                // Generate a unique filename (e.g., using the AWB number from the first label)
                string fileName = "OrderInvoice_" + InvoiceDetailsList[0].OrderNo + ".pdf";

                // Return the combined PDF file as a response
                return File(combinedPdf, "application/pdf", fileName);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
                return Ok(baseResponse);
            }
        }


        [NonAction]
        private byte[] CombinePdfs(List<byte[]> pdfList)
        {
            // Combine individual PDFs into a single PDF
            var combinedData = new PdfDocument();
            foreach (var pdfData in pdfList)
            {
                var individualPdf = PdfReader.Open(new MemoryStream(pdfData), PdfDocumentOpenMode.Import);
                foreach (var page in individualPdf.Pages)
                {
                    var importedPage = combinedData.AddPage(page);
                    importedPage.Orientation = page.Orientation;
                }
            }

            // Save the combined PDF data
            using (MemoryStream ms = new MemoryStream())
            {
                combinedData.Save(ms);
                return ms.ToArray();
            }
        }

        //public byte[] GeneratePdfWithBarcode(string barcodeText)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        using (PdfDocument document = new PdfDocument())
        //        {
        //            PdfPage page = document.AddPage();
        //            page.Size = PdfSharp.PageSize.A4;

        //            using (XGraphics gfx = XGraphics.FromPdfPage(page))
        //            {
        //                // Create a BarcodeWriter from BarcodeLib
        //                Barcode barcodeWriter = new Barcode();
        //                barcodeWriter.IncludeLabel = true;

        //                // Create a SKTypeface for the label font
        //                SKTypeface typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

        //                // Create a SKFont for the label font
        //                SKFont labelFont = new SKFont(typeface, 10);

        //                // Set the SKFont to the LabelFont property
        //                barcodeWriter.LabelFont = labelFont;

        //                // Generate the barcode as a SKImage
        //                SKImage barcodeImage = barcodeWriter.Encode(BarcodeStandard.Type.Code128, barcodeText);

        //                // Convert the SKImage to SKData
        //                SKData barcodeData = barcodeImage.Encode();

        //                // Save the SKData to a temporary file
        //                string tempFilePath = Path.GetTempFileName();
        //                using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create))
        //                {
        //                    barcodeData.SaveTo(fileStream);
        //                }

        //                // Draw the barcode from the temporary file to the PDF page
        //                XImage barcodeXImage = XImage.FromFile(tempFilePath);
        //                gfx.DrawImage(barcodeXImage, 15, 40);

        //                // Delete the temporary file
        //                System.IO.File.Delete(tempFilePath);
        //            }

        //            document.Save(stream);
        //        }

        //        return stream.ToArray();
        //    }
        //}
    }
}

