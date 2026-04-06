using OpenQA.Selenium;
using Ponto.Config;
using Ponto.Core;
using Ponto.Services;

Settings settings = Settings.Load();
IWebDriver? driver = null;

TextWriter originalOut = Console.Out;
TextWriter originalError = Console.Error;
StreamWriter? fileWriter = null;

try
{
    string logsDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
    Directory.CreateDirectory(logsDirectory);

    string logFilePath = Path.Combine(logsDirectory, $"historico-{DateTime.Now:yyyyMMdd-HHmmss}.txt");
    fileWriter = new StreamWriter(logFilePath, append: false) { AutoFlush = true };

    Console.SetOut(new TeeTextWriter(originalOut, fileWriter));
    Console.SetError(new TeeTextWriter(originalError, fileWriter));

    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Inicializando robô...");
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Histórico salvo em: {logFilePath}");

    driver = DriverFactory.CreateChromeDriver();
    WaitHelper waitHelper = new(driver, TimeSpan.FromSeconds(settings.TimeoutSeconds));

    PontoService pontoService = new(driver, waitHelper, settings);
    pontoService.ExecutarRegistro();

    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Robô concluído com sucesso.");
}
catch (WebDriverTimeoutException ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERRO: Tempo limite excedido. {ex.Message}");
}
catch (NoSuchElementException ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERRO: Elemento não encontrado. {ex.Message}");
}
catch (WebDriverException ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERRO: Falha do Selenium WebDriver. {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ERRO: Falha inesperada. {ex.Message}");
}
finally
{
    driver?.Quit();
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Navegador finalizado.");

    Console.SetOut(originalOut);
    Console.SetError(originalError);
    fileWriter?.Dispose();
}