using cli;

using models;
using banking_atm.libraries;
using System.Security.Principal;

var menulib = new MenuLibrary();

Console.WriteLine("MAX BOOT CAMP ATM");

menulib.customer = await menulib.Login();
if(menulib.customer is null) return;

CliLibrary cli = new CliLibrary();

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
            await menulib.AddAccount();
            break;
        case "b":
            cli.PromptLine("\nBalance");
            await menulib.Balance();
            break;
        case "c":
            cli.PromptLine("\nClose Account");
            await menulib.CloseAccount();
            break;
        case "d": 
            cli.PromptLine("\nDeposit");
            await menulib.Deposit();
            break;
        case "t": 
            cli.PromptLine("\nTransactions");
            await menulib.Transactions();
            break;
        case "w": 
            cli.PromptLine("\nWithdraw"); 
            await menulib.Withdraw();
            break;
        case "x": 
            cli.PromptLine("\nQuit");  
            return;
    }

}

/******************************************************/

