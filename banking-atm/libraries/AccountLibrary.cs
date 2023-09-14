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

    public async Task<IEnumerable<Account>> GetAccountsForCustomer(int id) {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{AccountUrl}/accounts/{id}");
        HttpResponseMessage res = await http.SendAsync(req);
        var json = await res.Content.ReadAsStringAsync();
        var accts = JsonSerializer.Deserialize(json, typeof(IEnumerable<Account>), options) as IEnumerable<Account>;
        return accts!;
    }

    public async Task Deposit(decimal amount, Account account) {
        account.Balance += amount;
        await PutAccount(account);
    }
    public async Task Withdraw(decimal amount, Account account) {
        account.Balance -= amount;
        await PutAccount(account);
    }
    public async Task AddAccount(Account account) {
        await PostAccount(account);
    }
    private async Task PostAccount(Account account) {
        var req = new HttpRequestMessage(HttpMethod.Post, $"{AccountUrl}");
        var json = JsonSerializer.Serialize<Account>(account, options);
        req.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var res = await http.SendAsync(req);
        if(res.StatusCode == System.Net.HttpStatusCode.Created)
            return;
        throw new Exception($"Post failed. StatusCode is {res.StatusCode}..");
    }
    private async Task PutAccount(Account account) {
        var req = new HttpRequestMessage(HttpMethod.Put, $"{AccountUrl}/{account.Id}");
        var json = JsonSerializer.Serialize<Account>(account, options);
        req.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var res = await http.SendAsync(req);
        if(res.StatusCode == System.Net.HttpStatusCode.NoContent)
            return;
        throw new Exception($"Put failed. StatusCode is {res.StatusCode}..");
    }
    public async Task DeleteAccount(int id) {
        var req = new HttpRequestMessage(HttpMethod.Delete, $"{AccountUrl}/{id}");
        var res = await http.SendAsync(req);
        if(res.StatusCode == System.Net.HttpStatusCode.NoContent)
            return;
        throw new Exception($"Delete failed. StatusCode is {res.StatusCode}..");
    }
}
