using System;

namespace ExcelCompiler
{
    public readonly struct CellReference : IEquatable<CellReference>
    {
        public CellReference(string reference)
        {
            Reference = reference;
        }

        public string Reference { get; }

        public bool Equals(CellReference other) => string.Equals(Reference, other.Reference, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Reference);

        public override bool Equals(object obj) => obj is CellReference other ? Equals(other) : false;

        public static implicit operator CellReference(string cell) => new CellReference(cell);
    }
}
