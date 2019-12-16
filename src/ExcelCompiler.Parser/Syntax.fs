module ExcelCompiler.Syntax

open System.Text.RegularExpressions
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
            match (this, other) with
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

type Operation =
    | Add
    | Subtract
    | Multiply
    | Divide

type ArgumentList = SyntaxList<Expression>
and Literal =
    | Int of int
    | Float of float
    | String of string
and CellReference =
    | Relative of string * int
    | Absolute of string * int
    | AbsoluteColumn of string * int
    | AbsoluteRow of string * int
and Expression =
    | CellReferenceExpression of CellReference
    | AnchoredCellReferenceExpression of string * bool * bool
    | BinaryExpression of Expression * Operation * Expression
    | LiteralExpression of Literal
    | FunctionExpression of string * ArgumentList
and Statement =
    | Formula of Expression
    | Literal of Literal
    | Text of string

let internal CreateList<'T when 'T : equality> (t : 'T list) =
    if t.IsEmpty then Empty
    elif t.Length = 1 then Single(List.head t)
    else List(List.rev t)

let (|Match|_|) pattern input =
    let m = Regex.Match(input, pattern) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None

let (|Integer|_|) input =
    Some(Int32.Parse(input))

let internal CreateAbsoluteCellReference (column: string) (row: int) =
    match column with
    | Match "(\w+)" [s] -> Absolute(s, row)
    | _ -> failwith "Invalid cell reference"

let internal CreateCellReference (input : string) isAbsolute =
    match input with
    | Match "(\w+)(\d+)" [s; Integer d] ->
        match isAbsolute with
        | true -> AbsoluteColumn(s, d)
        | false -> Relative(s, d)
    | _ -> failwith "Invalid cell reference"
