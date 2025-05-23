		ORG dwarf
bomb	DAT	#0
dwarf	ADD	#4,bomb
	MOV	bomb,@bomb
	JMP	dwarf
	END	dwarf

;redcode verbose
;name Dwarf
;author A. K. Dewdney
;strategy Throw DAT bombs around memory, hitting every 4th memory cell.
;strategy   This program was presented in the first Corewar article.
;assert (CORESIZE % 4)==0
