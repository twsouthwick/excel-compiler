module ExcelCompiler.ParseUtils

open FSharp.Text.Lexing

let public Parse text =
    let lexbuf = LexBuffer<char>.FromString text
    Parser.start Lexer.parsetokens lexbuf

let public Tokenize text =
    let lexbuf = LexBuffer<char>.FromString text

    seq {
        while not lexbuf.IsPastEndOfStream do
            yield sprintf "%A" (Lexer.parsetokens lexbuf)
    }

let public Flatten (input: Syntax.Statement) = sprintf "%A" input

open Syntax

let public GetIntValue statement =
    match statement with
        | Literal l -> match l with
                         | Number n -> match n with
                                        | Int i -> i
                                        | Float f -> int f
                         | _ -> 0
        | _ -> 0

let Binary left op right =
    Number(Int(5))

let Function f exp =
    Number(Int(5))

let Unwrap exp =
    match exp with
    | LiteralExpression l -> l
    | BinaryExpression (left, op, right) -> Binary left op right
    | FunctionExpression (f, exp) -> Function f exp
    | _ -> Number(Int(0))

let public EvaluateSyntax compiledDocument cell : Statement =
    let syntax : Statement = compiledDocument(cell)

    match syntax with
        | Formula f -> Literal (Unwrap f)
        | Literal l -> Literal l
        | Text t -> Text t
        | Nothing -> Nothing
