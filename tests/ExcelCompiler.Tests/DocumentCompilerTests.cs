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
        }
    }
}
