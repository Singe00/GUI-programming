using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow2 : MonoBehaviour
{
    public float speed2;
    public float distance2;
    public LayerMask isLayer2;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyArrow2", 2);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right, distance2, isLayer2);
        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")
            {
                GameObject.FindWithTag("Player").SendMessage("PowerOverwhelming");
            }
            DestroyArrow2();
        }

        transform.Translate(transform.right * speed2 * Time.deltaTime);
    }
    void DestroyArrow2()
    {
        Destroy(gameObject);
    }
}
