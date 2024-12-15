# ProjectRenamer

ProjectRenamer is a simple CLI tool built in C# that allows you to copy an existing project and rename it by updating folder names, file names, and file contents. It also excludes specific folders like `.vs`, `.git`, `.github`, and `obj` during the copy process.

## Features
- Copies a source project to a new destination.
- Renames all occurrences of the old project name in folder names, file names, and file contents.
- Excludes specified folders (`.vs`, `.git`, `.github`, `obj`) from being copied.

## Requirements
- .NET SDK (6.0 or later)

## Usage

### Build the Project
1. Clone the repository or download the source code.
2. Navigate to the project directory and build the CLI tool:
   ```bash
   dotnet build
   ```

### Run the Tool
Run the CLI tool using the following command:

```bash
dotnet run -- <SourcePath> <DestinationPath> <OldName> <NewName>
```

#### Arguments
- `SourcePath`: The path to the source project you want to copy.
- `DestinationPath`: The path where the new project will be created.
- `OldName`: The current name of the project that needs to be replaced.
- `NewName`: The new name for the project.

#### Example
```bash
dotnet run -- "C:\Projects\OldProject" "C:\Projects\NewProject" "OldProject" "NewProject"
```

## Excluded Folders
The following folders are excluded during the copy process:
- `.vs`
- `.git`
- `.github`
- `obj`

## How It Works
1. **Copy Directory**: The tool copies the source project to the destination, skipping excluded folders.
2. **Rename Files and Folders**: Files and folders containing the old project name are renamed to the new name.
3. **Update File Contents**: All occurrences of the old project name in file contents are replaced with the new name.

## Contributing
Contributions are welcome! Feel free to fork the repository and submit pull requests.

## License
This project is licensed under the MIT License. See the `LICENSE` file for more details.

