using System.IO;
using System.IO.Hashing;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Blackguard.Build;

public class HashTask : Task {
    [Required]
    public string LibraryDirectory { get; set; } = null!;

    private string HashFilePath => Path.Combine(LibraryDirectory, "Hashes");

    public override bool Execute() {
        string output = "";
        XxHash64 hasher = new();

        if (File.Exists(HashFilePath))
            File.Delete(HashFilePath);

        foreach (string file in Directory.GetFiles(LibraryDirectory)) {
            byte[] buffer = new byte[8192];
            using FileStream fs = File.OpenRead(file);

            while (fs.Read(buffer, 0, buffer.Length) > 0)
                hasher.Append(buffer);

            string line = $"{"Blackguard." + file.Replace('\\', '.').Replace('/', '.')} {hasher.GetCurrentHashAsUInt64()}\n";
            output += line;
            hasher.Reset();
        }

        File.WriteAllText(HashFilePath, output);

        return true;
    }
}
