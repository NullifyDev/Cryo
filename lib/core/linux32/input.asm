
; %include "src/linux/32/core/print.asm"
; %include "src/linux/32/core/strlen.asm"

section .bss
    str: resb 1024

section .text

input:
    mov eax, 3
    mov ebx, 0
    mov ecx, str
    mov edx, 1024
    int 80h

    ret
