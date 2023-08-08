namespace hl.Arquiteturas
{
    public class Arq8086 : Arquitetura
    {
        public override string Nome => "i86";
        public override string NomeSistemaOperacional => this.SistemaOperacional.ToString();

        public Arq8086(Saida saida, SistemaOperacional sistemaOperacional) : base(saida, sistemaOperacional)
        {
            switch(this.SistemaOperacional)
            {
                case SistemaOperacional.Padrao:
                    SistemaOperacional = SistemaOperacional.DOS;
                    break;
                case SistemaOperacional.CPM:
                case SistemaOperacional.DOS:
                case SistemaOperacional.fudebaSO:
                case SistemaOperacional.Windows:
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
                case Tipo.ParametrosVariaveis:
                    return Bits.Bits16;
                case Tipo.Int32:
                case Tipo.UInt32:
                case Tipo.String:
                    return Bits.Bits32;
                default:
                    return BitsPonteiro();
            }
        }

        public override Bits BitsPadrao()
        {
            return Bits.Bits16;
        }

        public override Bits BitsPonteiro()
        {
            return Bits.Bits32;
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
                    Saida.EscreverLinha("mov bl, al");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov bx, ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov bx, ax");
                    Saida.EscreverLinha("mov cx, dx");
                    break;
            }
        }

        public override void EmiteDefineA(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    if(valor == 0)
                        Saida.EscreverLinha("xor al, al");
                    else
                        Saida.EscreverLinha("mov al, {0}", valor);
                    break;
                case Bits.Bits16:
                    if(valor == 0)
                        Saida.EscreverLinha("xor ax, ax");
                    else
                        Saida.EscreverLinha("mov ax, {0}", valor);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, {0}", valor & 0xffff);
                    Saida.EscreverLinha("mov dx, {0}", (valor >> 16) & 0xffff);
                    break;
            }
        }

        public override void EmiteDefineB(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    if(valor == 0)
                        Saida.EscreverLinha("xor bl, bl");
                    else
                        Saida.EscreverLinha("mov bl, {0}", valor);
                    break;
                case Bits.Bits16:
                    if(valor == 0)
                        Saida.EscreverLinha("xor bx, bx");
                    else
                        Saida.EscreverLinha("mov bx, {0}", valor);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov bx, {0}", valor & 0xffff);
                    Saida.EscreverLinha("mov cx, {0}", (valor >> 16) & 0xffff);
                    break;
            }
        }

        public override void EmiteDesempilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop ax");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("pop ax");
                    Saida.EscreverLinha("pop dx");
                    break;
            }
        }

        public override void EmiteEmpilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor ah, ah");
                    Saida.EscreverLinha("push ax");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("push ax");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("push dx");
                    Saida.EscreverLinha("push ax");
                    break;
            }
        }

        public override void EmiteFimFuncao(string nome, Bits bitsRetorno)
        {
            Saida.EscreverLinha("xor ax, ax");
            Saida.EscreverLinha(".__END__:", nome);
            Saida.EscreverLinha("mov sp, bp");
            Saida.EscreverLinha("pop bp");
            Saida.EscreverLinha("retf");
        }

        public override void EmiteFuncao(bool publica, string nome, Bits bitsRetorno)
        {
            if(nome.EndsWith("_MAIN"))
            {
                Saida.EscreverLinha("global _MAIN");
                Saida.EscreverLinha("_MAIN:");
            }
            if(publica)Saida.EscreverLinha("global _{0}", nome);
            Saida.EscreverLinha("_{0}:", nome);
            Saida.EscreverLinha("push bp");
            Saida.EscreverLinha("mov bp, sp");
        }

        public override void EmitePulaParaRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jmp {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeDiferenteRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jne {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeIgualRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("je {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeMaiorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2}e {0}L{1}", local ? "." : "_", rotulo, sinal ? "g" : "a");
        }

        public override void EmitePulaSeMaiorQueRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2} {0}L{1}", local ? "." : "_", rotulo, sinal ? "g" : "a");
        }

        public override void EmitePulaSeMenorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2}e {0}L{1}", local ? "." : "_", rotulo, sinal ? "l" : "b");
        }

        public override void EmitePulaSeMenorQueRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("j{2} {0}L{1}", local ? "." : "_", rotulo, sinal ? "l" : "b");
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
                    Saida.EscreverLinha("add ax, bx");
                    Saida.EscreverLinha("adc dx, cx");
                    break;
            }
        }

        public override void EmiteSomaValorEmPtrPilha(long valor)
        {
            if(valor != 0)Saida.EscreverLinha("add sp, {0}", valor);
        }

        public override void EmiteSubtraiValorEmPtrPilha(long valor)
        {
            if(valor != 0)Saida.EscreverLinha("sub sp, {0}", valor);
        }

        public override int PadraoDesvioArgumentos()
        {
            return 6;
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
                    Saida.EscreverLinha("mov [{0}{1}], al", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov [{0}{1}], ax", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov [{0}{1}], ax", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("mov [{0}{1}+2], dx", local ? "bp+." : "_", nome);
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
                    Saida.EscreverLinha("sub ax, bx");
                    Saida.EscreverLinha("ssb dx, cx");
                    break;
            }
        }

        public override void EmiteSomaAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("add [{0}{1}], al", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("add [{0}{1}], ax", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("add [{0}{1}], ax", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("adc [{0}{1}+2], dx", local ? "bp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteSubtraiAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("sub [{0}{1}], al", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("sub [{0}{1}], ax", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("sub [{0}{1}], ax", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("ssb [{0}{1}+2], dx", local ? "bp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteMultiplicaAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xchg [{0}{1}], al", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("{2} byte [{0}{1}]", local ? "bp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xchg [{0}{1}], ax", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("{2} word [{0}{1}]", local ? "bp+." : "_", nome, sinal ? "imul" : "mul");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException("Multiplicacao 32bits nao implementada");
            }
        }

        public override void EmiteDivideAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("xchg [{0}{1}], al", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("{2} byte [{0}{1}]", local ? "bp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("xchg [{0}{1}], ax", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("{2} word [{0}{1}]", local ? "bp+." : "_", nome, sinal ? "idiv" : "div");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException("Divisao 32bits nao implementada");
            }
        }

        public override void EmiteCabecalho()
        {
            Saida.EscreverLinha("cpu 8086");
            Saida.EscreverLinha("bits 16");
            Saida.EscreverLinha("mov ax, cs");
            Saida.EscreverLinha("mov ds, ax");
            Saida.EscreverLinha("mov es, ax");
            Saida.EscreverLinha("mov ss, ax");
            Saida.EscreverLinha("mov sp, 0xfffe");
            Saida.EscreverLinha("push cs");
            Saida.EscreverLinha("call _MAIN");
            Saida.EscreverLinha("mov ah, 0x4c");
            Saida.EscreverLinha("int 0x21");
        }

        public override void EmiteRodape()
        {
        }

        public override void EmiteChamar(string nome)
        {
            Saida.EscreverLinha("push cs");
            Saida.EscreverLinha("call _{0}", nome);
        }

        public override void EmiteCopiaEnderecoRotuloParaA(Bits bits, int rotulo, bool local)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov ax, {0}L{1}", local ? "." : "_", rotulo);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, {0}L{1}", local ? "." : "_", rotulo);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, ds");
                    Saida.EscreverLinha("mov dx, ax");
                    Saida.EscreverLinha("mov ax, {0}L{1}", local ? "." : "_", rotulo);
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
                    Saida.EscreverLinha("db {0}, {0}, {0}, {0}", 
                        (conteudo ) & 0xff, 
                        (conteudo >> 8) & 0xff, 
                        (conteudo >> 16) & 0xff, 
                        (conteudo >> 24) & 0xff
                        );
                    break;
            }
        }

        public override void EmiteCopiaVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov al, [{0}{1}]", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, [{0}{1}]", local ? "bp+." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, [{0}{1}]", local ? "bp+." : "_", nome);
                    Saida.EscreverLinha("mov dx, [{0}{1}+2]", local ? "bp+." : "_", nome);
                    break;
            }
        }

        public override void EmiteCopiaAParaVarPtr(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("push ds");
            Saida.EscreverLinha("push word [{0}{1}]", local ? "bp+." : "_", nome);
            Saida.EscreverLinha("push word [{0}{1}+2]", local ? "bp+." : "_", nome);
            Saida.EscreverLinha("pop ds");
            Saida.EscreverLinha("pop si");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("lodsb");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("lodsw");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, [si]");
                    Saida.EscreverLinha("mov dx, [si+2]");
                    break;
            }
            Saida.EscreverLinha("pop ds");
        }

        public override void EmiteCopiaVarPtrParaA(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("push es");
            Saida.EscreverLinha("push word [{0}{1}]", local ? "bp+." : "_", nome);
            Saida.EscreverLinha("push word [{0}{1}+2]", local ? "bp+." : "_", nome);
            Saida.EscreverLinha("pop es");
            Saida.EscreverLinha("pop di");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("stosb");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("stosw");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("es mov [di], ax");
                    Saida.EscreverLinha("es mov [di+2], dx");
                    break;
            }
            Saida.EscreverLinha("pop es");
        }

        public override void EmiteCopiaEnderecoVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov ax, {0}{1}", local ? "." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, {0}{1}", local ? "." : "_", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, ds");
                    Saida.EscreverLinha("mov dx, ax");
                    Saida.EscreverLinha("mov ax, {0}{1}", local ? "." : "_", nome);
                    break;
            }
        }

        public override void EmiteCopiaEnderecoRotinaParaA(Bits bits, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov ax, _{0}", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov ax, _{0}", nome);
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("mov ax, cs");
                    Saida.EscreverLinha("mov dx, ax");
                    Saida.EscreverLinha("mov ax, _{0}", nome);
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
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDesempilhaB(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop bx");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop bx");
                    break;
                case Bits.Bits32:
                    Saida.EscreverLinha("pop bx");
                    Saida.EscreverLinha("pop cx");
                    break;
            }
        }
        public override void EmiteDeslocaDireitaBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov cx, bx");
                    Saida.EscreverLinha("shr al, cl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov cx, bx");
                    Saida.EscreverLinha("shr ax, cl");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDeslocaEsquerdaBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("mov cx, bx");
                    Saida.EscreverLinha("shl al, cl");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("mov cx, bx");
                    Saida.EscreverLinha("shl ax, cl");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteDivideBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("{0} bl", sinal ? "idiv" : "div");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("{0} bx", sinal ? "idiv" : "div");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException();
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
                    throw new NotImplementedException();
            }
        }
        public override void EmiteModuloBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("{0} bl", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov ax, dx");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("xor dx, dx");
                    Saida.EscreverLinha("{0} bx", sinal ? "idiv" : "div");
                    Saida.EscreverLinha("mov ax, dx");
                    break;
                case Bits.Bits32:
                    throw new NotImplementedException();
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
                    throw new NotImplementedException();
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
                    throw new NotImplementedException();
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
                    throw new NotImplementedException();
            }
        }
    }
}