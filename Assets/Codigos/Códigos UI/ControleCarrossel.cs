using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ControleCarrossel : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _container;
    private VisualElement _quadradoDestaque; 
    
    private bool _emAnimacao = false; 

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _container = _uiDocument.rootVisualElement.Q<VisualElement>("ContainerImagens");
        _quadradoDestaque = _uiDocument.rootVisualElement.Q<VisualElement>("QuadradoDestaque"); 
    }

    void Update()
    {
        //Se não tiver imagens na lista, o código não funciona
        if (_container == null || _container.childCount < 3 || _emAnimacao) return;
        if (Keyboard.current == null) return;

        //Aqui que vai ser modificado dependendo da necessidade

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            AnimarM(); //para mover as imagens pra esquerda
        }

        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            AnimarN(); //para retornar casinhas
        
        }
        //-----------------------------------------------------
    }

    private void LigarAnimacao(VisualElement elemento, float tempoSegundos)
    {
        elemento.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("translate") };
        elemento.style.transitionDuration = new List<TimeValue> { new TimeValue(tempoSegundos, TimeUnit.Second) };
    }

    private void DesligarAnimacao(VisualElement elemento)
    {
        elemento.style.transitionDuration = new List<TimeValue> { new TimeValue(0, TimeUnit.Second) };
        elemento.style.translate = new Translate(0, 0, 0); 
    }

    // --- BALANÇADINHA ---
    private void BalancarQuadrado()
    {
        if (_quadradoDestaque == null) return;

        // 1. Avisa que vamos animar a rotação por 0.1 segundos (rápido!)
        _quadradoDestaque.style.transitionProperty = new List<StylePropertyName> { new StylePropertyName("rotate") };
        _quadradoDestaque.style.transitionDuration = new List<TimeValue> { new TimeValue(0.1f, TimeUnit.Second) };

        // 2. Gira 5 graus para a esquerda
        _quadradoDestaque.style.rotate = new Rotate(new Angle(-5, AngleUnit.Degree));

        // 3. Após 100 milissegundos, gira 5 graus para a direita
        _quadradoDestaque.schedule.Execute(() => {
            _quadradoDestaque.style.rotate = new Rotate(new Angle(5, AngleUnit.Degree));
        }).StartingIn(100);

        // 4. Após 200 milissegundos, volta a ficar reto e desliga a animação
        _quadradoDestaque.schedule.Execute(() => {
            _quadradoDestaque.style.rotate = new Rotate(new Angle(15, AngleUnit.Degree));
            
            _quadradoDestaque.schedule.Execute(() => {
                _quadradoDestaque.style.transitionDuration = new List<TimeValue> { new TimeValue(0, TimeUnit.Second) };
            }).StartingIn(100);

        }).StartingIn(200);
    }

    private void AnimarM()
    {
        _emAnimacao = true;
        VisualElement primeira = _container[0];

        // Troca a imagem e chama a balançadinha!
        if (_quadradoDestaque != null)
        {
            _quadradoDestaque.style.backgroundImage = primeira.style.backgroundImage;
            BalancarQuadrado(); // <--- CHAMAMOS A ANIMAÇÃO AQUI
        }

        float passo = _container[1].layout.position.x - _container[0].layout.position.x;

        LigarAnimacao(_container, 0.3f);
        _container.style.translate = new Translate(-passo, 0, 0);

        _container.schedule.Execute(() => {
            DesligarAnimacao(_container); 
            _container.Add(primeira);     
            _emAnimacao = false;          
        }).StartingIn(300);
    }

    private void AnimarN()
    {
        _emAnimacao = true;
        VisualElement primeira = _container[0];
        
        // Se quiser trocar a foto no N, descomente as linhas abaixo (já inclui a balançadinha):
        /*
        if (_quadradoDestaque != null) {
            _quadradoDestaque.style.backgroundImage = primeira.style.backgroundImage;
            BalancarQuadrado();
        }
        */

        //O código trabalha pelo numero dos container(a lista de imagens)  
        //No futuro, vamos ter que arranjar um jeito de adicionar as imagens no container por código, 

        VisualElement segunda = _container[1];
        VisualElement terceira = _container[2];

        float passo = _container[1].layout.position.x - _container[0].layout.position.x;

        LigarAnimacao(primeira, 0.3f);
        LigarAnimacao(segunda, 0.3f);
        LigarAnimacao(terceira, 0.3f);

        //movimentação abaixo

        primeira.style.translate = new Translate(passo * 2, 0, 0);
        segunda.style.translate = new Translate(-passo, 0, 0);
        terceira.style.translate = new Translate(-passo, 0, 0);

        _container.schedule.Execute(() => {
            DesligarAnimacao(primeira);
            DesligarAnimacao(segunda);
            DesligarAnimacao(terceira);

            primeira.RemoveFromHierarchy();
            _container.Insert(2, primeira);

            _emAnimacao = false;
        }).StartingIn(300);
    }
}