namespace cli;

public class CliLibrary {

    public void Prompt(string prompt) {
        Console.Write($"{prompt}");
    }
    public void PromptLine(string prompt) {
        Prompt(prompt);
        Console.WriteLine();
    }
    public string? Respond() {
        var res = Console.ReadLine();
        return res;
    }
    public string? Ask(string prompt) {
        Prompt(prompt);
        return Respond();
    }
}