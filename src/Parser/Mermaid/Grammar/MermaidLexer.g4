lexer grammar MermaidLexer;

channels {
    COMMENTS_CHANNEL,
    POSITIONS_CHANNEL
}

// fragments
fragment DIGIT: [0-9];

// types
FLOWCHART: 'flowchart';
GRAPH: 'graph';
SEQUENCE_DIAGRAM: 'sequenceDiagram';

// orientations
TOP_DOWN: 'TD';
TOP_BOTTOM: 'TB';
BOTTOM_TOP: 'BT';
RIGHT_LEFT: 'RL';
LEFT_RIGHT: 'LR';

// links
LINK_ARROW: '-->';
LINK_OPEN: '--';
LINK_DOTTED: '-.->';
LINK_THICK: '==>';

POSITION: 'POSITION';

PARENTHESIS_OPEN: '(';
PARENTHESIS_CLOSE: ')';
MINUS: '-';
COMMA: ',';
PERCENT: '%';

NUMBER: MINUS?[0-9]+;
ID: [a-zA-Z0-9_]+;

NEWLINE
    : ('\r'| '\r\n' | '\n') -> channel(HIDDEN)
    ;
WHITE_SPACE: ' '+ -> channel(HIDDEN);
TAB: '\t'+ -> channel(HIDDEN);
POSITION_COMMENT : '%%POSITION-' ~[\r\n]+? '%%' -> channel(POSITIONS_CHANNEL);
COMMENT: '%%' ~[\r\n]* -> channel(COMMENTS_CHANNEL);
