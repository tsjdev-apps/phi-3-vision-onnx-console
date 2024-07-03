using Spectre.Console;

namespace Phi3VisionOnnxConsole.Utils;

internal static class ConsoleHelper
{
    /// <summary>
    ///     Clears the console and creates the header for the application.
    /// </summary>
    public static void ShowHeader()
    {
        AnsiConsole.Clear();

        Grid grid = new();
        grid.AddColumn();
        grid.AddRow(new FigletText("Phi-3 Vision ONNX").Centered().Color(Color.Red));
        grid.AddRow(Align.Center(new Panel("[red]Sample by Thomas Sebastian Jensen ([link]https://www.tsjdev-apps.de[/])[/]")));

        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    ///     Gets the folder path from the user.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <returns>The folder path entered by the user.</returns>
    public static string GetFolderPath(string prompt)
    {
        ShowHeader();

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .PromptStyle("white")
            .ValidationErrorMessage("[red]Invalid path[/]")
            .Validate(dictPath =>
            {
                if (!Directory.Exists(dictPath))
                {
                    return ValidationResult.Error("[red]Path does not exist[/]");
                }

                return ValidationResult.Success();
            }));
    }


    /// <summary>
    ///     Gets the file path from the user.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <returns>The file path entered by the user.</returns>
    public static string GetFilePath(string prompt)
    {
        ShowHeader();

        return AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
            .PromptStyle("white")
            .ValidationErrorMessage("[red]Invalid path[/]")
            .Validate(filePath =>
            {
                if (!File.Exists(filePath))
                {
                    return ValidationResult.Error("[red]File does not exist[/]");
                }

                if (!filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
                    !filePath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) &&
                    !filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Error("[red]File is not a picture[/]");
                }

                return ValidationResult.Success();
            }));
    }

    /// <summary>
    ///     Writes the specified text to the console.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public static void WriteToConsole(string text)
    {
        AnsiConsole.Markup($"[white]{text}[/]");
    }
}
