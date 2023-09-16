using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float distance;
    public LayerMask isLayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyArrow", 2);
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * -1, distance, isLayer);
        if (raycast.collider != null)
        {
            if (raycast.collider.tag == "Player")
            {
                GameObject.FindWithTag("Player").SendMessage("PowerOverwhelming");
            }
            DestroyArrow();
        }

        transform.Translate(transform.right * -1f * speed * Time.deltaTime);
    }
    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
