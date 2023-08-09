namespace hl.Arquiteturas
{
    public class ArqX64 : Arquitetura
    {

        public override string Nome => "x64";
        public override string NomeSistemaOperacional => this.SistemaOperacional.ToString();
        public ArqX64(Saida saida, SistemaOperacional sistemaOperacional) : base(saida, sistemaOperacional)
        {
            switch(this.SistemaOperacional)
            {
                case SistemaOperacional.Padrao:
                    SistemaOperacional = SistemaOperacional.macOS;
                    break;
                case SistemaOperacional.Windows:
                case SistemaOperacional.Linux:
                case SistemaOperacional.macOS:
                    break;
                default:
                    throw new NotImplementedException("Arquitetura não suportada");
            }
        }

        public override Bits BitsTipo(Tipo tipo)
        {
            switch(tipo)
            {
                case Tipo.Int8:
                case Tipo.UInt8:
                    return Bits.Bits16;
                case Tipo.Int16:
                case Tipo.UInt16:
                    return Bits.Bits16;
                case Tipo.Int32:
                case Tipo.UInt32:
                    return Bits.Bits32;
                case Tipo.Int64:
                case Tipo.UInt64:
                case Tipo.String:
                case Tipo.ParametrosVariaveis:
                    return Bits.Bits64;
                default:
                    return BitsPonteiro();
            }
        }

        public override Bits BitsPadrao()
        {
            return Bits.Bits64;
        }

        public override Bits BitsPonteiro()
        {
            return Bits.Bits64;
        }

        public override void Emite(string asm)
        {
            Saida.EscreverLinha(asm);
        }

        public override void EmiteComentario(string comentario)
        {
            Saida.EscreverLinha(";{0}", comentario);
        }

        public override void EmiteCopiaAParaB(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor rbx, rbx");
                    Saida.EscreverLinha("mov bl, al");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor rbx, rbx");
                    Saida.EscreverLinha("mov bx, ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor rbx, rbx");
                    Saida.EscreverLinha("mov ebx, eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rbx, rax");
                    break;
            }
        }

        public override void EmiteDefineA(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov al, {0}", valor);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, {0}", valor);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov eax, {0}", valor);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rax, {0}", valor);
                    break;
            }
        }

        public override void EmiteDefineB(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov bl, {0}", valor);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov bx, {0}", valor);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ebx, {0}", valor);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rbx, {0}", valor);
                    break;
            }
        }

        public override void EmiteDesempilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop rax");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop rax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("pop rax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("pop rax");
                    break;
            }
        }

        public override void EmiteEmpilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("and rax, 0xff");
                    Saida.EscreverLinha("push rax");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("and rax, 0xffff");
                    Saida.EscreverLinha("push rax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("and rax, 0xffffffff");
                    Saida.EscreverLinha("push rax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("push rax");
                    break;
            }
        }

        public override void EmiteFimFuncao(string nome, Bits bitsRetorno)
        {
            if(nome.EndsWith("_MAIN") && SistemaOperacional == SistemaOperacional.macOS)
            {
                Saida.EscreverLinha("mov rax, 0x2000001");
                Saida.EscreverLinha("mov rdi, 0");
                Saida.EscreverLinha("syscall");
            }
            else
            {
                Saida.EscreverLinha("xor rax, rax");
            }
            Saida.EscreverLinha(".__END__:", nome);
            Saida.EscreverLinha("mov rsp, rbp");
            Saida.EscreverLinha("pop rbp");
            Saida.EscreverLinha("ret");
        }

        public override void EmiteFuncao(bool publica, string nome, Bits bitsRetorno)
        {
            if(nome.EndsWith("_MAIN"))
            {
                if(SistemaOperacional == SistemaOperacional.macOS)
                {
                    Saida.EscreverLinha("global _main");
                    Saida.EscreverLinha("_main:");
                }
                else
                {
                    Saida.EscreverLinha("global main");
                    Saida.EscreverLinha("main:");
                }
            }
            if(publica)Saida.EscreverLinha("global _{0}", nome);
            Saida.EscreverLinha("_{0}:", nome);
            Saida.EscreverLinha("push rbp");
            Saida.EscreverLinha("mov rbp, rsp");
        }

        public override void EmitePulaParaRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jmp long {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeDiferenteRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jne long {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeIgualRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("je long {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeMaiorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2}e long {0}L{1}", local ? "." : "_", rotulo, sinal ? "g" : "a");
        }

        public override void EmitePulaSeMaiorQueRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2} long {0}L{1}", local ? "." : "_", rotulo, sinal ? "g" : "a");
        }

        public override void EmitePulaSeMenorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2}e long {0}L{1}", local ? "." : "_", rotulo, sinal ? "l" : "b");
        }

        public override void EmitePulaSeMenorQueRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2} long {0}L{1}", local ? "." : "_", rotulo, sinal ? "l" : "b");
        }

        public override void EmiteRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("{0}L{1}:", local ? "." : "_", rotulo);
        }

        public override void EmiteSomaBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("add al, bl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("add ax, bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("add eax, ebx");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("add rax, rbx");
                    break;
            }
        }

        public override void EmiteSomaValorEmPtrPilha(long valor)
        {
            if(valor != 0)Saida.EscreverLinha("add rsp, {0}", valor);
        }

        public override void EmiteSubtraiValorEmPtrPilha(long valor)
        {
            if(valor != 0)Saida.EscreverLinha("sub rsp, {0}", valor);
        }

        public override int PadraoDesvioArgumentos()
        {
            return 16;
        }

        public override int PadraoDesvioVariaveis()
        {
            return 0;
        }

        public override void EmiteConstanteNumerica(bool local, string nome, long valor)
        {
            Saida.EscreverLinha("{0}{1}: equ {2}", local ? "." : "_", nome, valor);
        }

        public override void EmiteCopiaAParaVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov [{0}{1}], al", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov [{0}{1}], ax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov [{0}{1}], eax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov [{0}{1}], rax", local ? "rbp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteSubtraiBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("sub al, bl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("sub ax, bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("sub eax, ebx");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("sub rax, rbx");
                    break;
            }
        }

        public override void EmiteSomaAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("add [{0}{1}], al", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("add [{0}{1}], ax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("add [{0}{1}], eax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("add [{0}{1}], rax", local ? "rbp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteSubtraiAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("sub [{0}{1}], al", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("sub [{0}{1}], ax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("sub [{0}{1}], eax", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("sub [{0}{1}], rax", local ? "rbp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteMultiplicaAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xchg [{0}{1}], al", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} byte [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xchg [{0}{1}], ax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} word [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xchg [{0}{1}], eax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} dword [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("xchg [{0}{1}], rax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} qword [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
            }
        }

        public override void EmiteDivideAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("xchg [{0}{1}], al", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} byte [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("xchg [{0}{1}], ax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} word [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor edx, edx");
                    Saida.EscreverLinha("xchg [{0}{1}], eax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} dword [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("xchg [{0}{1}], rax", local ? "rbp+." : "_", nome);
                    Saida.EscreverLinha("{2} qword [{0}{1}]", local ? "rbp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
            }
        }

        public override void EmiteCabecalho()
        {
            Saida.EscreverLinha("; USE YASM");
            Saida.EscreverLinha("bits 64");
            Saida.EscreverLinha("default rel");
        }

        public override void EmiteRodape()
        {
        }

        public override void EmiteChamar(string nome)
        {
            Saida.EscreverLinha("call _{0}", nome);
        }

        public override void EmiteCopiaEnderecoRotuloParaA(Bits bits, int rotulo, bool local)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("lea rax, [{0}L{1}]", local ? "." : "_", rotulo);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("lea rax, [{0}L{1}]", local ? "." : "_", rotulo);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("lea rax, [{0}L{1}]", local ? "." : "_", rotulo);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("lea rax, [{0}L{1}]", local ? "." : "_", rotulo);
                    break;
            }
        }

        public override void EmiteMudarSegmento(Segmento seg)
        {
            switch (seg)
            {
                case Segmento.Codigo:
                    Saida.EscreverLinha("segment .text");
                    break;
                case Segmento.DadosInicializados:
                    Saida.EscreverLinha("segment .data");
                    break;
                case Segmento.DadosNaoInicializados:
                    Saida.EscreverLinha("segment .bss");
                    break;
            }
        }

        public override void EmiteString(string conteudo)
        {
            List<byte> tmp = new();
            tmp.AddRange(System.Text.Encoding.UTF8.GetBytes(conteudo));
            tmp.InsertRange(0, BitConverter.GetBytes((UInt16)tmp.Count));
            tmp.Add(0);
            EmiteBinario(tmp.ToArray());
        }

        public override void EmiteBinario(byte[] conteudo)
        {
            for (int i = 0; i < conteudo.Length; i++)
            {
                if((i % 15) == 0)
                {
                    Saida.EscreverLinha("");
                    Saida.Escrever("db {0}", conteudo[i]);
                }
                else
                {
                    Saida.Escrever(", {0}", conteudo[i]);
                }
            }
            Saida.EscreverLinha("");
        }

        public override void EmiteNumero(Bits bits, long conteudo)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("db {0}", conteudo);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("dw {0}", conteudo);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("dd {0}", conteudo); 
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("qd {0}", conteudo); 
                    break;
            }
        }

        public override void EmiteCopiaVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov al, [{0}{1}]", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, [{0}{1}]", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov eax, [{0}{1}]", local ? "rbp+." : "_", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rax, [{0}{1}]", local ? "rbp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteCopiaAParaVarPtr(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("mov rdi, [{0}{1}]", local ? "rbp+." : "_", nome);
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("stosb");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("stosw");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("stosd");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("stosq");
                    break;
            }
        }

        public override void EmiteCopiaVarPtrParaA(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("mov rsi, [{0}{1}]", local ? "rbp+." : "_", nome);
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("lodsb");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("lodsw");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("lodsd");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("lodsq");
                    break;
            }
        }

        public override void EmiteCopiaEnderecoVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("lea rax, [{0}{1}]", local ? "." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("lea rax, [{0}{1}]", local ? "." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("lea rax, [{0}{1}]", local ? "." : "_", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("lea rax, [{0}{1}]", local ? "." : "_", nome);
                    break;
            }
        }

        public override void EmiteCopiaEnderecoRotinaParaA(Bits bits, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov rax, _{0}", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov rax, _{0}", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov rax, _{0}", nome);
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rax, _{0}", nome);
                    break;
            }
        }
        public override void EmiteComparacaoBComA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("cmp bl, al");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("cmp bx, ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("cmp ebx, eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("cmp rbx, rax");
                    break;
            }
        }

        public override void EmiteDesempilhaB(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop rbx");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop rbx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("pop rbx");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("pop rbx");
                    break;
            }
        }
        public override void EmiteDeslocaDireitaBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shr al, cl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shr ax, cl");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shr eax, cl");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shr rax, cl");
                    break;
            }
        }

        public override void EmiteDeslocaEsquerdaBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shl al, cl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shl ax, cl");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shl eax, cl");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rcx, rbx");
                    Saida.EscreverLinha("shl rax, cl");
                    break;
            }
        }
        public override void EmiteDivideBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor edx, edx");
                    Saida.EscreverLinha("{0} bl", sinal ? "idiv" : "div");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor edx, edx");
                    Saida.EscreverLinha("{0} bx", sinal ? "idiv" : "div");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor edx, edx");
                    Saida.EscreverLinha("{0} ebx", sinal ? "idiv" : "div");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("{0} rbx", sinal ? "idiv" : "div");
                    break;
            }
        }
        public override void EmiteEBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("and al, bl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("and ax, bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("and eax, eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("and rax, rax");
                    break;
            }
        }
        public override void EmiteModuloBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("{0} bl", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov rax, rdx");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("{0} bx", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov rax, rdx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("{0} ebx", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov rax, rdx");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("xor rdx, rdx");
                    Saida.EscreverLinha("{0} rbx", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov rax, rdx");
                    break;
            }
        }
        public override void EmiteMultiplicaBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("{0} bl", sinal ? "imul" : "mul");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("{0} bx", sinal ? "imul" : "mul");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("{0} ebx", sinal ? "imul" : "mul");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("{0} rbx", sinal ? "imul" : "mul");
                    break;
            }
        }
        public override void EmiteOuBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("or al, bl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("or ax, bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("or eax, eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("or rax, rax");
                    break;
            }
        }
        public override void EmiteOuExclusivoBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor al, bl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor ax, bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor eax, eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("xor rax, rax");
                    break;
            }
        }
        public override void EmiteCopiaAParaVarPtrIndiceEmB(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("mov rsi, [{0}{1}]", local ? "rbp+." : "_", nome);
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov [rsi+rbx], al");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov [rsi+rbx], ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov [rsi+rbx], eax");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov [rsi+rbx], rax");
                    break;
            }
        }
        public override void EmiteCopiaVarPtrIndiceEmBParaA(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("mov rsi, [{0}{1}]", local ? "rbp+." : "_", nome);
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor rax, rax");
                    Saida.EscreverLinha("mov al, [rsi+rbx]");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor rax, rax");
                    Saida.EscreverLinha("mov ax, [rsi+rbx]");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("xor rax, rax");
                    Saida.EscreverLinha("mov eax, [rsi+rbx]");
                    break;
                case Bits.Bits64:
                    Saida.EscreverLinha("mov rax, [rsi+rbx]");
                    break;
            }
 
        }

        public override bool ChamarAsmLink(string arqAsm, string arqSaida)
        {
            switch(SistemaOperacional)
            {
                case SistemaOperacional.Linux:
                    return ExecutarExterno("yasm", $"-f elf64 -o \"{arqSaida}\" \"{arqAsm}\"");
                case SistemaOperacional.macOS:
                    return ExecutarExterno("yasm", $"-f macho64 -o \"{arqSaida}\" \"{arqAsm}\"");
            }
            return false;
        }
    }
}