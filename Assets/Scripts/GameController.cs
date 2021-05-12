using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine. EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform CubeToPlace;
    private float camMoveToYPosition, camMoveSpeed = 2f;
    public GameObject CubeToCreate, AllCubes, vfx;
    public GameObject[] canvasStartPage;
    private Rigidbody AllCubesRb;
    private bool IsLose, firstCube;
    private Coroutine shpowCubePlace;
    public Color[] bgColors;
    private Color toCameraColor;
    public Text scoreTxt;
    private List<Vector3> AllCubePositions = new List<Vector3>
    {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        new Vector3(1,0,-1),
    };

private int prevCountMaxHorizontal;
private Transform mainCam;
    private void Start()
    {
        scoreTxt.text = "<color=#F80127><size=40>Best:</size></color> " + PlayerPrefs.GetInt("score") + "\n<color=#1301F8><size=40>Now:</size></color> 0";
        toCameraColor = Camera.main.backgroundColor;
        mainCam = Camera.main.transform;
        camMoveToYPosition = 5.9f + nowCube.y - 1f;

        AllCubesRb = AllCubes.GetComponent<Rigidbody>();
        shpowCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeToPlace != null && AllCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif

            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                {
                    Destroy(obj);
                }
            }

            GameObject newCube = Instantiate(CubeToCreate, CubeToPlace.position,Quaternion.identity) as GameObject;

            newCube.transform.SetParent(AllCubes.transform);
            nowCube.setVector(CubeToPlace.position);
            AllCubePositions.Add(nowCube.GetVector());

            GameObject newVfx = Instantiate(vfx, newCube.transform.position, Quaternion.identity) as GameObject;

            Destroy(newVfx, 1.5f);

            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            AllCubesRb.isKinematic = true;
            AllCubesRb.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBg();
        }

        if (!IsLose && AllCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(CubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(shpowCubePlace);
        }

        mainCam.localPosition = Vector3.MoveTowards(mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, camMoveToYPosition, mainCam.localPosition.z) ,
            camMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
    }

    IEnumerator ShowCubePlace()
    {
        while(true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if(IsPositionEmpty(new Vector3(nowCube.x+1, nowCube.y, nowCube.z))
            && nowCube.x+1 != CubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
         if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z))
            && nowCube.x - 1 != CubeToPlace.position.x)
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));

         if (IsPositionEmpty(new Vector3(nowCube.x , nowCube.y+1 , nowCube.z))
            && nowCube.y + 1 != CubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x , nowCube.y+1 , nowCube.z));
         if (IsPositionEmpty(new Vector3(nowCube.x , nowCube.y - 1, nowCube.z))
            && nowCube.y - 1 != CubeToPlace.position.y)
            positions.Add(new Vector3(nowCube.x , nowCube.y - 1, nowCube.z));

         if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y , nowCube.z + 1))
            && nowCube.z + 1 != CubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y , nowCube.z + 1));
         if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y , nowCube.z - 1))
            && nowCube.z - 1 != CubeToPlace.position.z)
            positions.Add(new Vector3(nowCube.x, nowCube.y , nowCube.z - 1));

        if (positions.Count > 1)
            CubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
            IsLose = true;
        else
            CubeToPlace.position = positions[0];
    }
    private bool IsPositionEmpty(Vector3 targetPos)
    {
        if (targetPos.y == 0)
            return false;

        foreach(Vector3 pos in AllCubePositions)
        {
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
                return false;
        }
        return true;
    }

     private void MoveCameraChangeBg()
    {
        int maxX = 0, maxY = 0, maxZ = 0, maxHor;

        foreach (Vector3 pos in AllCubePositions) {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX)
                maxX = Convert.ToInt32(pos.x);
            if (Convert.ToInt32(pos.y) > maxY)
                maxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ)
                maxZ = Convert.ToInt32(pos.z);
        }

        if (PlayerPrefs.GetInt("score") < maxY)
        {
            PlayerPrefs.SetInt("score", maxY);
        }

        scoreTxt.text = "<color=#F80127><size=40>Best:</size></color> " + PlayerPrefs.GetInt("score") + "\n<color=#1301F8><size=40>Now:</size></color> " + maxY;

        camMoveToYPosition = 5.9f + nowCube.y - 1f;

        maxHor = maxX > maxZ ? maxX : maxZ;
        if (maxHor % 3 == 0 && prevCountMaxHorizontal != maxHor) 
        {   mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            prevCountMaxHorizontal = maxHor;
        }

        if (maxY >= 20)
            toCameraColor = bgColors[3];
        else if (maxY >= 15)
            toCameraColor = bgColors[2];
        else if (maxY >= 10)
            toCameraColor = bgColors[1];
        else if (maxY >= 5)
            toCameraColor = bgColors[0];
    }
}

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
        
    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }

    public void setVector(Vector3 pos)
    {
        x = Convert.ToInt32 (pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}
