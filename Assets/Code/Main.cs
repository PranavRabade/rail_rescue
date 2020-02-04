using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{


    public GameObject bridgeLeft;
    public GameObject bridgeRight;
    public GameObject ground;
    public GameObject ropeThreading;
    public GameObject hook;
    public GameObject train;

    float bridgeCompSize = 1.766f;

    public GameObject left;
    public GameObject right;

    // Prefabs

    public List<GameObject> levels;

    public GameObject railPart;
    public GameObject railPartHalf;
    public GameObject railPartHalf2;
    public GameObject mountain;
    public GameObject railRepairPrefab;
    public GameObject railRepairedIndic;


    public GameObject point_00;
    public GameObject point_10;
    public GameObject point_11;
    public GameObject point_01;

    public GameObject pointlevel;

    public GameObject levelCompletePrefab;
    public GameObject earthQuakeIndicPrefab;


    private List<GameObject> rails=new List<GameObject>();
    private List<GameObject> railsDummy = new List<GameObject>();

   public int presentRailCount=0;


    public bool gameRunning;


    List<int> levelTimers = new List<int>();

    Vector2 railPos;


    Vector2 trainEnd;
    Vector2 trainStart;
    Vector2 trainSuccessPoint;


    public Animator cameraShake;


    public SpriteRenderer player;


    public GameObject gameMenue;
    public GameObject timerMenue;
    public GameObject gameOverMenue;

    
    int timeLeft = 35;

    public Text timeLeftText;


    private void Awake()
    {
        
        setUpCamera(28,0);
        float width = -1f;
        bridgeLeft.transform.position = VPtoWP(.5f, 0f) - new Vector2(7 + width, -1);
        bridgeRight.transform.position = VPtoWP(.5f, 0f) + new Vector2(7 + width, 1);
        ground.transform.position = VPtoWP(.5f, 0f) + new Vector2(0, 1);
        mountain.transform.position = VPtoWP(.5f, 0f);
        hook.transform.position = VPtoWP(.5f, 0f) + new Vector2(0, 1f);
        ropeThreading.transform.position = VPtoWP(.5f, 0f) + new Vector2(0, 1.9f);
        point_01.transform.position = VPtoWP(0f, 0f) - new Vector2(-2, 2);
        point_11.transform.position = VPtoWP(1f, 0f) - new Vector2(2, 2);
        point_00.transform.position = point_01.transform.position - new Vector3(0, 7, 0);
        point_10.transform.position = point_11.transform.position - new Vector3(0, 7, 0);
        pointlevel.transform.position = new Vector2((point_00.transform.position.x+point_10.transform.position.x)/2f, (point_00.transform.position.y + point_01.transform.position.y) / 2f);
    }

    void Start()
    {


        for (int i = 0; i < 11; i++)
        {
          var r =  Instantiate(railPartHalf, left.transform.parent.transform.position+new Vector3(.4f,0f,0f) + new Vector3((i+1)*(bridgeCompSize/2f), 0, 0), Quaternion.identity) as GameObject;
            rails.Add(r); 
        }

        levelTimers.Add(40);
     
        buildBorderCollider();
        createLevel(0);
        deactivateAllRails();


        trainEnd = bridgeLeft.transform.position + new Vector3(0, 9.1f, 0);
        trainStart = trainEnd - new Vector2(14,0);
        trainSuccessPoint = trainEnd + new Vector2(34,0);

        train.transform.position = trainStart;
        
        FindObjectOfType<Hook>().deactivate();
    }

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.S))
        {
            startGame();
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            stopTimer();
        }
    }


    Vector2 VPtoWP(float x,float y)
    {

        return Camera.main.ViewportToWorldPoint(new Vector2(x,y));

    }


    void setUpCamera(float width, float height)     {         float aspectRatio = (float)Screen.width / (float)Screen.height;         float verticalSize = (float)height / 2f;         float horizontalSize = ((float)width / 2f) / aspectRatio;         Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;      }        void buildBorderCollider()
    {

        var edgeCollider = new GameObject("Edge Collider");
        var edge = edgeCollider.AddComponent<EdgeCollider2D>();
        Vector2[] points = { point_01.transform.position,point_00.transform.position,point_10.transform.position,point_11.transform.position };
        edge.points = points;

        edgeCollider.tag = "border";

    }        
  
    void createLevel(int i)
    { 

        Instantiate(levels[i], pointlevel.transform.position, Quaternion.identity);

    }


    void deactivateAllRails()
    {

        foreach (var item in rails)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

    }


    public void stopTimer()
    {
        FindObjectOfType<mainCamera>().goUp();
        gameRunning = false;
        gameOver();

    }


    public void increaseRailCount(int c)
    {
        if (gameRunning)
        {
            for (int i = presentRailCount; i < presentRailCount + c; i++)
            {
                rails[i].GetComponent<SpriteRenderer>().enabled = true;
                if (i == presentRailCount)
                {
                    railPos = rails[i].transform.position;
                }
            }


            if (c == 1)
            {

                repair1();

            }

            if (c == 2)
            {

                repair1();
                Invoke("repair2", .14f);

            }

            if (c == 3)
            {

                repair1();
                Invoke("repair2", .14f);
                Invoke("repair3", .28f);

            }

            presentRailCount += c;
        }

    }


    public void railRepaiIndic()
    {

        Instantiate(railRepairedIndic, VPtoWP(.5f, .5f) + new Vector2(0, 2f), Quaternion.identity);

    }

    public void levelCompleteIndic()
    {

        Instantiate(levelCompletePrefab, VPtoWP(.5f, .5f) + new Vector2(0, 5f), Quaternion.identity);

    }

    void repair1()
    {
        Instantiate(railRepairPrefab, railPos + new Vector2(0, .5f),Quaternion.identity);
    }

    void repair2()
    {

        Instantiate(railRepairPrefab, railPos + new Vector2(bridgeCompSize/2f, .5f), Quaternion.identity);

    }

    void repair3()
    {
        Instantiate(railRepairPrefab, railPos + new Vector2(bridgeCompSize, .5f), Quaternion.identity);

    }


    void gameOver()
    {

        StartCoroutine("moveTrain");
        Invoke("collapseTrain", 1.5f);

    }


   public void gameSuccess()
    {

        CancelInvoke("tik");
        gameRunning = false;
        StartCoroutine("moveTrainSuccess");
        Invoke("levelCompleteIndic", 1f);
        Invoke("activateGameOver", 7f);

        timerMenue.SetActive(false);

    }



    void collapseTrain()
    {

        StopCoroutine("moveTrain");
        FindObjectOfType<Train>().collapseTrain();

        foreach (var item in rails)
        {
            item.AddComponent<Rigidbody2D>();
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Random.Range(2f, 7f));
            item.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(2f, 7f);
        }
    }


    IEnumerator moveTrain()
    {


        var t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            var x = Mathf.Clamp(t/1.5f, 0f, 1f);
            train.transform.position = Vector2.Lerp(trainStart, trainEnd, x);

            yield return null;

        }


    }

    IEnumerator moveTrainSuccess()
    {


        var t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            var x = Mathf.Clamp(t / 10f, 0f, 1f);
            train.transform.position = Vector2.Lerp(trainStart, trainSuccessPoint, x);

            yield return null;

        }


    }



    public void doEarthQuake()
    {

        for (int i = 0; i < 11; i++)
        {
            var r = Instantiate(railPartHalf2, left.transform.parent.transform.position + new Vector3(.4f, 0f, 0f) + new Vector3((i + 1) * (bridgeCompSize / 2f), 0, 0), Quaternion.identity) as GameObject;
            railsDummy.Add(r);
        }

        Invoke("breakBridge", 2f);

    }


    void breakBridge()
    {

        foreach (var item in railsDummy)
        {
            item.GetComponent<Rigidbody2D>().gravityScale = 1f;
            item.GetComponent<Rigidbody2D>().velocity = new Vector2(0,Random.Range(2f,7f));
            item.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(2f, 7f);

        }

    }



    void startGame()
    {

        player.enabled = true;
        FindObjectOfType<Hook>().activate();
        FindObjectOfType<mainCamera>().goDown();
        gameRunning = true;
        timerMenue.SetActive(true);
        StartTimer();


    }

    public void playGame()
    {

        Invoke("startGame", 4f);
        cameraShake.Play("camShake", 0, 0);
        doEarthQuake();
        gameMenue.SetActive(false);
        Invoke("play", .5f);
        

    }


    void play()
    {

        Instantiate(earthQuakeIndicPrefab, VPtoWP(.5f,.5f) + new Vector2(0, 4), Quaternion.identity);
        

    }



    void StartTimer()
    {

        timeLeftText.text = timeLeft.ToString();
        Invoke("tik", 1f);

    }


    void tik()
    {
        if (timeLeft > 0 && gameRunning)
        {
            Invoke("tik", 1f);
            timeLeft -= 1;
            timeLeftText.text = timeLeft.ToString();

        }
        else
        {

            stopTimer();
            Invoke("activateGameOver", 2f);

        }
    }


    

    public void reloadScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }



    void activateGameOver()
    {

        timerMenue.SetActive(false);
        gameOverMenue.SetActive(true);

    }
}
