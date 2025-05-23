grammar Redcode;

program:        line* EOF;
line:           comment
                | WS* instruction NL
                | instruction NL
                | WS* NL;
comment:        ';' .*? NL;
instruction:    label? WS* operation WS* adA? exprA? ','? WS* adB? exprB?;
operation:      opcode '.' modifier | opcode;
opcode:         'DAT' | 'MOV' | 'ADD' | 'SUB' | 'MUL' | 'DIV' | 'MOD' |
                'JMP' | 'JMZ' | 'JMN' | 'DJN' | 'CMP' | 'SLT' | 'SPL' |
                'SEQ' | 'SNE' | 'STP' | 'LDP' | 'NOP' |
                'ORG' | 'EQU' | 'END';
label:          ALPHA alphanumeral* ':'?;
modifier:       'A' | 'B' | 'AB' | 'BA' | 'F' | 'X' | 'I';
address_mode:   '#' | '$' | '@' | '<' | '>';
expr:           term (('+' | '-' | '*' | '/' | '%') term)*;
term:           label | ('+'|'-')? NUMERAL+ | '(' expr ')';
adA:            address_mode;
adB:            address_mode;
exprA:          expr;
exprB:          expr;
alphanumeral:   ALPHA | NUMERAL | '_' | '-' | '=';
ALPHA:          [A-Za-zÁáÉéÍíÓóÖöŐőÚúÜüŰű];
NUMERAL:        [0-9];
NL:      '\r\n' | '\n';
WS:     [ \t];