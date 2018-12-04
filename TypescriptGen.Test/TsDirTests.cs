using System;
using System.IO;
using TypeGen.FileTypes;
using Xunit;

namespace TypeGen.Test
{
    public class TsDirTests
    {
        [Fact]
        public void GeneratesValidPaths()
        {
            var expected = Directory.GetCurrentDirectory();
            var subject = TsDir.Create(expected);
            
            var actual = subject.ToPath();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GeneratesValidPath_with_filename()
        {
            var expected = Directory.GetCurrentDirectory();
            var subject = TsDir.Create(expected);
            
            var actual = subject.ToPath("test.ts");

            Assert.Equal(Path.Combine(expected, "test.ts"), actual);
        }

        [Fact]
        public void Generates_valid_import_paths_import_a_from_a()
        {
            var currentDir =CreateTestingTree();

            const string expected = "./test";
            
            var a = currentDir.Down("types").Down("models").Down("a");

            var actual = a.ImportPath(a, "test");

            Assert.Equal(expected, actual);
        }

        
        [Fact]
        public void Generates_valid_import_paths_import_models_from_a()
        {
            var currentDir =CreateTestingTree();

            const string expected = "../test";
            
            var models =  currentDir.Down("types").Down("models");
            var a = models.Down("a");

            var actual = a.ImportPath(models, "test");

            Assert.Equal(expected, actual);
        }
        
        
        [Fact]
        public void Generates_valid_import_paths_import_a_from_types()
        {
            var currentDir =CreateTestingTree();

            const string expected = "./models/a/test";
            
            var types =  currentDir.Down("types");
            var a = types.Down("models").Down("a");

            var actual = types.ImportPath(a, "test");

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Generates_valid_import_paths_import_services_from_a()
        {
            var currentDir =CreateTestingTree();

            const string expected = "../../services/test";
            
            var services =  currentDir.Down("types").Down("services");
            var a = currentDir.Down("types").Down("models").Down("a");

            var actual = a.ImportPath(services, "test");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public  void Throws_exception_when_importing_from_another_tree()
        {
            var treeA = CreateTestingTree();
            var treeB = CreateTestingTree();

            
            var services =  treeA.Down("types").Down("services");
            var a = treeB.Down("types").Down("models").Down("a");

            Assert.Throws<Exception>(() =>
            {
                a.ImportPath(services, "test");
            });
        }

        private TsDir CreateTestingTree()
        {
            var currentDir = TsDir.Create(Directory.GetCurrentDirectory());

            currentDir
                .Down("types")
                .Down("models")
                .Down("a")
                .Up()
                .Down("b")
                .Up()
                .Up()
                .Down("controllers")
                .Up()
                .Down("services");

            return currentDir;
        }
    }
}