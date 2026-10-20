using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;

public class CarrosselController : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [Tooltip("O caminho que o carrossel vai fazer")]
    [SerializeField] private SplineContainer caminhoCarrossel;
    
    [Tooltip("O Prefab base que será clonado (precisa ter um SpriteRenderer)")]
    [SerializeField] private GameObject prefabBase;
    
    [Tooltip("Quantas imagens o carrossel terá no total")]
    [SerializeField] private int numeroDeImagensNoCarrossel = 5;
    
    [Tooltip("Tempo em segundos que elas levam para deslizar até a posição")]
    [SerializeField] private float tempoDeDeslizamento = 1.5f;

    [Header("Imagens Manuais")]
    [Tooltip("Arraste as imagens (Sprites) para esta lista. O código aplicará na ordem.")]
    [SerializeField] private List<Sprite> imagensDoCarrossel = new List<Sprite>();

    private List<GameObject> itensInstanciados = new List<GameObject>();

    private void Start()
    {
        ConstruirCarrossel();
    }

    private void ConstruirCarrossel()
    {
        // Descobre onde é o ponto inicial exato (1.0 = final da spline, ou seja, à direita)
        Spline spline = caminhoCarrossel.Spline;
        Vector3 pontoDeNascimentoLocal = spline.EvaluatePosition(1f);
        Vector3 pontoDeNascimentoGlobal = caminhoCarrossel.transform.TransformPoint(pontoDeNascimentoLocal);

        // Calcula a distância entre uma imagem e outra (distribuído de 0 a 1)
        float espacamento = 1f / Mathf.Max(1, (numeroDeImagensNoCarrossel - 1));

        for (int i = 0; i < numeroDeImagensNoCarrossel; i++)
        {
            // 1. Instancia todas as imagens no mesmo ponto inicial (à direita)
            GameObject novoItem = Instantiate(prefabBase, pontoDeNascimentoGlobal, Quaternion.identity);

            // 2. Tenta colocar a imagem manual da lista (se você tiver colocado no Inspector)
            if (i < imagensDoCarrossel.Count && imagensDoCarrossel[i] != null)
            {
                SpriteRenderer renderizador = novoItem.GetComponent<SpriteRenderer>();
                if (renderizador != null)
                {
                    renderizador.sprite = imagensDoCarrossel[i];
                }
            }

            itensInstanciados.Add(novoItem);

            // 3. Calcula o destino final desta imagem na curva
            // i = 0 fica em 1.0 (extrema direita)
            // i = 1, 2, 3 vão subtraindo o espaçamento, indo cada vez mais para a esquerda (até 0.0)
            float pontoAlvo = 1f - (i * espacamento);

            // 4. Converte o destino para o espaço global da tela
            Vector3 destinoLocal = spline.EvaluatePosition(pontoAlvo);
            Vector3 destinoGlobal = caminhoCarrossel.transform.TransformPoint(destinoLocal);

            // OPCIONAL: Zera o eixo Z para evitar que fiquem atrás do cenário
            destinoGlobal.z = -1f;

            // 5. Faz a imagem deslizar suavemente do ponto de nascimento até o destino final
            // Adicionei um "Ease.OutBack" que dá um leve "efeito elástico" ao chegar.
            novoItem.transform.DOMove(destinoGlobal, tempoDeDeslizamento)
                            .SetEase(Ease.OutBack)
                            .SetDelay(i * 0.1f); // Dá um pequeno atraso entre cada uma para um efeito de "cascata"
        }
    }
}