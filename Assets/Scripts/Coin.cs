using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class Coin : MonoBehaviour
{
    //funcion para detectar colicion con el jugador 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //verificar si el objeto colisionado es el jugador
        if (collision.CompareTag("Player"))
        {
            //destruir el objeto
            Destroy(gameObject);
        }
    }
}
