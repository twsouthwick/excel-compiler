module ExcelCompiler.Syntax

open System
open System.Linq

[<CustomEquality; NoComparison>]
type  SyntaxList<'T when 'T : equality>  =
    | Single of 'T
    | List of seq<'T>

    override this.Equals(y) =
        match y with
        | :? SyntaxList<'T> as other ->
            match (this.unwrap this, this.unwrap other) with
            | (Single s1, Single s2) -> s1.Equals(s2)
            | (List l1, List l2) -> Enumerable.SequenceEqual(l1, l2)
            | _ -> false
        | _ -> false

    override this.GetHashCode() =
        match this with
        | Single s -> s.GetHashCode()
        | List l ->
            let x = new HashCode()
            for i in l do x.Add(i)
            x.ToHashCode()

     member private this.unwrap x =
            match x with
            | Single s -> x
            | List l -> if l.Count() > 1 then x else Single(l.First())

type Factor =
    | Int of int
    | Float of float

type Term =
    | Factors of SyntaxList<Factor>

type Expression =
    | Terms of SyntaxList<Term>

type Formula =
    | Expression of Expression
    | EmptyExpression

