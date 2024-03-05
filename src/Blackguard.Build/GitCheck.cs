using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Blackguard.Build;

public class GitCheckTask : Task {
    [Required]
    public string[] Repos { get; set; } = null!;

    [Required]
    public string RootDir { get; set; } = null!;

    [Output]
    public bool ShouldRecompile { get; set; }

    private string HashesPath => Path.Combine(RootDir, ".built-commit-hashes");

    private static string? ExecuteGit(string path) {
        Process p = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "git",
                Arguments = "rev-parse HEAD",
                WorkingDirectory = path,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                UseShellExecute = false,
            }
        };

        p.Start();
        p.WaitForExit();

        // Only one line of output �
        string? ret = p.StandardOutput.ReadLine();

        p.Close();
        p.Dispose();

        return ret;
    }

    private void LogHashes() {
        string lines = "";

        foreach (string repo in Repos)
            lines += $"{repo} {ExecuteGit(Path.Combine(RootDir, repo))}\n";

        File.WriteAllText(HashesPath, lines);
    }

    public override bool Execute() {
        // If the logs don't exist, don't recompile anyway. They can manually force it if they care
        if (!File.Exists(HashesPath)) {
            LogHashes();
            ShouldRecompile = false;
            return true;
        }

        string text = File.ReadAllText(HashesPath).TrimEnd();
        string[] lines = text.Split('\n');

        foreach (string line in lines) {
            string[] split = line.Split(' ');
            string path = split[0];
            string hash = split[1];

            if (hash != ExecuteGit(Path.Combine(RootDir, path))) {
                ShouldRecompile = true;
                LogHashes();
                return true;
            }
        }

        ShouldRecompile = false;
        return true;
    }
}
