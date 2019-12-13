module ExcelCompiler.Syntax

type Op =
    | Add

type Factor =
    | Int of int
    | Float of float

type Term =
    | Factor of Factor

type Expression =
    | Term of Term
    | Terms of Term * Op * Term

type Formula =
    | Expression of Expression
    | EmptyExpression

