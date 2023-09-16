using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMonster : MonoBehaviour
{
    public float distance;
    public float atkDistance;
    public LayerMask isLayer;
    public float speed;

    public GameObject arrow;
    public GameObject arrow2;
    public Transform pos;
    public Transform pos2;
    public Rigidbody2D rigid;
    public float cooltime;
    private float currenttime;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        Debug.DrawRay(transform.position, Vector3.left, new Color(1, 0, 0));
        if (raycast.collider != null)
        {
            if (Vector2.Distance(transform.position, raycast.collider.transform.position) < atkDistance)
            {
                if (currenttime <= 0)
                {
                    animator.SetTrigger("atk");
                    GameObject arrowCopy = Instantiate(arrow, pos.position, transform.rotation);
                    currenttime = cooltime;
                }
                
            }
            else
            {
                animator.SetBool("walking", true);
                spriteRenderer.flipX = false;
                transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime * speed);

            }
            currenttime -= Time.deltaTime;
        }

        RaycastHit2D raycast2 = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);
        Debug.DrawRay(transform.position, Vector3.right, new Color(1, 0, 0));
        if (raycast2.collider != null)
        {
            if (Vector2.Distance(transform.position, raycast2.collider.transform.position) < atkDistance)
            {
                if (currenttime <= 0)
                {
                    animator.SetTrigger("atk");
                    GameObject arrowCopy = Instantiate(arrow2, pos2.position, transform.rotation);
                    currenttime = cooltime;
                }

            }
            else
            {
                animator.SetBool("walking", true);
                spriteRenderer.flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, raycast2.collider.transform.position, Time.deltaTime * speed);

            }
            currenttime -= Time.deltaTime;
        }
    }

}
