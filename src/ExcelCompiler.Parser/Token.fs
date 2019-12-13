module Token

type Tokens =
    | EQUALS
    | STRING of string
    // Numeric types
    | INT of int        | FLOAT of float
    // Constants
    | PI                | E
    // Trig functions
    | SIN               | COS           | TAN
    // Operators
    | PLUS              | DASH          | ASTERISK
    | SLASH             | CARET
    // Misc
    | LPAREN            | RPAREN        | EOF

type Expression =
    | Text of string

type Formula =
    | Expression of Expression
