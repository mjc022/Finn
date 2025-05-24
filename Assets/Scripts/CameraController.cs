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
    public Transform objetivo; // El objeto que la cámara seguirá
    public float velocidadCamera = 0.025f; // Velocidad de la cámara
    public Vector3 desplazamiento; // Desplazamiento de la cámara respecto al objeto objetivo

    private void LateUpdate()
    {
                    // Actualiza la posición de la cámara para que siga al objeto objetivo
        Vector3 posicionDeseada = objetivo.position + desplazamiento;                            // La posición deseada es la posición del objetivo más el desplazamiento
        Vector3 posicionSavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamera); // Interpola suavemente entre la posición actual de la cámara y la posición deseada
        transform.position = posicionSavizada; // Actualiza la posición de la cámara
    }
}
