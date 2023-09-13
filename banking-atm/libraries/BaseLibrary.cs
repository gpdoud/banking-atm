using System.Text.Json;

namespace banking_atm.libraries;

public abstract class BaseLibrary {

    protected HttpClient http = new HttpClient();
    protected static string URL = "http://localhost:5210";
    protected JsonSerializerOptions options = new JsonSerializerOptions() {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

}