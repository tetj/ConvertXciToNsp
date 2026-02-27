# ConvertXciToNsp

A simple C# console application that batch converts Nintendo Switch XCI files to NSP format using 4nxci.
Can also be used to rename NSP files to this format : 
- **Base Games**: `GameTitle[TitleID][BASE].nsp`
- **Updates/Patches**: `GameTitle[TitleID][v65536][UPD].nsp`
- **DLC/Add-ons**: `GameTitle[TitleID][DLC].nsp`

## Description

This tool automates the conversion of multiple XCI files to NSP format by processing all `.xci` files found in a specified folder. 

## Prerequisites

[4nxci.exe](https://github.com/tetj/4NXCI-2026/releases) - Must be available in your system PATH or in the same directory as the executable (ConvertXciToNsp.exe)

## Usage

Run the application with a folder path containing XCI files:

```bash
ConvertXciToNsp.exe <folder_path> [-e] [-d] [-r] [-recursive]
```

### Options
- `-c` : Convert XCI files to .NSP files
- `-d` : Delete XCI files after successful conversion (be careful with that!)
- `-r` : Rename NSP files to predefined format : `GameTitle[TitleID][BASE].nsp`
- `-recursive` : Process XCI files in subfolders as well

### Examples

Convert XCI files to NSP and delete the original XCI files:
```bash
ConvertXciToNsp.exe -c -d "C:\Games\Nintendo Switch\XCI Files"
```

Convert XCI files to NSP including all subfolders:
```bash
ConvertXciToNsp.exe -c -recursive "C:\Games\Nintendo Switch\XCI Files"
```

Rename NSP files recursively
```bash
ConvertXciToNsp.exe -r -recursive "C:\Games\Nintendo Switch\NSP Files"
```

The application will:
1. Scan the specified folder for all `.xci` files (and subfolders if `-recursive` is specified)
2. Process each file using `4nxci.exe` with the specified options (`-c`, `-d`, `-r`)
3. Display progress and exit codes for each conversion
4. Report when all files have been processed

## Output

- The converted NSP files will be created in the same location as the original XCI files

## Performance

- On a hard drive (not SSD), expect approximately 2 minutes to convert a 4GB .xci file
- Conversion times will vary based on file size and storage speed
- SSD storage will provide significantly faster conversion times

## License

This project is provided as-is for educational and personal use.
