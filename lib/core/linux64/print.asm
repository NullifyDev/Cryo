; print(rsi msg)
print:
    push rdi
    push rax
    push rdx

    xor rdx, rdx
    call strlen

    mov rax, 1
    mov rdi, 1
    syscall

    pop rdx
    pop rax
    pop rdi
    ret

; print_sl(rsi msg, rdx len)
print_sl:
    push rax
    push rbx

    call strlen

    mov rax, 1
    mov rdi, 1
    syscall

    pop rbx
    pop rax
    ret