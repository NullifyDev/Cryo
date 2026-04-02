; strlen(esi str): edx
strlen:
    cmp edx, 0
    je .strlen_loop
    xor edx, edx

.strlen_loop:
    cmp byte [ecx + edx], 0
    je .strlen_done
    inc edx
    jmp .strlen_loop

.strlen_done:
    ret