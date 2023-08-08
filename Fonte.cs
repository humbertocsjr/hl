using System.Collections;

namespace hl
{
    public class LinhaDeCodigo : IEnumerator<Trecho>
    {
        List<Trecho> _trechos;
        int _atual = -1;

        public LinhaDeCodigo(List<Trecho> trechos)
        {
            _trechos = trechos;
        }
        public Trecho Current => _trechos[_atual];
        public Trecho? Next => _atual >= _trechos.Count ? null : _trechos[_atual + 1];
        public Trecho? Prev => _atual == 0 ? null : _trechos[_atual - 1];

        object IEnumerator.Current => _trechos[_atual];

        public bool Any()
        {
            return _trechos.Any();
        }

        public Trecho First()
        {
            return _trechos.First();
        }

        public List<Trecho> ToList()
        {
            return _trechos.ToList();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if((_atual + 1) >= _trechos.Count) return false;
            _atual++;
            return true;
        }

        public void Reset()
        {
            _atual = -1;
        }
    }

    public class LinhasDeCodigo : IEnumerator<LinhaDeCodigo>
    {
        String[] _linhas;
        Fonte _fonte;
        int _atual = -1;

        public LinhasDeCodigo(String[] linhas, Fonte fonte)
        {
            _linhas = linhas;
            _fonte = fonte;
        }

        public LinhasDeCodigo(List<string> linhas, Fonte fonte)
        {
            _linhas = linhas.ToArray();
            _fonte = fonte;
        }

        private List<Trecho> Processar()
        {
            List<Trecho> ret = new();
            Trecho? atual = null;
            int ptr = _atual == -1 ? 0 : _atual;
            for (int i = 0; i < _linhas[ptr].Length; i++)
            {
                char c = _linhas[ptr][i];
                char p = (i + 1) < _linhas[ptr].Length ? _linhas[ptr][i+1] : (char)0;
                int linha = ptr + 1;
                int coluna = i + 1;

                if(atual == null)
                {
                    if(char.IsLetter(c) || c == '_')
                    {
                        ret.Add(atual = new Trecho(_fonte, TipoTrecho.Id, linha, coluna, c));
                    }
                    else if(char.IsDigit(c))
                    {
                        ret.Add(atual = new Trecho(_fonte, TipoTrecho.Numero, linha, coluna, c));
                    }
                    else if(c == ' ' || c == '\t') {}
                    else if(c == '"') ret.Add(atual = new Trecho(_fonte, TipoTrecho.String, linha, coluna));
                    else if(c == '+') ret.Add(atual = new Trecho(_fonte, TipoTrecho.MatSoma, linha, coluna, c));
                    else if(c == '-') ret.Add(atual = new Trecho(_fonte, TipoTrecho.MatSubtracao, linha, coluna, c));
                    else if(c == '/') ret.Add(atual = new Trecho(_fonte, TipoTrecho.MatDivisao, linha, coluna, c));
                    else if(c == '*') ret.Add(atual = new Trecho(_fonte, TipoTrecho.MatMultiplicacao, linha, coluna, c));
                    else if(c == '%') ret.Add(atual = new Trecho(_fonte, TipoTrecho.MatModulo, linha, coluna, c));
                    else if(c == '<') ret.Add(atual = new Trecho(_fonte, TipoTrecho.LogicaMenorQue, linha, coluna, c));
                    else if(c == '>') ret.Add(atual = new Trecho(_fonte, TipoTrecho.LogicaMaiorQue, linha, coluna, c));
                    else if(c == '^') ret.Add(new Trecho(_fonte, TipoTrecho.BinarioOuExclusivo, linha, coluna, c));
                    else if(c == '~') ret.Add(new Trecho(_fonte, TipoTrecho.BinarioNao, linha, coluna, c));
                    else if(c == '@') ret.Add(new Trecho(_fonte, TipoTrecho.Ponteiro, linha, coluna, c));
                    else if(c == '(') ret.Add(new Trecho(_fonte, TipoTrecho.ParentesesAbre, linha, coluna, c));
                    else if(c == ')') ret.Add(new Trecho(_fonte, TipoTrecho.ParentesesFecha, linha, coluna, c));
                    else if(c == '[') ret.Add(new Trecho(_fonte, TipoTrecho.IndiceAbre, linha, coluna, c));
                    else if(c == ']') ret.Add(new Trecho(_fonte, TipoTrecho.IndiceFecha, linha, coluna, c));
                    else if(c == ',') ret.Add(new Trecho(_fonte, TipoTrecho.Virgula, linha, coluna, c));
                    else if(c == '.') ret.Add(atual = new Trecho(_fonte, TipoTrecho.Ponto, linha, coluna, c));
                    else if(c == '=') ret.Add(atual = new Trecho(_fonte, TipoTrecho.Atribuicao, linha, coluna, c));
                    else if(c == '!') ret.Add(atual = new Trecho(_fonte, TipoTrecho.LogicaNegacao, linha, coluna, c));
                    else throw new Erro(new Trecho(_fonte, TipoTrecho.MatModulo, linha, coluna, c), "Caractere não reconhecido");
                }
                else
                {
                    switch (atual.Tipo)
                    {
                        case TipoTrecho.BinarioE:
                            if(c == '>' || c == '=')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                if(atual.Conteudo == "&&") atual.Tipo = TipoTrecho.LogicaE;
                                else if(atual.Conteudo == "&") atual.Tipo = TipoTrecho.BinarioE;
                                else throw new Erro(atual, "Operação desconhecida");
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.BinarioOu:
                            if(c == '>' || c == '=')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                if(atual.Conteudo == "||") atual.Tipo = TipoTrecho.LogicaOu;
                                else if(atual.Conteudo == "|") atual.Tipo = TipoTrecho.BinarioOu;
                                else throw new Erro(atual, "Operação desconhecida");
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.LogicaMaiorQue:
                            if(c == '>' || c == '=')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                if(atual.Conteudo == ">>") atual.Tipo = TipoTrecho.BinarioDeslocarDireita;
                                else if(atual.Conteudo == ">=") atual.Tipo = TipoTrecho.LogicaMaiorOuIgual;
                                else if(atual.Conteudo == ">") atual.Tipo = TipoTrecho.LogicaMaiorQue;
                                else throw new Erro(atual, "Operação desconhecida");
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.LogicaMenorQue:
                            if(c == '<' || c == '>' || c == '=')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                if(atual.Conteudo == "<<") atual.Tipo = TipoTrecho.BinarioDeslocarEsquerda;
                                else if(atual.Conteudo == "<=") atual.Tipo = TipoTrecho.LogicaMenorOuIgual;
                                else if(atual.Conteudo == "<>") atual.Tipo = TipoTrecho.LogicaDiferente;
                                else if(atual.Conteudo == "<") atual.Tipo = TipoTrecho.LogicaMenorQue;
                                else throw new Erro(atual, "Operação desconhecida");
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.LogicaDiferente:
                            if(c == '<' || c == '>' || c == '=')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                if(atual.Conteudo == "!") atual.Tipo = TipoTrecho.LogicaNegacao;
                                else if(atual.Conteudo == "!=") atual.Tipo = TipoTrecho.LogicaDiferente;
                                else throw new Erro(atual, "Operação desconhecida");
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.Ponto:
                            {
                                if(c == '.')
                                {
                                    atual.Conteudo += c;
                                }
                                else
                                {
                                    if(atual.Conteudo == ".")
                                        atual.Tipo = TipoTrecho.Ponto;
                                    else if(atual.Conteudo == "..")
                                        atual.Tipo = TipoTrecho.Intervalo;
                                    else if(atual.Conteudo == "...")
                                        atual.Tipo = TipoTrecho.ParametrosVariaveis;
                                    else throw new Erro(atual, "Trecho invalido");
                                    i--;
                                    atual = null;
                                }
                            }
                            break;
                        case TipoTrecho.String:
                            {
                                if(c == '\\')
                                {
                                    i++;
                                    switch(p)
                                    {
                                        case 'n':
                                            atual.Conteudo += '\n';
                                            break;
                                        case 'r':
                                            atual.Conteudo += '\r';
                                            break;
                                        case 't':
                                            atual.Conteudo += '\t';
                                            break;
                                        case '0':
                                            atual.Conteudo += '\0';
                                            break;
                                        default:
                                            atual.Conteudo += p;
                                            break;
                                    }
                                }
                                else if(c != '"')
                                {
                                    atual.Conteudo += c;
                                }
                                else
                                {
                                    atual = null;
                                }
                            }
                            break;
                        case TipoTrecho.Atribuicao:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.LogicaIgual;
                                atual = null;
                            }
                            else
                            {
                                atual = null;
                                i--;
                            }
                            break;
                        case TipoTrecho.Id:
                            if(char.IsLetterOrDigit(c) || c == '_')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.Numero:
                            if(c == 'x' && atual.Conteudo == "0")
                            {
                                atual.Conteudo = "";
                                atual.Tipo = TipoTrecho.NumeroHex;
                            }
                            else if(c == 'b' && atual.Conteudo == "0")
                            {
                                atual.Conteudo = "";
                                atual.Tipo = TipoTrecho.NumeroBin;
                            }
                            else if(char.IsDigit(c))
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.NumeroBin:
                            if(c == '0' || c == '1')
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.NumeroHex:
                            if((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
                            {
                                atual.Conteudo += c;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.MatSoma:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.AtribSoma;
                                atual = null;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.MatSubtracao:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.AtribSubtracao;
                                atual = null;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.MatDivisao:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.AtribDivisao;
                                atual = null;
                            }
                            else if(c == '/')
                            {
                                atual.Tipo = TipoTrecho.Comentario;
                                atual.Conteudo = "";
                                ret.Remove(atual);
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.MatMultiplicacao:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.AtribMultiplicacao;
                                atual = null;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.MatModulo:
                            if(c == '=')
                            {
                                atual.Tipo = TipoTrecho.AtribModulo;
                                atual = null;
                            }
                            else
                            {
                                atual = null;
                                i --;
                            }
                            break;
                        case TipoTrecho.Comentario:
                            atual.Conteudo += c;
                            break;
                        default:
                            throw new Erro(atual, "Trecho não implementado");
                    }
                }
            }
            foreach (var item in ret)
            {
                if(item.IgualId("if")) item.Tipo = TipoTrecho.IdIf;
                else if(item.IgualId("do")) item.Tipo = TipoTrecho.IdDo;
                else if(item.IgualId("end")) item.Tipo = TipoTrecho.IdEnd;
                else if(item.IgualId("else")) item.Tipo = TipoTrecho.IdElse;
                else if(item.IgualId("func")) item.Tipo = TipoTrecho.IdFunc;
                else if(item.IgualId("proc")) item.Tipo = TipoTrecho.IdProc;
                else if(item.IgualId("while")) item.Tipo = TipoTrecho.IdWhile;
                else if(item.IgualId("loop")) item.Tipo = TipoTrecho.IdLoop;
                else if(item.IgualId("until")) item.Tipo = TipoTrecho.IdUntil;
                else if(item.IgualId("select")) item.Tipo = TipoTrecho.IdSelect;
                else if(item.IgualId("case")) item.Tipo = TipoTrecho.IdCase;
                else if(item.IgualId("return")) item.Tipo = TipoTrecho.IdReturn;
                else if(item.IgualId("exit")) item.Tipo = TipoTrecho.IdExit;
                else if(item.IgualId("import")) item.Tipo = TipoTrecho.IdImport;
                else if(item.IgualId("struct")) item.Tipo = TipoTrecho.IdStruct;
                else if(item.IgualId("for")) item.Tipo = TipoTrecho.IdFor;
                else if(item.IgualId("each")) item.Tipo = TipoTrecho.IdEach;
                else if(item.IgualId("in")) item.Tipo = TipoTrecho.IdIn;
                else if(item.IgualId("sizeof")) item.Tipo = TipoTrecho.IdSizeOf;
                else if(item.IgualId("addressof")) item.Tipo = TipoTrecho.IdAddressOf;
                else if(item.IgualId("capacityof")) item.Tipo = TipoTrecho.IdCapacityOf;
                else if(item.IgualId("typeof")) item.Tipo = TipoTrecho.IdTypeOf;
                else if(item.IgualId("as")) item.Tipo = TipoTrecho.IdAs;
                else if(item.IgualId("var")) item.Tipo = TipoTrecho.IdVar;
                else if(item.IgualId("asm")) item.Tipo = TipoTrecho.IdAsm;
                else if(item.IgualId("ptr")) item.Tipo = TipoTrecho.IdPtr;
                else if(item.IgualId("public")) item.Tipo = TipoTrecho.IdPublic;
                else if(item.IgualId("if")) item.Tipo = TipoTrecho.IdIf;
                else if(item.IgualId("else")) item.Tipo = TipoTrecho.IdElse;
                else if(item.IgualId("iftarget")) item.Tipo = TipoTrecho.IdIfTarget;
                else if(item.IgualId("to")) item.Tipo = TipoTrecho.IdTo;
                else if(item.IgualId("step")) item.Tipo = TipoTrecho.IdStep;
                else if(item.IgualId("string")) item.Tipo = TipoTrecho.TipoString;
                else if(item.IgualId("int8")) item.Tipo = TipoTrecho.TipoInt8;
                else if(item.IgualId("int16")) item.Tipo = TipoTrecho.TipoInt16;
                else if(item.IgualId("int32")) item.Tipo = TipoTrecho.TipoInt32;
                else if(item.IgualId("uint8")) item.Tipo = TipoTrecho.TipoUInt8;
                else if(item.IgualId("uint16")) item.Tipo = TipoTrecho.TipoUInt16;
                else if(item.IgualId("uint32")) item.Tipo = TipoTrecho.TipoUInt32;
            }
            ret.Add(new Trecho(_fonte, TipoTrecho.FimDaLinha, ptr + 1, _linhas[ptr].Length));
            return ret;
        }

        public LinhaDeCodigo Current => new LinhaDeCodigo(Processar());

        object IEnumerator.Current => new LinhaDeCodigo(Processar());

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if((_atual + 1) >= _linhas.Length) return false;
            _atual++;
            return true;
        }

        public void Reset()
        {
            _atual = -1;
        }
    }
    public class Fonte : IEnumerable<LinhaDeCodigo>
    {
        String[] _linhas;

        public string Nome { get; private set; }
        public string Endereco { get; private set; }

        public Fonte(string endereco)
        {
            _linhas = File.ReadAllLines(endereco);
            Nome = Path.GetFileNameWithoutExtension(endereco);
            Endereco = Path.GetFullPath(endereco);
        }

        public IEnumerator<LinhaDeCodigo> GetEnumerator()
        {
            return new LinhasDeCodigo(_linhas, this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LinhasDeCodigo(_linhas, this);
        }
    }

}
