section .bss
    str: resb 1024

section .text 

input:
    mov rax, 0
    mov rdi, 1
    mov rsi, str
    mov rdx, 1024
    syscall

    ret