using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TypeGen.FileTypes
{
    public class TsDir
    {
        public string DirName { get; set; }

        public TsDir ParentDir { get; set; }

        private readonly List<TsDir> _children = new List<TsDir>();
        public TsDir[] Children => _children.ToArray();

        public TsDir Up()
        {
            return ParentDir ?? this;
        }

        public TsDir Down(string dirName)
        {
            var child = _children.FirstOrDefault(c => c.DirName.Equals(dirName, StringComparison.OrdinalIgnoreCase));
            if (child != null)
                return child;

            child = new TsDir {DirName = dirName, ParentDir = this};
            _children.Add(child);
            return child;
        }

        public string ToPath(string fileName = null)
        {
            var folders = new List<string>();

            if (!string.IsNullOrEmpty(fileName))
                folders.Add(fileName);

            var f = this;
            folders.Add(f.DirName);
            while (f.ParentDir != null)
            {
                f = f.Up();
                folders.Add(f.DirName);
            }

            folders.Reverse();
            var drive = folders.First();
            var rest = folders.Skip(1);
            return $"{drive}\\{Path.Combine(rest.ToArray())}";
        }

        public override string ToString()
        {
            return DirName;
        }

        public static TsDir Create(string path)
        {
            var folders = path.Split('\\');
            var dir = folders.Aggregate<string, TsDir>(null, (current, folder) => current == null ? new TsDir {DirName = folder} : current.Down(folder));

            var p = dir.ToPath();
            return dir;
        }

        public string ImportPath(TsDir targetDir, string targetFile)
        {
            if (this == targetDir)
            {
                return $"./{targetFile}";
            }

            if (ParentDir == targetDir)
            {
                return $"../{targetFile}";
            }

            //need to switch to a non recursive solution?
            TsDir ChildPath(TsDir start)
            {
                foreach (var child in start.Children)
                {
                    if (child == targetDir)
                        return child;

                    if (!child.Children.Any())
                        continue;

                    var r = ChildPath(child);
                    if (r != null)
                        return child;
                }

                return null;
            }

            var result = new List<string>();
            var s = this;
            var childTest = ChildPath(s);

            if (childTest != null)
            {
                result.Add(".");
            }

            while (childTest == null && s != null)
            {
                //once s is null we have made it to the top of the tree
                result.Add("..");
                s = s.ParentDir;
                if (s == null)
                    throw new Exception("target directory not found");
                childTest = ChildPath(s);
            }

            if (childTest != null)
            {
                result.Add(childTest.DirName);
                while (childTest != targetDir)
                {
                    childTest = ChildPath(childTest);
                    result.Add(childTest.DirName);
                }

                result.Add(targetFile);

                return string.Join("/", result.ToArray());
            }

            throw new Exception("Something went wrong");
        }
    }
}