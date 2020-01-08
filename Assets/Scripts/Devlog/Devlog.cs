
#region TEST RUNNER
/*
A janela do Teste Runner fica em Window -> General -> Test Runner.
Test Runner vamos ter duas opções PlayMode e Edit Mode
PlayMode são os testes que serão realizados com o jogo rodando, normalmente testes de física, movimentação e coisas do tipo
EditMode são os testes que não são realizado com o jogo rodando. É a parte mais extensa do testes e envolve a lógica do jogo.
Para Criar a pasta onde serão colocados os scripts de teste e o Assembly, vamos em Create EditMode(ou PlayMode) Test Assembly Folder.
É importante colocar o PlayMode e o EditMode em pastas diferentes.
Nessa pasta será criado então um arquivo assembly, que quando visualizado no Inspector vai apresentar as configurações Assembly, como outras bibliotecas referenciadas e em quais plataformas esse assembly vai rodar
Podemos criar, então, scripts para incluir os testes. Eles devem estar na mesma pasta do Assembly e devem ter referencias à UnityEngine.TestsTools e NUnit.Framework.
Aqui tem uma breve explicação sobre o que são testes unitários e a sua importância: https://medium.com/@dayvsonlima/entenda-de-uma-vez-por-todas-o-que-s%C3%A3o-testes-unit%C3%A1rios-para-que-servem-e-como-faz%C3%AA-los-2a6f645bab3
Para realizar testes unitários na Unity, usamos o atributo [UnityTest]

Cada função de teste possui três etapas:
ARRANGE - Onde são criados os objetos e realizadas as configurações que serão utilizadas nos testes.
ACT - Onde é realizada a ação que será testada.
ASSERT - Onde é realizada a comparação entre o resultado obtido e o esperado. É utilizada uma das funções estáticas da classe Assert do NUnit.

Quando o teste não for instatâneo requerer um intervalo de tempo para ser realizado, o tipo de retorno do método será IEnumerable e será utilizado entre as ações:
yield return new WaitForSerconds(segundos);
*/
#endregion

#region HUMBLE OBJECT PATTERN
/*
Classes que implementem MonoBehaviour são mais complicadas de testar. Uma opção é a utilização do padrão Humble Object, explicado aqui:
https://blogs.unity3d.com/pt/2014/06/03/unit-testing-part-2-unit-testing-monobehaviours/
e resumido aqui:
https://stackoverflow.com/questions/5324049/what-is-the-humble-object-pattern-and-when-is-it-useful

A ideia é a classe ser só o envelope, delegando a maior parte da lógica para outras classes, facilitando os testes.

Um exemplo seria uma classe Player que realizasse a movimentação do personagem a partir de um input do jogador.
Ela poderia ter um método GetInput(), onde seria lido o input do jogador e então realizada a lógica de movimentação.
O problema é que no teste seria difícil receber esse input sem criar gambiarras, quebrando encpsulamento e outras boas práticas.
*/
#endregion

#region INTERFACES
/*
Uma solução é a criação de uma interface IPlayerInput que teria propriedades de leitura Vertical e Horizontal.
A classe Player teria uma referência para uma classe que implementasse essa interface e utilizaria os valores retornados por Vertical e Horizontal para realizar a movimentação.
Assim, no teste bastaria criar uma nova classe implementando IPlayerInput que retornasse os valores necessários ao teste.

O problema dessa abordagem é que teríamos que criar uma classe de teste para cada classe que quisessemos testar. Quanto maior fica o código, mas complicado fica manter essa prática.
*/
#endregion

#region  NSUBSTITUTE
/*
Pode ser baixado ou clonado no github:
https://github.com/nsubstitute/NSubstitute

As versões mais novas estavam compatíveis com unity em 12/2019. Quando baixarmos o arquivo, vamos encontrar a versão que iremos utilizar em Docs/Downloads/NSubstitute.2.0.3.0.zip
Dentro desse arquivo, vamos em lib/net45/NSubstitute.dll, que é a biblioteca que queremos. Vamos copiar esse arquivo para uma nova pasta dentro dos Assets do Unity, que podemos nomear como Plugins.
Com o Assembly do NSubstitute selecionado, vamos indicar que ele só será utilizado no modo editor. Fazemos isso marcando somente o editor nas plataformas apresentadas no inspector e aplicando.
Agora precisamos incluir a referencia para o Assembly do NSubtitute no Assembly dos testes. Agora no inspector do assembly do Testes, adicionamos um novo item ao Assembly References, selecionamos NSubstitute.dll e aplicamos.

Com isso podemos utilizar o Substitute para substituir as classes de testes que teríamos que criar quando estávamos utilizando somente as Interfaces para nos ajudar nos testes.
Vamos trocar isso:

MockPlayerInput playerInput = new MockPlayerInput();
playerInput.Vertical = 1f;

Por isso:

var playerInput = Substitute.For<PlayerInput>();
playerInput.Vertical.Returns(1f);

E deletar a classe MockPlayerInput.

É importante notar que NSubstitute só funciona com propriedades e que utilizamos o var como tipagem da referência.
*/
#endregion

#region FACTORY PARA CLASSES DE TESTE
/*
Nas classes de testes é comum termos que testar diversos comportamentos nos mesmos objetos, como movimentação nos quatro sentidos de um player. 
Para que não tenhamos que incluir toda a lógica de criação do objeto a cada método de teste, devemos utilizar o padrão factory e criar uma classe Helper que conterá essa lógica.
Dessa forma, teremos métodos estáticos nessa classe que criam, por exemplo, o chão e o jogador dos nossos testes.
Por serem métodos estáticos, não será necessária a instanciação dessas classes para criação desses objetos.
*/
#endregion

#region SEPARAÇÃO DE CLASSES E SUAS RESPONSABILIDADES
/*
É normal ver novas funcionalidades serem adicionadas à uma classe já existente até que elas virem uma bagunça, ficando com mais responsabilidades do que deveriam.

Olhando para a nossa classe Player, vemos que a lógica de movimentação no Update poderiam ser colocada em uma classe separada, exclusiva para movimentação.
Além de dominuir as responsabilidades da classe player, outro ponto positivo dessa refatoração é poder criar um sistema de troca de formas de movimentação (teclado e mouse, controle, point and click...) de forma bem simples.
Para isso usamos uma interface. A classe player terá uma referência à essa interface e as classes que representam as diferentes formas de movimentação implementarão essa interface.

É importante notar que essa abordagem com a utilização de interface só é necessária se houver mais de uma forma de implementação do mecanismo. Caso contrário, basta criar uma classe.
*/
#endregion

#region UPDATE
/*
Um ponto interessante é que essas classes não precisam extender Monobehaviour. A lógica que seria executada no Update pode ser colocada em uma função que será chamada no Update da classe container.
Aqui foi usada a função Tick() para isso.
*/
#endregion

#region INJEÇÃO DE CONSTRUTOR
/*
Essas classes auxiliares precisarão de informações da classe container. Para resolver isso, vamos passar a classe container dentro do contrutor e pegar ali as referências que precisaremos.
Exemplo:

public PlayerMoviment(Player player)
   {
       _player = player;
       _characterController = player.GetComponent<CharacterController>();
   }
*/
#endregion

#region ROTAÇÃO
/*
A utilização de interfaces é importante quando temos diferentes tipos de implementação de uma mesma mecânica.
Quando só visualizamos uma forma de implementação, o mais fácil é criar somente uma classe.

É isso que fizemos com o PlayerRotator. O separamos da classe Player, seguindo o padrão humble, mas não criamos uma interface.

A classe Rotator recebe o player no construtor e tem o seguinte código na função Tick():

public void Tick()
    {
        var rotation = new Vector3(0, _player.PlayerInput.MouseX, 0);
        _player.transform.Rotate(rotation);
    }

Também tivemos que alterar IPlayerInput para que tivesse uma propriedade MouseX.

Para calcular a rotação, afim de testa-la, utilizamos o seguinte código:

    var crossProduct = Vector3.Cross(startingRotation * Vector3.forward, endingRotation * Vector3.forward);
    var dotProduct = Vector3.Dot(crossProduct, Vector3.up);

    return dotProduct;
*/
#endregion

#region CENA DE TESTE
/*
Um dos problemas de criar o chão e o player em cada teste, é que corremos o risco dos players de testes diferentes serem criados no mesmo lugar e, por ação da física, terem seu posicionamento alterado, invalidando os testes.
A solução para isso é a criação de uma cena exclusiva para testes e o reaproveitamento dos objetos criados.
Vamos duplicar a cena que utilizamos até agora e renomeá-la para TestScene.

Vamos aproveitar a função CreateFloor para carregar a cena. Vamos alterá-la e renomeá-la conforme abaixo:

        public static IEnumerator LoadTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("TestScene");
            while(operation.isDone == false)
            {
                yield return null;
            }
        }

Alguns detalhes: 
Tivemos que alterar o tipo de retorno para IEnumarator, já que vamos querer carregar a cena de forma assíncrona.
SceneManager.LoadSceneAsync retorna um AsyncOperation, o qual podemos pegar o estado do carregamento da cena.

Outra coisa importante é avisar às funções que utilizarão a cena de que elas precisam esperar o seu carregamento.
Fazemos isso colocando yield return antes da chamada da função LoadTestScene() em todos os lugares onde ela for chamada, como abaixo:

    yield return TestHelper.LoadTestScene();

O passo segunite é alterar a função CreatePlayer. Não queremos mais que um objeto Player seja criado a cada teste, mas que o Player em cena seja passado para os testes seguintes.
A parte onde o PlayerInput é substituido continua valendo.
A função fica como abaixo:

        public static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();

            var playerObjectInput = Substitute.For<IPlayerInput>();
            player.PlayerInput = playerObjectInput;

            return player;
        }
*/
#endregion

#region CAMERA
/*
Uma forma simples de criar uma câmera em primeira pessoa é arrastar a nossa câmera para dentro do objeto jogador e resetar o seu transform. Assim ela vai sempre acompanhar o player.
*/
#endregion

#region INICIO ITENS E INVENTÁRIO
/*
Vamos criar uma estrutura simples de itens e inventário. Teremos duas classes: Item e Inventário, ambas herdando de Monobehaviour

Item:
Existem várias formas de se pegar itens em jogos, seja clicando nos objetos ou por aproximação. Para começar vamos utilizar a segunda opção.
Dentro da Função OnTriggerEnter, vamos verificar se o item já foi coletado com uma variável booleana _wasPicked.
Se o _wasPicked for falsa, vamos ver se o objeto de iniciou o gatilho possui um inventário, caso sim, chamaremos a função Pick() desse inventário e alteraremos a _wasPicked para true.

Uma opção interessante aqui é marcar a opção de trigger no colisor através do código. Dessa forma há menos chance de esquecermos de marcá-la.
Para isso vamos setar a propriedade isTrigger do collisor dentro da função OnValidate. 
Também é interessante usar o [RequireComponent(typeof(Collider))] para que o objeto tenha sempre um colisor.

Inventório:
Como vamos adiocionar o inventário como um componente do nosso player, precisamos que ele herde de Monobehaviour
A função Pick() vai adicionar o item à uma lista de itens. Também vai alterar o objeto pai do item para o ItemsContainer, que é um objeto filho do player.
O ItemsContrainer será criado dentro da função Awake do Inventário com new GameObject.
*/
#endregion

#region IMPORTAÇÃO DE MODELO E TEXTURAS
/*
No nosso caso temos um modelo de uma shotgun, que possui as seguintes texturas: Albedo Transparency, Emission, Metallic Smoothness e Normal.
Temos duas formas de criar o material para esse modelo. Uma é criando primeiro o material e depois colocando as texturas nos campos.
A outra é arrastando uma das texturas para o modelo já em cena. Isso cria automáticamente o material, mas nós ainda temos que colocar o restante das texturas nos campos corretos.
Para colocar as texturas restantes, é só arrastá-las para os seus campos dentro do inspector do material.

Pode acontecer do modelo ser composto por várias partes, e o material só estar em umas delas.
Para corrigir isso, basta selecionar todas as partes e arrastar o material para o campo Material->Element no inspector.

Outras coisa interessante é criar um GameObject para ser o container do modelo, serparando, assim, arte e lógica do objeto.
*/
#endregion

#region EQUIPANDO ITEM
/*
Atualmente quando coletamos um item ele ele fica colado ao jogador. Esse não é o comportamento que desejamos.
Como estamos lidando inicialmente só com armas, o comportamento esperado é que o item seja equipado na mão direita do personagem.
Para isso vamos criar um novo objeto chamado Right Hand como filho do player e posicioná-lo de forma que em jogo as armas parecam estarem sendo seguradas pelo jogador.
Para facilitar o posicionamento visualmente, podemos criar um cubo e colocá-lo como filho de Right Hand. Também podemos utilizar o modelo da Shotgun para isso.

Na função de equipar, além de setar o item coletado como equipado, temos que resetar seu LocalPosition e LocalRotation, ficando como abaixo:
    private void Equip(Item item)
    {
        EquipedItem = item;
        EquipedItem.transform.SetParent(_rightHand);
        EquipedItem.transform.localPosition = Vector3.zero;
        EquipedItem.transform.localRotation = Quaternion.identity;
    }

EquipedItem é uma propriedade com get public e set privado.
*/
#endregion

#region GIT
/*
Link com dicas para preparar o projeto para uso com GIT
https://thoughtbot.com/blog/how-to-git-with-unity
*/
#endregion






















*/