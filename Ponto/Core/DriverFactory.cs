using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Ponto.Core;

public static class DriverFactory
{
    public static IWebDriver CreateChromeDriver(bool headless = false)
    {
        ChromeOptions options = new();

        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
        }
        else
        {
            options.AddArgument("--start-maximized");
        }

        return new ChromeDriver(options);
    }
}
