using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
public enum PlayerState
{
    Idle,       // 0
    Walking,    // 1
    Flying,     // 2
    Stunned     // 3
}

public class PlayerController : MonoBehaviour
{
    public PlayerState curState;            // current player state

    // values
    public float moveSpeed;                 // force applied horizontally when moving
    public float flyingSpeed;               // force applied upwards when flying
    public bool grounded;                   // is the player currently standing on the ground?
    public float stunDuration;              // duration of a stun
    private float stunStartTime;            // time that the player was stunned
    public float invincibleDuration = 0.75f;   // time after stun ends
    public float invincibleAlpha   = 0.5f;   // desired opacity  (0-1)
    private bool  isInvincible;                // flag
    private float invincibleStartTime;         // when invincibility began
    public float windPush;                  // wind push force
    Color  normalColor;     // store original sprite colour
    SpriteRenderer spriteRenderer;   // already declared in Awake

    // components
    public Rigidbody2D rig;                 // Rigidbody2D component
    public Animator anim;                   // Animator component
    public ParticleSystem jetpackParticle;  // ParticleSystem of jetpack

    public float minX = -20f, maxX = 20f, minY = -10f, maxY = 10f;  // Biên giới khu vực chơi
    public int currentHealth = 3;  // Số mạng của Player
    public int score = 0;  // Điểm số của Player
    AudioManager audioManager;

    // I frame
    

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor    = spriteRenderer.color;
        //audioManager = AudioManager.instance;
        //rig = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        //jetpackParticle = GetComponent<ParticleSystem>();

    }
    void FixedUpdate ()
    {
        grounded = IsGrounded();
        CheckInputs();
        ClampPosition();
        // is the player stunned?
        if (curState == PlayerState.Stunned)
        {
            if (Time.time - stunStartTime >= stunDuration)
            {
                curState = PlayerState.Idle;

                // begin i-frames
                isInvincible        = true;
                invincibleStartTime = Time.time;
                SetInvincibleVisual(true);
            }
        }
        else if (isInvincible && Time.time - invincibleStartTime >= invincibleDuration)
        {
            // i-frames finished
            isInvincible = false;
            SetInvincibleVisual(false);
        }
    }

    // Giới hạn vị trí của Player trong khu vực chơi
    void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, -8.9f, 8.9f);  // Giới hạn X
        //float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);  // Giới hạn Y
        //Debug.Log(clampedX + " " + clampedY);
        // Áp dụng vị trí đã giới hạn
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    // checks for user input to control player
    void CheckInputs ()
    {
        if (curState != PlayerState.Stunned)
        {
            // movement
            Move();

            // flying
            if (Input.GetKey(KeyCode.UpArrow))
                Fly();
            else
                jetpackParticle.Stop();
        }

        // update our current state
        SetState();
    }

    // sets the player's state
    void SetState ()
    {
        // don't worry about changing states if the player's stunned
        if (curState != PlayerState.Stunned)
        {
            // idle
            if (rig.linearVelocity.magnitude == 0 && grounded)
                curState = PlayerState.Idle;
            // walking
            if (rig.linearVelocity.x != 0 && grounded)
                curState = PlayerState.Walking;
            // flying
            if (rig.linearVelocity.magnitude != 0 && !grounded)
                curState = PlayerState.Flying;
        }

        // tell the animator we've changed states
        anim.SetInteger("State", (int)curState);
    }

    // moves the player horizontally
    void Move ()
    {
        float dir = Input.GetAxis("Horizontal");

        // flip sprite
        if (dir > 0)        transform.localScale = Vector3.one;
        else if (dir < 0)   transform.localScale = new Vector3(-1, 1, 1);

        // final X velocity = player input + wind
        float vx = dir * moveSpeed + windPush;
        rig.linearVelocity = new Vector2(vx, rig.linearVelocity.y);
    }

    // adds force upwards to player
    void Fly ()
    {
        // add force upwards
        rig.AddForce(Vector2.up * flyingSpeed, ForceMode2D.Impulse);

        // play jetpack particle effect
        if (!jetpackParticle.isPlaying)
        {
            jetpackParticle.Play();
            //AudioManager.instance.PlaySFX(audioManager.jetpack);
        }
            
    }

    // called when the player gets stunned
    public void Stun ()
    {
        curState = PlayerState.Stunned;
        rig.linearVelocity = Vector2.down * 3;
        stunStartTime = Time.time;
        jetpackParticle.Stop();
        // hit a obstacle, lose 5 points
        GameManager.instance.minusScore(5);
    }

    // returns true if player is on ground, false otherwise
    bool IsGrounded ()
    {
        // shoot a raycast down underneath the player
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.85f), Vector2.down, 0.3f);

        // did we hit anything?
        if(hit.collider != null)
        {
            // was it the floor?
            if(hit.collider.CompareTag("Floor"))
            {
                return true;
            }
        }

        return false;
    }

    // called when the player enters another object's collider
    void OnTriggerEnter2D (Collider2D col)
    {
        if (curState == PlayerState.Stunned || isInvincible) return;

        if (col.GetComponent<Obstacle>() || col.CompareTag("Laser"))
        {
            var shield = GetComponent<PlayerShield>();
            if (shield != null && shield.ConsumeHit())
            {
                // absorbed! maybe play a fx / sfx here
                AudioManager.instance?.PlaySFX(AudioManager.instance.shieldHit);
                return;
            }

            // no shield → regular damage
            audioManager.PlaySFX(audioManager.hit);
            GetComponent<PlayerStatus>().TakeDamage(1);
            Stun();
        }
    }


    void SetInvincibleVisual(bool on)
    {
        Color c = spriteRenderer.color;
        c.a     = on ? invincibleAlpha : normalColor.a;   // adjust alpha
        spriteRenderer.color = c;
    }
}

