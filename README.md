# RazorCoreToPdf

This is the code for displaying the Razor-rendered results in PDF for ASP.NET Core MVC project.

This project was inspired by "RazorPDF". However, I felt difficult for RazorPDF to work in the ASP.NET Core MVC environment, so I built it again here.

(Please note, this do not support the application of CSS in Razor yet.)

Process overview

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

# Appendix: Process overview
![razor-to-pdf-process](https://user-images.githubusercontent.com/36692455/202465458-1602f447-f259-4408-b65b-76d33123c12d.png)
![download-complete](https://user-images.githubusercontent.com/36692455/202465889-018eda91-77ab-47eb-b86b-b73475dcbcc6.png)
![download-result](https://user-images.githubusercontent.com/36692455/202465774-d8f1edd4-1d9f-43be-a528-e806b2dc6aba.png)
