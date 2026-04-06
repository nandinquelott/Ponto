using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ponto.Core;

public sealed class WaitHelper
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public WaitHelper(IWebDriver driver, TimeSpan timeout)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, timeout);
    }

    public IWebElement WaitForVisible(params By[] locators)
    {
        return _wait.Until(d =>
        {
            foreach (By locator in locators)
            {
                IWebElement? element = d.FindElements(locator).FirstOrDefault(e => e.Displayed);
                if (element is not null)
                {
                    return element;
                }
            }

            return null;
        })!;
    }

    public IWebElement WaitForClickable(params By[] locators)
    {
        return _wait.Until(d =>
        {
            foreach (By locator in locators)
            {
                IWebElement? element = d.FindElements(locator).FirstOrDefault(e => e.Displayed && e.Enabled);
                if (element is not null)
                {
                    return element;
                }
            }

            return null;
        })!;
    }

    public bool WaitForUrlContains(string value)
    {
        return _wait.Until(d => d.Url.Contains(value, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsVisible(TimeSpan timeout, params By[] locators)
    {
        TimeSpan previousTimeout = _wait.Timeout;
        _wait.Timeout = timeout;

        try
        {
            return _wait.Until(d =>
            {
                foreach (By locator in locators)
                {
                    if (d.FindElements(locator).Any(e => e.Displayed))
                    {
                        return true;
                    }
                }

                return false;
            });
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
        finally
        {
            _wait.Timeout = previousTimeout;
        }
    }

    public bool WaitUntilNotVisible(TimeSpan timeout, params By[] locators)
    {
        TimeSpan previousTimeout = _wait.Timeout;
        _wait.Timeout = timeout;

        try
        {
            return _wait.Until(d =>
            {
                foreach (By locator in locators)
                {
                    if (d.FindElements(locator).Any(e => e.Displayed))
                    {
                        return false;
                    }
                }

                return true;
            });
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
        finally
        {
            _wait.Timeout = previousTimeout;
        }
    }

    public void RetryOnStale(Action action, int maxAttempts = 3)
    {
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                action();
                return;
            }
            catch (StaleElementReferenceException) when (attempt < maxAttempts)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Elemento obsoleto, nova tentativa ({attempt}/{maxAttempts}).");
            }
        }
    }

    public void WaitDocumentReady()
    {
        _wait.Until(d =>
        {
            string? readyState = ((IJavaScriptExecutor)_driver).ExecuteScript("return document.readyState")?.ToString();
            return string.Equals(readyState, "complete", StringComparison.OrdinalIgnoreCase);
        });
    }
}
