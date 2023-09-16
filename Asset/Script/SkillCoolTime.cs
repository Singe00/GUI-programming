using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolTime : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    public float skillTime = 0;
    public float skillCoolTime = 5.0f;
    void Start()
    {
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillTime <= 0)
        {
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetTrigger("Skill");
                skillTime = skillCoolTime;
            }
        }
        else
        {
            skillTime -= Time.deltaTime;
        }

    }
}
