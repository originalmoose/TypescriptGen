using System;

namespace TypescriptGen.Test.TestClasses.Models
{
    public abstract class TestSimpleBaseClass
    {
        public abstract string Type { get; }
        public int Id { get; set; }
        public string Name { get; set; }
        public char Initial { get; set; }
        public Guid Guid { get; set; }
        public TimeSpan TimeSpan { get; set; }

        public byte Byte { get; set; }
        public sbyte Sbyte { get; set; }
    }

    public class TestSimpleClass : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass);
    }

    public class TestSimpleClass_A : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass_A);
    }

    public class TestSimpleClass_B : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass_B);
    }

    public class TestSimpleClass_C : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass_C);
    }

    public class TestSimpleClass_D : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass_D);
    }

    public class TestSimpleClass_E : TestSimpleBaseClass
    {
        public override string Type => nameof(TestSimpleClass_E);
    }
}