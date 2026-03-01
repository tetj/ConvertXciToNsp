if (args.Length == 0)
{
    Console.WriteLine("Usage: ConvertXciToNsp <folder_path> [-c] [-d] [-r] [-recursive]");
    Console.WriteLine("  -c         : Convert XCI files");
    Console.WriteLine("  -d         : Delete XCI files after conversion");
    Console.WriteLine("  -r         : Rename NSP/NSZ files");
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

string fileExtension = renameNsp ? ".nsp / .nsz" : ".xci";

SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
string[] patterns = renameNsp ? ["*.nsp", "*.nsz"] : ["*.xci"];
string[] files = patterns.SelectMany(p => Directory.GetFiles(folderPath, p, searchOption)).ToArray();

if (files.Length == 0)
{
    Console.WriteLine($"No {fileExtension} files found in '{folderPath}'{(recursive ? " (including subfolders)" : "")}.");
    return 0;
}

Console.WriteLine($"Found {files.Length} {fileExtension} file(s) in '{folderPath}'{(recursive ? " (including subfolders)" : "")}.");

string executablePath = Path.Combine(AppContext.BaseDirectory, "4nxci.exe");
if (!File.Exists(executablePath))
{
    Console.WriteLine($"Error: 4nxci.exe not found at '{executablePath}'.");
    Console.WriteLine("Make sure 4nxci.exe is in the same directory as ConvertXciToNsp.exe.");
    Console.WriteLine("https://github.com/tetj/4NXCI-2026");
    return 1;
}

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

    try
    {
        using var process = System.Diagnostics.Process.Start(processInfo);
        if (process != null)
        {
            process.WaitForExit();
            string RED = Console.IsOutputRedirected ? "" : "\x1b[91m";
            string NORMAL = Console.IsOutputRedirected ? "" : "\x1b[39m";

            if (process.ExitCode == 0)
            {
                Console.WriteLine($"Completed: {Path.GetFileName(file)} (Exit code: {process.ExitCode})");
            }
            else
            {
                Console.WriteLine($"{RED}Error: {Path.GetFileName(file)} (Exit code: {process.ExitCode}){NORMAL}");
                Console.WriteLine($"{RED}You can try running 4nxci.exe manually for more details.{NORMAL}");
            }
        }
        else
        {
            Console.WriteLine($"Error: Failed to start process for {Path.GetFileName(file)}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: Failed to start 4nxci.exe for {Path.GetFileName(file)}: {ex.Message}");
    }
}

Console.WriteLine("All files processed.");
return 0;
