# Introdução #

Este software visa atender uma demanda crescente de geração de memoriais descritivos, com base em coordenadas obtidas em campo, sendo utilizável para a criação de memoriais descritivos de perímetros de propriedades rurais, reservas legais e etc.


# Requisitos #

Atualmente, o software opera importando uma tabela contendo os pontos e suas coordenadas, e gerando por fim o memorial descritivo dos mesmos.

**A tabela contendo os pontos deve obedecer as seguintes características**
  * A primeira linha da tabela deve conter o cabeçalho da tabela (nome dos campos);
  * Colunas separadas por ponto-e-virgula ou tabulação (tab);
  * A tabela deve ter exatamente três colunas (não menos e não mais);
  * A primeira coluna deve ter os nomes dos pontos, nenhuma célula pode estar em branco;
  * A segunda coluna deve ter as coordenadas X (este), nenhuma célula pode estar em branco;
  * A terceira coluna deve ter as coordenadas Y (norte), nenhuma célula pode estar em branco.

# Instruções de uso #

  1. Abra o programa. Caso haja alguma atualização do software, ele o irá informar neste momento;
  1. Anter de abrir o arquivo CSV, escolha o tipo de separação das colunas (ponto-e-virgula ou tabulação);
  1. Clique em **Abrir arquivo CSV**, navegue até o arquivo desejado e clique em **Abrir**. A tabela será carregada na janela do programa;
  1. Clique em **Calcular distância e azimute entre pontos e adicionar confrontantes**;
  1. Cadastre os confrontantes, no mesmo sentido dos pontos. Exemplo: se o confrontante A está entre os pontos 01 a 03, digite "A" na célula correspondente do ponto 01. Na célula correspondente do ponto 03, digite o nome do próximo confrontante;
  1. Caso queira exporar a tabela, clique em **Exportar tabela CSV**. Uma outra forma de copiar a tabela é selecionar todas as linhas, pressionar Ctrl + C, e colar em algum editor de planilhas ou de texto de sua preferência;
  1. Clique na aba **Definições**;
  1. Caso deseje, complete as informações sobre o imóvel e proprietário, redefina os padrões de ligação do texto do memorial, e informe os detalhes do profissional responsável técnico;
  1. Clique na aba **Memorial Descritivo**;
  1. Clique em **Gerar memorial**;
  1. Para salvar o memorial descritivo como arquivo RTF, clique em **Gravar arquivo RTF**;
  1. Para copiar o memorial descritivo para a área de transferência, clique em **Copiar para área de transferência**.