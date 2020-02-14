using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

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
                        if (cell.CellFormula != null)
                        {
                            var value = cell.CellFormula;
                            var result = ParseUtils.Parse("=" + value.InnerText);

                            dictionary.Add(new CellReference(cell.CellReference.InnerText), result);
                        }
                        else
                        {
                            var result = ParseUtils.Parse(cell.InnerText);

                            dictionary.Add(new CellReference(cell.CellReference.InnerText), result);
                        }
                    }
                }
            }

            return new CompiledDocument(dictionary);
        }

        private class CompiledDocument : ICompiledDocument
        {
            internal CompiledDocument(Dictionary<CellReference, Syntax.Statement> lookup)
            {
            }
        }
    }
}
