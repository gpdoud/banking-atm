using cli;

using models;
using banking_atm.libraries;
using System.Security.Principal;

var userlib = new UserLibrary();
var acctlib = new AccountLibrary();
var translib = new TransactionLibrary();
var cli = new CliLibrary();

Console.WriteLine("MAX BOOT CAMP ATM");

var customer = await Login();
if(customer is null) return;

while(true) {

    Console.WriteLine();
    cli.PromptLine("(A) Add Account");
    cli.PromptLine("(B) Balance");
    cli.PromptLine("(C) Close Account");
    cli.PromptLine("(D) Deposit");
    cli.PromptLine("(T) Transactions");
    cli.PromptLine("(W) Withdraw");
    cli.PromptLine("(X) Quit");
    var ans = cli.Ask("Enter character: ");
    
    switch(ans.ToLower()) {
        case "a":
            cli.PromptLine("\nAdd Account");
            await AddAccount();
            break;
        case "b":
            cli.PromptLine("\nBalance");
            await Balance();
            break;
        case "c":
            cli.PromptLine("\nClose Account");
            break;
        case "d": 
            cli.PromptLine("\nDeposit");
            await Deposit();
            break;
        case "t": 
            cli.PromptLine("\nTransactions");
            await Transactions();
            break;
        case "w": 
            cli.PromptLine("\nWithdraw"); 
            await Withdraw();
            break;
        case "x": 
            cli.PromptLine("\nQuit");  
            return;
    }

}

/******************************************************/
async Task AddAccount() {

    var newAcct = new Account();
    newAcct.CustomerId = customer!.Id;
    var type = cli!.Ask("Type of account (C-Checking, S-Savings): ") ?? "C";
    newAcct.Type = type.ToUpper().Substring(0,1) == "S" 
        ? Account.ACCOUNT_SAVINGS 
        : Account.ACCOUNT_CHECKING;
    newAcct.Description = cli!.Ask("Description: ") ?? "New Account";
    await acctlib.AddAccount(newAcct);
    cli.PromptLine("Account added.");
}
async Task<Account> SelectAccount() {
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
async Task Balance() {
    var account = await SelectAccount();
    Console.WriteLine($"Balance is {account.Balance:C}");

}
async Task Deposit() {
    var account = await SelectAccount();
    var ans = cli.Ask("Amount to deposit? ");
    var amount = Convert.ToInt32(ans);
    await acctlib.Deposit(amount, account);
    cli.PromptLine($"Deposited {amount:C} into {account.Description}.");
    await translib.Log("D", account.Id, $"Deposited {amount:c}");
}
async Task Withdraw() {
    var account = await SelectAccount();
    var ans = cli.Ask("Amount to withdrawn? ");
    var amount = Convert.ToInt32(ans);
    await acctlib.Withdraw(amount, account);
    cli.PromptLine($"Withdraw {amount:C} from {account.Description}.");
    await translib.Log("W", account.Id, $"Withdrew {amount:c}");
}
async Task Transactions() {
    var account = await SelectAccount();
    Console.WriteLine($"Transactions for account {account.Description}");
    if(account.Transactions is null) return;
    foreach(var t in account.Transactions) {
        Console.WriteLine(t);
    }
}
async Task<Customer?> Login() {

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
