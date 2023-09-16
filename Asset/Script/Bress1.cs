using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bress1 : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBress1", 2);
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        Debug.DrawRay(transform.position, Vector3.left, new Color(0, 0, 1));
        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")
            {
                GameObject.FindWithTag("Player").SendMessage("PowerOverwhelming");
            }
            Invoke("DestroyBress1", 5);
        }

        transform.Translate(transform.right * -1f * speed * Time.deltaTime);
    }
    void DestroyBress1()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("Player").SendMessage("PowerOverwhelming");
        }
    }
}
