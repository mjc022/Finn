using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class Run : MonoBehaviour
{
    //codigo para que el personaje se mueva horizontalmente
    public float speed = 5f;
    public int vida= 3; // Vida del jugador

    public Rigidbody2D rb;
    public Animator animator; // referencia al animator del jugador
    //Variables para el salto
    public float jumpForce = 5f; // Fuerza del salto
    public float fuerzaRebote = 5f; // Fuerza del rebote
    public float longitudRaicast = 0.1f; // Longitud del raycast
    public LayerMask capaSuelo; // Punto de verificaci�n del suelo
    private bool Jump; // Indica si el jugador est� en el suelo
    private bool hit; // Indica si el jugador ha recibido da�o
    private bool atacando; // Indica si el jugador est� atacando
    public bool muerto; // Indica si el jugador est� muerto
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtener el componente Animator del jugador
    }



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
       if (!muerto)
        { if (!atacando)
            {
                Movimiento(); // Llamar al m�todo de movimiento

                Salto(); // Llamar al m�todo de salto


            }
        }

        Animaciones(); // Llamar al m�todo de animaciones


    }

    //Script de animacion de recibir da�o
    public void Damage(Vector2 direccion, int da�o)
    {
        if (!hit)
        {
            hit = true; // Indicar que el jugador ha recibido da�o
            vida -= da�o; // Restar vida al jugador
            if (vida <= 0)
            {
                // Si la vida del jugador es menor o igual a 0
                muerto = true; // Indicar que el jugador est� muerto
                Animaciones();
            }

            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized; // Calcular la direcci�n del rebote
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse); // Aplicar fuerza en la direcci�n del rebote     
            }
                   
        }
    }
    public void Animaciones()
    {
        animator.SetBool("enSuelo", Jump); // Establecer el par�metro "enSuelo" en el animator
        animator.SetBool("damage", hit); // Activar la animaci�n de recibir da�o
        animator.SetBool("Atack", atacando); // Activar la animaci�n de ataque
        animator.SetBool("Muerto", muerto); // Activar la animaci�n de muerte
    }
    public void Movimiento()
    {
        float velocidadX = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("Movement", Mathf.Abs(velocidadX)); // para evitar valor negativo en animaci�n

        if (!hit)
        {
            rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y); // uso correcto del Rigidbody2D
        }

        // Voltear sprite
        if (velocidadX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (velocidadX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Salto()
    {
        RaycastHit2D Rayhit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaicast, capaSuelo); // Crear un raycast hacia abajo
        Jump = Rayhit.collider != null; // Verificar si el raycast colisiona con algo en la capa de suelo

        if (Input.GetKeyDown(KeyCode.Space) && Jump && !hit) // Si se presiona la tecla de salto y el jugador est� en el suelo
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // Aplicar fuerza hacia arriba para saltar
            animator.SetTrigger("enSuelo"); // Activar la animaci�n de salto
        }
        // Si se presiona el click izquierdo ataca, si el jugador no est� atacando y est� en el suelo
        if (Input.GetMouseButtonDown(0) && !atacando && Jump) // Si se presiona el bot�n izquierdo del rat�n y el jugador no est� atacando
        {
            Attack(); // Llamar al m�todo de ataque
        }
    }

    // M�todo para reiniciar el estado de da�o
    public void ResetHit()
    {
        hit = false; // Reiniciar el estado de da�o
        
    }

    // M�todo para reiniciar el estado de ataque
    public void Attack()
    {
        atacando = true; // Indicar que el jugador est� atacando  
    }

    // M�todo para reiniciar el estado de ataque
    // Se llama desde la animaci�n de ataque
    // para que el ataque dure el tiempo de la animaci�n
    // y no se pueda cancelar
    // Reinicia el estado de ataque despu�s de un tiempo
    public void ResetAttack()
    {
        atacando = false; // Reiniciar el estado de ataque
    }



    //Gizmoz para visualizar el raycast en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Color del gizmo
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaicast); // Dibujar la l�nea del raycast
    }

    
}

