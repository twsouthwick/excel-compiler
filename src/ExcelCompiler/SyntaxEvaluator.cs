using Microsoft.FSharp.Core;
using System;

namespace ExcelCompiler
{
    public class SyntaxEvaluator
    {
        private readonly FSharpFunc<CellReference, Syntax.Statement> _func;

        public SyntaxEvaluator(ICompiledDocument document)
        {
            _func = FSharpFunc<CellReference, Syntax.Statement>.FromConverter(document.GetCell);
        }

        public T GetCell<T>(CellReference cell)
        {
            var result = Evaluation.EvaluateSyntax<CellReference>(_func, cell);

            if (typeof(T) == typeof(int))
            {
                return (T)(object)Evaluation.GetIntValue(result);
            }

            throw new InvalidOperationException();
        }
    }
}
