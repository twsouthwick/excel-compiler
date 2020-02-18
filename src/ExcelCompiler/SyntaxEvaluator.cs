using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;

namespace ExcelCompiler
{
    public interface IFunctionProvider
    {
        Syntax.Statement Evaluate(string name, IEnumerable<Syntax.Statement> args);
    }

    public class SyntaxEvaluator
    {
        private readonly FSharpFunc<CellReference, Syntax.Statement> _doc;
        private readonly FSharpFunc<Tuple<string, IEnumerable<Syntax.Statement>>, Syntax.Statement> _functions;

        public SyntaxEvaluator(ICompiledDocument document, IFunctionProvider functionProvider)
        {
            _doc = FSharpFunc<CellReference, Syntax.Statement>.FromConverter(document.GetCell);
            _functions = FSharpFunc<Tuple<string, IEnumerable<Syntax.Statement>>, Syntax.Statement>.FromConverter(t => functionProvider.Evaluate(t.Item1, t.Item2));
        }

        public T GetCell<T>(CellReference cell)
        {
            var result = Evaluation.EvaluateSyntax<CellReference>(cell, _doc, _functions);

            if (typeof(T) == typeof(int))
            {
                return (T)(object)Evaluation.GetIntValue(result);
            }

            throw new InvalidOperationException();
        }
    }
}
