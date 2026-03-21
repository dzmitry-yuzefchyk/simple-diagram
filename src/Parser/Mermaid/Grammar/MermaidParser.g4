parser grammar MermaidParser;

options {
    tokenVocab = MermaidLexer;
}

diagram
    : type orientation statements EOF
    ;

type
    : 'flowchart'
    | 'graph'
    | 'sequenceDiagram'
    ;

orientation
    : 'TD' | 'TB'   #topToBottom
    | 'BT'          #bottomToTop
    | 'RL'          #rightToLeft
    | 'LR'          #leftToRight
    ;

statements
    : statement+
    ;

statement
    : nodeDefinition                        #standaloneNode
    | nodeDefinition link nodeDefinition    #reference
    ;

link
    : LINK_ARROW
    | LINK_OPEN
    | LINK_DOTTED
    | LINK_THICK
    ;

nodeDefinition
    : ID
    ;
