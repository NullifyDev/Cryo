; exit(ebx code)
exit:
    mov eax, 1
    int 80h
    ret