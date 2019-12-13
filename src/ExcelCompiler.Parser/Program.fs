// Learn more about F# at http://fsharp.net

open System.IO
open FSharp.Text.Lexing

let testLexerAndParserFromString text =
    let lexbuf = LexBuffer<char>.FromString text

    while not lexbuf.IsPastEndOfStream do
        printfn "%A" (Lexer.parsetokens lexbuf)

    printfn "Done"

testLexerAndParserFromString "=cos pi * 42.0"

