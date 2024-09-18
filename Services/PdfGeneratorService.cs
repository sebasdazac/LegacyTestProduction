using Microsoft.Playwright;

public class PdfGeneratorService
{
    public async Task<byte[]> GeneratePdfAsync(string url)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // Ejecutar en modo headless
        });
        var page = await browser.NewPageAsync();

       

        await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

        // Configura las opciones de PDF según tus necesidades
        var pdfBytes = await page.PdfAsync(new PagePdfOptions
        {
            Format = "A4", 
            PrintBackground = true,
  
        });

        return pdfBytes;
    }
}
