module ExcelCompiler.Syntax

open System
open System.Linq

[<CustomEquality; NoComparison>]
type  SyntaxList<'T when 'T : equality>  =
    | Empty
    | Single of 'T
    | List of seq<'T>

    override this.Equals(y) =
        match y with
        | :? SyntaxList<'T> as other ->
            match (this.unwrap this, this.unwrap other) with
            | (Empty, Empty) -> true
            | (Single s1, Single s2) -> s1.Equals(s2)
            | (List l1, List l2) -> Enumerable.SequenceEqual(l1, l2)
            | _ -> false
        | _ -> false

    override this.GetHashCode() =
        match this with
        | Empty -> 0
        | Single s -> s.GetHashCode()
        | List l ->
            let x = new HashCode()
            for i in l do x.Add(i)
            x.ToHashCode()

     member private this.unwrap x =
            match x with
            | List l -> if l.Count() > 1 then x else Single(l.First())
            | _ -> x

type Factor =
    | Int of int
    | Float of float

type Term =
    | Factors of SyntaxList<Factor>

type Expression =
    | Terms of SyntaxList<Term>
    | Function of string * SyntaxList<Expression>

type Formula =
    | Expression of Expression
    | EmptyExpression

