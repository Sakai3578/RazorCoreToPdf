using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace RazorCoreToPdf {
    internal static class RenderingAsStringAsync {
        internal static async Task<string> Run(
            string viewName,
            object model,
            ControllerContext controllerContext,
            bool isPartial = false
        ) {

            var actionContext = controllerContext as ActionContext;
            var razorViewEngine = controllerContext.HttpContext.RequestServices.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = controllerContext.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;





            using var stringWriter = new StringWriter();
            var viewResult = razorViewEngine!.FindView(actionContext, viewName, !isPartial);

            if (viewResult?.View == null) {
                throw new ArgumentException($"Fatal: {viewName} does not match any available view.");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, tempDataProvider!),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }
}