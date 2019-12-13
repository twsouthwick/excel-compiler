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
