using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{

 public LineRenderer rope;

    Vector2 point_00;
    Vector2 point_10;
    Vector2 point_11;
    Vector2 point_01;


    float minRotation;
    float maxRotation;
    float rotation;


    Vector2 hookInitialPos;

    public Rigidbody2D myRB;

    bool hookReleased=false;
    bool comingBack;
    bool isRock;
    int railCount;




    List<GameObject> rails=new List<GameObject>();

    public List<SpriteRenderer> deactives;


    public Animator playerAnim;


    void Start()
    {
        hookInitialPos = transform.position-new Vector3(0,-.4f,0);
        point_01 = VPtoWP(0f, 0f) - new Vector2(-2, 2);
        point_11 = VPtoWP(1f, 0f) - new Vector2(2, 2);
        point_00 = point_01 - new Vector2(0, 7);
        point_10 = point_11 - new Vector2(0, 7);

        StartCoroutine("rotateLeft");

        rope.SetPosition(0, hookInitialPos);

    }


    void Update()
    {

        if (Vector2.Distance(transform.position, hookInitialPos) < .2f && hookReleased && comingBack && isRock)
        {
            comingBack = false;
            playerAnim.Play("idl", 0, 0);

            Invoke("makeThreadReady", .4f);
            if (FindObjectOfType<Main>().presentRailCount < 11 && FindObjectOfType<Main>().gameRunning)
            {
                goDown();
            }
            else
            {
                FindObjectOfType<Main>().gameRunning = false;
            }

            myRB.velocity = Vector2.zero;
            transform.position = hookInitialPos;

            foreach (var item in rails)
            {
                Destroy(item.gameObject);
            }

        }

        if (Vector2.Distance(transform.position, hookInitialPos) < .2f && hookReleased && comingBack && !isRock)
        {

            comingBack = false;
            playerAnim.Play("idl", 0, 0);
            FindObjectOfType<Main>().increaseRailCount(railCount);


            if (FindObjectOfType<Main>().presentRailCount < 11 && FindObjectOfType<Main>().gameRunning)
            {
                Invoke("goDown", 1f);
            }
            else if(FindObjectOfType<Main>().gameRunning)
            {
                FindObjectOfType<Main>().railRepaiIndic();
                FindObjectOfType<Main>().gameSuccess();
                FindObjectOfType<Main>().gameRunning = false;
            }

            Invoke("makeThreadReady", 1.4f);

            myRB.velocity = Vector2.zero;
            transform.position = hookInitialPos;

            foreach (var item in rails)
            {
                Destroy(item.gameObject);
            }


        }


        if (Input.GetKeyDown(KeyCode.Space) && !hookReleased && FindObjectOfType<Main>().gameRunning)
        {

            StopAllCoroutines();
            hookReleased = true;
            myRB.velocity = -5 * transform.up;
            playerAnim.Play("throw", 0, 0);
            hookInitialPos = transform.position;

        }


        rope.SetPosition(1, transform.position);
    }


  

    void makeThreadReady()
    {

        hookReleased = false;
        StartCoroutine("rotateLeft");

    }


    void goDown()
    {

        FindObjectOfType<mainCamera>().goDown();


    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "border" && !comingBack)
        {
            FindObjectOfType<mainCamera>().goUp();
            isRock = true;
            railCount = 0;
            comingBack = true;
            myRB.velocity = (hookInitialPos - (Vector2)transform.position).normalized * 6f;
            playerAnim.Play("Picking", 0, 0);

            FindObjectOfType<AudioManager>().PlayUnity("wrongobject", 1);


        }


        if (collision.gameObject.tag == "rail" && !comingBack)
        {

            playerAnim.Play("Picking", 0, 0);

            FindObjectOfType<mainCamera>().goUp();

            isRock = collision.gameObject.GetComponent<dragVelocity>().isRock;
            comingBack = true;
            collision.gameObject.transform.parent = this.transform;
            rails.Add(collision.gameObject);
            collision.gameObject.transform.localRotation = Quaternion.Euler(0,0,0);

            if (!isRock)
            {
                collision.gameObject.GetComponent<Collider2D>().enabled = false;
                collision.gameObject.transform.localPosition = new Vector2(0, -.7f);
                railCount = collision.gameObject.GetComponent<dragVelocity>().size;
                FindObjectOfType<AudioManager>().PlayUnity("metalting", 1);



            }

            else
            {
                FindObjectOfType<AudioManager>().PlayUnity("wrongobject", 1);
                collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
                collision.gameObject.transform.localPosition = new Vector2(0, -1.1f);


            }

            myRB.velocity = (hookInitialPos - (Vector2)transform.position).normalized * collision.gameObject.GetComponent<dragVelocity>().dragV;

        }



    }





    Vector2 VPtoWP(float x, float y)
    {

        return Camera.main.ViewportToWorldPoint(new Vector2(x, y));

    }




  
    IEnumerator rotateLeft()
    {
       
        var presentRot = 60f;

        while (true)
        {

            if(presentRot<-60f)
            {

                transform.localRotation = Quaternion.Euler(0, 0, -60f);

                StopCoroutine("rotateLeft");
                StartCoroutine("rotateRight");

            }

            presentRot -= Time.deltaTime*70f;

            transform.localRotation = Quaternion.Euler(0, 0, presentRot);

            yield return null;

        }

    }

    IEnumerator rotateRight()
    {


        var presentRot = -60f;

        while (true)
        {

            if (presentRot > 60f)
            {

                StopCoroutine("rotateRight");
                StartCoroutine("rotateLeft");

            }

            presentRot += Time.deltaTime*70f;

            transform.localRotation = Quaternion.Euler(0, 0, presentRot);

            yield return null;

        }

    }



  public  void deactivate()
    {


        foreach (var item in deactives)
        {
            item.enabled = false;
        }

        rope.enabled = false;

    }

  public  void activate()
    {

        foreach (var item in deactives)
        {
            item.enabled = true;
        }

        rope.enabled = true;

    }



    void throwPlayer()
    {

        playerAnim.Play("throw", 0, 0);

    }

    void pull()
    {

        playerAnim.Play("Picking", 0, 0);

    }




}
