using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Linq;

namespace ExcelCompiler
{
    public class DocumentCompiler
    {
        public ICompiledDocument Compile(string path)
        {
            var dictionary = new Dictionary<CellReference, Syntax.Statement>();

            using (var doc = SpreadsheetDocument.Open(path, isEditable: false))
            {
                foreach (var sheet in doc.WorkbookPart.Workbook.Descendants<Sheet>())
                {
                    var wsPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id);

                    foreach (var cell in wsPart.Worksheet.Descendants<Cell>())
                    {
                        dictionary.Add(new CellReference(cell.CellReference.InnerText), GetStatement(cell, doc.WorkbookPart));
                    }
                }
            }

            return new CompiledDocument(dictionary);
        }

        private string GetSharedString(Cell cell, WorkbookPart wbPart)
        {
            var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>()
                .First();
            var value = int.Parse(cell.InnerText);

            return stringTable.SharedStringTable.ElementAt(value).InnerText;
        }

        private Syntax.Statement GetStatement(Cell cell, WorkbookPart wbPart)
        {
            if (cell.CellFormula != null)
            {
                return ParseUtils.Parse(cell.CellFormula.InnerText);
            }
            else if (cell.DataType != null && cell.DataType.HasValue)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        return Syntax.Statement.NewText(GetSharedString(cell, wbPart));
                    case CellValues.Boolean:
                    case CellValues.Date:
                    case CellValues.InlineString:
                    case CellValues.Error:
                    case CellValues.String:
                    default:
                        return Syntax.Statement.NewText(cell.InnerText);
                }
            }
            else if (int.TryParse(cell.InnerText, out var intResult))
            {
                return Syntax.Statement.NewLiteral(
                    Syntax.Literal.NewNumber(
                        Syntax.Number.NewInt(intResult)));
            }
            else if (double.TryParse(cell.InnerText, out var floatResult))
            {
                return Syntax.Statement.NewLiteral(
                    Syntax.Literal.NewNumber(
                        Syntax.Number.NewFloat(floatResult)));
            }
            else
            {
                return Syntax.Statement.NewText(cell.InnerText);
            }
        }

        private class CompiledDocument : ICompiledDocument
        {
            internal CompiledDocument(Dictionary<CellReference, Syntax.Statement> lookup)
            {
            }
        }
    }
}
