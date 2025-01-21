## Notes

### 'IsA'

- Check IsA relationships on pattern matching -- not equality!
- For example:
  - Recipe: Word => All(Token => Token.IsA(LetterChar))
  - True if every token is a LetterChar || UppercaseLetter || LowercaseLetter || ...
  - So, UppercaseLetter.IsA(LetterChar) is **true**, and the same with LowercaseLetter, etc
  - The converse is false, i.e. LetterChar.IsA(UppercaseLetter) is **false**

- Consider keeping IsA() extension for Tokens...
  - For example: UppercaseLetter.IsA(LetterChar) == **true**
    - And LetterChar.IsA(UppercaseLetter) == **false**
  - Helps with readability!

## Processing Steps

## Double Lexing Process

> Source Code  -- [LexToSymbols] -->  Char Token Sequence 
> 
> Char Token Sequence  -- [LexToTokens] -->  Token Sequence


### 'Lex' the Source Code
- Convert source text characters into symbol sequence
  - 'Throw' error if any source text is not convertable into symbols

### 'Lex' the 
- Convert symbol sub-sequences to tokens based on hierarchical pattern matching

## Parsing Process

### Given the YAML AST:
```
Token:
  Literal:
      CharSequenceLiteral:
        Pattern: AllTypeOf(CharLiteral)
        
        WhiteSpaceLiteral:
          Pattern: OnOrMoreOfType(SeparatorChar)
          
        OperatorLiteral:
          Pattern: OnOrMoreOfType(PunctuationChar)
```

### Seperate the YAML AST to Taxonomy:

```
Token:
  Literal:
      CharSequenceLiteral:
        WhiteSpaceLiteral:
        OperatorLiteral:

```

### And YAML Recipe List:
```
CharSequenceLiteral: AllTypeOf(CharLiteral)
WhiteSpaceLiteral: OnOrMoreOfType(SeparatorChar)
OperatorLiteral: OnOrMoreOfType(PunctuationChar)
```

## Talking Through Scenarios

### Order of Operations

Parenthesis

- Sample Sequence: 
>this is outside "and this is inside, and with a sep;";


- We want to tokenize the string literal first.
- Results should look like:
>this is outside T::StringLiteral;

- Where **T::StringLiteral** has 
>representedValue => "and this is inside, and with a sep;"

- We literally(!) do not care about anything in the middle.
- We want to be able to extract these StringLiterals before anything else!

Sequence Needed
- This must be completed after tokenizing to CharTokens
- Sequence can then be defined by a Token Sequence
- We need
  - start token
  - middle token(s)
  - end token
  
- Middle token(s) are optional
- To let the *T::StringLiteral** capture anything, we need a top level T::Token
- No... We need a top level T::CharToken!
>T::CharToken


> Note: We probably still want a T::Token as parent to all Token types




## Example Recipe Definitions

### Using Predicates

```

T::Integer
    AllAreOf(T::DecimalDigitNumber)

```

### Symbol Recipes

```
T::ErrorMeta
    Symbol.Type == S::Unknown
    
T::IntegerLiteral
    All(Symbol => Symbol.Type == S::NumberSymbol)
        && All(Symbol).Count > 0

T::Whitespace
    All(Symbol => Symbol.Type == S::SpaceSymbol || T::TabSymbol)
        && All(Symbol).Count > 0
```

### Token Recipes
```

        
        
T::LineBlock
    All(Token => Token.Type == T::Whitespace || T::Literal)
        && All(Token).Count > 0
    T::EndOfLine




T::Literal
    T::BoolLiteral
        || T::StringLiteral
        || T::NumberLiteral


T::StringLiteral
    S::QuoteSymbol
    All(Symbol)
        && All(Symbol).Count >= 0
    S::QuoteSymbol


T::NumberLiteral
    T::IntegerLiteral
        || T::FloatLiteral




T::FloatLiteral
T::IntegerLiteral
T::DecimalSymbol
T::IntegerLiteral



T::BodyBlock
T::StartOfBodyBlock
All(Token => Token.Type == T::LineBlock)
&& All(Token).Count >= 0
T::EndofBodyBlock

T::ReturningBodyBlock
T::StartOfBodyBlock
All(Token => Token.Type == T::LineBlock)
&& All(Token).Count >= 0
T::ReturnLineBlock
T::EndofBodyBlock

T::ReturnLineBlock
All(Token => Token.Type == T::Whitespace || T::Literal)
&& All(Token).Count > 0
T::ReturnStatement
T::EndOfLine

T::ReturnStatement
T::ReturnKeyword
T::Whitespace
T::Literal

```
