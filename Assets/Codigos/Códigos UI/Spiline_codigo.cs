using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Necessário para alterar imagens de UI

public class Spiline_codigo : MonoBehaviour
{
    [SerializeField] private int numeroDeBandeirinhas;
    [SerializeField] private GameObject ImagemdaCarta;
    [SerializeField] private SplineContainer Linha1;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float distanciaEntreBandeirinhas = 0.1f;
    
    // Variável para você testar a injeção de imagens antes de ter o script de turnos pronto
    [SerializeField] private Sprite imagemTeste;

    private List<GameObject> ListaDeCartas = new();

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Futuramente, você chamará isso passando a imagem do personagem atual
            DrawCard(imagemTeste);
        }
    }

    // Parâmetro opcional adicionado para receber a imagem via código
    public void DrawCard(Sprite arteDoPersonagem = null)
    {
        if (ListaDeCartas.Count >= numeroDeBandeirinhas) return; 
        
        GameObject g = Instantiate(ImagemdaCarta, spawnPoint.position, spawnPoint.rotation);
        
        // --- INSERÇÃO DA IMAGEM ---
        if (arteDoPersonagem != null)
        {
            // Se for Canvas (UI)
            Image imagemUI = g.GetComponentInChildren<Image>();
            if (imagemUI != null) 
            {
                imagemUI.sprite = arteDoPersonagem;
            }
            else
            {
                // Se for 2D convencional (SpriteRenderer)
                SpriteRenderer sprite2D = g.GetComponentInChildren<SpriteRenderer>();
                if (sprite2D != null) sprite2D.sprite = arteDoPersonagem;
            }
        }

        ListaDeCartas.Add(g);
        UpdateCardPosition();
    }

    private void UpdateCardPosition()
    {
        if(ListaDeCartas.Count == 0) return; 
        
        float cardSpacing = distanciaEntreBandeirinhas;
        float firstCardPosition = 1f;
        Spline spline = Linha1.Spline;

        for (int i = 0; i < ListaDeCartas.Count; i++) 
        {
            // Impede que 'p' fique negativo e cause comportamentos estranhos fora da spline
            float p = Mathf.Clamp01(firstCardPosition - (i * cardSpacing));

            Vector3 localPosition = spline.EvaluatePosition(p);
            Vector3 worldPosition = Linha1.transform.TransformPoint(localPosition);

            Vector3 localForward = spline.EvaluateTangent(p);
            // .normalized resolve o bug da carta amassada na ponta
            Vector3 worldForward = Linha1.transform.TransformDirection(localForward).normalized;
            
            Vector3 localUp = spline.EvaluateUpVector(p);
            Vector3 worldUp = Linha1.transform.TransformDirection(localUp).normalized;

            Quaternion baseRotation = Quaternion.LookRotation(worldUp, Vector3.Cross(worldUp, worldForward));
            Quaternion rotation = baseRotation * Quaternion.Euler(0, 0, 180);

            // SetEase(Ease.OutCubic) faz o objeto deslizar e frear suavemente no final
            ListaDeCartas[i].transform.DOMove(worldPosition, 0.4f).SetEase(Ease.OutCubic);
            ListaDeCartas[i].transform.DORotateQuaternion(rotation, 0.4f).SetEase(Ease.OutCubic);
        }
    }
}