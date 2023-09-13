using System.Text.Json;
using models;

namespace banking_atm.libraries;

public class UserLibrary : BaseLibrary {

    public async Task<Customer?> Login(int card, int pin) {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{URL}/api/customers/{card}/{pin}");
        HttpResponseMessage res = await http.SendAsync(req);
        var json = await res.Content.ReadAsStringAsync();
        var cust = JsonSerializer.Deserialize(json, typeof(Customer), options) as Customer;
        return cust;
    }
}