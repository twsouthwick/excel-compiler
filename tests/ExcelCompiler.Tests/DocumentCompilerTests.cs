using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace ExcelCompiler.Tests
{
    public class DocumentCompilerTests
    {
        [Fact]
        public void Test()
        {
            var compiler = new DocumentCompiler();
            var compiled = compiler.Compile("SimpleFormulas.xlsx");
            var evaluator = new SyntaxEvaluator(compiled, new CustomFunctionProvider());

            var b2 = evaluator.GetCell<int>("a2");
        }

        private class CustomFunctionProvider : IFunctionProvider
        {
            public Syntax.Statement Evaluate(string name, IEnumerable<Syntax.Statement> args)
            {
                return Syntax.Statement.Nothing;
            }
        }
    }
}
