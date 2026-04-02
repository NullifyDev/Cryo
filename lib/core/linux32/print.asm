; print(ecx msg)
print:
    push edx
    push eax
    push ebx

    call strlen
    ; %strlen(ecx)

    mov eax, 4
    mov ebx, 1
    int 80h

    pop ebx
    pop eax
    pop edx
    ret

; print_sl(ecx msg, edx, len)
print_sl:
    push eax
    push ebx

    mov eax, 4
    mov ebx, 1
    int 80h

    pop ebx
    pop eax
    ret