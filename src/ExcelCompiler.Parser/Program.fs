// Learn more about F# at http://fsharp.net

open System.IO
open FSharp.Text.Lexing

let testLexerAndParserFromString text =
    let lexbuf =LexBuffer<char>.FromString text

    let result = Parser.start Lexer.parsetokens lexbuf
    while not lexbuf.IsPastEndOfStream do
        printfn "%A" (Lexer.parsetokens lexbuf)

    printfn "Done"

testLexerAndParserFromString "=1"
testLexerAndParserFromString "=1.2"

