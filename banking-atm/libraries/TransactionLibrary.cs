using models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace banking_atm.libraries;

public class TransactionLibrary : BaseLibrary {

    private static string TransactionUrl = $"{URL}/api/transactions";

    public async Task Log(string transType, int accountId, string message) {
        var transaction = new Transaction() {
            Description = message,
            TransactionType = transType,
            AccountId = accountId
        };
        var req = new HttpRequestMessage(HttpMethod.Post, $"{TransactionUrl}");
        var json = JsonSerializer.Serialize<Transaction>(transaction, options);
        req.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var res = await http.SendAsync(req);
        if(res.StatusCode == System.Net.HttpStatusCode.Created)
            return;
        throw new Exception($"Post failed. StatusCode is {res.StatusCode}..");
    }
}
