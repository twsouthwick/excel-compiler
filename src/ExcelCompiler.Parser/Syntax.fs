module Syntax

type Expression =
    | Int of int
    | Float of float

type Formula =
    | Expression of Expression

