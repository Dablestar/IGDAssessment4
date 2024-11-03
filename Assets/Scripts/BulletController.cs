using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Tweener BulletTweener;
    // Start is called before the first frame update
    void Start()
    {
        BulletTweener = gameObject.GetComponent<Tweener>();
        BulletTweener.AddTween(transform, transform.position, transform.position + new Vector3(100f, 0f, 0f), 1f);
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            Destroy(other);
            AltPacStudentController.AddScore(300);
        }
    }
}
