using Microsoft.ML.OnnxRuntimeGenAI;
using Phi3VisionOnnxConsole.Utils;

// Show the header
ConsoleHelper.ShowHeader();

// Get the model path from the user
string modelPath
    = ConsoleHelper.GetFolderPath(Statics.ModelInputPrompt);

// Show the header
ConsoleHelper.ShowHeader();
ConsoleHelper.WriteToConsole(Statics.ModelLoadingMessage);

// Load the model and tokenizer
using Model model = new(modelPath);
using MultiModalProcessor processor = new(model);
using Tokenizer tokenizer = new(model);

// Show the header
ConsoleHelper.ShowHeader();

// Simulate the chat loop
while (true)
{
    // Get path to a picture file
    string picturePath
        = ConsoleHelper.GetFilePath(Statics.PictureInputPrompt);

    // Load the image
    Images image =
        Images.Load(picturePath);

    // Show the header
    ConsoleHelper.ShowHeader();

    // Show process message
    ConsoleHelper.WriteToConsole(Statics.AnalyzeImageMessage);
    ConsoleHelper.WriteToConsole(Environment.NewLine);
    ConsoleHelper.WriteToConsole(Environment.NewLine);
    ConsoleHelper.WriteToConsole(Statics.OutputPrompt);

    // Create the prompt
    string fullPrompt
        = $"<|system|>{Statics.SystemPrompt}<|end|>" +
          $"<|user|><|image_1|>{Statics.UserImagePrompt}<|end|>" +
          $"<|assistant|>";

    // Process the image
    NamedTensors inputTensors
        = processor.ProcessImages(fullPrompt, image);

    // Specify the generator parameters
    using GeneratorParams generatorParams = new(model);
    generatorParams.SetSearchOption("max_length", 2048);
    generatorParams.SetInputs(inputTensors);

    // Create the generator
    using Generator generator = new(model, generatorParams);

    // Generate the response
    while (!generator.IsDone())
    {
        generator.ComputeLogits();
        generator.GenerateNextToken();

        string output = tokenizer.Decode(generator.GetSequence(0)[^1..]);

        // Break if the end token is found
        if (output.Contains("</s>"))
        {
            break;
        }

        ConsoleHelper.WriteToConsole(output);
    }

    ConsoleHelper.WriteToConsole(Environment.NewLine);
    ConsoleHelper.WriteToConsole(Environment.NewLine);
    ConsoleHelper.WriteToConsole(Statics.RestartPrompt);

    // Wait for the user to press a key
    Console.ReadKey();
}