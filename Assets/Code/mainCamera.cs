using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{
    Vector3 initialPos = new Vector3(0,0,-10);
    Vector3 digPos = new Vector3(0,-11.88f,-10);

    Vector3 targetPoint;


    bool up;
   

    void Start()
    {

        targetPoint = initialPos;
        
    }


    


    public void goDown()
    {
        up = false;
        targetPoint = digPos;

    }


    public void goUp()

    {
        up = true;
        targetPoint = initialPos-new Vector3(0,2,0);

    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))

        {

            goUp();

        }


        if(Input.GetKeyDown(KeyCode.D))
        {

            goDown();

        }
    }

    void LateUpdate()
    {
        if (up)
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime*.8f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime*2);

        }
    }
}
