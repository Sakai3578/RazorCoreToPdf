/*
 * Copyright 2022 by Yasuyuki Sakai（堺 康行）.
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 *
 * The inherits function is:
 * ISC license | https://github.com/isaacs/inherits/blob/master/LICENSE
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace RazorCoreToPdf {
    internal static class RenderingAsStringAsync {
        internal static async Task<string> Run(
            object? model,
            ControllerContext controllerContext,
            bool isPartial = false
        ) {

            var actionContext = controllerContext as ActionContext;
            var razorViewEngine = controllerContext.HttpContext.RequestServices.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = controllerContext.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            var viewName = GetActionName(actionContext);

            using var sw = new StringWriter();
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
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return sw.ToString();
        }

        /// <summary>
        /// referenced: Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor.GetActionName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static string? GetActionName(ActionContext context) {

            var ActionNameKey = "action";
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.RouteData.Values.TryGetValue(ActionNameKey, out var routeValue)) {
                return null;
            }

            var actionDescriptor = context.ActionDescriptor;
            string? normalizedValue = null;
            if (actionDescriptor.RouteValues.TryGetValue(ActionNameKey, out var value) &&
                !string.IsNullOrEmpty(value)) {
                normalizedValue = value;
            }

            var stringRouteValue = Convert.ToString(routeValue, CultureInfo.InvariantCulture);
            if (string.Equals(normalizedValue, stringRouteValue, StringComparison.OrdinalIgnoreCase)) {
                return normalizedValue;
            }

            return stringRouteValue;
        }
    }
}