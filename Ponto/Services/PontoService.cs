using OpenQA.Selenium;
using Ponto.Config;
using Ponto.Core;
using Ponto.Pages;

namespace Ponto.Services;

public sealed class PontoService
{
    private readonly IWebDriver _driver;
    private readonly WaitHelper _wait;
    private readonly Settings _settings;
    private readonly LoginPage _loginPage;
    private readonly RegistrarPontoPage _registrarPontoPage;

    public PontoService(IWebDriver driver, WaitHelper wait, Settings settings)
    {
        _driver = driver;
        _wait = wait;
        _settings = settings;
        _loginPage = new LoginPage(driver, wait, settings);
        _registrarPontoPage = new RegistrarPontoPage(driver, wait, settings);
    }

    public void ExecutarRegistro()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Iniciando fluxo de registro de ponto...");

        _loginPage.Open();
        _loginPage.Login(_settings.Usuario, _settings.Senha);

        _wait.WaitForUrlContains("registrar-ponto");

        _registrarPontoPage.Navigate();
        _registrarPontoPage.ConfirmarEnderecoSeExistir();
        _registrarPontoPage.BaterPonto();

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Fluxo finalizado.");
    }
}
