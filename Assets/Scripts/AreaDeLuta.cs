using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeLuta : MonoBehaviour
{
    
    [Header("Verificacoes")]
    private bool podeVerificarJogador;
    private bool podeSpawnar;

    [Header("Cronometros Do Spawn")]
    [SerializeField] private float tempoMaximoEntreSpawns;
    private float tempoAtualEntreSpawns;

    [Header("Controle Do Spawn")]
    [SerializeField] private Transform[] pontosDeSpawn;
    [SerializeField] private GameObject[] inimigosParaSpawnar;
    private int inimigosSpawnados;
    private int inimigoAtual;

    private void Start()
    {
        podeVerificarJogador = true;
        podeSpawnar = false;
        inimigoAtual = 0;
        inimigosSpawnados = 0;

    }

    
    private void Update()
    {
        if(podeSpawnar && inimigosSpawnados < inimigosParaSpawnar.Length){
            RodarCronometroDoSpawn();
        }

    }

    private void RodarCronometroDoSpawn()
    {
        // Controla a quantidade de inimigos spawnados por segundo
        tempoAtualEntreSpawns -= Time.deltaTime;
        if(tempoAtualEntreSpawns <= 0)
        {
            SpawnarInimigo();
            tempoAtualEntreSpawns = tempoMaximoEntreSpawns;
        }
    }

    private void SpawnarInimigo()
    {
        // Escolhe um novo Local de Spawn e um novo inimigo
        Transform pontoAleatorio = pontosDeSpawn[Random.Range(0,pontosDeSpawn.Length)];
        GameObject novoInimigo = inimigosParaSpawnar[inimigoAtual];


        // Spawna o novo inimigo no local escolhido anteriormente
        Instantiate(novoInimigo, pontoAleatorio.position, pontoAleatorio.rotation);
        inimigoAtual++;
        inimigosSpawnados++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(podeVerificarJogador ==  true)
        {
            if(other.gameObject.GetComponent<ControleDoJogador>() != null)
            {
                podeSpawnar = true;
                podeVerificarJogador = false;
            }
        }
    }
}
