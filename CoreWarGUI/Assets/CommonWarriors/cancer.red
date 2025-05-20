			   ORG	   start
               JMP     start
               JMP     -1
start          SPL     copy2
copy1          MOV     cntr,           ptr
infect1        MOV     germ,           <ptr
               JMN     infect1,        ptr
               MOV     cntr,           ptr
kill1          MOV     poison,         <ptr
               JMN     kill1,          ptr
               JMP     copy1
copy2          MOV     cntr,           ptr
infect2        MOV     germ,           <ptr
               JMN     infect2,        <ptr
               MOV     cntr,           ptr
kill2          MOV     poison,         <ptr
               JMN     kill2,          ptr
               JMP     copy2
germ           SPL     0
poison         DAT     #1
cntr           DAT     #-20
ptr            DAT     #0
               END

;redcode
;name CANCER
;author Thomas Gettys
;1987
;assert 1
