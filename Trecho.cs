namespace hl
{

    public enum TipoTrecho
    {
        Desconhecido,
        Comentario,
        FimDaLinha,
        Id,
        Numero,
        NumeroHex,
        NumeroBin,
        String,
        Virgula,
        Ponto,
        Intervalo,
        ParametrosVariaveis,
        Ponteiro,
        Atribuicao,
        AtribSoma,
        AtribSubtracao,
        AtribDivisao,
        AtribMultiplicacao,
        AtribModulo,
        MatSoma,
        MatSubtracao,
        MatDivisao,
        MatMultiplicacao,
        MatModulo,
        MatIncremento,
        MatDecremento,
        ParentesesAbre,
        ParentesesFecha,
        IndiceAbre,
        IndiceFecha,
        BinarioE,
        BinarioOu,
        BinarioNao,
        BinarioOuExclusivo,
        BinarioDeslocarEsquerda,
        BinarioDeslocarDireita,
        LogicaE,
        LogicaOu,
        LogicaMaiorQue,
        LogicaMaiorOuIgual,
        LogicaMenorQue,
        LogicaMenorOuIgual,
        LogicaIgual,
        LogicaDiferente,
        LogicaNegacao,
        IdIf,
        IdDo,
        IdEnd,
        IdElse,
        IdFunc,
        IdProc,
        IdWhile,
        IdLoop,
        IdUntil,
        IdSelect,
        IdCase,
        IdReturn,
        IdExit,
        IdImport,
        IdStruct,
        IdFor,
        IdEach,
        IdIn,
        IdSizeOf,
        IdAddressOf,
        IdCapacityOf,
        IdTypeOf,
        IdAs,
        IdVar,
        IdAsm,
        IdPtr,
        IdPublic,
        IdIfTarget,
        IdTo,
        IdStep,
        TipoString,
        TipoInt8,
        TipoUInt8,
        TipoInt16,
        TipoUInt16,
        TipoInt32,
        TipoUInt32
    }
    public class Trecho
    {
        public Fonte Fonte { get; set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }
        public TipoTrecho Tipo { get; set; }
        public string Conteudo { get; set; } = "";
        public string ConteudoMaiusculo => Conteudo.ToUpper();
        public long ConteudoNumero => Convert.ToInt64(Conteudo);
        public long ConteudoNumeroHex => Convert.ToInt64(Conteudo, 16);
        public long ConteudoNumeroBin => Convert.ToInt64(Conteudo, 1);
        public Tipo ConteudoTipo => 
            Tipo == TipoTrecho.TipoString ? 
                hl.Tipo.String :
            Tipo == TipoTrecho.TipoInt8 ?
                hl.Tipo.Int8 :
            Tipo == TipoTrecho.TipoUInt8 ? 
                hl.Tipo.UInt8 :
            Tipo == TipoTrecho.TipoInt16 ?
                hl.Tipo.Int16 :
            Tipo == TipoTrecho.TipoUInt16 ? 
                hl.Tipo.UInt16 :
            Tipo == TipoTrecho.TipoInt32 ?
                hl.Tipo.Int32 :
            Tipo == TipoTrecho.TipoUInt32 ? 
                hl.Tipo.UInt32 :
            Tipo == TipoTrecho.Id ? 
                hl.Tipo.Struct :
                hl.Tipo.Desconhecido;
        public bool TipoNivelBinario => Tipo == TipoTrecho.BinarioOu || Tipo == TipoTrecho.BinarioOuExclusivo || Tipo == TipoTrecho.BinarioE || Tipo == TipoTrecho.BinarioDeslocarDireita || Tipo == TipoTrecho.BinarioDeslocarEsquerda;
        public bool TipoNivelMultiplicacao => Tipo == TipoTrecho.MatDivisao || Tipo == TipoTrecho.MatMultiplicacao || Tipo == TipoTrecho.MatModulo;
        public bool TipoNivelSoma => Tipo == TipoTrecho.MatSoma || Tipo == TipoTrecho.MatSubtracao;
        public bool TipoNivelComparacao => Tipo == TipoTrecho.LogicaIgual || Tipo == TipoTrecho.LogicaDiferente || Tipo == TipoTrecho.LogicaMaiorOuIgual || Tipo == TipoTrecho.LogicaMaiorQue || Tipo == TipoTrecho.LogicaMenorOuIgual || Tipo == TipoTrecho.LogicaMenorQue;
        public bool TipoNivelOu => Tipo == TipoTrecho.LogicaOu;
        public bool TipoNivelE =>  Tipo == TipoTrecho.LogicaE;
        public bool TipoAtribuicao => 
            Tipo == TipoTrecho.Atribuicao ||
            Tipo == TipoTrecho.AtribSoma ||
            Tipo == TipoTrecho.AtribSubtracao ||
            Tipo == TipoTrecho.AtribMultiplicacao ||
            Tipo == TipoTrecho.AtribDivisao ||
            Tipo == TipoTrecho.AtribModulo;


        public bool Igual(string conteudo)
        {
            return Conteudo == conteudo;
        }

        public bool IgualId(string conteudo)
        {
            return Tipo == TipoTrecho.Id && ConteudoMaiusculo == conteudo.ToUpper();
        }

        public void Exige(params TipoTrecho[] tipo)
        {
            if(!tipo.Contains(Tipo))
            {
                throw new Erro(this, $"Esperado ({string.Join(',',tipo.Select(t => t.ToString()))}) porém encontrado '{Conteudo}'({Tipo})");
            }
        }

        public void ExigeTipo()
        {
            if(ConteudoTipo == hl.Tipo.Desconhecido)
            {
                throw new Erro(this, $"Esperado (Tipo) porém encontrado '{Conteudo}'({Tipo})");
            }
        }

        public void ExigeId()
        {
            if(Tipo != TipoTrecho.Id)
            {
                throw new Erro(this, $"Esperado Identificador(Id) porém encontrado '{Conteudo}'({Tipo})");
            }
        }

        public void ExigeId(string conteudo)
        {
            if(!IgualId(conteudo))
            {
                throw new Erro(this, $"Esperado '{conteudo}'(Id) porém encontrado '{Conteudo}'({Tipo})");
            }
        }

        public Trecho(Fonte fonte, TipoTrecho tipo = TipoTrecho.FimDaLinha, int linha = 0, int coluna = 0)
        {
            Fonte = fonte;
            Tipo = tipo;
            Linha = linha;
            Coluna = coluna;
        }

        public Trecho(Fonte fonte, TipoTrecho tipo = TipoTrecho.FimDaLinha, int linha = 0, int coluna = 0, char? conteudo = null)
        {
            Fonte = fonte;
            Tipo = tipo;
            Linha = linha;
            Coluna = coluna;
            if(conteudo != null) Conteudo = "" + conteudo;
        }
    }
}