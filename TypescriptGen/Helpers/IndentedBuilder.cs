using System;
using System.Linq;
using System.Text;

namespace TypescriptGen.Helpers
{
    public class IndentedStringBuilder
    {
        private readonly StringBuilder _builder;

        public IndentedStringBuilder()
        {
            _builder = new StringBuilder("");
        }

        public int TabCount { get; private set; }

        public IndentedStringBuilder AppendLine()
        {
            _builder.AppendLine();
            return this;
        }

        public IndentedStringBuilder AppendLine(string line)
        {
            _builder.AppendLine($"{Tabs()}{line}");
            return this;
        }

        public IDisposable Indent()
        {
            ++TabCount;
            return Disposable.Create(() => --TabCount);
        }

        private string Tabs()
        {
            return string.Join("", Enumerable.Repeat("    ", TabCount));
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public static implicit operator string(IndentedStringBuilder builder)
        {
            return builder.ToString();
        }
    }
}