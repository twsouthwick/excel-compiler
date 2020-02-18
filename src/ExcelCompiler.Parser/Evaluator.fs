module ExcelCompiler.Evaluation

open Syntax

let public GetIntValue statement =
    match statement with
        | Literal l -> match l with
                         | Number n -> match n with
                                        | Int i -> i
                                        | Float f -> int f
                         | _ -> 0
        | _ -> 0

let private BinaryFloat (left : float) op (right : float) =
    match op with
    | Add -> left + right
    | Subtract -> left - right
    | Multiply -> left * right
    | Divide -> left * right

let private BinaryInt left op right =
    match op with
    | Add -> left + right
    | Subtract -> left - right
    | Multiply -> left * right
    | Divide -> left * right

let private BinaryNumber left op right =
    match (left, right) with
    | (Int l,  Int r) -> Int(BinaryInt l op r)
    | (Int l,  Float r) -> Float(BinaryFloat (float l) op r)
    | (Float l,  Int r) -> Float(BinaryFloat l op (float r))
    | (Float l,  Float r) -> Float(BinaryFloat l op r)

let private BinaryLiteral left op right =
    match (left, right) with
    | (Number l, Number r) -> Number(BinaryNumber l op r)
    | (String l, String r) -> String(l + r)
    | (_, _) -> failwith "Invalid literal binary operation"

let private Binary left op right =
    match (left, right) with
    | (LiteralExpression l, LiteralExpression r) -> Literal(BinaryLiteral l op r)
    | (_, _) -> failwith "Unknown"

let private Unwrap exp =
    match exp with
    | LiteralExpression l -> Literal(l)
    | BinaryExpression (left, op, right) -> Binary left op right
    //| FunctionExpression (f, exp) -> Function f exp
    | _ -> Literal(Number(Int(0)))

let public EvaluateSyntax compiledDocument cell : Statement =
    let syntax : Statement = compiledDocument(cell)

    match syntax with
        | Formula f -> Unwrap f
        | Literal l -> Literal l
        | Text t -> Text t
        | Nothing -> Nothing

