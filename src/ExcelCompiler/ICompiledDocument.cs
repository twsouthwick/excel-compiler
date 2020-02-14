namespace ExcelCompiler
{
    public interface ICompiledDocument
    {
        Syntax.Statement GetCell(CellReference cell);
    }
}
