using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: ProjectRenamer <SourcePath> <DestinationPath> <OldName> <NewName>");
            return;
        }

        string sourcePath = args[0];
        string destinationPath = args[1];
        string oldName = args[2];
        string newName = args[3];

        try
        {
            // Step 1: Copy the source directory to the destination, excluding specific folders
            Console.WriteLine("Copying project directory (excluding specific folders)...");
            DirectoryCopy(sourcePath, destinationPath, oldName, newName);

            // Step 2: Update file contents, filenames, and folder names
            Console.WriteLine("Updating file contents, filenames, and folder names...");
            UpdateProjectContentAndNames(destinationPath, oldName, newName);

            Console.WriteLine("Project successfully copied and renamed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, string oldName, string newName)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        // Check if the source directory exists
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDirName}");
        }

        // Loop through subdirectories
        foreach (DirectoryInfo subdir in dir.GetDirectories())
        {
            // Skip excluded folders
            if (subdir.Name.Equals(".vs", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals(".git", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals(".github", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals("obj", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            // Replace old name with new name in the subdirectory name
            string newSubdirName = subdir.Name.Replace(oldName, newName);
            string tempPath = Path.Combine(destDirName, newSubdirName);
            Directory.CreateDirectory(tempPath);

            // Recursively copy subdirectories
            DirectoryCopy(subdir.FullName, tempPath, oldName, newName);
        }

        // Copy files in the current directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string newFileName = file.Name.Replace(oldName, newName);
            string tempPath = Path.Combine(destDirName, newFileName);
            file.CopyTo(tempPath, false);
        }
    }

    private static void UpdateProjectContentAndNames(string directoryPath, string oldName, string newName)
    {
        // Update file contents and filenames
        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
        {
            // Replace old name with new name in the file content
            string content = File.ReadAllText(file);
            content = content.Replace(oldName, newName);
            File.WriteAllText(file, content);

            // Rename file if it contains the old name
            string fileName = Path.GetFileName(file);
            string newFileName = fileName.Replace(oldName, newName);
            if (fileName != newFileName)
            {
                string newPath = Path.Combine(Path.GetDirectoryName(file), newFileName);
                File.Move(file, newPath);
            }
        }

        // Update folder names
        foreach (string dir in Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories))
        {
            string dirName = Path.GetFileName(dir);
            string newDirName = dirName.Replace(oldName, newName);
            if (dirName != newDirName)
            {
                string newPath = Path.Combine(Path.GetDirectoryName(dir), newDirName);
                Directory.Move(dir, newPath);
            }
        }
    }
}
