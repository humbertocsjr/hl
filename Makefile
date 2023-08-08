all: bin/Debug/net6.0/hl
clean:
	@dotnet clean
	@-rm Exemplos/*.bin
	@-rm Exemplos/*.com

install: 
	@-rm -fR bin/Release
	@dotnet build -c Release --sc -o bin/Release
	@mkdir -p /opt/hl
	@install -t /opt/hl bin/Release/*
	@ln -f -s /opt/hl/hl /usr/bin/hl
	@-rm -fR bin/Release

bin/Debug/net6.0/hl: $(wildcard Nos/*.cs) $(wildcard Arquiteturas/*.cs) $(wildcard *.cs)
	@dotnet build

testedos: all
	@bin/Debug/net6.0/hl -i86 -dos -o Exemplos/Teste.asm Exemplos/Teste.hl > Exemplos/Teste.log
	@nasm -f bin --before "org 0x100" -o Exemplos/Teste.com Exemplos/Teste.asm >> Exemplos/Teste.log
	@rm Exemplos/Teste.asm
	@ndisasm -b 16 -o 0x100 Exemplos/Teste.com >> Exemplos/Teste.log
	@echo -= TESTE.COM =- >> Exemplos/Teste.log
	@dosbox -C "mount c: Exemplos" -C "c:\Teste.com >> c:\Teste.log" -C "exit" -exit
	@echo -= TESTE.COM INFO =- >> Exemplos/Teste.log
	@ls -l Exemplos/Teste.com >> Exemplos/Teste.log

testelinux: all
	@bin/Debug/net6.0/hl -i386 -linux -o Exemplos/Teste.asm Exemplos/Teste.hl > Exemplos/Teste.log
	@nasm -f elf32 -o Exemplos/Teste.o Exemplos/Teste.asm >> Exemplos/Teste.log
	@gcc -z noexecstack -m32 -o Exemplos/TesteLinux386.bin Exemplos/Teste.o >> Exemplos/Teste.log
	@bin/Debug/net6.0/hl -x64 -linux -o Exemplos/Teste.asm Exemplos/Teste.hl >> Exemplos/Teste.log
	@cat Exemplos/Teste.asm >> Exemplos/Teste.log
	@yasm -f elf64 -o Exemplos/Teste.o Exemplos/Teste.asm >> Exemplos/Teste.log
	@gcc -z noexecstack -m64 -o Exemplos/TesteLinux64.bin Exemplos/Teste.o >> Exemplos/Teste.log
	@rm Exemplos/Teste.asm
	@rm Exemplos/Teste.o
	@Exemplos/TesteLinux386.bin >> Exemplos/Teste.log
	@Exemplos/TesteLinux64.bin >> Exemplos/Teste.log

testemac: all
	@bin/Debug/net6.0/hl -x64 -macos -o Exemplos/Teste.asm Exemplos/Teste.hl > Exemplos/Teste.log
	@cat Exemplos/Teste.asm >> Exemplos/Teste.log
	@yasm -f macho64 -o Exemplos/Teste.o Exemplos/Teste.asm >> Exemplos/Teste.log
	@gcc -o Exemplos/TesteMac64.bin Exemplos/Teste.o  >> Exemplos/Teste.log
	@rm Exemplos/Teste.asm
	@rm Exemplos/Teste.o
	@Exemplos/TesteMac64.bin >> Exemplos/Teste.log