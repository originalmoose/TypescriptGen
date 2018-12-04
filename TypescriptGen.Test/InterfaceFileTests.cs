using TypeGen.Test.TestClasses.Models;
using Xunit;

namespace TypeGen.Test
{
    public class InterfaceFileTests
    {
        [Fact]
        public void StringOutputTest()
        {
            var builder = new TypeBuilder();
            var def = builder.Interface<TestSimpleClass>();
            var expected = @"export interface ITestSimpleClass
{
    Type: string;
    Id: number;
    Name: string;
    Initial: string;
    Guid: string;
    TimeSpan: string;
    Byte: number;
    Sbyte: number;
}
";

            Assert.Equal(expected, def.ToString());
        }
    }
}
