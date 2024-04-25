using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaDoInimigo : MonoBehaviour
{
    [Header("Verificacoes")]
    public bool inimigoVivo;

    [Header("Parametros")]
    [SerializeField] private string nomeDoInimigo;

    [Header("Controle da vida")]
    [SerializeField] private int vidaMaxima;
    private int vidaAtual;
    [SerializeField] private float tempoParaSumir;


    [Header("Drop ao morrer")]
    [SerializeField] private int chanceDeDroparComida;
    [SerializeField] private GameObject[] comidasParaDropar;

    private void Start()
    {
        // Configura a vida do Inimigo
        inimigoVivo = true;
        vidaAtual = vidaMaxima;
    }

    public void LevarDano(int danoParaReceber)
    {
        // Aplica o dano no inimigo
        if(inimigoVivo)
        {
            vidaAtual -= danoParaReceber;
            GetComponent<ControleDoInimigo>().RodarAnimacaoDeDano();
            UIManager.instance.AtualizaBarraDeVidaDoInimigoAtual(vidaMaxima, vidaAtual, nomeDoInimigo);
            SoundManager.instance.inimigoLevandoDano.Play();

            if(vidaAtual <= 0)
            {
                inimigoVivo = false;
                GetComponent<ControleDoInimigo>().RodarAnimacaoDeDerrota();
                SpawnarComida();
                UIManager.instance.DesativarPainelDoInimigo();

                
                Destroy(this.gameObject, tempoParaSumir);
                
            }
        }
    }
    
    private void SpawnarComida()
    {
        // Sorteia a chance de drop de comida
        int numeroAleatorio = Random.Range(0,101);

        // ROda se a chance estiver dentro do limite
        if(numeroAleatorio <= chanceDeDroparComida)
        {
        GameObject comidaEscolhida = comidasParaDropar[Random.Range(0,comidasParaDropar.Length)];
        // Instantiate(GameObject, position, rotation);
        Instantiate(comidaEscolhida, transform.position, transform.rotation);
        }
    }

}
