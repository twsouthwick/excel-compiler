module ExcelCompiler.Syntax

open System
open System.Linq

type SyntaxList<'T> private (x: seq<'T>) =
    static member Create(x:seq<'T>) = new SyntaxList<'T>(x)
    static member Create(x:'T) = new SyntaxList<'T>([x])
    member this.Value = x
    override this.Equals(y) =
        match y with
        | :? SyntaxList<'T> as other -> Enumerable.SequenceEqual(this.Value, other.Value)
        | :? seq<'T> as other -> Enumerable.SequenceEqual(this.Value, other)
        | _ -> false
    override this.GetHashCode() =
        let x = new HashCode()
        for i in this.Value do x.Add(i)
        x.ToHashCode()

type Factor =
    | Int of int
    | Float of float

type Expression =
    | Term of SyntaxList<Factor>

type Formula =
    | Expression of Expression
    | EmptyExpression

