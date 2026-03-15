lexer grammar MermaidLexer;

// fragments
fragment DIGIT: [0-9];

COMMENT: '%%' -> skip;

// types
FLOWCHART: 'flowchart';
GRAPH: 'graph';
SEQUENCE_DIAGRAM: 'sequenceDiagram';

// orientations
TOP_DOWN: 'TD';
TOP_BOTTOM: 'TB';
BOTTOM_RIGHT: 'BT';
RIGHT_LEFT: 'RL';
LEFT_RIGHT: 'LR';

// links
LINK_ARROW: '-->';
LINK_OPEN: '--';
LINK_DOTTED: '-.->';
LINK_THICK: '==>';

ID: [a-zA-Z0-9_]+;

NEWLINE
    : ('\r'| '\r\n' | '\n') -> channel(HIDDEN)
    ;
WHITE_SPACE: ' '+ -> channel(HIDDEN);
TAB: '\t'+ -> channel(HIDDEN);
