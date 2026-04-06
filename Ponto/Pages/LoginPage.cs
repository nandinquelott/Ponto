using OpenQA.Selenium;
using Ponto.Config;
using Ponto.Core;

namespace Ponto.Pages;

public sealed class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly WaitHelper _wait;
    private readonly Settings _settings;

    public LoginPage(IWebDriver driver, WaitHelper wait, Settings settings)
    {
        _driver = driver;
        _wait = wait;
        _settings = settings;
    }

    public void Open()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Abrindo login...");
        _driver.Navigate().GoToUrl(_settings.LoginUrl);
    }

    public void Login(string usuario, string senha)
    {
        IWebElement inputLogin = _wait.WaitForVisible(
            By.CssSelector("input[data-testid='login-input']"),
            By.CssSelector("input[type='text']"));

        IWebElement inputSenha = _wait.WaitForVisible(
            By.CssSelector("input[type='password']"),
            By.XPath("//pm-password//input"));

        inputLogin.Clear();
        inputLogin.SendKeys(usuario);

        inputSenha.Clear();
        inputSenha.SendKeys(senha);

        _wait.RetryOnStale(() =>
        {
            IWebElement botaoEntrar = _wait.WaitForClickable(
                By.XPath("//button[.//span[normalize-space()='Entrar'] or normalize-space()='Entrar']"));

            botaoEntrar.Click();
        });

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Login enviado.");
    }
}
