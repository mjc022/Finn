using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class EnemyController : MonoBehaviour
{

    public Transform player; // Referencia al jugador
    public float speed = 5f; // Velocidad de movimiento del enemigo
    public float detectionRadius = 5.0f; // Radio de detección del jugador

    public int vida = 3; // Vida del enemigo
    private bool enemigoMuerto; // Bandera para verificar si el enemigo está muerto

    private Rigidbody2D rb; // Referencia al Rigidbody2D del enemigo
    private Vector2 movement; // Vector de movimiento del enemigo
    private bool isMoving; // Bandera para verificar si el enemigo se está moviendo
    private bool hit; // Bandera para verificar si el enemigo ha recibido daño
    private Animator animator; // Referencia al Animator del enemigo
    public float fuerzaRebote = 6f; // Fuerza de rebote al recibir daño


    private bool playerVivo; // Bandera para verificar si el jugador está vivo
    private void Start()
    {
        enemigoMuerto = false; // Indica que el enemigo esta vivo 
        playerVivo = true; // Inicializar la bandera de jugador vivo
        rb = GetComponent<Rigidbody2D>(); // Obtener el componente Rigidbody2D del enemigo
        animator = GetComponent<Animator>(); // Obtener el componente Animator del enemigo
    }

    private void Update()
    {
        if (playerVivo)
        {
            if (!enemigoMuerto) 
            {
                MovimientoEnemigo(); // Llamar al método de movimiento del enemigo
            }
            
        } 

    }

    private void FixedUpdate()
    {
       
        if (!hit)
        {
            rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
        }

        AnimacionesEnemigo(); // Llamar al método de animaciones del enemigo
    }

    // Método para detectar colisiones con el jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hit) return; // ← evita dañar al jugador mientras el enemigo retrocede

        if (collision.gameObject.CompareTag("Player")) // Si el enemigo colisiona con el jugador
        {
            Vector2 direccionDanio = new (transform.position.x, 0); // Calcular la dirección del daño
            Run playerScript = collision.gameObject.GetComponent<Run>(); // Obtener el componente PlayerController del jugador
            playerScript.Damage(direccionDanio, 1); // Aplicar daño al jugador 
            playerVivo = !playerScript.muerto; // Actualizar la bandera de jugador vivo
        }
        if (!playerVivo)
        {
            isMoving = false; // Detener el movimiento del enemigo si el jugador está muerto
            speed=0; // Establecer la velocidad a 0 para detener al enemigo
        }
    }
    
    // Método para detectar daño
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arm")) // Si el enemigo colisiona con el arma del jugador
        {
            Vector2 direccionDanio = new (collision.gameObject.transform.position.x, 0); // Calcular la dirección del daño

            Damage(direccionDanio, 1); // Aplicar daño al enemigo
        }
    }


   

    // Método para activar las animaciones del enemigo
    public void AnimacionesEnemigo()
    {
        animator.SetBool("InMovement", isMoving); // Activar la animación de movimiento
        animator.SetBool("HitEnemy", hit); // Activar la animación de recibir daño
        animator.SetBool("enemyDeath", enemigoMuerto); // Activar la animación de muerte del enemigo
    }
    public void MovimientoEnemigo()
    {
        float DistanceToPlayer = Vector2.Distance(transform.position, player.position); // Calcular la distancia al jugador
        //el enemigo se gira si va a la derecha o a la izquierda sin afectar su tamaño


        if (DistanceToPlayer < detectionRadius) // Si el jugador está dentro del radio de detección
        {
            // Calcular la dirección hacia el jugador y girar el enemigo en consecuencia
            if (player.position.x > transform.position.x) // Si el jugador está a la derecha del enemigo
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Girar el enemigo a la derecha
            }
            else // Si el jugador está a la izquierda del enemigo
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Girar el enemigo a la izquierda
            }

            Vector2 direction = (player.position - transform.position).normalized; // Calcular la dirección hacia el jugador
            movement = new Vector2(direction.x, 0); // Crear un vector de movimiento en la dirección del jugador
            isMoving = true; // Establecer la bandera de movimiento a verdadero
        }

        else
        {
            movement = Vector2.zero; // Detener el movimiento si el jugador está fuera del radio de detección
            isMoving = false; // Establecer la bandera de movimiento a falso
        }
       
    }

    // Método para aplicar daño al enemigo
    public void Damage(Vector2 direccion, int daño)
    {
        if (!hit)
        {
            hit = true;

            Vector2 rebote = new Vector2(transform.position.x - direccion.x, 0).normalized;
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            vida -= daño; // Restar vida al enemigo
            if(vida<=0) // Si la vida del enemigo es menor o igual a 0
            {
                enemigoMuerto = true; // Indicar que el enemigo está muerto
                isMoving = false; // Detener el movimiento del enemigo
                speed = 0; // Establecer la velocidad a 0 para detener al enemigo
                AnimacionesEnemigo();
            }
            StartCoroutine(Recuperarse());
        }
    }

    // Método para recuperar al enemigo después de recibir daño
    // Este método se llama después de un tiempo determinado para permitir que el enemigo se recupere
    // y vuelva a su estado normal
    // Podés ajustar el tiempo de espera según tus necesidades
    private IEnumerator Recuperarse() 
    {
        yield return new WaitForSeconds(0.3f); // Ajustá el tiempo si querés
        hit = false;
    }

    public void EnemyDestroy()
    {
        Destroy(gameObject); // Destruir el objeto enemigo al morir
    }
    //Gizmoz de detección del enemigo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Color del gizmo
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Dibujar el gizmo de detección
    }
}