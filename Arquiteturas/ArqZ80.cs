namespace hl.Arquiteturas
{
    public class ArqZ80: Arquitetura
    {
        public override string Nome => "z80";
        public override string NomeSistemaOperacional => this.SistemaOperacional.ToString();

        public ArqZ80(Saida saida, SistemaOperacional sistemaOperacional) : base(saida, sistemaOperacional)
        {
            switch(this.SistemaOperacional)
            {
                case SistemaOperacional.Padrao:
                    SistemaOperacional = SistemaOperacional.CPM;
                    break;
                case SistemaOperacional.CPM:
                case SistemaOperacional.DOS:
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
                case Tipo.String:
                    return Bits.Bits16;
                case Tipo.Int32:
                case Tipo.UInt32:
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
            return Bits.Bits16;
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
                    Saida.EscreverLinha("ld c, a");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld bc, hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDefineA(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld a, {0}", valor);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, {0}", valor);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDefineB(Bits bits, long valor)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld c, {0}", valor);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld bc, {0}", valor);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDesempilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop af");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteEmpilhaA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("push af");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("push hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteFimFuncao(string nome, Bits bitsRetorno)
        {
            Saida.EscreverLinha("xor hl");
            Saida.EscreverLinha(".__END__:", nome);
            Saida.EscreverLinha("ld sp, iy");
            Saida.EscreverLinha("pop iy");
            Saida.EscreverLinha("ret");
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
            Saida.EscreverLinha("push iy");
            Saida.EscreverLinha("ld iy, 0");
            Saida.EscreverLinha("add iy, sp");
        }

        public override void EmitePulaParaRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jp {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeDiferenteRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jp nz, {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeIgualRotulo(int rotulo, bool local)
        {
            Saida.EscreverLinha("jp z, {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeMaiorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("jp nc, {0}L{1}", local ? "." : "_", rotulo);
        }

        public override void EmitePulaSeMaiorQueRotulo(int rotulo, bool local, bool sinal)
        {
            int rotuloNao = RotuloNovo;
            Saida.EscreverLinha("jp c, {0}L{1}", local ? "." : "_", rotuloNao);
            Saida.EscreverLinha("jp z, {0}L{1}", local ? "." : "_", rotuloNao);
            Saida.EscreverLinha("jp {0}L{1}", local ? "." : "_", rotulo);
            EmiteRotulo(rotuloNao, local);
        }

        public override void EmitePulaSeMenorIgualRotulo(int rotulo, bool local, bool sinal)
        {
            int rotuloNao = RotuloNovo;
            Saida.EscreverLinha("jp z, {0}L{1}", local ? "." : "_", rotulo);
            Saida.EscreverLinha("jp nc, {0}L{1}", local ? "." : "_", rotuloNao);
            Saida.EscreverLinha("jp {0}L{1}", local ? "." : "_", rotulo);
            EmiteRotulo(rotuloNao, local);
        }

        public override void EmitePulaSeMenorQueRotulo(int rotulo, bool local, bool sinal)
        {
            Saida.EscreverLinha("jp c, {0}L{1}", local ? "." : "_", rotulo);
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
                    Saida.EscreverLinha("add a, c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("add hl, bc");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteSomaValorEmPtrPilha(long valor)
        {
            if(valor != 0)
            {
                Saida.EscreverLinha("ld ix, 0");
                Saida.EscreverLinha("add ix, sp");
                Saida.EscreverLinha("add ix, {0}", valor);
                Saida.EscreverLinha("ld sp, ix");
            }
        }

        public override void EmiteSubtraiValorEmPtrPilha(long valor)
        {
            if(valor != 0)
            {
                Saida.EscreverLinha("ld ix, 0");
                Saida.EscreverLinha("add ix, sp");
                Saida.EscreverLinha("ld de, {0}", valor);
                Saida.EscreverLinha("or a");
                Saida.EscreverLinha("sbc ix, de");
                Saida.EscreverLinha("ld sp, ix");
            }
        }

        public override int PadraoDesvioArgumentos()
        {
            return 4;
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
                    Saida.EscreverLinha("ld ({0}{1}), a", local ? "iy+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld ({0}{1}), hl", local ? "iy+." : "_", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteSubtraiBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("sub c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("or a");
                    Saida.EscreverLinha("sbc hl, bc");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteSomaAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("add a, ({0}{1})", local ? "iy+." : "_", nome);
                    Saida.EscreverLinha("ld ({0}{1}), a", local ? "iy+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld de, ({0}{1})", local ? "iy+." : "_", nome);
                    Saida.EscreverLinha("add hl, de");
                    Saida.EscreverLinha("ld ({0}{1}), hl", local ? "iy+." : "_", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteSubtraiAEmVar(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("sub a, ({0}{1})", local ? "iy+." : "_", nome);
                    Saida.EscreverLinha("ld ({0}{1}), a", local ? "iy+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld de, ({0}{1})", local ? "iy+." : "_", nome);
                    Saida.EscreverLinha("or a");
                    Saida.EscreverLinha("sbc hl, de");
                    Saida.EscreverLinha("ld ({0}{1}), hl", local ? "iy+." : "_", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteMultiplicaAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDivideAEmVar(Bits bits, bool local, string nome, bool sinal)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCabecalho()
        {
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
                    Saida.EscreverLinha("ld hl, {0}L{1}", local ? "." : "_", rotulo);
                    Saida.EscreverLinha("ld a, l");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, {0}L{1}", local ? "." : "_", rotulo);
                    break;
                default:
                    throw new NotImplementedException();
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
                default:
                    throw new NotImplementedException();
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
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCopiaVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld a, ({0}{1})", local ? "iy+." : "_", nome);
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, ({0}{1})", local ? "iy+." : "_", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCopiaAParaVarPtr(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("ld e, ({0}{1})", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld d, ({0}{1}+1)", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld ix, de");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld (ix), a");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld (ix), hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCopiaVarPtrParaA(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("ld e, ({0}{1})", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld d, ({0}{1}+1)", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld ix, de");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld a, (ix)");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, (ix)");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCopiaEnderecoVarParaA(Bits bits, bool local, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld hl, {0}{1}", local ? "." : "_", nome);
                    Saida.EscreverLinha("ld a, l");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, {0}{1}", local ? "." : "_", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteCopiaEnderecoRotinaParaA(Bits bits, string nome)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ls hl, _{0}", nome);
                    Saida.EscreverLinha("ld a, l");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, _{0}", nome);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteComparacaoBComA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("cp c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("push hl");
                    Saida.EscreverLinha("or a");
                    Saida.EscreverLinha("sbc hl, bc");
                    Saida.EscreverLinha("pop hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDesempilhaB(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("pop bc");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("pop bc");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteDeslocaDireitaBEmA(Bits bits)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public override void EmiteDeslocaEsquerdaBEmA(Bits bits)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteDivideBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteEBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("and c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld a, l");
                    Saida.EscreverLinha("and c");
                    Saida.EscreverLinha("ld l, a");
                    Saida.EscreverLinha("ld a, h");
                    Saida.EscreverLinha("and b");
                    Saida.EscreverLinha("ld h, a");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteModuloBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteMultiplicaBEmA(Bits bits, bool sinal)
        {
            switch (bits)
            {
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteOuBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("or c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld a, l");
                    Saida.EscreverLinha("or c");
                    Saida.EscreverLinha("ld l, a");
                    Saida.EscreverLinha("ld a, h");
                    Saida.EscreverLinha("or b");
                    Saida.EscreverLinha("ld h, a");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteOuExclusivoBEmA(Bits bits)
        {
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("xor c");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld a, l");
                    Saida.EscreverLinha("xor c");
                    Saida.EscreverLinha("ld l, a");
                    Saida.EscreverLinha("ld a, h");
                    Saida.EscreverLinha("xor b");
                    Saida.EscreverLinha("ld h, a");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteCopiaAParaVarPtrIndiceEmB(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("ld e, ({0}{1})", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld d, ({0}{1}+1)", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld ix, de");
            Saida.EscreverLinha("add ix, bc");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld (ix), a");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld (ix), hl");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void EmiteCopiaVarPtrIndiceEmBParaA(Bits bits, bool local, string nome)
        {
            Saida.EscreverLinha("ld e, ({0}{1})", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld d, ({0}{1}+1)", local ? "iy+." : "_", nome);
            Saida.EscreverLinha("ld ix, de");
            Saida.EscreverLinha("add ix, bc");
            switch (bits)
            {
                case Bits.Bits8:
                    Saida.EscreverLinha("ld a, (ix)");
                    break;
                case Bits.Bits16:
                    Saida.EscreverLinha("ld hl, (ix)");
                    break;
                default:
                    throw new NotImplementedException();
            }
 
        }

        public override bool ChamarAsmLink(string arqAsm, string arqSaida)
        {
            return false;
        }
    }
}