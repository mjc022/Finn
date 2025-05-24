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
    public LayerMask capaSuelo; // Punto de verificación del suelo
    private bool Jump; // Indica si el jugador está en el suelo
    private bool hit; // Indica si el jugador ha recibido daño
    private bool atacando; // Indica si el jugador está atacando
    public bool muerto; // Indica si el jugador está muerto
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
                Movimiento(); // Llamar al método de movimiento

                Salto(); // Llamar al método de salto


            }
        }

        Animaciones(); // Llamar al método de animaciones


    }

    //Script de animacion de recibir daño
    public void Damage(Vector2 direccion, int daño)
    {
        if (!hit)
        {
            hit = true; // Indicar que el jugador ha recibido daño
            vida -= daño; // Restar vida al jugador
            if (vida <= 0)
            {
                // Si la vida del jugador es menor o igual a 0
                muerto = true; // Indicar que el jugador está muerto
                Animaciones();
            }

            if (!muerto)
            {
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, 1).normalized; // Calcular la dirección del rebote
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse); // Aplicar fuerza en la dirección del rebote     
            }
                   
        }
    }
    public void Animaciones()
    {
        animator.SetBool("enSuelo", Jump); // Establecer el parámetro "enSuelo" en el animator
        animator.SetBool("damage", hit); // Activar la animación de recibir daño
        animator.SetBool("Atack", atacando); // Activar la animación de ataque
        animator.SetBool("Muerto", muerto); // Activar la animación de muerte
    }
    public void Movimiento()
    {
        float velocidadX = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("Movement", Mathf.Abs(velocidadX)); // para evitar valor negativo en animación

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

        if (Input.GetKeyDown(KeyCode.Space) && Jump && !hit) // Si se presiona la tecla de salto y el jugador está en el suelo
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // Aplicar fuerza hacia arriba para saltar
            animator.SetTrigger("enSuelo"); // Activar la animación de salto
        }
        // Si se presiona el click izquierdo ataca, si el jugador no está atacando y está en el suelo
        if (Input.GetMouseButtonDown(0) && !atacando && Jump) // Si se presiona el botón izquierdo del ratón y el jugador no está atacando
        {
            Attack(); // Llamar al método de ataque
        }
    }

    // Método para reiniciar el estado de daño
    public void ResetHit()
    {
        hit = false; // Reiniciar el estado de daño
        
    }

    // Método para reiniciar el estado de ataque
    public void Attack()
    {
        atacando = true; // Indicar que el jugador está atacando  
    }

    // Método para reiniciar el estado de ataque
    // Se llama desde la animación de ataque
    // para que el ataque dure el tiempo de la animación
    // y no se pueda cancelar
    // Reinicia el estado de ataque después de un tiempo
    public void ResetAttack()
    {
        atacando = false; // Reiniciar el estado de ataque
    }



    //Gizmoz para visualizar el raycast en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Color del gizmo
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaicast); // Dibujar la línea del raycast
    }

    
}

