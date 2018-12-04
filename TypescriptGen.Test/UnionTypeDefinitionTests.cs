using System.Linq;
using TypeGen.FileTypes;
using TypeGen.Test.TestClasses.Models;
using Xunit;

namespace TypeGen.Test
{
    public class UnionTypeDefinitionTests
    {
        public UnionTypeDefinitionTests()
        {
        }

        [Fact]
        public void UnionTypeDefinition_generic_UnionType_creates_union_on_base_class()
        {
            var builder = new TypeBuilder();
            builder.UnionType<TestSimpleBaseClass>();

            var expected = new[]
            {
                typeof(TestSimpleClass),
                typeof(TestSimpleClass_A),
                typeof(TestSimpleClass_B),
                typeof(TestSimpleClass_C),
                typeof(TestSimpleClass_D),
                typeof(TestSimpleClass_E),
            };

            var actual = builder.UnionTypeDefinitions.SelectMany(x => x.TypesForUnion).Select(x => x.Type).ToArray();

            foreach (var e in expected)
            {
                Assert.Contains(e, actual);
            }
        }

        [Fact]
        public void UnionTypeDefinition_PreferInterface_false_results_in_ClassDefinitions_getting_generated()
        {
            var builder = new TypeBuilder();
            builder.UnionType<TestSimpleBaseClass>();

            foreach (var u in builder.UnionTypeDefinitions)
            {
                Assert.All(u.TypesForUnion, d => { Assert.IsType<ClassFile>(d); });
            }
        }

        [Fact]
        public void UnionTypeDefinition_UseInterfaces_true_results_in_InterfaceDefinitions_getting_generated()
        {
            var builder = new TypeBuilder();
            builder.Interface<TestSimpleClass>();
            builder.Interface<TestSimpleClass_A>();
            builder.Interface<TestSimpleClass_B>();
            builder.Interface<TestSimpleClass_C>();
            builder.Interface<TestSimpleClass_D>();
            builder.Interface<TestSimpleClass_E>();
            builder.UnionType<TestSimpleBaseClass>();
            //def.UseInterfaces = true;

            foreach (var u in builder.UnionTypeDefinitions)
            {
                Assert.All(u.TypesForUnion, d => { Assert.IsType<InterfaceFile>(d); });
            }
        }
        
        [Fact]
        public void UnionTypeDefinition_UseInterfaces_true_results_in_InterfaceDefinitions_getting_generated_override_single_type()
        {
            var builder = new TypeBuilder();
            builder.Class<TestSimpleClass>();
            builder.Interface<TestSimpleClass_A>();
            builder.Interface<TestSimpleClass_B>();
            builder.Interface<TestSimpleClass_C>();
            builder.Interface<TestSimpleClass_D>();
            builder.Interface<TestSimpleClass_E>();
            builder.UnionType<TestSimpleBaseClass>();
            
            Assert.Single(builder.UnionTypeDefinitions);
            foreach (var definition in builder.UnionTypeDefinitions[0].TypesForUnion)
            {
                if (definition.Type == typeof(TestSimpleClass))
                    Assert.IsType<ClassFile>(definition);
                else
                    Assert.IsType<InterfaceFile>(definition);
            }
        }

        [Fact]
        public void UnionTypeDefinition_valid_output()
        {
            var builder = new TypeBuilder();
            var def = builder.UnionType<TestSimpleBaseClass>(directory: builder.RootDir.Down("TypeGen").Down("Test"));

            var expected = @"import { TestSimpleClass } from './TestClasses/Models/TestSimpleClass';
import { TestSimpleClass_A } from './TestClasses/Models/TestSimpleClass_A';
import { TestSimpleClass_B } from './TestClasses/Models/TestSimpleClass_B';
import { TestSimpleClass_C } from './TestClasses/Models/TestSimpleClass_C';
import { TestSimpleClass_D } from './TestClasses/Models/TestSimpleClass_D';
import { TestSimpleClass_E } from './TestClasses/Models/TestSimpleClass_E';

export type TestSimpleBaseClass = TestSimpleClass | TestSimpleClass_A | TestSimpleClass_B | TestSimpleClass_C | TestSimpleClass_D | TestSimpleClass_E;
";
            var actual = def.ToString();

            Assert.Equal(expected, actual);
        }
    }
}