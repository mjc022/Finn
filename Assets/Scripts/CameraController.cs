using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif


public class CameraController : MonoBehaviour
{
    // Moimiento de la camara
    public Transform objetivo; // El objeto que la c�mara seguir�
    public float velocidadCamera = 0.025f; // Velocidad de la c�mara
    public Vector3 desplazamiento; // Desplazamiento de la c�mara respecto al objeto objetivo

    private void LateUpdate()
    {
                    // Actualiza la posici�n de la c�mara para que siga al objeto objetivo
        Vector3 posicionDeseada = objetivo.position + desplazamiento;                            // La posici�n deseada es la posici�n del objetivo m�s el desplazamiento
        Vector3 posicionSavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamera); // Interpola suavemente entre la posici�n actual de la c�mara y la posici�n deseada
        transform.position = posicionSavizada; // Actualiza la posici�n de la c�mara
    }
}
