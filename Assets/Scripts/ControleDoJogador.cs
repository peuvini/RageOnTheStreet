using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoJogador : MonoBehaviour
{
    [Header("Referencias Gerais")]
    private Rigidbody2D oRigidbody2D;
    private Animator oAnimator;

    [Header("Movimento do jogador")]
    [SerializeField] private float velocidadeDoJogador;
    private Vector2 inputDeMovimento;
    private Vector2 direcaoDoMovimento;

    [Header("Limites da Movimentacao")]
    [SerializeField] private float xMinimo;
    [SerializeField] private float xMaximo;
    [SerializeField] private float yMinimo;
    [SerializeField] private float yMaximo;

    [Header("Controle do Ataque")]
    [SerializeField] private float tempoMaximoEntreAtaques;
    private float tempoAtualEntreAtaques;
    private bool podeAtacar;


    [Header("Controle do Dano")]
    [SerializeField] private float tempoMaximoDoDano;
    private float tempoAtualDoDano;
    private bool levouDano;

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();

        tempoAtualDoDano = tempoMaximoDoDano;
    }

    
    private void Update()
    {
        if(GetComponent<VidaDoJogador>().jogadorVivo){
        RodarCronometroDosAtaques();
        if(levouDano == false)
            {
                ReceberInputs();
                RodarAnimacoesEAtaques();
                MovimentarJogador();
                EspelharJogador();
            }
            else
            {
                RodarCronometroDoDano();
            }
        }
        else 
        {
            RodarAnimacaoDeDerrota();
        }
    }

    private void RodarCronometroDosAtaques()
    {
        // controla o tempo entre um ataque e outro
        tempoAtualEntreAtaques -= Time.deltaTime;
        if(tempoAtualEntreAtaques <= 0)
        {
            podeAtacar = true;
            tempoAtualEntreAtaques = tempoMaximoEntreAtaques;
        }
    }

    private void RodarCronometroDoDano()
    {
        // Controla o congelar do Jogador ao levar dano
        tempoAtualDoDano -= Time.deltaTime;
        RodarAnimacaoDeDano();

        if(tempoAtualDoDano <= 0)
        {
            levouDano = false;
            tempoAtualDoDano = tempoMaximoDoDano;
        }
    }
    private void RodarAnimacoesEAtaques()
    {
        // Rodam as animacoes de movimento
        if(inputDeMovimento.magnitude == 0)
        {
            oAnimator.SetTrigger("parado");
        }
        else if(inputDeMovimento.magnitude != 0 )
        {
            oAnimator.SetTrigger("andando");
        }

        // Rodam as animacoes de Ataque(Soco e chute)
        if(Input.GetKeyDown(KeyCode.J) && podeAtacar == true)
        {   

            oAnimator.SetTrigger("socando");
            podeAtacar = false;
            SoundManager.instance.impactoSoco.Play();
        }
        if(Input.GetKeyDown(KeyCode.K) && podeAtacar == true)
        {
            oAnimator.SetTrigger("chutando");
            podeAtacar = false;
            SoundManager.instance.impactoChute.Play();
        }
    }
    private void ReceberInputs()
    {
        // Armazena a direcao que o Jogador quer seguir
        inputDeMovimento = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        //Testar dano do jogador
        /* if(Input.GetKeyDown(KeyCode.L))
        {
            RodarAnimacaoDeDano();
        } */
    }
     
    private void EspelharJogador()
    {
        // Faz o Jogador olhar para a direcao que esta andando (esquerda/direita)
        if(inputDeMovimento.x == 1)
        {
            transform.localScale = new Vector3(1f,1f,1f);
        }
        else if(inputDeMovimento.x == -1)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }

    private void MovimentarJogador()
    {
       // Movimenta o Jogador com base na sua direcao
       direcaoDoMovimento = inputDeMovimento.normalized; 
       oRigidbody2D.velocity = direcaoDoMovimento * velocidadeDoJogador;
       // Limita a movimentacao horizontal do jogador
       oRigidbody2D.position = new Vector2(Mathf.Clamp(oRigidbody2D.position.x, xMinimo, xMaximo),oRigidbody2D.position.y);
       // Limita a movimentacao vertical do jogador
       oRigidbody2D.position = new Vector2(oRigidbody2D.position.x, Mathf.Clamp(oRigidbody2D.position.y, yMinimo, yMaximo));
    }

    public void RodarAnimacaoDeDano()
    {
        // Roda a animacao de dano e zera a velocidade do Jogador
        oAnimator.SetTrigger("levando-dano");
        levouDano = true;

        oRigidbody2D.velocity = new Vector2(0,0);
    }

    public void RodarAnimacaoDeDerrota()
    {
        oAnimator.Play("jogador-derrotado");
    }

}
