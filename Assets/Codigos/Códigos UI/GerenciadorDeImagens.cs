using UnityEngine;
using UnityEngine.UIElements;

public class GerenciadorDeImagens : MonoBehaviour
{
    // Crie uma lista pública para você arrastar suas imagens no Inspector do Unity
    public Sprite[] artesDasCartas; 

    private UIDocument _uiDocument;
    private VisualElement _container;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _container = _uiDocument.rootVisualElement.Q<VisualElement>("ContainerImagens");

        PreencherImagens();
    }

    private void PreencherImagens()
    {
        // Se não houver container ou não houver artes cadastradas, paramos aqui
        if (_container == null || artesDasCartas.Length == 0) return;

        // Vamos passar por cada caixinha dentro do container
        for (int i = 0; i < _container.childCount; i++)
        {
            // Para não dar erro caso você tenha mais caixas do que imagens cadastradas
            if (i < artesDasCartas.Length)
            {
                VisualElement caixinha = _container[i];

                // A mágica acontece aqui: Trocamos o background via código!
                // O UI Toolkit exige que a imagem seja "embrulhada" nesse StyleBackground
                caixinha.style.backgroundImage = new StyleBackground(artesDasCartas[i]);
            }
        }
    }
}