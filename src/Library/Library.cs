/*
    !!! ALWAYS REMEMBER TO GRAB CHECKSUMS FROM ORIGINAL REPOSITORIES WHEN CHECKING FILE CHECKSUMS !!!!  

*/


using System.Diagnostics;
using System.Text.Json;

namespace Cryo;

public class Library
{
    public string name { get; set; }
    public string author { get; set; }
    public string version { get; set; }
    public Dictionary<string, string>? checksums { get; set; }
    private string? libloc, libpath;

    public void ShowInfo() => Console.WriteLine($"{name} v{version} - {author}");

    public Library()
    {
        this.libloc = $"{Directory.GetCurrentDirectory()}/lib";
        this.libpath = $"{this.libloc}/{this.name}";
    }

    public Library(string name, string author, string version, Dictionary<string, string> checksums)
    {
        this.name      = name;
        this.author    = author;
        this.version   = version;
        this.checksums = checksums;
        this.libloc    = $"{Directory.GetCurrentDirectory()}/lib";
        this.libpath = $"{this.libloc}/{this.name}";
    }

    public string ToJson() => JsonSerializer.Serialize(this);


    public static Library? GetData(string library) => JsonSerializer.Deserialize<Library>(File.ReadAllText(library));
    public void SaveData() => File.WriteAllText(
        $"{this.libloc}/{this.name}/{this.name}.json", 
        JsonSerializer.Serialize<Library>(this, new JsonSerializerOptions() { WriteIndented = true })
    );

    public void BuildChecksums(bool trim = false)
    {
        if (this.checksums == null) 
            this.checksums = new();

        string[] files = this.ScanFiles();
        foreach(var o in files) 
        {
            if (o.Length < 1) continue;
            string? name = o.Replace($"{this.libloc}/{this.name}/", ""),
                     cs = this.checksum(name);

            if(name == $"{this.name}.json"
            || cs == null) continue;

            if (this.checksums.ContainsKey(name)) 
            {
                if (this.checksums[name] == null) this.checksums.Remove(name);
                else                              this.checksums[name] = cs;
            }
            else this.checksums.Add(name, cs);
        }

        this.SaveData();
    }

    public Library(string name)
    {
        Library? lib = Library.GetData(name);
        if (lib == null
        ||  lib.name == null
        ||  lib.author == null
        ||  lib.version == null) {
            Console.WriteLine($"Either not found, doesn't have \"{name}.json\" in its root folder or \"{name}.json\" is broken");
            return;
        }

        this.name = name;
        this.author = lib.author;
        this.version = lib.version;
        this.libloc = $"{Directory.GetCurrentDirectory()}/lib";
        this.libpath = $"{this.libloc}/{this.name}";

        if (this.checksums == null)    this.checksums = new();
        if (this.checksums.Count <= 0) this.BuildChecksums();

        this.VerifyIntegrity();
    }

    private string? checksum(string file)
    {
        string f = $"{libloc}/{this.name}/{file}";
        if (!File.Exists(f)) return null;

        ProcessStartInfo psi = new ProcessStartInfo() {
            FileName = "/bin/sha256sum",
            Arguments = f,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using (Process? proc = Process.Start(psi)) 
        {
            if(proc == null)
            {   
                Console.WriteLine("failed to hash - Process (sha256sum) failed to start.");
                return null;
            }
            proc.WaitForExit();
            return proc!.StandardOutput.ReadToEnd().Split(" ")[0];
        }
    }

    private bool FileExists(string file)
        => File.Exists($"{this.libpath}/{this.name}/{file}");

    private string[] ScanFiles()
    {
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "find",
            Arguments = $"{this.libloc}/{this.name} -type f",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        };

        using (Process? proc = Process.Start(psi))
        {
            proc.WaitForExit();
            return proc!.StandardOutput.ReadToEnd().Split("\n");
        }
    }

    public void RepairIntegrity(bool trim = false) {
        RepairBroken(VerifyIntegrity(), trim);
    }

    public string[] VerifyIntegrity()
    {
        if (this.checksums == null) 
            this.BuildChecksums();

        List<string> broken = new();
        foreach (KeyValuePair<string, string> f in this.checksums!)
        {
            if (f.Key == $"{this.name}.json") continue;

            string? cs = checksum(f.Key), ccs = this.checksums[f.Key];
            if (cs != null && ccs != null && ccs == cs) continue;

            broken.Add(f.Key);
        }
        if (broken.Count > 0)
            Console.Write($"broken! ");

        return broken.ToArray();
    }

    public void RepairBroken(string[] broken, bool trim = false) 
    {
        if (broken.Length < 1) return;

        Console.Write("Repairing... ");
        if (this.checksums == null) 
        { 
            this.checksums = new();
            return;
        }

        foreach(string f in broken)
        {
            string? cs = checksum(f);
            if (cs != null && this.checksums[f] == cs) continue;
            if (trim) {
                if (!this.FileExists(f) || this.checksums[f] == null) {
                    this.checksums.Remove(f);
                    continue;
                }
            }
            this.checksums[f] = cs!;
        }

        this.SaveData();
        if (broken.Length > 0) {
            this.RepairIntegrity(trim);
        }
    }
}
