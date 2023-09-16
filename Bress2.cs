using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bress2 : MonoBehaviour
{
    public float speed2;
    public float distance2;
    public LayerMask isLayer2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBress2", 2);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right, distance2, isLayer2);
        Debug.DrawRay(transform.position, Vector3.right, new Color(0, 0, 1));
        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")
            {
                GameObject.FindWithTag("Player").SendMessage("PowerOverwhelming");
            }
            Invoke("DestroyBress2", 5);
        }

        transform.Translate(transform.right * speed2 * Time.deltaTime);
    }
    void DestroyBress2()
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
