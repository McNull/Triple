<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
    var files = new List<string>() 
    {
        "install.cmd", 
        "uninstall.cmd", 
        "readme.md", 
        @"Triple\bin\release\Triple.exe"
    };

    var dest = @"Dist/Triple.zip";
    
    Directory.SetCurrentDirectory(Path.GetDirectoryName (Util.CurrentQueryPath));    
    
    ////////////////////////////////////////////////////////////////////////////
    
    var tempDirectory = GetTemporaryDirectory();
    
    try
    {
        Console.WriteLine ("Created temporary directory: {0}", tempDirectory);
        
        files.ForEach(x => 
        {
            var y = Path.Combine(tempDirectory, Path.GetFileName(x));
            Console.WriteLine ("Copying {0} to {1} ...", x, y);   
            File.Copy(x, y);
        });

        if (File.Exists(dest))
        {
            Console.WriteLine("Removing {0} ...", dest);
            File.Delete(dest);
        }
        
        Console.WriteLine("Compressing files into archive {0} ...", dest);
        ZipFile.CreateFromDirectory(tempDirectory, dest, CompressionLevel.Optimal, false);
        
    }
    finally
    {
        Console.WriteLine ("Deleting temporary files ...");
        Directory.Delete(tempDirectory, true);
    }
    
    Console.WriteLine("Done");
}

// Define other methods and classes here

public string GetTemporaryDirectory()
{
   string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
   Directory.CreateDirectory(tempDirectory);
   return tempDirectory;
}