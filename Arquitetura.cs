namespace hl
{
    public enum Bits
    {
        Nenhum = 0,
        Bits8 = 1,
        Bits16 = 2,
        Bits32 = 4,
        Bits64 = 8
    }

    public enum Segmento
    {
        Codigo,
        DadosInicializados,
        DadosNaoInicializados
    }
    public abstract class Arquitetura
    {
        public abstract string Nome{get;}
        public abstract string NomeSistemaOperacional{get;}
        public SistemaOperacional SistemaOperacional{get;set;}
        public string NomeCompleto => (Nome + "_" + NomeSistemaOperacional).Replace(' ', '_');

        public Ambiente? Ambiente { get; set; }
        public Saida Saida { get; set; }

        public Arquitetura(Saida saida, SistemaOperacional sistemaOperacional)
        {
            Saida = saida;
            this.SistemaOperacional = sistemaOperacional;
        }
        public abstract void Emite(string asm);
        public abstract void EmiteComentario(string comentario);
        public abstract void EmiteDefineA(Bits bits, long valor);
        public abstract void EmiteDefineB(Bits bits, long valor);
        public abstract void EmiteCopiaAParaB(Bits bits);
        public abstract void EmiteCopiaEnderecoRotuloParaA(Bits bits, int rotulo, bool local);
        public abstract void EmiteEmpilhaA(Bits bits);
        public abstract void EmiteDesempilhaA(Bits bits);
        public abstract void EmiteSomaBEmA(Bits bits);
        public abstract void EmiteSubtraiBEmA(Bits bits);
        public abstract void EmiteMultiplicaBEmA(Bits bits, bool sinal);
        public abstract void EmiteDivideBEmA(Bits bits, bool sinal);
        public abstract void EmiteModuloBEmA(Bits bits, bool sinal);
        public abstract void EmiteCopiaAParaVar(Bits bits, bool local, string nome);
        public abstract void EmiteCopiaVarParaA(Bits bits, bool local, string nome);
        public abstract void EmiteCopiaAParaVarPtr(Bits bits, bool local, string nome);
        public abstract void EmiteCopiaVarPtrParaA(Bits bits, bool local, string nome);
        public abstract void EmiteCopiaEnderecoVarParaA(Bits bits, bool local, string nome);
        public abstract void EmiteCopiaEnderecoRotinaParaA(Bits bits, string nome);
        public abstract void EmiteSomaAEmVar(Bits bits, bool local, string nome);
        public abstract void EmiteSubtraiAEmVar(Bits bits, bool local, string nome);
        public abstract void EmiteMultiplicaAEmVar(Bits bits, bool local, string nome, bool sinal);
        public abstract void EmiteDivideAEmVar(Bits bits, bool local, string nome, bool sinal);

        public abstract void EmiteFuncao(bool publica, string nome, Bits bitsRetorno);
        public abstract void EmiteFimFuncao(string nome, Bits bitsRetorno);
        public abstract void EmiteRotulo(int rotulo, bool local);
        public abstract void EmitePulaParaRotulo(int rotulo, bool local);
        public abstract void EmitePulaSeIgualRotulo(int rotulo, bool local);
        public abstract void EmitePulaSeDiferenteRotulo(int rotulo, bool local);
        public abstract void EmitePulaSeMaiorQueRotulo(int rotulo, bool local, bool sinal);
        public abstract void EmitePulaSeMenorQueRotulo(int rotulo, bool local, bool sinal);
        public abstract void EmitePulaSeMaiorIgualRotulo(int rotulo, bool local, bool sinal);
        public abstract void EmitePulaSeMenorIgualRotulo(int rotulo, bool local, bool sinal);
        public abstract void EmiteSomaValorEmPtrPilha(long valor);
        public abstract void EmiteSubtraiValorEmPtrPilha(long valor);
        public abstract void EmiteConstanteNumerica(bool local, string nome, long valor);
        public abstract void EmiteChamar(string nome);
        public abstract void EmiteMudarSegmento(Segmento seg);
        public abstract void EmiteString(string conteudo);
        public abstract void EmiteBinario(byte[] conteudo);
        public abstract void EmiteNumero(Bits bits, long conteudo);
        public abstract void EmiteDesempilhaB(Bits bits);
        public abstract void EmiteComparacaoBComA(Bits bits);
        public abstract void EmiteEBEmA(Bits bits);
        public abstract void EmiteOuBEmA(Bits bits);
        public abstract void EmiteOuExclusivoBEmA(Bits bits);
        public abstract void EmiteDeslocaEsquerdaBEmA(Bits bits);
        public abstract void EmiteDeslocaDireitaBEmA(Bits bits);

        public abstract Bits BitsPadrao();
        public abstract Bits BitsPonteiro();
        public abstract Bits BitsTipo(Tipo tipo);
        public abstract int PadraoDesvioVariaveis();
        public abstract int PadraoDesvioArgumentos();
        public abstract void EmiteCabecalho();
        public abstract void EmiteRodape();
    }
}