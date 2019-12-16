module ExcelCompiler.ParseUtils

open FSharp.Text.Lexing
open Syntax

let public Parse text =
    let lexbuf = LexBuffer<char>.FromString text
    let result = Parser.start Lexer.parsetokens lexbuf

    match result with
    | Text _ -> Text(text)
    | _ as original -> original

let public Tokenize text =
    let lexbuf = LexBuffer<char>.FromString text

    seq {
        while not lexbuf.IsPastEndOfStream do
            yield sprintf "%A" (Lexer.parsetokens lexbuf)
    }

let public Flatten (input: Syntax.Statement) = sprintf "%A" input
