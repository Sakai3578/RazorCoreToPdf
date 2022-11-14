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
        public static async Task<FileContentResult> Download(
            this Controller controller,
            //string viewName,
            object model,
            bool isPartial = false
        ) {
            var viewAsString = await RenderingAsStringAsync.Run(model, controller.ControllerContext, isPartial);
            var viewAsPdfVyteArray = _Main(viewAsString);
            return new FileContentResult(viewAsPdfVyteArray, "application/pdf");
        }


        static byte[] _Main(string html) {
            using System.IO.MemoryStream ms = new System.IO.MemoryStream();

            Dictionary<string, object> dicProvider = new Dictionary<string, object>();
            //dicProvider.Add(HTMLWorker.FONT_PROVIDER, new NewFontProvider());

            //string outfilepath = @"c:\tmp\test-html.pdf";
            TextReader reader = new StringReader(html);

            // step 1: creation of a document-object
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);

            // step 2:
            // we create a writer that listens to the document
            // and directs a XML-stream to a file
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            // step 3: we create a worker parse the document
            HTMLWorker worker = new HTMLWorker(document);
            //worker.SetProviders(dicProvider);

            // step 4: we open document and start the worker on the document
            document.Open();
            worker.StartDocument();

            // step 5: parse the html into the document
            worker.Parse(reader);

            // step 6: close the document and the worker
            worker.EndDocument();
            worker.Close();
            document.Close();


            byte[] result = ms.ToArray();

            return result;
        }
    }
}