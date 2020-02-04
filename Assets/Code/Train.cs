using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{

   public List<Rigidbody2D> rbs;
    public List<GameObject> joints;


   public void collapseTrain()
    {

        foreach (var item in rbs)
        {
            item.bodyType = RigidbodyType2D.Dynamic;
            item.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
            item.velocity = new Vector2(Random.Range(1, 20), 5);
            item.angularVelocity = Random.Range(1f, 10f);
        }


        foreach (var item in joints)
        {
            Destroy(item.gameObject);
        }


    }

   
}
