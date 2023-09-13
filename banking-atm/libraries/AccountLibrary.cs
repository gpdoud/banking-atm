using models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using static System.Net.WebRequestMethods;

namespace banking_atm.libraries;

public class AccountLibrary : BaseLibrary {

    private static string AccountUrl = $"{URL}/api/accounts";

    public async Task<IEnumerable<Account>> GetAccounts() {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{AccountUrl}");
        HttpResponseMessage res = await http.SendAsync(req);
        var json = await res.Content.ReadAsStringAsync();
        var accts = JsonSerializer.Deserialize(json, typeof(IEnumerable<Account>), options) as IEnumerable<Account>;
        return accts;
    }

    public async Task Deposit(decimal amount, Account account) {
        account.Balance += amount;
        var req = new HttpRequestMessage(HttpMethod.Put, $"{AccountUrl}/{account.Id}");
        var json = JsonSerializer.Serialize<Account>(account, options);
        req.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var res = await http.SendAsync(req);
        if(res.StatusCode == System.Net.HttpStatusCode.NoContent)
            return;
        throw new Exception($"Deposit of {amount:C} failed. StatusCode is {res.StatusCode}..");
    }
}
