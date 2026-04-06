using OpenQA.Selenium;
using Ponto.Config;
using Ponto.Core;

namespace Ponto.Pages;

public sealed class RegistrarPontoPage
{
    private readonly IWebDriver _driver;
    private readonly WaitHelper _wait;
    private readonly Settings _settings;

    public RegistrarPontoPage(IWebDriver driver, WaitHelper wait, Settings settings)
    {
        _driver = driver;
        _wait = wait;
        _settings = settings;
    }

    public void Navigate()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Navegando para registrar ponto...");
        _driver.Navigate().GoToUrl(_settings.RegistrarPontoUrl);
        _wait.WaitDocumentReady();

        _wait.WaitForVisible(
            By.CssSelector("vrgente-time-card-register-point pm-button button"),
            By.XPath("//pm-button//button[.//span[contains(normalize-space(), 'Bater')] or contains(normalize-space(), 'Bater')]")
        );
    }

    public void ConfirmarEnderecoSeExistir()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Verificando confirmação de endereço...");

        bool existeConfirmacao = _wait.IsVisible(
            TimeSpan.FromSeconds(5),
            By.CssSelector("vrgente-address-time-card-register small"),
            By.XPath("//*[contains(translate(normalize-space(.), 'ENDERECO', 'endereco'), 'endereco') and (self::small or self::span or self::p)]"));

        if (!existeConfirmacao)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Confirmação de endereço não exibida.");
            return;
        }

        _wait.RetryOnStale(() =>
        {
            IWebElement confirmacao = _wait.WaitForClickable(
                By.CssSelector("vrgente-address-time-card-register small"),
                By.XPath("//*[contains(translate(normalize-space(.), 'ENDERECO', 'endereco'), 'endereco') and (self::small or self::span or self::p)]"));

            confirmacao.Click();
        });

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Endereço confirmado.");
    }

    public void BaterPonto()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Clicando em bater ponto...");
        Thread.Sleep(3000);
        try
        {
            _wait.RetryOnStale(() =>
            {
                IWebElement? botaoVisible = _wait.WaitForClickable(
                    By.XPath("/html/body/app-mfe-remote/app-side-nav-outer-toolbar/dx-drawer/div/div[2]/dx-scroll-view/div[1]/div/div[1]/div[2]/div[1]/time-card-register/div/div[2]/div[2]/vrgente-time-card-register-point/pm-card/div/div[2]/div[1]/div[2]/div/pm-button/button"));
                botaoVisible.Click();
            });

            Thread.Sleep(3000);

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Registro de ponto executado.");
            return;
        }
        catch (ElementClickInterceptedException)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Clique interceptado por overlay/toast...");
        }


        throw new WebDriverException("Não foi possível clicar em 'Bater ponto' devido a interceptação de clique.");
    }
}
