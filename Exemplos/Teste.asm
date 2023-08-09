cpu 8086
bits 16
mov ax, cs
mov ds, ax
mov es, ax
mov ss, ax
mov sp, 0xfffe
push cs
call _MAIN
mov ah, 0x4c
int 0x21
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:3:1: FuncaoNo
global _MAIN
_MAIN:
_TESTE_MAIN:
push bp
mov bp, sp
sub sp, 8
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:3:1: DeclaraVariavelNo
.ARGC: equ 6
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:3:1: DeclaraVariavelNo
.ARGV: equ 8
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:3:1: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:4:5: DeclaraVariavelNo
.I: equ -2
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:5:5: DeclaraVariavelNo
.C: equ -4
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:6:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:6:23: StringNo
segment .data
.L0:

db 11, 0, 79, 105, 101, 101, 101, 32, 109, 117, 110, 100, 111, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L0
push dx
push ax
push cs
call _CONSOLE_WRITELINE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:7:5: DeclaraVariavelNo
.TXT: equ -8
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:7:25: StringNo
segment .data
.L1:

db 3, 0, 32, 32, 32, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L1
mov [bp+.TXT], ax
mov [bp+.TXT+2], dx
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:8:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:8:23: LeiaVariavelNo
mov ax, [bp+.TXT]
mov dx, [bp+.TXT+2]
push dx
push ax
push cs
call _CONSOLE_WRITELINE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:9:5: SeNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:9:13: ComparacaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:9:8: NumeroNo
mov ax, 1
push ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:9:13: NumeroNo
xor ax, ax
pop bx
cmp bx, ax
jae .L3
jmp .L2
.L3:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:9:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:10:9: ChamarFuncNo
push cs
call _TESTE_OIE
.L2:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:5: ParaNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:13: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:13: NumeroNo
mov ax, 1
mov [bp+.I], ax
.L4:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:5: LeiaVariavelNo
mov ax, [bp+.I]
mov bx, 10
cmp bx, ax
jae .L5
jmp .L6
.L5:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:13:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:13:23: StringNo
segment .data
.L7:

db 1, 0, 46, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L7
push dx
push ax
push cs
call _CONSOLE_WRITE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:26: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:12:26: NumeroNo
mov ax, 2
add [bp+.I], ax
jmp .L4
.L6:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: ParaCadaNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: NumeroNo
xor ax, ax
mov [bp+.I], ax
.L8:
mov ax, [bp+.I]
push ax
push ds
push word [bp+.TXT]
push word [bp+.TXT+2]
pop ds
pop si
lodsw
pop ds
pop bx
cmp bx, ax
jb .L9
jmp .L11
.L9:
mov ax, [bp+.I]
mov bx, 2
add ax, bx
mov bx, ax
mov si, [bp+.TXT]
xor ax, ax
mov al, [si+bx]
xor bl, bl
cmp bl, al
jne .L10
jmp .L11
.L10:
mov [bp+.C], al
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:16:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:16:23: StringNo
segment .data
.L12:

db 1, 0, 35, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L12
push dx
push ax
push cs
call _CONSOLE_WRITE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:15:5: NumeroNo
mov ax, 1
add [bp+.I], ax
jmp .L8
.L11:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:18:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:18:9: NumeroNo
xor ax, ax
mov [bp+.I], ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:19:5: EnquantoNo
.L14:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:19:15: ComparacaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:19:11: LeiaVariavelNo
mov ax, [bp+.I]
push ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:19:15: NumeroNo
mov ax, 10
pop bx
cmp bx, ax
jb .L15
jmp .L13
.L15:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:19:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:20:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:20:23: StringNo
segment .data
.L16:

db 1, 0, 36, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L16
push dx
push ax
push cs
call _CONSOLE_WRITE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:21:9: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:21:14: NumeroNo
mov ax, 1
add [bp+.I], ax
jmp .L14
.L13:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:23:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:23:9: NumeroNo
xor ax, ax
mov [bp+.I], ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:24:5: AteNo
.L18:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:24:16: ComparacaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:24:11: LeiaVariavelNo
mov ax, [bp+.I]
push ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:24:16: NumeroNo
mov ax, 10
pop bx
cmp bx, ax
jae .L20
jmp .L19
.L20:
jmp .L17
.L19:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:24:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:25:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:25:23: StringNo
segment .data
.L21:

db 1, 0, 37, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L21
push dx
push ax
push cs
call _CONSOLE_WRITE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:26:9: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:26:14: NumeroNo
mov ax, 1
add [bp+.I], ax
jmp .L18
.L17:
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:28:5: SeAsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:28:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:29:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:29:27: StringNo
segment .data
.L22:

db 14, 0, 82, 111, 100, 97, 110, 100, 111, 32, 100, 111, 32, 105, 56
db 54, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L22
push dx
push ax
push cs
call _CONSOLE_WRITELINE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:31:5: SeAsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:31:5: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:32:9: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:32:27: StringNo
segment .data
.L23:

db 14, 0, 82, 111, 100, 97, 110, 100, 111, 32, 100, 111, 32, 68, 79
db 83, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L23
push dx
push ax
push cs
call _CONSOLE_WRITELINE
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:34:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:34:23: StringNo
segment .data
.L24:

db 3, 0, 70, 73, 77, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L24
push dx
push ax
push cs
call _CONSOLE_WRITELINE
xor ax, ax
.__END__:
mov sp, bp
pop bp
retf
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:117:1: FuncaoNo
_CONSOLE_WRITELINE:
push bp
mov bp, sp
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:117:1: DeclaraVariavelNo
.TXT: equ 6
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:117:1: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:118:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:118:11: LeiaVariavelNo
mov ax, [bp+.TXT]
mov dx, [bp+.TXT+2]
push dx
push ax
push cs
call _CONSOLE_WRITE
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:119:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:119:11: StringNo
segment .data
.L25:

db 2, 0, 13, 10, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L25
push dx
push ax
push cs
call _CONSOLE_WRITE
xor ax, ax
.__END__:
mov sp, bp
pop bp
retf
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:48:1: FuncaoNo
_CONSOLE_WRITE:
push bp
mov bp, sp
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:48:1: DeclaraVariavelNo
.TXT: equ 6
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:48:1: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:50:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:51:9: AsmNo
mov ax, [bp+.TXT+2]
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:52:9: AsmNo
mov si, [bp+.TXT]
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:53:9: AsmNo
push ds
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:54:9: AsmNo
mov ds, ax
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:55:9: AsmNo
lodsw
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:56:9: AsmNo
mov cx, ax
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:57:9: AsmNo
.loop:
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:58:9: AsmNo
lodsb
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:59:9: AsmNo
cmp al, 0
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:60:9: AsmNo
je .fim
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:61:9: AsmNo
mov dl, al
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:62:9: AsmNo
mov ah, 0x2
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:63:9: AsmNo
int 0x21
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:64:9: AsmNo
loop .loop
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:65:9: AsmNo
.fim:
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:66:9: AsmNo
pop ds
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:69:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:70:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:71:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:72:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:73:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:74:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:75:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:76:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:77:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:80:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:81:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:82:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:83:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:84:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:85:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:86:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:87:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:88:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:89:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:90:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:91:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:92:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:93:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:94:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:97:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:98:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:99:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:100:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:101:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:102:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:103:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:104:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:107:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:108:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:109:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:110:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:111:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:112:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:113:9: AsmNo
;/Users/humberto/Nextcloud/hl/Exemplos/Console.hli:114:9: AsmNo
xor ax, ax
.__END__:
mov sp, bp
pop bp
retf
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:37:1: FuncaoNo
_TESTE_OIE:
push bp
mov bp, sp
sub sp, 2
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:37:1: BlocoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:38:5: DeclaraVariavelNo
.A: equ -2
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:38:22: NumeroNo
mov ax, 557
mov [bp+.A], ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:39:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:39:9: NumeroNo
mov ax, 123
mov [bp+.A], ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:40:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:40:10: NumeroNo
mov ax, 4
add [bp+.A], ax
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:41:5: AtribuicaoNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:41:10: NumeroNo
mov ax, 5
xchg [bp+.A], ax
imul word [bp+.A]
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:42:5: ChamarFuncNo
;/Users/humberto/Nextcloud/hl/Exemplos/Teste.hl:42:23: StringNo
segment .data
.L26:

db 3, 0, 45, 45, 45, 0
segment .text
mov ax, ds
mov dx, ax
mov ax, .L26
push dx
push ax
push cs
call _CONSOLE_WRITELINE
xor ax, ax
.__END__:
mov sp, bp
pop bp
retf
