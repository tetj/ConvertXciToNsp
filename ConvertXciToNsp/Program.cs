if (args.Length == 0)
{
    Console.WriteLine("Usage: ConvertXciToNsp <folder_path> [-c] [-d] [-r] [-recursive]");
    Console.WriteLine("  -c         : Convert NSP files");
    Console.WriteLine("  -d         : Delete XCI files after conversion");
    Console.WriteLine("  -r         : Rename NSP files");
    Console.WriteLine("  -recursive : Process files in subfolders as well");
    return 1;
}

bool deleteAfterConversion = args.Contains("-d");
bool convert = args.Contains("-c");
bool renameNsp = args.Contains("-r");
bool recursive = args.Contains("-recursive");
string folderPath = args.FirstOrDefault(arg => !arg.StartsWith("-")) ?? string.Empty;

if (string.IsNullOrEmpty(folderPath))
{
    Console.WriteLine("Error: No folder path specified.");
    Console.WriteLine("Usage: ConvertXciToNsp <folder_path> [-c] [-d] [-r] [-recursive]");
    return 1;
}

if (!Directory.Exists(folderPath))
{
    Console.WriteLine($"Error: Folder '{folderPath}' does not exist.");
    return 1;
}

string filePattern = renameNsp ? "*.nsp" : "*.xci";
string fileExtension = renameNsp ? ".nsp" : ".xci";

SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
string[] files = Directory.GetFiles(folderPath, filePattern, searchOption);

if (files.Length == 0)
{
    Console.WriteLine($"No {fileExtension} files found in '{folderPath}'{(recursive ? " (including subfolders)" : "")}.");
    return 0;
}

Console.WriteLine($"Found {files.Length} {fileExtension} file(s) in '{folderPath}'{(recursive ? " (including subfolders)" : "")}.");

foreach (string file in files)
{
    Console.WriteLine($"Processing: {Path.GetFileName(file)}");

    var flags = new System.Text.StringBuilder("");
    if (convert) flags.Append(" -c");
    if (deleteAfterConversion) flags.Append(" -d");
    if (renameNsp) flags.Append(" -r");
    string arguments = $"{flags} \"{file}\"";

    var processInfo = new System.Diagnostics.ProcessStartInfo
    {
        FileName = "4nxci.exe",
        Arguments = arguments,
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = AppContext.BaseDirectory
    };

    using var process = System.Diagnostics.Process.Start(processInfo);
    if (process != null)
    {
        process.WaitForExit();
        Console.WriteLine($"Completed: {Path.GetFileName(file)} (Exit code: {process.ExitCode})");
    }
    else
    {
        Console.WriteLine($"Error: Failed to start process for {Path.GetFileName(file)}");
    }
}

Console.WriteLine("All files processed.");
return 0;
