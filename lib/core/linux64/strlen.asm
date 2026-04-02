section .text

; strlen(esi str): rdx
strlen:
    push rsi
    cmp rdx, 0
    je .strlen_loop
    xor rdx, rdx
    
.strlen_loop:
    cmp byte [rsi + rdx], 0
    je .strlen_done
    inc rdx
    jmp .strlen_loop

.strlen_done:
    pop rsi
    ret