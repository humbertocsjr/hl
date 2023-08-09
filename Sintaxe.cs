using hl.Nos;

namespace hl
{
    public class Sintaxe
    {

        private static No? Expr6(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = null;
            switch (cod.Current.Tipo)
            {
                case TipoTrecho.IdPtr:
                case TipoTrecho.Id:
                    {
                        var itrecho = cod.Current;
                        var iptr = cod.Current.Tipo == TipoTrecho.IdPtr;
                        if(iptr) cod.MoveNext();
                        var imodulo = cod.Current.Fonte.Nome.ToUpper();
                        var inome = cod.Current.ConteudoMaiusculo;
                        cod.MoveNext();
                        if(cod.Current.Tipo == TipoTrecho.Ponto)
                        {
                            cod.MoveNext();
                            imodulo = inome;
                            cod.Current.ExigeId();
                            inome = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                        }
                        if(cod.Current.Tipo == TipoTrecho.ParentesesAbre)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            ret = new LeiaVariavelNo(itrecho, imodulo, inome, false, 0, iptr);
                        }

                    }
                    break;
                case TipoTrecho.Numero:
                    {
                        ret = new NumeroNo(cod.Current, cod.Current.ConteudoNumero);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.NumeroHex:
                    {
                        ret = new NumeroNo(cod.Current, cod.Current.ConteudoNumeroHex);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.NumeroBin:
                    {
                        ret = new NumeroNo(cod.Current, cod.Current.ConteudoNumeroBin);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.String:
                    {
                        ret = new StringNo(cod.Current, cod.Current.Conteudo);
                        cod.MoveNext();
                    }
                    break;
            }
            return ret;
        }

        private static No? Expr5(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr6(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelBinario)
            {
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.BinarioE:
                        {
                            BinarioENo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.BinarioOu:
                        {
                            BinarioOuNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.BinarioOuExclusivo:
                        {
                            BinarioOuExclusivoNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.BinarioDeslocarDireita:
                        {
                            BinarioDeslocaDireitaNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.BinarioDeslocarEsquerda:
                        {
                            BinarioDeslocaEsquerdaNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                }
            }
            return ret;
        }

        private static No? Expr4(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr5(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelMultiplicacao)
            {
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.MatMultiplicacao:
                        {
                            MultiplicacaoNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.MatDivisao:
                        {
                            DivisaoNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                    case TipoTrecho.MatModulo:
                        {
                            ModuloNo op = new (cod.Current);
                            cod.MoveNext();
                            op.Abaixo.Add(ret);
                            ret = Expr5(amb, cod);
                            if(ret != null)op.Abaixo.Add(ret);
                            ret = op;
                        }
                        break;
                }
            }
            return ret;
        }

        private static No? Expr3(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr4(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelSoma)
            {
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.MatSoma:
                        SomaNo soma = new (cod.Current);
                        cod.MoveNext();
                        soma.Abaixo.Add(ret);
                        ret = Expr4(amb, cod);
                        if(ret != null)soma.Abaixo.Add(ret);
                        ret = soma;
                        break;
                    case TipoTrecho.MatSubtracao:
                        SubtracaoNo sub = new (cod.Current);
                        cod.MoveNext();
                        sub.Abaixo.Add(ret);
                        ret = Expr4(amb, cod);
                        if(ret != null)sub.Abaixo.Add(ret);
                        ret = sub;
                        break;
                }
            }
            return ret;
        }

        private static No? Expr2(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr3(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelComparacao)
            {
                TipoComparacao ctipo = TipoComparacao.Igual;
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.LogicaIgual:
                        ctipo = TipoComparacao.Igual;
                        break;
                    case TipoTrecho.LogicaDiferente:
                        ctipo = TipoComparacao.Diferente;
                        break;
                    case TipoTrecho.LogicaMaiorOuIgual:
                        ctipo = TipoComparacao.MaiorOuIgual;
                        break;
                    case TipoTrecho.LogicaMaiorQue:
                        ctipo = TipoComparacao.MaiorQue;
                        break;
                    case TipoTrecho.LogicaMenorOuIgual:
                        ctipo = TipoComparacao.MenorOuIgual;
                        break;
                    case TipoTrecho.LogicaMenorQue:
                        ctipo = TipoComparacao.MenorQue;
                        break;

                }
                cod.MoveNext();
                ComparacaoNo op = new (cod.Current, ctipo, ret, Expr3(amb, cod) ?? throw new Erro(cod.Current, "Esperado argumnto da comparacao"));
                ret = op;
            }
            return ret;
        }

        private static No? Expr1(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr2(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelOu)
            {
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.LogicaOu:
                        LogicaOuNo logou = new (cod.Current);
                        cod.MoveNext();
                        logou.Abaixo.Add(ret);
                        ret = Expr2(amb, cod);
                        if(ret != null)logou.Abaixo.Add(ret);
                        ret = logou;
                        break;
                }
            }
            return ret;
        }

        private static No? Expr(Ambiente amb, LinhaDeCodigo cod)
        {
            No? ret = Expr1(amb, cod);
            if(ret == null) return ret;
            while(cod.Current.TipoNivelE)
            {
                switch (cod.Current.Tipo)
                {
                    case TipoTrecho.LogicaE:
                        LogicaENo loge = new (cod.Current);
                        cod.MoveNext();
                        loge.Abaixo.Add(ret);
                        ret = Expr1(amb, cod);
                        if(ret != null)loge.Abaixo.Add(ret);
                        ret = loge;
                        break;
                }
            }
            return ret;
        }

        public static No? ProcessarComando(Ambiente amb, LinhasDeCodigo fonte, LinhaDeCodigo cod)
        {
            No? ret = null;
            var atual = cod.Current;
            bool publica = cod.Current.Tipo == TipoTrecho.IdPublic;
            if(publica) cod.MoveNext();

            switch (cod.Current.Tipo)
            {
                case TipoTrecho.IdWhile:
                    {
                        cod.MoveNext();
                        var wcmp = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado uma condição para repetição");
                        ret = new EnquantoNo(atual, wcmp, ProcessarComando(amb, fonte, cod)  ?? throw new Erro(cod.Current, "Esperado o conteudo da repetição"));
                    }
                    break;
                case TipoTrecho.IdUntil:
                    {
                        cod.MoveNext();
                        var ucmp = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado uma condição para repetição");
                        ret = new AteNo(atual, ucmp, ProcessarComando(amb, fonte, cod)  ?? throw new Erro(cod.Current, "Esperado o conteudo da repetição"));
                    }
                    break;
                case TipoTrecho.IdFor:
                    {
                        cod.MoveNext();
                        if(cod.Current.Tipo == TipoTrecho.IdEach)
                        {
                            cod.MoveNext();
                            cod.Current.ExigeId();
                            var feindice = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                            cod.Current.Exige(TipoTrecho.Virgula);
                            cod.MoveNext();
                            cod.Current.ExigeId();
                            var feitem = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                            cod.Current.Exige(TipoTrecho.IdIn);
                            cod.MoveNext();
                            cod.Current.ExigeId();
                            var feorigem = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                            ret = new ParaCadaNo(atual, feindice, feitem, feorigem, ProcessarComando(amb, fonte, cod)  ?? throw new Erro(cod.Current, "Esperado o conteudo da repetição"));
                        }
                        else
                        {
                            cod.Current.ExigeId();
                            var flvar = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                            cod.Current.Exige(TipoTrecho.Atribuicao);
                            cod.MoveNext();
                            var flde = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado o valor inicial da repetição");
                            cod.Current.Exige(TipoTrecho.IdTo);
                            cod.MoveNext();
                            var flpara = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado o valor final da repetição");
                            No flpasso = new NumeroNo(cod.Current, 1);
                            if(cod.Current.Tipo == TipoTrecho.IdStep)
                            {
                                cod.MoveNext();
                                flpasso = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado o tamanho do passo da repetição");
                            }
                            ret = new ParaNo(atual, flvar, flde, flpara, flpasso, ProcessarComando(amb, fonte, cod)  ?? throw new Erro(cod.Current, "Esperado o conteudo da repetição"));

                        }
                    }
                    break;
                case TipoTrecho.IdIf:
                case TipoTrecho.IdIfTarget:
                    {
                        var ifasm = cod.Current.Tipo == TipoTrecho.IdIfTarget;
                        cod.MoveNext();
                        var ifcmp = Expr(amb, cod) ?? throw new Erro(cod.Current, "Esperado comparacao");
                        No? ifnao = null;
                        No? ifsim;
                        if (cod.Current.Tipo == TipoTrecho.IdDo)
                        {
                            cod.MoveNext();
                            BlocoNo ifbloco = new(fonte.Current.First());
                            ifsim = ifbloco;
                            while (fonte.MoveNext())
                            {
                                No? no = ProcessarTrechos(amb, fonte, fonte.Current);
                                if (no != null)
                                {
                                    if (no is FimBloco)
                                    {
                                        break;
                                    }
                                    else if (no is SeNaoNo)
                                    {
                                        ifbloco = new(fonte.Current.First());
                                        ifnao = ifbloco;
                                    }
                                    else ifbloco.Abaixo.Add(no);
                                }
                            }
                        }
                        else
                        {
                            ifsim = ProcessarComando(amb, fonte, cod);
                            if (cod.Current.Tipo == TipoTrecho.IdElse)
                            {
                                cod.MoveNext();
                                ifnao = ProcessarComando(amb, fonte, cod);
                            }
                        }
                        if(ifasm)
                            ret = new SeAsmNo(atual, ifcmp is LeiaVariavelNo ifcmpnome ? ifcmpnome.Nome : throw new Erro(ifcmp.Trecho, "Esperado o nome da arquitetura"), ifsim ?? new BlocoNo(fonte.Current.First()), ifnao);
                        else
                            ret = new SeNo(atual, ifcmp, ifsim ?? new BlocoNo(fonte.Current.First()), ifnao);
                    }
                    break;
                case TipoTrecho.IdElse:
                    {
                        ret = new SeNaoNo(atual);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.IdFunc:
                case TipoTrecho.IdProc:
                    {
                        bool fret = cod.Current.Tipo == TipoTrecho.IdFunc;
                        cod.MoveNext();
                        cod.Current.ExigeId();
                        var fnome = cod.Current.ConteudoMaiusculo;
                        cod.MoveNext();
                        Tipo ftipo = Tipo.Desconhecido;
                        List<DeclaraVariavelNo> fargs = new();
                        if(cod.Current.Tipo == TipoTrecho.ParentesesAbre)
                        {
                            while(cod.MoveNext() && cod.Current.Tipo != TipoTrecho.ParentesesFecha)
                            {
                                if(cod.Current.Tipo == TipoTrecho.ParametrosVariaveis)
                                {
                                    cod.Next?.Exige(TipoTrecho.ParentesesFecha);
                                    fargs.Add(new DeclaraVariavelNo(atual, "__PARAMS__", Tipo.ParametrosVariaveis, false, 0, false, null));
                                }
                                else
                                {
                                    cod.Current.ExigeId();
                                    var anome = cod.Current.ConteudoMaiusculo;
                                    cod.MoveNext();
                                    cod.Current.Exige(TipoTrecho.IdAs);
                                    cod.MoveNext();
                                    var aptr = false;
                                    if(cod.Current.Tipo == TipoTrecho.IdPtr)
                                    {
                                        aptr = true;
                                        cod.MoveNext();
                                    }
                                    var atipo = cod.Current.ConteudoTipo;
                                    fargs.Add(new DeclaraVariavelNo(atual, anome, atipo, false, 0, aptr, null));
                                    if(cod.Next?.Tipo != TipoTrecho.ParentesesFecha)
                                    {
                                        cod.MoveNext();
                                        cod.Current.Exige(TipoTrecho.Virgula);
                                    }
                                }
                            }
                            cod.Current.Exige(TipoTrecho.ParentesesFecha);
                            cod.MoveNext();
                        }
                        if(fret)
                        {
                            cod.Current.Exige(TipoTrecho.IdAs);
                            cod.MoveNext();
                            cod.Current.ExigeTipo();
                            ftipo = cod.Current.ConteudoTipo;
                            cod.MoveNext();
                        }
                        var funcao = new FuncaoNo(atual, publica, fnome, ftipo, false, fargs);
                        var fconteudo = ProcessarComando(amb, fonte, cod);
                        if(fconteudo != null)
                        { 
                            funcao.Abaixo.Add(fconteudo);
                            amb.Funcoes.Add(funcao);
                        }
                        else
                        {
                            amb.FuncoesDeclaradas.Add(funcao);
                        }
                    }
                    break;
                case TipoTrecho.Id:
                case TipoTrecho.Ponteiro:
                    {
                        bool iptr = cod.Current.Tipo == TipoTrecho.Ponteiro;
                        if(iptr)cod.MoveNext();
                        string imodulo = cod.Current.Fonte.Nome.ToUpper();
                        string inome = cod.Current.ConteudoMaiusculo;
                        cod.MoveNext();
                        if(cod.Current.Tipo == TipoTrecho.Ponto)
                        {
                            imodulo = inome;
                            cod.MoveNext();
                            cod.Current.ExigeId();
                            inome = cod.Current.ConteudoMaiusculo;
                            cod.MoveNext();
                        }
                        if(cod.Current.TipoAtribuicao)
                        {
                            TipoTrecho iatrib = cod.Current.Tipo;
                            cod.MoveNext();
                            No? ivalor = Expr(amb, cod);
                            ret = new AtribuicaoNo(atual, iatrib, imodulo, inome, false, 0, iptr, ivalor);
                        }
                        else
                        {
                            bool iparenteses = cod.Current.Tipo == TipoTrecho.ParentesesAbre;
                            List<No> iargs = new ();
                            if(iparenteses) cod.MoveNext();
                            while(cod.Current.Tipo != TipoTrecho.FimDaLinha && (!iparenteses || cod.Current.Tipo != TipoTrecho.ParentesesFecha))
                            {
                                var iarg = Expr(amb, cod);
                                if(iarg != null) iargs.Add(iarg);
                                if(!(iparenteses && cod.Current.Tipo == TipoTrecho.ParentesesFecha))
                                {
                                    if(cod.Current.Tipo == TipoTrecho.FimDaLinha) break;
                                    cod.Current.Exige(TipoTrecho.Virgula);
                                    cod.MoveNext();
                                }
                            }
                            if(iparenteses)
                            {
                                cod.Current.Exige(TipoTrecho.ParentesesFecha);
                                cod.MoveNext();
                            }
                            ret = new ChamarFuncNo(atual, imodulo, inome, iargs);
                        }
                    }
                    break;
                case TipoTrecho.IdAsm:
                    {
                        cod.MoveNext();
                        cod.Current.Exige(TipoTrecho.String, TipoTrecho.Id);
                        ret = new AsmNo(cod.Current, cod.Current.Conteudo, cod.Current.Tipo == TipoTrecho.Id);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.IdImport:
                    {
                        cod.MoveNext();
                        cod.Current.Exige(TipoTrecho.String);
                        var endereco = cod.Current.Conteudo;
                        if(File.Exists(endereco))
                        {
                            amb.Adicionar(endereco);
                        }
                        else if(!Path.IsPathRooted(endereco) && File.Exists(Path.Combine(Path.GetDirectoryName(cod.Current.Fonte.Endereco) ?? "", endereco)))
                        {
                            amb.Adicionar(Path.Combine(Path.GetDirectoryName(cod.Current.Fonte.Endereco) ?? "", endereco));
                        }
                        else throw new Erro(cod.Current, "Arquivo não encontrado");
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.IdDo:
                    {
                        ret = ProcessarBloco(amb, fonte);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.IdEnd:
                    {
                        ret = new FimBloco(cod.Current);
                        cod.MoveNext();
                    }
                    break;
                case TipoTrecho.IdVar:
                    {
                        cod.MoveNext();
                        cod.Current.ExigeId();
                        var vnome = cod.Current.ConteudoMaiusculo;
                        cod.MoveNext();
                        cod.Current.Exige(TipoTrecho.IdAs);
                        cod.MoveNext();
                        var vptr = false;
                        if(cod.Current.Tipo == TipoTrecho.IdPtr)
                        {
                            vptr = true;
                            cod.MoveNext();
                        }
                        var vtipo = cod.Current.ConteudoTipo;
                        cod.MoveNext();
                        No? vinicial = null;
                        if(cod.Current.Tipo == TipoTrecho.Atribuicao)
                        {
                            cod.MoveNext();
                            vinicial = Expr(amb, cod);
                        }
                        ret = new DeclaraVariavelNo(atual, vnome, vtipo, false, 0, vptr, vinicial);
                    }
                    break;
            }
            cod.Current.Exige(TipoTrecho.FimDaLinha);
            return ret;
        }

        public static No? ProcessarTrechos(Ambiente amb, LinhasDeCodigo fonte, LinhaDeCodigo cod)
        {
            No? ret = null;
            if(!cod.Any()) return ret;
            cod.MoveNext();
            ret = ProcessarComando(amb, fonte, cod);
            return ret; 
        }

        public static BlocoNo ProcessarBloco(Ambiente amb, LinhasDeCodigo fonte)
        {
            BlocoNo ret = new (fonte.Current.First());
            while(fonte.MoveNext())
            {
                No? no = ProcessarTrechos(amb, fonte, fonte.Current);
                if(no != null)
                {
                    if(no is FimBloco)
                    {
                        break;
                    }
                    ret.Abaixo.Add(no);
                }
            }
            return ret;
        }
        public static void Processar(Ambiente amb, Fonte fonte)
        {
            BlocoNo bloco = ProcessarBloco(amb, (LinhasDeCodigo)fonte.GetEnumerator());
            if(bloco.Abaixo.Any())
            {
                amb.Funcoes.Add(new FuncaoNo(bloco.Trecho, false, "MAIN", Tipo.Desconhecido, false, new List<DeclaraVariavelNo>()));
            }
        }
    }
}