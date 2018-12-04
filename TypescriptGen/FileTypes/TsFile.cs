using System.IO;

namespace TypeGen.FileTypes
{
    public abstract class TsFile
    {
        public TsDir Directory { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; } = TypeBuilder.DefaultExtension;

        public string Export { get; set; }

        public string Import(TsDir targetDir) => $"import {{ {Export} }} from {TypeBuilder.TickStile}{targetDir.ImportPath(Directory, FileName)}{TypeBuilder.TickStile};";

        public string FilePath => Path.Combine(Directory.ToPath(), $"{FileName}{FileExtension}");

        protected TsFile(string fileName, TsDir directory)
        {
            FileName = fileName;
            Directory = directory;
        }
    }
}