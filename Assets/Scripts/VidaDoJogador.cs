using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class VidaDoJogador : MonoBehaviour
{
    [Header("Verificacoes")]
    public bool jogadorVivo;

    [Header("Parametros")]
    [SerializeField] private float tempoParaGameOver;

    [Header("Controle da Vida")]
    [SerializeField] private int vidaMaxima;
    private int vidaAtual;

    void Start()
    {
        // Configura a vida do Jogador
        jogadorVivo = true;
        vidaAtual = vidaMaxima;

        UIManager.instance.AtualizarBarraDeVidaDoJogador(vidaMaxima,vidaAtual);
    }

    public void GanharVida(int vidaParaGanhar)
    {
        // Se a vida atual somada com a vida para ganhar nao supera o limite
        if(vidaAtual + vidaParaGanhar <= vidaMaxima)
        {
            vidaAtual += vidaParaGanhar;
        }
        // Se a vida atual somada com a vida para ganhar superar o limite
        else  
        {
            vidaAtual = vidaMaxima;
        }

        // Atualiza o status da Barra de vida do Jogador
        UIManager.instance.AtualizarBarraDeVidaDoJogador(vidaMaxima,vidaAtual);

    }

    public void LevarDano(int danoParaReceber)
    {
        // Aplica o dano do jogador
        if(jogadorVivo)
        {
            vidaAtual -= danoParaReceber;
            

            GetComponent<ControleDoJogador>().RodarAnimacaoDeDano();
            UIManager.instance.AtualizarBarraDeVidaDoJogador(vidaMaxima,vidaAtual);
            SoundManager.instance.jogadorLevandoDano.Play();

            if(vidaAtual <= 0)
            {
                jogadorVivo = false;
                GetComponent<ControleDoJogador>().RodarAnimacaoDeDerrota();
                StartCoroutine(AtivarGameOver());
            }
        }
    }

    private IEnumerator AtivarGameOver()
    {
        // Espera por X segundos e ativa o Painel de Game Over
        yield return new WaitForSeconds(tempoParaGameOver);
        UIManager.instance.AtivarPainelDeGameOver();
    }
}
