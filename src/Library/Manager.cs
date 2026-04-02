namespace Cryo;

public static class LibraryManager
{
    public static string LibraryLocation = $"{Directory.GetCurrentDirectory()}/lib";
    public static List<Library> Libraries = new();

    public static void ShowAllLibraries()
    {
        foreach(var l in Libraries)
            l.ShowInfo();
    }

    public static void VerifyIntegrity()
    {
        // List<string> broken = new(); // implement for developers (when Library development is being made)
        foreach(var l in Libraries)
        {
            Console.Write($"{l.name}: ");
            bool isClean = l.VerifyIntegrity().Length <= 0;
            Console.WriteLine(isClean  ? "Ready!" : "Failed!");
        }
    }

    public static void RepairIntegrity()
    {
        foreach(var l in Libraries)
            l.RepairIntegrity();
    }

    public static void ScanLibrary()
    {
        foreach(var l in Directory.GetDirectories(LibraryLocation))
        {
            string lib   = $"{l.Split("/")[^1]}",
                infoJson = $"{LibraryLocation}/{lib}/{lib}.json";

            if (!File.Exists(infoJson)) continue;

            Library? libInfo = Library.GetData(infoJson);
            if (libInfo == null) continue;

            Console.Write($"{libInfo.name}: ");
            libInfo.RepairIntegrity(true);
            
            // if (libInfo.VerifyIntegrity().Length > 0) 
            // Console.WriteLine(libInfo.VerifyIntegrity().Length <= 0  ? "Ready!" : "Failed!");
            
                // System.Console.WriteLine("All libraries are intact!");
            
            
            Libraries.Add(libInfo);
            Console.WriteLine("Done!");
        }
    }
}