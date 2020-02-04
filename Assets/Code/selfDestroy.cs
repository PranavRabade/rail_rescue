using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestroy : MonoBehaviour
{

    public float lifeTime;


    private void Awake()
    {

        Invoke("destroyThis", lifeTime);

    }



    void destroyThis()
    {

        Destroy(gameObject);

    }

}
