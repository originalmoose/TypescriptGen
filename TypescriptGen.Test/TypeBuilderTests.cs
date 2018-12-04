using TypescriptGen.Test.TestClasses.Models;
using Xunit;

namespace TypescriptGen.Test
{
    public class TypeBuilderTests
    {
        [Fact]
        public void Class_adds_ClassDefinition_to_ClassDefinitions()
        {
            var builder = new TypeBuilder();

            var classDef = builder.Class<TestSimpleClass>();

            Assert.NotNull(classDef);
            Assert.True(builder.ClassFiles.ContainsKey(typeof(TestSimpleClass)));
            Assert.Contains(classDef, builder.ClassFiles.Values);
        }

        [Fact]
        public void Classes_adds_ClassDefinition_to_ClassDefinitions_using_passed_in_filter_after_calling_BuildDefinitions()
        {
            var builder = new TypeBuilder();

            builder.Classes(x => x == typeof(TestSimpleClass));

            Assert.True(builder.ClassFiles.ContainsKey(typeof(TestSimpleClass)));
        }
    }
}