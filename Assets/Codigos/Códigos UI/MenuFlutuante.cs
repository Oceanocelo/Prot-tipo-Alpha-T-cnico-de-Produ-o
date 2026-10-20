using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class MenuFlutuante : MonoBehaviour
{
    [Header("Personagem que está jogando")]
    public Transform personagemDaVez; 

    [Header("Ajuste de Posição")]
    public Vector2 deslocamento = new Vector2(100, -50); 

    private UIDocument _uiDocument;
    private VisualElement _menuBatalha;
    
    // Referência para o novo bloco
    private VisualElement _blocoDetalhes; 
    
    private List<VisualElement> _opcoesMenu = new List<VisualElement>();
    public int _indiceSelecionado = 0;

    // Setup attacks moves:
    private bool TurnoPlayer;
    private GameObject player;
    private GameObject enemy;
    private GameObject CurrentAction;
    [SerializeField]
    private GameObject Attack;
    [SerializeField]
    private GameObject Special;
    [SerializeField]
    private GameObject Item;
    [SerializeField]
    private GameObject TurnController;
    [SerializeField]
    private GameObject bandeira;
    [SerializeField]
    private GameObject bandeiraenemy;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _menuBatalha = _uiDocument.rootVisualElement.Q<VisualElement>("menuzinho");
        
        // Puxa o novo bloco pelo nome
        _blocoDetalhes = _uiDocument.rootVisualElement.Q<VisualElement>("menuzao");

        // Garante que o bloco detalhes comece invisível
        if (_blocoDetalhes != null)
        {
            _blocoDetalhes.style.display = DisplayStyle.None;
        }

        _opcoesMenu.Add(_menuBatalha.Q<VisualElement>("Ataque"));
        _opcoesMenu.Add(_menuBatalha.Q<VisualElement>("Especial"));
        _opcoesMenu.Add(_menuBatalha.Q<VisualElement>("Item"));

        foreach(var opcao in _opcoesMenu)
        {
            if (opcao != null)
            {
                opcao.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("scale") };
                opcao.style.transitionDuration = new List<TimeValue> { new TimeValue(0.15f, TimeUnit.Second) };
            }
        }

        AtualizarDestaque();
    }

    void Update()
    {
        if (personagemDaVez == null)
        {
            _menuBatalha.style.display = DisplayStyle.None;
            
            // Se esconder o menu, esconde o sub-menu também
            if (_blocoDetalhes != null) _blocoDetalhes.style.display = DisplayStyle.None; 
            return;
        }
        personagemDaVez = GameObject.FindGameObjectWithTag("Player").transform;
        _menuBatalha.style.display = DisplayStyle.Flex;

        IPanel painelDaUI = _menuBatalha.panel;
        Vector2 posicaoNaTela = RuntimePanelUtils.CameraTransformWorldToPanel(painelDaUI, personagemDaVez.position, Camera.main);
        _menuBatalha.style.left = posicaoNaTela.x + deslocamento.x;
        _menuBatalha.style.top = posicaoNaTela.y + deslocamento.y;

        if (Keyboard.current == null) return;

        // --- NAVEGAÇÃO ---
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
        {
            _indiceSelecionado--;
            if (_indiceSelecionado < 0) _indiceSelecionado = _opcoesMenu.Count - 1;
            AtualizarDestaque();
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            _indiceSelecionado++;
            if (_indiceSelecionado >= _opcoesMenu.Count) _indiceSelecionado = 0;
            AtualizarDestaque();
        }

        // --- AÇÃO DE CONFIRMAR (ENTER OU ESPAÇO) ---
         if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ConfirmarAcao();
        }
        bool TurnoPlayer = TurnController.GetComponent<GameplayController>().TurnOrder;
        if (!TurnoPlayer) { _menuBatalha.style.display = DisplayStyle.None; }
        else { _menuBatalha.style.display = DisplayStyle.Flex; }
    }

    private void AtualizarDestaque()
    {
        for (int i = 0; i < _opcoesMenu.Count; i++)
        {
            if (_opcoesMenu[i] == null) continue;

            if (i == _indiceSelecionado)
            {
                _opcoesMenu[i].style.scale = new StyleScale(new Scale(new Vector3(1.30f, 1.30f, 1f)));
            }
            else
            {
                _opcoesMenu[i].style.scale = new StyleScale(new Scale(Vector3.one));
            }
        }
    }

    // --- LÓGICA DO NOVO BLOCO ---
    public void ConfirmarAcao()
    {
        if (_blocoDetalhes == null) return;

        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject Victim = player;
        GameObject Attacker = enemy;
        if (tag == "Player") {
            Victim = enemy;
            Attacker = player;
        }

        // Se o bloco já estiver visível, nós o escondemos. Senão, nós mostramos. (Efeito de liga/desliga)
        if (_blocoDetalhes.style.display == DisplayStyle.Flex)
        {
            _blocoDetalhes.style.display = DisplayStyle.None;
        }
        else
        {
            //_blocoDetalhes.style.display = DisplayStyle.Flex;

            // DICA: Como temos a variável _indiceSelecionado, você sabe exatamente qual opção o jogador escolheu!
            if (!TurnoPlayer) { bandeiraenemy.GetComponent<CarrosselBandeirinhas>().Deslizar(); }
            if (_indiceSelecionado == 0 && !TurnoPlayer) { Attack.GetComponent<AttackSetup>().Attack(Victim, Attacker); TurnController.GetComponent<GameplayController>().NextTurn(); } // ATT
            if (_indiceSelecionado == 1 && !TurnoPlayer) { Special.GetComponent<AttackSetup>().Special(Victim, Attacker); TurnController.GetComponent<GameplayController>().NextTurn(); } // SPECIAL
            if (_indiceSelecionado == 2 && !TurnoPlayer) { Item.GetComponent<AttackSetup>().Item(Attacker, Victim); TurnController.GetComponent<GameplayController>().NextTurn(); } // ITEM
            Debug.Log("O jogador abriu o menu para a opção: " + _indiceSelecionado + "atacando  " + Attacker + "/" + Victim);
        }
    }
    public void ConfirmarAcaoEnemy() {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject Victim = enemy;
        GameObject Attacker = player;
        if (_blocoDetalhes.style.display == DisplayStyle.Flex)
        {
            _blocoDetalhes.style.display = DisplayStyle.None;
        }
        else
        {
            //_blocoDetalhes.style.display = DisplayStyle.Flex;

            // DICA: Como temos a variável _indiceSelecionado, você sabe exatamente qual opção o jogador escolheu!
            bandeira.GetComponent<CarrosselBandeirinhas>().Deslizar();
            if (_indiceSelecionado == 0 ) { Attack.GetComponent<AttackSetup>().Attack(Victim, Attacker); TurnController.GetComponent<GameplayController>().NextTurn(); } // ATT
            if (_indiceSelecionado == 1 ) { Special.GetComponent<AttackSetup>().Special(Victim, Attacker); TurnController.GetComponent<GameplayController>().NextTurn(); } // SPECIAL
            if (_indiceSelecionado == 2 ) { Item.GetComponent<AttackSetup>().Item(Attacker, Victim); TurnController.GetComponent<GameplayController>().NextTurn(); } // ITEM
            Debug.Log("O Inimigo abriu o menu para a opção: " + _indiceSelecionado + "atacando  "+ Attacker + "/" + Victim);
        }
    }
}