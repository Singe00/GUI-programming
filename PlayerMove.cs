using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    BoxCollider2D collider;

    public int FirstKeyGet = 0;
    public int SecondKeyGet = 0;
    public int ThirdKeyGet = 0;
    public int FourthKeyGet = 0;
    public int FifthKeyGet = 0;

    public AudioClip audioJump;
    public AudioClip audioAttack1;
    public AudioClip audioAttack2;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    AudioSource audioSource;


    private float curTime;
    public float coolTime = 0.5f;
    private int attackMotion = 1;

    public float skillTime;
    public float skillCoolTime = 5.0f;
    private int SkillDamage = 40;

    public Transform pos;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Vector2 boxSize;
    public Vector2 boxSize2;
    public int playerDamage;
    public bool isPlayerDead = false;
    void Start()
    {

    }

    private void Awake()
    {
        this.audioSource = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (isPlayerDead) return;

        if (Input.GetButtonDown("Jump") && (!animator.GetBool("Jumping")))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            PlaySound("JUMP");
            animator.SetBool("Jumping", true);
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x*0.5f, rigid.velocity.y);
        }

        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == 1;
        }



        if (Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Walking", true);
        }



        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Collider2D[] collider2Ds;

                if (spriteRenderer.flipX == true)
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                }
                else if (spriteRenderer.flipX == false)
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos2.position, boxSize, 0);
                }
                else
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
                }
                
                foreach(Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster")
                    {
                        collider.GetComponent<MonsterMove>().TakeDamage(playerDamage);
                    }
                }

                if (attackMotion == 1)
                {
                    PlaySound("ATTACK1");
                    animator.SetTrigger("Attack1");
                    attackMotion = 2;
                }
                else if (attackMotion ==2)
                {
                    PlaySound("ATTACK2");
                    animator.SetTrigger("Attack2");
                    attackMotion = 1;
                }

                curTime = coolTime;
            }            
        }
        else
        {
            curTime -= Time.deltaTime;
        }


        if (skillTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                Collider2D[] collider2Ds;
                if (spriteRenderer.flipX == true)
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos3.position, boxSize2, 0);
                }
                else if (spriteRenderer.flipX == false)
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos4.position, boxSize2, 0);
                }
                else
                {
                    collider2Ds = Physics2D.OverlapBoxAll(pos3.position, boxSize2, 0);
                }

                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Monster")
                    {
                        collider.GetComponent<MonsterMove>().TakeDamage(SkillDamage);
                    }
                }
                PlaySound("ATTACK1");
                animator.SetTrigger("Skill");

                skillTime = skillCoolTime;
            }
        }
        else
        {
            skillTime -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (isPlayerDead) return;

        float h = Input.GetAxisRaw("Horizontal");
        
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if(rigid.velocity.x>maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed*(-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }

        

        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.0f)
                {

                    animator.SetBool("Jumping", false);
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (gameObject.layer == 3)
            {
                PowerOverwhelming();
                OnDamaged(collision.transform.position);
            }

        }
    }

    public void PowerOverwhelming()
    {
        gameManager.HealthDown();
        if (gameManager.Health > 0)
        {
            PlaySound("DAMAGED");
            gameObject.layer = 8;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);

            animator.SetTrigger("Hit");
            Invoke("OffDamaged", 2);
        }
        else if (gameManager.Health <= 0)
        {
            playerOnDie();
            isPlayerDead = true;
            Invoke("GameReset", 3);
        }
    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (gameManager.Health > 0)
        {
            int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
            rigid.AddForce(new Vector2(dirc * 7, 3), ForceMode2D.Impulse);
        }
    }

    public void OffDamaged()
    {
        gameObject.layer = 3;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
        Gizmos.DrawWireCube(pos2.position, boxSize);
        Gizmos.DrawWireCube(pos3.position, boxSize2);
        Gizmos.DrawWireCube(pos4.position, boxSize2);
    }

    public void playerOnDie()
    {
        PlaySound("DIE");
        rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
        animator.SetTrigger("Dead");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AnywayItWillBeUseful")
        {
            item item = collision.gameObject.GetComponent<item>();

            switch (item.type)
            {
                case "FirstKey":
                    FirstKeyGet = 1;
                    break;
                case "SecondKey":
                    SecondKeyGet = 1;
                    break;
                case "ThirdKey":
                    ThirdKeyGet = 1;
                    break;
                case "FourthKey":
                    FourthKeyGet = 1;
                    break;
                case "FifthKey":
                    FifthKeyGet = 1;
                    break;
            }
            PlaySound("ITEM");
            collision.gameObject.SetActive(false);
        }
    }

    void GameReset()
    {
        SceneManager.LoadScene("Main");
    }

    public void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK1":
                audioSource.clip = audioAttack1;
                break;
            case "ATTACK2":
                audioSource.clip = audioAttack2;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
        }

        audioSource.Play();
    }
}
