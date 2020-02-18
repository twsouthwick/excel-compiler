namespace ExcelCompiler

open ExcelCompiler.Syntax

[<Struct>]
type public Cell = {Location : string}

type public ICompiledDocument =
    abstract GetCell : cell:Cell -> Statement

type public IFunctionProvider =
    abstract Evaluate : name:string -> args:seq<Statement> -> Statement


