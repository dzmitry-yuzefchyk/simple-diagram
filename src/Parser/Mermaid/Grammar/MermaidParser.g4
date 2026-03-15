parser grammar MermaidParser;

options {
    tokenVocab = MermaidLexer;
}

diagram
    : type orientation nodes EOF
    ;

type
    : FLOWCHART
    | GRAPH
    | SEQUENCE_DIAGRAM
    ;

orientation
    : TOP_DOWN
    | TOP_BOTTOM
    | BOTTOM_RIGHT
    | RIGHT_LEFT
    | LEFT_RIGHT
    ;

nodes
    : reference+
    ;

reference
    : node_definition link node_definition
    ;

link
    : LINK_ARROW
    | LINK_OPEN
    | LINK_DOTTED
    | LINK_THICK
    ;

node_definition
    : ID
    ;

//indent_block:


