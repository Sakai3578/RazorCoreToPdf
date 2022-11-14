/*
 * Copyright 2022 by Yasuyuki Sakai（堺 康行）.
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 *
 * The inherits function is:
 * ISC license | https://github.com/isaacs/inherits/blob/master/LICENSE
 */

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace RazorCoreToPdf {

    public static class RazorCoreToPdfMain {
        public static async Task<FileContentResult> RazorToPdf(
            this Controller controller,
            object? model = null,
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