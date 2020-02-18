using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;

namespace ExcelCompiler
{
    public class SyntaxEvaluator
    {
        private readonly FSharpFunc<Cell, Syntax.Statement> _doc;
        private readonly FSharpFunc<Tuple<string, IEnumerable<Syntax.Statement>>, Syntax.Statement> _functions;

        public SyntaxEvaluator(ICompiledDocument document, IFunctionProvider functionProvider)
        {
            _doc = FSharpFunc<Cell, Syntax.Statement>.FromConverter(document.GetCell);
            _functions = FSharpFunc<Tuple<string, IEnumerable<Syntax.Statement>>, Syntax.Statement>.FromConverter(t => functionProvider.Evaluate(t.Item1, t.Item2));
        }

        public T GetCell<T>(Cell cell)
        {
            var result = Evaluation.EvaluateSyntax<Cell>(cell, _doc, _functions);

            if (typeof(T) == typeof(int))
            {
                return (T)(object)Evaluation.GetIntValue(result);
            }

            throw new InvalidOperationException();
        }
    }
}
