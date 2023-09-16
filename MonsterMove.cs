using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D collider;
    public int nextMove;
    public int Hp;
    public int Speed = 1;
    public int boss = 0;
    public int distance = 10;
    public LayerMask isLayer2;

    public GameObject Key;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        Think();

        Invoke("Think", 4);
    }

    // Update is called once per frame
    void Update()
    {
        if (nextMove == 0)
        {
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove * Speed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);

        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 5, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 5, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
        }

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer2);
        Debug.DrawRay(transform.position, Vector3.left, new Color(1, 0, 0));
        if (raycast.collider != null)
        {
            if (Vector2.Distance(transform.position, raycast.collider.transform.position) < distance)
            {
                animator.SetBool("walking", true);
                spriteRenderer.flipX = false;
                transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime * Speed);
            }

        }

        RaycastHit2D raycast2 = Physics2D.Raycast(transform.position, transform.right, distance, isLayer2);
        Debug.DrawRay(transform.position, Vector3.right, new Color(1, 0, 0));
        if (raycast2.collider != null)
        {
            if (Vector2.Distance(transform.position, raycast2.collider.transform.position) < distance)
            {
                animator.SetBool("walking", true);
                spriteRenderer.flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, raycast2.collider.transform.position, Time.deltaTime * Speed);
            }

        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 2);
    }

    public void TakeDamage(int damage)
    {

        Hp -= damage;

        if (Hp > 0)
        {
            animator.SetTrigger("hit");
            rigid.AddForce(new Vector2(nextMove, 3), ForceMode2D.Impulse);
        }
        else if (Hp <= 0)
        {

            if (boss != 4)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
                spriteRenderer.flipY = true;
                collider.enabled = false;
                rigid.AddForce(Vector2.up * 3, ForceMode2D.Impulse);

            }
            else
            {
                animator.SetTrigger("dead");
            }


            if (boss > 0)
            {
                Instantiate(Key, transform.position, Key.transform.rotation);
            }


            Invoke("DeActive", 2);
        }

    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
