module ExcelCompiler.ParseUtils

open FSharp.Text.Lexing

let public Parse text =
    let lexbuf = LexBuffer<char>.FromString text

    Parser.start Lexer.parsetokens lexbuf
