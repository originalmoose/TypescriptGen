using System.IO;

namespace TypescriptGen.FileTypes
{
    public abstract class TsFile
    {
        protected TsFile(string fileName, TsDir directory)
        {
            FileName = fileName;
            Directory = directory;
        }

        public TsDir Directory { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; } = TypeBuilder.DefaultExtension;

        public string Export { get; set; }

        public string FilePath => Path.Combine(Directory.ToPath(), $"{FileName}{FileExtension}");

        public string Import(TsDir targetDir)
        {
            return $"import {{ {Export.Replace("[]","")} }} from {TypeBuilder.TickStile}{targetDir.ImportPath(Directory, FileName)}{TypeBuilder.TickStile};";
        }
    }
}