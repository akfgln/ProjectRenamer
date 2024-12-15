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
            // 1. Klasörleri Kopyala
            Console.WriteLine("Klasör kopyalanıyor (belirtilen dosyalar ve klasörler hariç)...");
            DirectoryCopy(sourcePath, destinationPath, oldName, newName);

            // 2. Proje dosyasını ve klasör içeriğini güncelle
            Console.WriteLine("Dosya içerikleri ve isimleri güncelleniyor...");
            UpdateProjectContentAndNames(destinationPath, oldName, newName);

            Console.WriteLine("Proje başarıyla kopyalandı ve yeniden adlandırıldı!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, string oldName, string newName)
    {
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException($"Kaynak klasör bulunamadı: {sourceDirName}");
        }

        // Kopyalamayı başlat
        foreach (DirectoryInfo subdir in dir.GetDirectories())
        {
            // Hariç tutulacak klasörler
            if (subdir.Name.Equals(".vs", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals(".git", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals(".github", StringComparison.OrdinalIgnoreCase) ||
                subdir.Name.Equals("obj", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            string newSubdirName = subdir.Name.Replace(oldName, newName);
            string tempPath = Path.Combine(destDirName, newSubdirName);
            Directory.CreateDirectory(tempPath);

            // Alt klasörleri kopyala
            DirectoryCopy(subdir.FullName, tempPath, oldName, newName);
        }

        // Dosyaları kopyala
        foreach (FileInfo file in dir.GetFiles())
        {
            string newFileName = file.Name.Replace(oldName, newName);
            string tempPath = Path.Combine(destDirName, newFileName);
            file.CopyTo(tempPath, false);
        }
    }

    private static void UpdateProjectContentAndNames(string directoryPath, string oldName, string newName)
    {
        foreach (string file in Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories))
        {
            // Dosya içeriğini güncelle
            string content = File.ReadAllText(file);
            content = content.Replace(oldName, newName);
            File.WriteAllText(file, content);

            // Dosya adını güncelle
            string fileName = Path.GetFileName(file);
            string newFileName = fileName.Replace(oldName, newName);
            if (fileName != newFileName)
            {
                string newPath = Path.Combine(Path.GetDirectoryName(file), newFileName);
                File.Move(file, newPath);
            }
        }

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
