# RazorCoreToPdf

This is the code for displaying the Razor-rendered results in PDF for ASP.NET Core MVC project.

This project was inspired by "RazorPDF". However, I felt difficult for RazorPDF to work in the ASP.NET Core MVC environment, so I built it again here.

Please note, this do not support the application of CSS in Razor yet.

# How to use

1. Import RazorCoreToPdf.csproj to your sln and add dependence.

2. Add using statement to your sourse code like following.

```
using RazorCoreToPdf;
```

3. Call RazorToPdf method at your controller like following. You can 

```
public async Task<IActionResult> PdfTestView() {
    return await this.RazorToPdf();
}
```

Or you can also use @model in razor binding with RazorToPdf method argument.

```
public async Task<IActionResult> PdfTestView() {
    return await this.RazorToPdf(DateTime.Now);
}
```

# Acknowledgments

This source code depends on iTextSharp-LGPL-Core and uses it for the PDF binary generation process.

# License

RazorCoreToPdf is licensed under MIT License.

https://opensource.org/licenses/MIT

