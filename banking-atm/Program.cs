using cli;

using models;
using banking_atm.libraries;

var userlib = new UserLibrary();
var acctlib = new AccountLibrary();
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
            break;
        case "b":
            cli.PromptLine("\nBalance");
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
            break;
        case "w": 
            cli.PromptLine("\nWithdraw"); 
            break;
        case "x": 
            cli.PromptLine("\nQuit");  
            return;
    }

}

/******************************************************/
async Task<Account> SelectAccount() {
    var accounts = await acctlib!.GetAccounts();
    cli.PromptLine("Select an account number:");
    foreach(var acct in accounts) {
        cli.PromptLine($"({acct.Id}) {acct.Description}");
    }
    var ans = cli.Respond();
    var acctId = Convert.ToInt32(ans);
    return accounts.Single(x => x.Id == acctId);
}
async Task Deposit() {
    var account = await SelectAccount();
    Console.WriteLine($"Selected account: {account.Description}");
    var ans = cli.Ask("Amount to deposit? ");
    var amount = Convert.ToInt32(ans);
    await acctlib.Deposit(amount, account);
    cli.PromptLine($"Deposited {amount:C} into {account.Description}.");
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
