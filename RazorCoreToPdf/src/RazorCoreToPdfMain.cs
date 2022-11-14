/*
 * Copyright 2022 by Yasuyuki Sakai（堺 康行）.
 * 
 * The contents of this file may be used under the terms of the LGPL license 
 * (the "GNU LIBRARY GENERAL PUBLIC LICENSE")
 * https://www.gnu.org/licenses/old-licenses/lgpl-2.0-standalone.html
 * 
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.
 * 
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