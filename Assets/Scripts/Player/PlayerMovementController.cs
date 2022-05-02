using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Diferentes poderes, utilizados como argumento para PowerUp() para que otras entidades puedan potenciar a Mario
public enum Power { None, Flower, Mushroom, Star }

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    // false = No hay ordenes para Mario
    // true = Mario lee el input
    public static bool InputEnabled = true;

    // 1 = Derecha
    // -1 = Izquierda
    public static float AutoMoveDir = 1;
    public MarioState MarioState;

    private GameController GC;

    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D bigMarioBox;
    [Space]

    // Velocidad de andar predeterminada
    [SerializeField] private float walkSpeed = 5;
    // Velocidad de correr predeterminada
    [SerializeField] private float runSpeed = 5;
    // Como de alto saltara
    [SerializeField] private float jumpMultiplier = 5;
    // Velocidad del salto
    // Cambiando esto también cambia la altura del salto ya que se añadirá más o menos gravedad
    [SerializeField] private float jumpSpeed = 5;
    // El desplazamiento de donde se comprobará el terreno
    [SerializeField] private float groundOffset;
    // Hasta dónde se revisará el suelo
    [SerializeField] private float groundDistance;
    // La curva utilizada para determinar la cantidad de gravedad que se aplicará para el salto.
    [SerializeField] private AnimationCurve jumpCurve;
    // La escala de la gravedad cuando el jugador está cayendo
    [SerializeField] private float gravityScale = -.1f;
    [SerializeField] private float maxGravityDownForce = 0.5f;

    [SerializeField] private float accelrationTime = 0.25f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerRigidbody;
    private Camera mainCam;
    private float currentSpeed;
    private float currentJumpTimer = 0;
    private bool jump = false;
    private float gravity = 0;
    private float desiredXDir = 0;
    private float acceleration = 0;
    private Mariotransform MarioTransform;
    private LayerMask layerMask;
    private BoxCollider2D collider;

    public static Action<Power, MarioState> OnPowerupPickup;

    public float CurrentAcceleration { get { return desiredXDir; } }

    private bool isGrounded
    {
        get
        {
            return IsGrounded();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MarioTransform = GetComponent<Mariotransform>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        mainCam = Camera.main;
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        MarioTransform.OnTransform += (sr, an, state) =>
        {
            Debug.Log("OnTransform()");
            spriteRenderer = sr;
            animator = an;
            MarioState = state;
            if (state == MarioState.Big)
            {
                collider.size = new Vector2(1, 2);
                collider.offset = new Vector2(0, 0.5f);
                
            }
            else
            {
                collider.size = new Vector2(1, 1);
                collider.offset = new Vector2(0, 0);
                
            }
        };

        layerMask = LayerMask.NameToLayer("Untagged");
    }

    void Update()
    {

        if (InputEnabled)
            desiredXDir = Mathf.SmoothDamp(desiredXDir, Input.GetAxisRaw("Horizontal"), ref acceleration, accelrationTime);
        else
            desiredXDir = AutoMoveDir;

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        animator.SetFloat("speed", Mathf.Abs(desiredXDir) * (currentSpeed * 10));
        animator.SetBool("isJumping", !isGrounded);

        MarioTransform.CurrentSpriteRenderer.flipX = Mathf.Sign(desiredXDir) < 0 ? true : false;

        if (IsColliding())
            desiredXDir = 0;

        if (!IsGrounded())
        {
            gravity += gravityScale;
        }
        else if (!jump)
        {
            gravity = 0;
        }
        else
            gravity = 0;

        gravity = Mathf.Clamp(gravity, -maxGravityDownForce, jumpMultiplier);

        if (jump)
        {
            if (currentJumpTimer <= 1)
            {
                currentJumpTimer += Time.deltaTime * jumpSpeed;
            }
            else
            {
                currentJumpTimer = 0;
                jump = false;
            }

            gravity = jumpCurve.Evaluate(currentJumpTimer) * jumpMultiplier;
        }

        if (IsHeadJumping() && !isGrounded)
        {
            jump = false;
            gravity = -.1f;
        }

        if (InputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
               
                jump = true;
            }

            if (Input.GetKey(KeyCode.Space) && currentJumpTimer > 0)
            {
                currentJumpTimer -= Time.deltaTime * jumpSpeed * 0.5f;
            }
        }

        if (mainCam.WorldToViewportPoint(transform.position + new Vector3(-0.5f, 0, 0)).x <= 0 && desiredXDir < 0)
        {
            desiredXDir = 0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerRigidbody.MovePosition(transform.position + new Vector3(desiredXDir * currentSpeed, gravity)); 
    }

    private bool IsHeadJumping()
    {
        float headOffset = MarioState == MarioState.Big ? 1f : 0;
        RaycastHit2D headRay = Physics2D.BoxCast(transform.position + new Vector3(0, headOffset + -groundOffset, 0), new Vector2(.25f, 0.05f), 0, Vector2.up, groundDistance, layerMask);
        if (headRay.collider != null)
        {
            Debug.Log(headRay.collider.name);
            return true;
        }
        return false;
    }
    
    private bool IsColliding()
    {
        RaycastHit2D collisionCheck = Physics2D.BoxCast(transform.position + new Vector3(Mathf.Sign(desiredXDir) * 0.55f, 0, 0), new Vector2(0.05f, .9f), 0, Vector3.right * Mathf.Sign(desiredXDir), .1f, layerMask);
        if (collisionCheck.collider != null)
        {
            Debug.Log(collisionCheck.collider.name);
            return true;
        }
        return false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D groundRay = Physics2D.BoxCast(transform.position + new Vector3(0, groundOffset,0 ), new Vector2(1,0.05f),0 , Vector2.down, groundDistance, layerMask);
        Debug.DrawRay(transform.position + new Vector3(0, groundOffset, 0), Vector2.down);
        if (groundRay.collider != null)
        {
            
            return true;
        }

        return false;
    }

    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.Flower:
                
                break;
            case Power.Mushroom:
                
                OnPowerupPickup(power, MarioState.Small);
                SoundGuy.Instance.PlaySound("Resources/Sounds/smb_powerup.wav");
                break;
            case Power.Star:
                
                break;
            default:
                break;
        }
    }

    public void AddLives(int numLives)
    {
        
       
    }

    public void Die()
    {

        if (MarioState == MarioState.Big)
        {
            MarioTransform.isDamaged = true;
        }
        else
        {
            animator.SetBool("isDead", true);
            GC.MarioDie();
        }
        
    }

    [ContextMenu("Bouce")]
    public void Bounce()
    {
        jump = true;
        currentJumpTimer = 0.5f;
    }
}
