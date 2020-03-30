
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

#region ROTAÇÃO DO PERSONAGEM
/*
A utilização de interfaces é importante quando temos diferentes tipos de implementação de uma mesma mecânica.
Quando só visualizamos uma forma de implementação, o mais fácil é criar somente uma classe.

É isso que fizemos com o PlayerRotator. O separamos da classe Player, seguindo o padrão humble, mas não criamos uma interface.

Rotação horizontal:

A classe Rotator recebe o player no construtor e tem o seguinte código na função Tick():

public void Tick()
    {
        var rotation = new Vector3(0, _player.PlayerInput.MouseX, 0);
        _player.transform.Rotate(rotation);
    }

Também tivemos que alterar IPlayerInput para que tivesse uma propriedade MouseX.

Para calcular a rotação horizontal, afim de testa-la, utilizamos o seguinte código:

    var crossProduct = Vector3.Cross(startingRotation * Vector3.forward, endingRotation * Vector3.forward);
    var dotProduct = Vector3.Dot(crossProduct, Vector3.up);

    return dotProduct;

Rotação vertical:
Enquanto a rotação horizontal é uma rotação do objeto do personagem, a rotação vertical só rotaciona a câmera.
Por esse motivo a abordagem escolhida no curso foi criar uma classe CameraController para realizar essa rotação.
Por uma questão de consistência, resolvi deixá-la dentro de PlayerRotator.
Vamos ter que fazer a adição do MouseY tanto no IPlayerInput, quanto no PlayerInput, aos moldes do que já foi feito com o MouseX.
No método Tick do PlayerRotator vamos incluir:

        _tilt = Mathf.Clamp(_tilt - _player.PlayerInput.MouseY, -_tiltRange, _tiltRange);
        Camera.main.transform.localRotation = Quaternion.Euler(_tilt, 0f, 0f);

A primeira linha limita a rotação vertical de acordo com o _tiltRange, que pode ser serializado para ser alterado no inspector.
Nós subtraímos o valor do MouseY para que nossa rotação não seja invertida. Funciona como uma câmera de auditório, você precisa abaixar a parte de trás da câmera para que ela aponte para cima.
Vamos guardar essa informação em uma variável _tilt para que a rotação seja acumulada entre os frames.

Na segunda linha estamos rotacionando a câmera. Quaternion.Euler retorna uma rotação com base em graus.
Detalhe que pegamos a posição Y do mouse, mas estamos rotacionando o eixo X da câmera. 

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

#region CLASSE ABSTRATA
/*
Assim como interfaces, não podem ser instanciadas diretamente.
É indicada quando há compartilhamento de uma mesma funcionalidade entre classes do mesmo tipo e essa funcionalidade não irá mudar entre elas. 
Uma classe abstrata pode herdar outra classe abstrata ou implementar outras interfaces, mas uma classe só pode herdar uma classe abstrata.
As variaveis e propriedades de uma classe abstrata devem ser protected para que possam ser herdadas pelas classes filhas.

Uma classe abstrata pode ter métodos abstratos, que não possuem implementação e funcionam como as assinaturas de métodos das interfaces. Esse método deverá ser implementado nas classes filhas.

    public abstract void MetodoAbstrato(); 

Classes abstratas também podem ter métodos normais, que serão também herdados pelas classes filhas.
Caso queiramos que a implementação de um método seja alterado por uma classe filha, devemos incluir virtual à sua assinatura.

    public virtual void MetodoQuePodeSerSobrescrito()
    {
        implementação
    }

Em ambos os casos, quando vamos implementar um método abstrato ou alterar um método virtual, devemos utilizar a assinatura override antes do nome do método que queremos utilizar.
    
    public override void MetodoQuePodeSerSobrescrito()
    {
        implementação
    }

 */
#endregion

#region ITENS USÁVEIS 
/*
ITEMCOMPONENT
Nesse projeto vamos usar classes abstratas para melhorar a reusabilidade dos itens. A ideia é criar uma classe abstrata ItemComponent, da qual herdarão os diversos tipos de itens. 
Essa classe vai herdar de Monobehavior para que seus filhos possam ser componentes e aparecerem no inspector.
Ela vai ter um método abstrato Use(), que terá a implementação de uso nas classes filhas e uma propriedade booleana pública CanUse que irá informar se o item pode ser usado naquele momento. Ela tem a função de dar um delay entre os usos do item.

    public bool CanUse => Time.time >= _timeNextUse;

Um exemplo de classe que herda de ItemComponent é ItemLogger. Sua única função é escrever no console quando o item for usado. Para isso basta implementar o método abstrato Use como abaixo:
    
    public override void Use()
    {
        Debug.Log("Item Used");
    }

USEACTION
Um item pode ter mais de um tipo diferente de uso. Uma arma, por exemplo, pode ter um tiro simples e um carregado, cada um representado por um ItemComponent diferente.
Vamos querer mapear esses usos para teclas diferentes. 

Para isso vamos criar um Struct UseAction. A escolha do Struct é por ser uma estrutura mais simples e que fica armazenada no stack e não no heap, não gerando garbage.
Ele terá a seguinte implementação:

    using System;

    [Serializable]
    public struct UseAction
    {
        public UseMode useMode;
        public ItemComponent TargetComponent;
    }

O [Seriazable] é para que o UseAction seja visualizada no inpector. 
UseMode é uma referencia para um Enun UseMode contendo os diversos comandos que podem chamar o uso do Item, como "RightClick" e "LeftClick".
TargetComponent é o componente que será vinculado ao comando selecionado.

Dessa forma, podemos escolher no inspector o comando e o componente que ele irá chamar.

Na classe Item vamos adicionar um array de UseAction, assim será possível definir no item quantas ações diferentes ele poderá ter, quais os comandos que irão chamá-las e quais os ItemComponent que serão utilizados.

    [SerializeField] private UseAction[] _actions;
    public UseAction[] Actions => _actions;

INVENTORYUSE
A classe InventoryUse será a responsável pelo uso dos itens. Dessa forma não sobrecarregamos as classes Inventory ou Player com essa responsabilidade.
Ela terá uma referência para Inventory, que será cacheada no Awake.

Uma função privada WasPressed vai verificar se o comando vinculado à ação foi chamado naquele momento e retornará true se tiver sido.
Por exemplo, vamos supor que seja passado o UseMode "LeftClick" para o método. No Switch ele irá verificar se o Input.GetMouseButtonDown(0) foi chamado naquele frame e retornar true ou false dependendo do resultado.

    private bool WasPressed(UseMode useMode)
    {
        switch(useMode)
        {
            case UseMode.LeftClick: return Input.GetMouseButtonDown(0);
            case UseMode.RightClick: return Input.GetMouseButtonDown(1);
            default: return false;
                    
        }

No Update ela irá verificar se o EquipedItem do inventário possui um item. Caso sim, iniciará o seguinte laço:

    foreach (var useAction in  _inventory.EquipedItem.Actions)
    {
        if (useAction.TargetComponent.CanUse && WasPressed(useAction.useMode))
        {
            useAction.TargetComponent.Use();
        }
    }

Aqui serão percorridas todas as ações mapeadas para o objeto equipado. Para cada ação existente, será verificada se ele pode ser usada naquele momento e se o UseMode vinculado àquela ação foi acionado.
Então será chamado o método Use().
 */
#endregion

#region ITEM RAYCASTER
/*
Esse ItemComponent tem a função de tiro. Ele utiliza o Raycast para verificar se algum alvo foi acertado ou não e retornar o q foi atingido.
Ele vai herdar de ItemComponent e a sua implementação vai se dar basicamente dentro do método Use. 
A primeira coisa é definir um delay, que vai impedir o spam do tiro:

    _timeNextUse = Time.time + _delay;

Assim, a cada tiro será somado um tempo  de intervalo para q o tiro possa ser utilizado de novo. A variável _delay será privada, mas [SerializeField], para que possa ser definida no inspector.

Vamos querer que o nosso raio saia da câmera e vá em direção ao meio da tela. Para pegar a coordenada do meio da tela, vamos usar:

    Ray ray = Camera.main.ViewportPointToRay(Vector3.one / 2);

Esse método retorna um raio e mapeia a tela de forma normalizada, com o canto inferior esquerdo sendo (0,0) e o canto superior direito (1,1). A coordenada Z é ignorada.
Como queremos o meio, utilizamos o Vector3.one / 2. Uma outra alternativa era utilizar um new Vector3(0,5f, 0, 0.5f) que teria o mesmo resultado. Mais informações sobre o método no link:
https://docs.unity3d.com/ScriptReference/Camera.ViewportPointToRay.html

Em seguida vamos lançar o raio e pegar a quantidade de contatos:

    int hits = Physics.RaycastNonAlloc(ray, _results, _range, _layerMask, QueryTriggerInteraction.Collide);

hits recebe a quantidade de objetos que foram tocados pelo raio.
RaycastNonAlloc é melhor explicado no link: https://docs.unity3d.com/ScriptReference/Physics.RaycastNonAlloc.html 
De forma resumida, ele é como o RaycastAll, mas utiliza o _results como buffer para o resultado, não gerando um novo array a cada chamada.
Vamos definir variáveis privadas para _result, _layerMask e _range.
Vamos inicializar a _result diretamente na sua declaração

    private RaycastHit[] _results = new RaycastHit[100];

_layerMask vai ser atribuída no awake:

    _layerMask = LayerMask.GetMask("Default");

_range vai ser serializable para que possa ser definida no inspector.

Por último vamos identificar qual foi o hit mais próximo:

        double nearestDistance = double.MaxValue;
        RaycastHit nearestHit = new RaycastHit();

        for (int i = 0; i < hits; i++)
        {
            double distance = _results[i].distance;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestHit = _results[i];
            }
        }

Vale destacar que RaycastHit é um struct e não pode ser nulo.

O passo seguinte é realizar a lógica do acerto do tiro.

 */
#endregion

#region CENA DE UI
/*
É possível ter mais de uma cena aberta ao mesmo tempo na Unity.
Dessa forma, vamos criar uma cena especialmente para UI e adicioná-la dentro das outras cenas.
Com isso não é mais necessário fazer um grande Prefab contendo toda a UI, mas trabalhar dentro da cena de UI de forma normal, com vários Prefabs.
A primeira coisa é criar uma nova cena e nomeá-la de UI. Vamos deixar somente a câmera dentro dessa cena.

Nas propriedades da UI Camera vamos mudar:
Tag -> De MainCamera para nenhuma. Só pode haver uma ativa e essa será aquela que está dentro do player.
Nome -> renomear para UI Camera.
Clear Flags -> para Don't clear.
Culling Mask -> selecionar somente UI.

Na Main Camera vamos alterar:
Culling Mask -> selecionar todos menos UI.

Vamos também excluir o Audio Listener da nossa UI Camera.

Agora é só arastar a cena de UI para dentro da cena principal.

Uma coisa a se observar é qual cena está marcada como ativa, ou seja, principal. Queremos que a cena do level, e não do UI, seja a ativa.
Temos que ter cuidado também quando formos adicionar os elementos de Ui para que estejam na cena correta.
 */
#endregion

#region CROSSHAIR, EVENTOS E SCRIPTABLEOBJECT
/*
Queremos colocar um crosshair no jogo. Para isso vamos adicionar uma imagem na cena de UI e centralizá-la com a tela. 
Existem vários pacotes de imagens de crosshair grátis na internet, basta baixar um.

Podemos querer ter imagens do crosshair diferentes dependendo do contexto, como crosshair diferentes para armars diferentes.
Existem várias formas de se fazer isso.
Vamos criar um script Crosshair e adicioná-lo no objeto Crosshair que criamos na UI.

A primeira coisa é criar uma referência para a imagem no objeto.
Podemos ter também uma referência para o inventário, que será utilizado para fazer a assinatura no evento OnActiveItemChanged. 
Como essa assinatura só ocorre na OnEnable, também podemos decidir por não guardar essa referência.
Teremos que ter, então, referências para todas as imagens de crosshair que podem ser utilizadas.

No OnEnable vamos assinar o evento de troca de item equipado:
    _inventory.OnActiveItemChanged += HandleActiveItemChanged;

E vamos definir a imagem de crosshair que será utilizada:
        if (_inventory.EquipedItem != null)
        {
            _crosshairImage = _invalidSprite;
        }
        else
        {
            HandleActiveItemChanged(_inventory.EquipedItem);
        }

O OnActiveItemChanged é um evento definido no Inventário que recebe como parâmetro o item que foi equipado.
Mais informações sobre eventos e delegates nos vídeos:
https://www.youtube.com/watch?v=OuZrhykVytg
https://www.youtube.com/watch?v=G5R4C8BLEOc
https://www.youtube.com/watch?v=TdiN18PR4zk

Para cada tipo de crosshair, teremos uma entrada em um enum chamado CrosshairMode. Uma referência para CrosshairMode será colocado em Item.

O método HandleActiveItemChanged recebe um item e ativa a imagem correspondente ao crosshairMode dele: 

    private void HandleActiveItemChanged(Item item)
    {
        switch (item.crosshairMode)
        {
            case CrosshairMode.Invalid: _crosshairImage = _gunSprite;
                break;
            case CrosshairMode.Gun: _crosshairImage = _invalidSprite;
                break;
            default: _crosshairImage = _invalidSprite;
                break;
        }
    }

Essa abordagem funciona, mas traz um problema: Toda alteração e inclusão de novas modalidades de crosshair deverão ser incluídas diretamente no código.
Além de aumentar o risco de erro, isso aumenta a complexidade do processo, ao mesmo tempo que exige um programador para implementação.
O ideal é criar alternativas que sejam faceis de serem expandidas e alteradas, construindo ferramentas que possam ser utilizadas diretamente pelos game designers.

Para isso vamos usar ScriptableObjects, que são objetos que podem ser criados diretamente nos assets do projeto e que só contém dados.
Nesse caso eles vão substituir o enum CrosshairMode, que será deletado. Os atributos em item e Crosshair que referenciavam CrosshairMode, vão passar a referenciar CrosshairDefinition, a nova classe.

    [CreateAssetMenu(menuName = "CrosshairDefinition")]
    public class CrosshairDefinition : ScriptableObject
    {
        public Sprite Sprite;
    }

A propriedade [CreateAssetMenu(menuName = "CrosshairDefinition")] é o que permite que o objeto seja criado no projeto. 
O menuName é o nome do menu onde a criação do objeto irá aparecer. É possível criar submenus utilizando /.
A classe tem somente um atributo público Sprite, que conterá a imagem do crosshair daquele item.

Basicamente, o único item que teremos que alterar é o método:

    private void HandleActiveItemChanged(Item item)
    {
        if (item?.CrosshairDefinition != null)
        {
            _crosshairImage.sprite = item.CrosshairDefinition.Sprite;
            Debug.Log($"Crosshair detected {item.CrosshairDefinition.Sprite.name}");
        }
        else
        {
            _crosshairImage.sprite = _invalidSprite.Sprite;
        }       
    }

Ao invés do switch, vamos verificar se o CrosshairDefinition do item não é nulo e atribuir seu sprite ao _crosshairImage.
Os atributos de imagem como _gunSprite podem ser deletados.

Agora é só criar os CrosshairDifinition no projeto, adicionar a imagem que quisermos e incluí-los nos objetos diretamente no Inspector.


TESTE
Para testar o nosso sistema de alteração de crosshair vamos criar uma nova cena. 
Ela será uma cópia da cena de teste de movimento e iremos adicionar um prefab do Player e um de um item, nesse caso da shotgun.
O importante de usar os prefabs é que eles serão atualizados automaticamente quando fizermos alterações.

Na classe TestHelper, vamos adicionar mais um método estático para carregar essa cena:

        public static IEnumerator LoadItemTestScene()
        {
            var operation = SceneManager.LoadSceneAsync("Scenes/Test Scene/ItemTestScene");
            while(operation.isDone == false)
            {
                yield return null;
            }
            
            operation = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            while(operation.isDone == false)
            {
                yield return null;
            }
        }
        
A diferença do método de carregamento de cena anterior fica por conta do parâmetro LoadSceneMode.Additive, que irá adicionar a cena de UI à cena de teste já carregada.

À uma classe ui_test, vamos adicionar o seguinte método, que irá testar a alteração crosshair:

        [UnityTest]
        public IEnumerator change_crosshair()
        {
            yield return TestHelper.LoadItemTestScene();
            var player = TestHelper.GetPlayer();
            Crosshair crosshair = GameObject.FindObjectOfType<Crosshair>();
            
            Item item = GameObject.FindObjectOfType<Item>();
            
            Assert.AreNotSame(item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
            
            item.transform.position = player.transform.position;

            yield return null;

            Assert.AreSame(item.CrosshairDefinition.Sprite, crosshair.GetComponent<Image>().sprite);
        }

Uma alteração importante nesse método, comparado com os anteriores, foi exclusão da movimentação do player.
Nesse nós movemos o item direto para a posição do player e esperamos um frame, o que representa um ganho de velocidade no teste:

            item.transform.position = player.transform.position;
            yield return null;

*/
#endregion

#region HOTBAR UI
/*
Para criar a hotbar vamos utilizar um panel. Vamos fixá-lo no centro inferior da tela definindo o anchor, position e pivot do Rect Transform para aquela posição.

Outra mudança importante é no Cavas Scaler. Para que a UI seja reposicionada de forma automática nas diversas resoluções.
UI Scale Mode: Scale with screen size - Isso vai avisar a nossa UI para escalonar com o tamanho da tela.
Reference Resolution 1920x1080 - Aqui é importante decidir a resolução alvo. Full HD tem um aspect ratio muito comum e funciona bem. É importante que quando for desenvolver, o projeto esteja setado para a resolução alvo.
Screen Match Mode: Match width or height  - Informa se a UI vai acompanhar a largura ou altura da tela.
Match: width - O mais interessante é acompanhar a largura.
 
Dentro dele vamos incluir um Grid Layoult Group, que é quem vai organizar os slots.
O seu Child Alignment será Middle Center, para que os slots fiquem centralizados.
O cell size será de 64x64, ou a resolução que quisermos para os ícones dos itens.
Vamos adicionar um spacing 5 em x para que os slots não fiquem colados.
O start Axis será horizontal.
 
Os Slots também serão panels.
Uma vez adicionados como filhos de hotbar, vão adquirir o tamanho e posicionamento automaticamente, de acordo com o definido no Grid Layoult Group.

CÓDIGO HOTBAR
Primeiramente vamos criar uma classe Hotbar que implementa monobehaviour.
Ela terá referência para Inventory e um array de Slots. Todas serão pegas dentro do OnEnable, assim como a assinatura do evento OnItemPickedUp do Inventory:

    private void OnEnable()
    {
        _inventory = FindObjectOfType<Inventory>();
        _inventory.OnItemPickedUp += HandleItemPickedUp;
        _slots = GetComponentsInChildren<Slot>();
    }
    
O HandleItemPickedUp vai receber o item passado pelo invetory e adicioná-lo ao próximo slot vazio.

 private void HandleItemPickedUp(Item item)
    {
        Slot slot = FindNextOpenSlot();
        if (slot != null)
        {
            slot.SetItem(item);
        }
    }
    
A identificação de slots vazios fica por conta do FindNextOpenSlot:

private Slot FindNextOpenSlot()
    {
        
        foreach (Slot slot in _slots)
        {
            if (slot.IsEmpty)
            {
                return slot;
            }
        }
        
        Debug.LogError("No empty slots available");
        return null;
    }
    
Vamos olhar também a classe Slot, que vai gerenciar cada slot do hotbar. 
A única coisa um pouco diferente dessa classe é o isEmpty

    public bool IsEmpty => _item == null;
    
Ela terá uma variável para armezenar o Item e uma para o ícone que será apresentado.

Vamos ter que adicionar uma variável ícone também à classe Item:

    [SerializeField] private Sprite _icon;
    public Sprite Icon => _icon;


CRIAÇÃO DE ÍCONES COM BASE EM MODELOS 3D

Para criar ícones com bases nos modelos 3D existentes no jogo, vamos precisar criar uma nova cena no Unity.
Na Main Camera:
Clear Flags -> Solid Color
Background ->  uma cor estilo 00FF00

Vamos adicionar o objeto que queremos transformar em ícone na cena.
Resetar o seu transform, rotacioná-lo para que fique na posição que queremos na imagem, normalmente 90 em Y, e modificar o scale para que ele fique em um bom tamanho.
Alinhar a luz da cena para que a iluminação no objeto fique correta. Ctrl + Shift + F com a luz selecionada faz com que ela fique direcionada para o ponto onde estamos olhando.

Agora é tirar um print Screen. O programa Ferramenta de Captura do windows faz um bom trabalho.
Vamos colar a imagem no GIMP e usar a ferramenta de seleção para excluir a cor de fundo. No GIMP é a ferramenta com o símbolo de varinha de condão. 
A ferramenta de seleção padrão é a de seleção contígua, mas teremos um melhor resultado com a seleção por cor. Basta deixar o mouse pressionado na ferramenta para a opção aparecer.
Feitas as alterações, vamos exportar a imagem. Nas opções de exportação, vamos desmarcar a Salvar Cor de Fundo.

Ainda não está finalizado. Nós queremos uma imagem quadrada e não retangular. Vamos fazer a seleção utilizando a Seleção Retangular e pressionando o SHIFT para que a área selecionada seja um quadrado.
Vamos copiar a seleção e criar uma nova imagem com CTRL + N
No menu de criação da nova imagem, vamos definir o seu tamanho para que seja quadrado, como 512 x 512 ou 1024 x 1024, e na opção avançada "Preencher com" selecionamos Transparência.
Caso o fundo da imagem fique muito maior que a imagem em si, podemos fazer mais uma seleção com a Seleção Retangular e, em Imagem -> Cortar para Seleção.
Agora é só exportar novamente.

Com a imagem já dentro da Unity, temos que alterar o Texture Type para Sprite 2D.

INPUT PARA A HOTBAR

Para não começarmos a espalhar os nossos inputs pelo código, vamos adicionar os inputs para a hotbar ao PlayerInput.
Vamos fazer isso na forma de evento:

    event Action<int> OnHotkeyPressed;
    
Como PlayerInput não implementa MonoBehaviour, criaremos uma função Tick para ser chamada no update de Player:

    public void Tick()
    {
        if (OnHotkeyPressed == null)
        {
            return;
        }

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                OnHotkeyPressed(i);
            }
        }
    }
    
Essa função verifica se uma tecla de 1 à 9 foi pressionada e chama o evento passando o número da tecla como parâmetro.
No começo há uma verificação para ver se o evento foi assinado.

Temos também que incluir o evento e a função na interface IPlayerInput.

O próximo passo é fazer a implementação do tratamento em Hotbar.
A primeira coisa é assinar o evento OnHotkeyPressed dentro do OnEnable:

    FindObjectOfType<Player>().PlayerInput.OnHotkeyPressed += HandleHotkeyPressed;

Existe a opção de cachear o Player, mas como só vamos fazer uso dele, por enquanto, para assinar o evento, não há necessidade.

Agora temos que escrever o tratador do evento:

    private void HandleHotkeyPressed(int index)
    {
        if (index < 0 || index >= _slots.Length )
        {
            return;
        }
    
        if (_slots[index].IsEmpty == false)
        {
            _inventory.Equip(_slots[index].Item);
        }
    }

Ele vai receber o index do slot e verificar se há um item setado para ele. Caso haja, esse item vai ser equipado.
Temos que alterar o acesso do Inventory.Equip() para public, assim como criar uma propriedade somente de leitura para acessar o _item em Slot.

    public Item Item => _item;

ou excluir o _item e definir a propriedade como abaixo:

    public Item Item { get; private set; }
    
Quando trabalhamos com eventos, é importante lembrar de se desregistrar quando o objeto fica desabilitado. 
Isso evita que nossa classe se registre mais de uma vez no mesmo evento, gerando múltiplas chamadas dos métodos.
Na classe Hotbar:

    private void OnDisable()
    {
        FindObjectOfType<Player>().PlayerInput.OnHotkeyPressed -= HandleHotkeyPressed;
        _inventory.OnItemPickedUp -= HandleItemPickedUp;
    }
    
Agora, também utilizando eventos, vamos refatorar a parte que alterna entre os modos de movimento do Player.
Toda a sua lógica, incluíndo a leitura de input, está dentro de Player. Vamos alterar isso criando um evento OnMoveModeTogglePressed em PlayerInput e assinando-o em Player:

    PlayerInput.OnMoveModeTogglePressed += HandleMoveModeTogglePressed;

Sua implementação em PlayerInput fica dentro de Tick() da seguinte forma:

    if (OnMoveModeTogglePressed != null && Input.GetKeyDown(KeyCode.Minus))
        {
            OnMoveModeTogglePressed();
        }

Já a implementação do HandleMoveModeTogglePressed fica assim:

    private void HandleMoveModeTogglePressed()
    {
        if (_playerMoviment is NavmeshPlayerMoviment)
        {
            _playerMoviment = new PlayerMoviment(this);
            GetComponent<NavMeshAgent>().enabled = false;
        }
        else
        {
            _playerMoviment = new NavmeshPlayerMoviment(this);
        }
    }
    
Dessa forma o código ficou mais claro e com as responsabilidades com as classes corretas.

NOMES E TEXTO DOS SLOTS

A configuração dos nossos Slots se dá de forma manual. Temos que alterar tanto o seu nome como o número representado no texto caso a caso.
É interessante que isso aconteça automaticamente.

Como estamos utilizando o TextMeshPro nos textos da UI, precisamos adicionar o seu assembly para que possa ser referenciado nos scripts.
Fazemos isso no Scripts.asmdef na pasta de scripts. Nele vamos adicionar um novo Assembly Definition Reference e selecionar o Unity.TextMeshPro.
Agora já podemos referencia-lo utilizando using TMPro

Na classe Slot vamos adicionar uma variável para armazenar o texto:

    private TMP_Text _text;
    
Vamos implementar o método OnValidate da seguinte forma:

     private void OnValidate()
    {
        _text = GetComponentInChildren<TMP_Text>();
        int hotkeyNumber = transform.GetSiblingIndex() + 1;
        _text.SetText(hotkeyNumber.ToString());
        gameObject.name = "Slot " + hotkeyNumber;
    }

Explicando:
Estamos pegando o índice do gameObject entre os seus irmãos e adicionando 1, uma vez que os índices começam em 0.
Estamos adicionando esse número no texto do Slot.
E também alterando o seu nome com esse índice.

TESTANDO OS ÍCONES
O teste para os ícones se parece muito com o teste para o Crosshair:

As principais alterações foram:
Utilização do new WaitForFixedUpdate, para que não houvesse risco do frame seguinte acontecer antes da simulação de física. É importante alterar nos outros testes que utilizam física.
Renomear Slot.Icon para Slot.IconImage, para reduzir a possibilidade de confusão. Já que Item.Icon faz referência a um Sprite e Slot.Icon a um Image.

Outra alteração interessante é utilizar o método abaixo para inicializar certas variáveis e carregar as cenas:

[UnitySetUp]
    public IEnumerator init()
    {
        yield return TestHelper.LoadItemTestScene();
        _player = TestHelper.GetPlayer();
        _item = GameObject.FindObjectOfType<Item>();
    }
    
Esse método é chamado antes de todos os outros métodos. Assim podemos excluir essas linhas redundantes e utilizar as variáveis _player e _item,
 */
#endregion

#region EDITOR CUSTOMIZADO
/*
É possivel customizar o Inspector do Unity para facilitar a sua utilização.
Existem algumas ferramentas prontas que ficilitam isso, como o Odin.

Vamos criar um editor customizado para criação dos itens.
Para isso, vamos criar uma nova classe:

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        Item item = (Item) target;
        EditorGUILayout.LabelField("Custom Item Editor");
        GUILayout.Box(item.Icon.texture, GUILayout.Height(40), GUILayout.Width(40));

        base.OnInspectorGUI();
    }
}

A tag [CustomEditor(typeof(Item))] indica que essa é uma classe para criação de um editor customizado para a classe Item.
A classe deve herdar de Editor e não de monobehaviour.
Para começarmos a alterar o Inspector, vamos sobrescrever o método OnInspectorGui.
Todo inspector tem um objeto alvo, que é o que está sendo mostrado no inspector. Para pegarmos esse objeto, utilizamos: Item item = (Item) target; Detalhe para o cast.
EditorGuiLayout apresenta os itens que podemos criar no nosso inspector.

 */
#endregion
























