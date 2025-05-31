using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class LifeBar : MonoBehaviour
{

    public Image barraVida; // referencia a la imagen de la barra de vida
    private PlayerController playerController; // referencia al script del jugador
    private float vidaMaxima; // vida maxima del jugador


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>(); // encontrar el script del jugador en la escena
        vidaMaxima=playerController.vida; // obtener la vida maxima del jugador
    }

    

    // Update is called once per frame
    void Update()
    {
        barraVida.fillAmount = playerController.vida / vidaMaxima; // actualizar la barra de vida en base a la vida actual del jugador
    }
}