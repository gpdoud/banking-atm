using cli;

using models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace banking_atm.libraries;

public class MenuLibrary {

    public Customer? customer { get; set; }

    UserLibrary userlib = new UserLibrary();
    AccountLibrary acctlib = new AccountLibrary();
    TransactionLibrary translib = new TransactionLibrary();
    CliLibrary cli = new CliLibrary();

    public async Task CloseAccount() {
        var account = await SelectAccount();
        Console.WriteLine($"Balance is {account.Balance:C}");
        var holdBalance = account.Balance;
        await acctlib.DeleteAccount(account.Id);
        if(holdBalance == 0) return;
        Console.WriteLine("Select account to deposit balance.");
        var account2 = await SelectAccount();
        await acctlib.Deposit(holdBalance, account2);
        cli.PromptLine($"Deposited {holdBalance:C} into {account2.Description}.");
        await translib.Log("D", account2.Id, $"Deposited {holdBalance:c}");
    }

    public async Task AddAccount() {

        var newAcct = new Account();
        newAcct.CustomerId = customer!.Id;
        var type = cli!.Ask("Type of account (C-Checking, S-Savings): ") ?? "C";
        newAcct.Type = type.ToUpper().Substring(0, 1) == "S"
            ? Account.ACCOUNT_SAVINGS
            : Account.ACCOUNT_CHECKING;
        newAcct.Description = cli!.Ask("Description: ") ?? "New Account";
        await acctlib.AddAccount(newAcct);
        cli.PromptLine("Account added.");
    }
    public async Task<Account> SelectAccount() {
        var accounts = await acctlib!.GetAccountsForCustomer(customer.Id);
        cli.PromptLine("Select an account number:\n");
        foreach(var a in accounts) {
            cli.PromptLine($"({a.Id}) {a.Description} [{a.Type}]");
        }
        var ans = cli.Ask("\nEnter account number: ");
        var acctId = Convert.ToInt32(ans);
        var acct = accounts.Single(x => x.Id == acctId);
        Console.WriteLine($"Selected account: {acct.Description}");
        return acct;
    }
    public async Task Balance() {
        var accounts = await acctlib!.GetAccountsForCustomer(customer.Id);
        foreach(var a in accounts) {
            Console.WriteLine(a);
        }

    }
    public async Task Deposit() {
        var account = await SelectAccount();
        var ans = cli.Ask("Amount to deposit? ");
        var amount = Convert.ToInt32(ans);
        await acctlib.Deposit(amount, account);
        cli.PromptLine($"Deposited {amount:C} into {account.Description}.");
        await translib.Log("D", account.Id, $"Deposited {amount:c}");
    }
    public async Task Withdraw() {
        var account = await SelectAccount();
        var ans = cli.Ask("Amount to withdrawn? ");
        var amount = Convert.ToInt32(ans);
        await acctlib.Withdraw(amount, account);
        cli.PromptLine($"Withdraw {amount:C} from {account.Description}.");
        await translib.Log("W", account.Id, $"Withdrew {amount:c}");
    }
    public async Task Transactions() {
        var account = await SelectAccount();
        Console.WriteLine($"Transactions for account {account.Description}");
        if(account.Transactions is null) return;
        foreach(var t in account.Transactions) {
            Console.WriteLine(t);
        }
    }
    public async Task<Customer?> Login() {

        for(var i = 1; i <= 3; i++) {
            var strcard = cli!.Ask("Enter a card code: ");
            var strpin = cli.Ask("Enter a pin code: ");

            var card = Convert.ToInt32(strcard);
            var pin = Convert.ToInt32(strpin);

            var cust = await userlib.Login(card, pin);
            if(cust!.Id == 0)
                cli.PromptLine($"Login failed. {3 - i} trys remaining.");
            else
                return cust;
        }
        return null;
    }

}
