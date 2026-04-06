namespace Ponto.Config;

public class Settings
{
    public string LoginUrl { get; init; } = "https://app2.pontomais.com.br/login";
    public string RegistrarPontoUrl { get; init; } = "https://app2.pontomais.com.br/registrar-ponto";
    public string Usuario { get; init; } = "";
    public string Senha { get; init; } = "";
    public int TimeoutSeconds { get; init; } = 30;
    public bool Headless { get; init; } = false;

    public static Settings Load() => new();
}
