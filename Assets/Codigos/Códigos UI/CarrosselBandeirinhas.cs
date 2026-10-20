using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class CarrosselBandeirinhas : MonoBehaviour
{
    // --- NOVA OPÇÃO PARA O INSPECTOR ---
    public enum DirecaoMovimento { Direita, Esquerda }

    [Header("Configurações das Bandeirinhas")]
    [Tooltip("Escolha para qual lado as bandeirinhas vão deslizar")]
    public DirecaoMovimento direcao = DirecaoMovimento.Direita;
    
    public int quantidadeDeBandeirinhas = 5;
    public Vector2 tamanhoBandeirinha = new Vector2(150, 200); 
    public List<Sprite> artesDisponiveis;

    [Header("Controles")]
    [Tooltip("Qual tecla o jogador aperta para mover este carrossel?")]
    public Key teclaDeAtivacao = Key.RightArrow;

    private UIDocument _uiDocument;
    private VisualElement _container;
    private bool _emAnimacao = false;

    [SerializeField]
    private GameObject TurnController;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _container = _uiDocument.rootVisualElement.Q<VisualElement>("ContainerBandeirinhas");

        GerarBandeirinhas();
    }

    void Update()
    {
        if (_container == null || _container.childCount < 2 || _emAnimacao) return;
        if (Keyboard.current == null) return;

        // Agora o script lê a tecla que você configurou no Inspector!
        if (Keyboard.current[teclaDeAtivacao].wasPressedThisFrame)
        {
            Deslizar();
        }
    }

    private void GerarBandeirinhas()
    {
        if (_container == null || artesDisponiveis.Count == 0) return;
        _container.Clear();

        for (int i = 0; i < quantidadeDeBandeirinhas; i++)
        {
            VisualElement novaBandeirinha = new VisualElement();
            novaBandeirinha.style.width = tamanhoBandeirinha.x;
            novaBandeirinha.style.height = tamanhoBandeirinha.y;
            novaBandeirinha.style.marginLeft = 10;
            novaBandeirinha.style.marginRight = 10;

            int indexArte = i % artesDisponiveis.Count;
            novaBandeirinha.style.backgroundImage = new StyleBackground(artesDisponiveis[indexArte]);
            novaBandeirinha.style.backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain));

            _container.Add(novaBandeirinha);
        }
    }

    public void Deslizar()
    {
        _emAnimacao = true;
        float passo = _container[1].layout.position.x - _container[0].layout.position.x;

        LigarAnimacao(_container, 0.3f);

        // --- VERIFICA A DIREÇÃO ESCOLHIDA NO INSPECTOR ---
        if (direcao == DirecaoMovimento.Direita)
        {
            _container.style.translate = new Translate(passo, 0, 0);

            _container.schedule.Execute(() => {
                DesligarAnimacao(_container); 
                VisualElement ultima = _container[_container.childCount - 1];
                ultima.RemoveFromHierarchy();
                _container.Insert(0, ultima);     
                _emAnimacao = false;          
            }).StartingIn(300);
            
        }
        else // SE FOR PARA A ESQUERDA (Vilões)
        {
            _container.style.translate = new Translate(-passo, 0, 0); // Desliza negativo

            _container.schedule.Execute(() => {
                DesligarAnimacao(_container); 
                VisualElement primeira = _container[0];
                _container.Add(primeira); // Tira do índice 0 e joga pro final da fila
                _emAnimacao = false;          
            }).StartingIn(300);
        }
    }

    private void LigarAnimacao(VisualElement elemento, float tempo)
    {
        elemento.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("translate") };
        elemento.style.transitionDuration = new List<TimeValue> { new TimeValue(tempo, TimeUnit.Second) };
    }

    private void DesligarAnimacao(VisualElement elemento)
    {
        elemento.style.transitionDuration = new List<TimeValue> { new TimeValue(0, TimeUnit.Second) };
        elemento.style.translate = new Translate(0, 0, 0); 
    }
}