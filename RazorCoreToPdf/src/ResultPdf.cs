using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.IO;

namespace RazorCoreToPdf {

    public static class ResultPdf {
        public static async Task<FileContentResult> RazorToPdf(
            this Controller controller,
            object model,
            bool isPartial = false
        ) {
            var viewAsString = await RenderingAsStringAsync.Run(model, controller.ControllerContext, isPartial);
            var viewAsPdfVyteArray = BuildPdfDocument(viewAsString);
            return new FileContentResult(viewAsPdfVyteArray, "application/pdf");
        }

        /// <summary>
        /// Build Pdf Text
        /// </summary>
        /// <param name="html">Html document as string</param>
        /// <returns></returns>
        static byte[] BuildPdfDocument(string html) {
            using MemoryStream ms = new MemoryStream();

            Dictionary<string, object> dicProvider = new Dictionary<string, object>();

            TextReader reader = new StringReader(html);

            Document document = new Document(PageSize.A4, 30, 30, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            HTMLWorker worker = new HTMLWorker(document);

            document.Open();
            worker.StartDocument();

            worker.Parse(reader);

            worker.EndDocument();
            worker.Close();
            document.Close();

            byte[] result = ms.ToArray();

            return result;
        }
    }
}