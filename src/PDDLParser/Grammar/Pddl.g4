/**
 * PDDL Grammar for ANTLR4
 *
 * This grammar is a custom implementation for .NET Standard 2.1 and Unity compatibility.
 * Supports: STRIPS, Typing, Negative Preconditions, and basic ADL features.
 * Target: C# 8.0+ with .NET Standard 2.1
 *
 * Acknowledgments:
 * This grammar was inspired by and references the PDDL grammar work from:
 * - Original ANTLR v3 Grammar by Zeyn Saigol, University of Birmingham
 * - ANTLR v4 port by Tom Everett
 * - Source: https://github.com/antlr/grammars-v4/tree/master/pddl
 *
 * PDDL (Planning Domain Definition Language) is a standardized language
 * developed by the AI Planning community.
 */

grammar Pddl;

// ===== Entry Points =====

pddlDoc
    : domain
    | problem
    ;

// ===== Domain =====

domain
    : '(' 'define' '(' 'domain' name ')'
      requireDef?
      typesDef?
      predicatesDef?
      actionDef*
      ')'
    ;

requireDef
    : '(' ':requirements' requireKey+ ')'
    ;

typesDef
    : '(' ':types' typedNameList ')'
    ;

predicatesDef
    : '(' ':predicates' atomicFormulaSkeleton+ ')'
    ;

actionDef
    : '(' ':action' name
      ':parameters' '(' typedVariableList ')'
      ':precondition' (goalDesc | '(' ')')
      ':effect' (effect | '(' ')')
      ')'
    ;

// ===== Problem =====

problem
    : '(' 'define' '(' 'problem' name ')'
      '(' ':domain' name ')'
      objectDecl?
      init
      goal
      ')'
    ;

objectDecl
    : '(' ':objects' typedNameList ')'
    ;

init
    : '(' ':init' initEl* ')'
    ;

initEl
    : literal
    ;

goal
    : '(' ':goal' goalDesc ')'
    ;

// ===== Goal Descriptions (Preconditions) =====

goalDesc
    : atomicFormula                                      # GoalSimple
    | '(' 'and' goalDesc* ')'                           # GoalAnd
    | '(' 'or' goalDesc* ')'                            # GoalOr
    | '(' 'not' goalDesc ')'                            # GoalNot
    | '(' 'imply' goalDesc goalDesc ')'                 # GoalImply
    | '(' 'exists' '(' typedVariableList ')' goalDesc ')' # GoalExists
    | '(' 'forall' '(' typedVariableList ')' goalDesc ')' # GoalForall
    ;

// ===== Effects =====

effect
    : '(' 'and' cEffect* ')'                            # EffectAnd
    | cEffect                                           # EffectSimple
    ;

cEffect
    : '(' 'forall' '(' typedVariableList ')' effect ')' # CEffectForall
    | '(' 'when' goalDesc condEffect ')'                # CEffectWhen
    | pEffect                                           # CEffectSimple
    ;

condEffect
    : '(' 'and' pEffect* ')'
    | pEffect
    ;

pEffect
    : '(' 'not' atomicFormula ')'                       # PEffectNot
    | atomicFormula                                     # PEffectPos
    ;

// ===== Atomic Formulas =====

atomicFormulaSkeleton
    : '(' predicate typedVariableList ')'
    ;

atomicFormula
    : '(' predicate term* ')'
    ;

literal
    : atomicFormula
    | '(' 'not' atomicFormula ')'
    ;

// ===== Terms and Types =====

term
    : name
    | variable
    ;

predicate
    : name
    ;

// Typed lists
typedVariableList
    : (singleTypeVarList)*
    | variable*
    ;

singleTypeVarList
    : variable+ '-' type
    ;

typedNameList
    : (singleTypeNameList)*
    | name*
    ;

singleTypeNameList
    : name+ '-' type
    ;

type
    : '(' 'either' primitiveType+ ')'
    | primitiveType
    ;

primitiveType
    : name
    ;

// ===== Terminals =====

variable
    : VARIABLE
    ;

name
    : NAME
    | 'at'          // Allow 'at' as name (common in PDDL)
    | 'over'
    ;

requireKey
    : ':strips'
    | ':typing'
    | ':negative-preconditions'
    | ':disjunctive-preconditions'
    | ':equality'
    | ':existential-preconditions'
    | ':universal-preconditions'
    | ':quantified-preconditions'
    | ':conditional-effects'
    | ':adl'
    ;

// ===== Lexer Rules =====

VARIABLE
    : '?' LETTER (LETTER | DIGIT | '-' | '_')*
    ;

NAME
    : LETTER (LETTER | DIGIT | '-' | '_')*
    ;

fragment LETTER
    : [a-zA-Z]
    ;

fragment DIGIT
    : [0-9]
    ;

// Whitespace and comments
WS
    : [ \t\r\n]+ -> skip
    ;

LINE_COMMENT
    : ';' ~[\r\n]* -> skip
    ;
