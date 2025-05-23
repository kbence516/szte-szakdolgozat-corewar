	ORG start
	DAT #22
	DAT #300
start:  MOV <-2,<-1
	JMN start,-3
	MOV #22,275
	MOV #300,275
        SPL 275 
        SPL check
	JMP dwfs
datd:  DAT #-9
dwfs:  SPL 5
	MOV -2,@-2
	SUB #7,-3
	JMP -2
datu:  DAT #9
	MOV -1,@-1
	ADD #7,-2
	JMP -2
check:	SLT #250,datu
	JMP -1
	MOV #-9,datd
	MOV #9,datu
	JMP check 
	END start
	
;redcode
;name ARMY
;author Neil Robertson
;strategy - copy itself to a location 300 ahead then bomb either side of  
;strategy - itself using dwarfs which are reset when there bomb runs are
;strategy - getting close to the next copy of army
;assert 1
