using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repaired : MonoBehaviour
{


    private void Start()
    {
        StartCoroutine("oscillate");
    }


    IEnumerator oscillate()
    {

        var amplitude = .4f;
        var time = 0f;
        var y = 0f;

        while (true)
        {
            time += Time.deltaTime;
            y = 2f;
             y += amplitude * Mathf.Exp(-4f * time) * Mathf.Sin(time*10);
            transform.localScale = new Vector3(y,y,1);
            yield return null;
        }

    }
    
}
