using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    Transform cam; //Camara que sigue al jugador
    Vector3 camStartPos;
    float distance; // Distancia entre la camara y el fondo

    GameObject[] backgrounds;   //Array para background
    Material[] mat;             //Array para materiales de los backgrounds
    float[] backSpeed;          //Array para la velocidad de los backgrounds

    float farthestBack;         //Fondo más lejano

    [Range(0.01f, 1f)]          //Velocidad de movimiento del fondo
    public float parallaxSpeed; // Velocidad de movimiento del fondo

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;        //Camara principal
        camStartPos = cam.position;         //Posición inicial de la camara

        int backCount = transform.childCount;   //Contar los hijos del objeto padre
        mat = new Material[backCount];          //Array para materiales de los backgrounds
        backSpeed = new float[backCount];       //Array para la velocidad de los backgrounds
        backgrounds = new GameObject[backCount];// //Array para background

        for (int i = 0; i < backCount; i++)     //Contar los hijos del objeto padre
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);      //Calcular la velocidad de los backgrounds
    }

    void BackSpeedCalculate(int backCount)      //Calcular la velocidad de los backgrounds
    {
        for (int i = 0; i < backCount; i++) //busca el fondo más lejano
        
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++) //Calcula la velocidad de los backgrounds
        
            //La velocidad de los backgrounds es inversamente proporcional a la distancia entre la camara y el fondo
            //La velocidad de los backgrounds es 1 cuando la distancia es 0 y 0 cuando la distancia es igual a farthestBack
            //backSpeed[i] = (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
            //backSpeed[i] = 1 - backSpeed[i];
            {
                backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }
    }

    private void LateUpdate() 
        
        //Llamado después de Update para evitar problemas de sincronización con la camara
        //La posición de la camara se guarda en la variable camStartPos y se usa para calcular la distancia entre la camara y el fondo
        //La posición de los backgrounds se actualiza en función de la distancia entre la camara y el fondo
        //Calcula la distancia entre la camara y el fondo
        {
            distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x - 2, transform.position.y, 3.14f);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
