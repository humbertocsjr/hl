# HL - Linguagem de Programação H

Esta linguagem tem como objetivo ser uma linguagem de simples aprendizado, que gera executáveis para sistemas antigos ou limitados, também podendo ser utilizada em ambientes modernos.

# Status do projeto

- Compila código 8086/386/x86-64 sendo este último menos otimizado.
- Falta criar a biblioteca padrão da linguagem para todos os destinos que ela suporta.

# Destinos Suportados

| CPU  | Sistema Operacional | Status             |
|------|---------------------|--------------------|
| z80  | DOS (MSX)           | Testes iniciais    |
| z80  | CPM                 | Testes iniciais    |
| i86  | DOS                 | Falta a biblioteca |
| i86  | CPM/86              | Falta a biblioteca |
| i86  | fudebaOS            | Falta a biblioteca |
| i86  | Windows             | Falta a biblioteca |
| i386 | Windows             | Falta a biblioteca |
| i386 | Linux               | Falta a biblioteca |
| x64  | Windows             | Falta a biblioteca |
| x64  | macOS               | Falta a biblioteca |
| x64  | Linux               | Falta a biblioteca |

# Linguagem

Esta linguagem tem como inspiração linguagens modernas e antigas como: QuickBASIC, Python, GoLang, Ruby.

Sempre com o objetivo de simplificar ao maximo a sintaxe e deixa-la o mais simples para aprendizado.

Com essas simplificações todos os comandos, nomes e identificadores não diferenciam maiúsculas de minúsculas.

## Tipos de Dados

A HL suporta tipos de dados rigidos, e suas variaveis devem ser previamente declaradas antes do uso.

| Tipo   | Descrição                                |
|--------|------------------------------------------|
| int8   | Inteiro de 8 bits                        |
| int16  | Inteiro de 16 bits                       |
| int32  | Inteiro de 32 bits                       |
| uint8  | Inteiro de 8 bits sem sinal              |
| uint16 | Inteiro de 16 bits sem sinal             |
| uint32 | Inteiro de 32 bits sem sinal             |

## Comentários

Todo texto após o marcador ```//``` é ignorado pelo compilador, sendo considerado um comentário, exemplo:

```
// Comentário Teste
```

## Variáveis

Toda variável é declarada com um comando ```var``` antes de ser utilizada, conforme sintaxes abaixo:

```
// Declaração de variável sem valor inicial
var NOME as TIPO
// Declaração de variável com valor inicial
var NOME as TIPO = VALOR_INICIAL
```

Após a declaração pode se utilizar a variável por exemplo atrelando um valor a ela:

```
// Atribuição simples de valor a uma variável
NOME = VALOR
```

Exemplo:

```
var A as UInt16
A = 1 + 35
```

## Atribuições

Existem alguns tipos de atribuições, segue abaixo um exemplo de cada atribuição

```
var A as UInt16

// Atribuição simples de valor inicial
A = 1 + 35

// Atribuição somando o valor existente na variável
A += 23

// Atribuição subtraindo o valor existente na variável
A -= 34

// Atribuição multiplicando o valor existente na variável
A *= 2

// Atribuição dividindo o valor existente na variável
A /= 5

// Atribuição modulando o valor existente na variável
A %= 63

// Resultado A = 10
```

## Módulos

Cada arquivo de código fonte é um módulo, onde o nome do arquivo sem extensão, é usado para nomea-lo.

Recomenda-se o uso de CamelCase na nomeclatura e a não utilização de espaços no nome do arquivo.

## Rotinas

O código nesta linguagem é dividido em pequenas rotinas dentro de cada módulo.

As rotinas são divididas em dois tipos

- Procedimento = Rotina que não retorna valor
- Função = Rotina que retorna valor


### Função

As funções utilizam o comando ```return [VALOR]``` para retornar o valor para quem a chamou.

Para declarar uma função é utilizada a seguinte sintaxe:

```
func NOME(ARGUMENTOS) as TIPO do
    // Código aqui
end
```

Exemplo de função:


```
func FuncaoSoma(a as Int16, b as Int16) as Int16 do
    // Comando utilizado para retornar um valor
    Procedimento a, b
    return a + b
end
```

### Procedimento

Os procedimentos utilizam o comando ```return``` para encerrar prematuramente sua execução.

Para declarar um procedimento é utilizado a seguinte sintaxe:

```
proc NOME(ARGUMENTOS) do
    // Código aqui
end
```

Exemplo de procedimento:


```
proc Procedimento(a as Int16, b as Int16) do
    var c as Int16
    c = a + b + FuncaoSoma(1,2) // Chamando uma função
end
```

### Chamando funções e procedimentos

Para simplificar ao chamar uma rotina fora de uma expressão não é necessário o uso de parenteses para especificar o limite dos argumentos

```
proc Procedimento(a as Int16, b as Int16) do
    var c as Int16
    c = a + b + FuncaoSoma(1,2) // Chamando uma função
    FuncaoSoma 1, 2 // Chamando a mesma função fora de uma expressão
end

```

## Expressões

Nas atribuições ou parametros de rotinas são utilizadas expressões matemáticas/lógicas, que seguem regras de prioridade descrito abaixo:

As operações mais ao topo da tabela são executadas por último, englobando trechos das operações abaixo.

| Operações          | Descrição                        | 
|--------------------|----------------------------------|
| &&                 | E Lógico                         |
| ||                 | Ou Lógico                        |
| == != \<\> < > \<\= \>\= | Comparações                      |
| + -                | Soma, Subtração                  |
| * / %              | Multiplicação, Divisão, Módulo   |
| & \| \<\< \>\>         | Operações binárias               |

### Operações Matemáricas / Lógicas

- **&&** \
    Comparação E entre dois valores, retornando 1 para Sim e 0 para Não
- **||** \
    Comparação OU entre dois valores, retornando 1 para Sim e 0 para Não
- **&** \
    Aplica E entre dois valores
- **|** \
    Aplica OU entre dois valores
- **+** \
    Soma entre dois valores
- **-** \
    Subtração entre dois valores
- **\*** \
    Multiplicacao entre dois valores
- **/** \
    Divisão entre dois valores
- **%** \
    Módulo entre dois valores
- **==** \
    Igualdade entre dois valores, retornando 1 para Sim e 0 para Não
- **<>** ou **!=** \
    Diferença entre dois valores, retornando 1 para Sim e 0 para Não
- **<** \
    Menor Que entre dois valores, retornando 1 para Sim e 0 para Não
- **\<\=** \
    Menor Ou Igual entre dois valores, retornando 1 para Sim e 0 para Não
- **>** \
    Maior Que entre dois valores, retornando 1 para Sim e 0 para Não
- **\>\=** \
    Maior Ou Igual entre dois valores, retornando 1 para Sim e 0 para Não
- **\<\<** \
    Aplica deslocamento de bits a esqueda
- **\>\>** \
    Aplica deslocamento de bits a direita

## If

O comando ```if``` faz uma comparação e executa o seu conteúdo dependendo do resultado.

```
// Comando de uma única linha
if COMPARACAO COMANDO_INLINE // Executa se comparação for verdadeira

// Comando multilinhas
if COMPARACAO do
    // Executa se comparação for verdadeira
end

// Comando multilinhas com ELSE
if COMPARACAO do
    // Executa se comparação for verdadeira
else
    // Executa se comparação for falsa
end

```

## For Simples

O comando ```for``` é utilizado para percorrer uma sequência prédefinida.

```
var i as UInt16
for i = 1 to 16 do
    // Executa 16 vezes definindo o valor em i para a posicão atual
end

for i = 1 to 16 step 2 do
    // Executa 8 vezes definindo o valor em i para a posicão atual, sempre pulando de 2 em 2
end

```

## For Each

Este comando permite passar por todos os bytes de uma string.

```
var txt as string = "Oieeeee"
var i as uint16
var c as uint8
for each i, c in txt do
    // O 'i' contém o indice (Posição) dentro do txt
    // O 'c' contém o caractere/byte na posição dentro do txt
end
```

## While

Repete um comando ou um bloco enquanto uma condição for valida.

```
var i as uint16

i = 0
while i < 10 do
    i += 1
    // Repete 10 vezes
end
```

## Until

Repete um comando ou um bloco até uma condição for valida.

```
var i as uint16

i = 0
until i >= 10 do
    i += 1
    // Repete 10 vezes
end
```

## Asm DESTINO

Marca para qual plataforma os próximos comandos ```asm "codigo"``` serão emitidos, permitindo que um único arquivo gere códigos nativos para várias plataformas.

O destino aceita dois formatos:

- Processador
- Processador_SistemaOperacional

Exemplos de destinos válidos:

- i86
- i86_dos
- i386_windows
- x64_linux
- x64

Exemplo de utilização:

```

asm i86
asm "mov ax, 1"
asm "mov dx, 0"

asm i386
asm "mov eax, 1"

asm x64_linux
asm "mov rax, 1"

```

## Asm COMANDO

Emite um comando assembly para ser montado diretamente pelo montador da plataforma selecionada, conforme tópico acima.

Nos montadores que aceitar rótulos locais, esses serão utilizados para declarar as variáveis locais e agumentos facilitando a utilização, estes sempre serão declarados em maiúsculas.

Exemplo:

```

proc rotina(a as uint16, teste as uint16) do
    asm i86
    asm "mov ax, [bp+.A]"
    asm "mov bx, [bp+.TESTE]"

end

```

## IfTarget

Para que seja mais facil gerar código exclusivo para sistemas operacionais e destinos, foi implementado o comando ```iftarget DESTINO``` no qual apenas gera seu conteúdo para o executável final se o destino for o informado.

O campo destino utiliza a mesma formatação do comando ```asm DESTINO```.


Exemplo:

```
proc rotina do
    var c as uint16
    iftarget i86 do
        c = 15
    else 
        c = 33
    end
end

```

# Biblioteca Padrão

## Módulos

A biblioteca padrão é dividida em módulos, onde cada módulo tem comandos utilizáveis pelo usuário.

### Console

- **Console.ReadLine [string]** \
    Le uma linha de texto em uma string vindo do terminal
- **Console.WriteLine [string]** \
    Imprime uma string e cria uma nova linha no terminal
