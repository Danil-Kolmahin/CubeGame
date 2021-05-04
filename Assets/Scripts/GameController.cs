using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform CubeToPlace;
    public GameObject CubeToCreate, AllCubes;
    private Rigidbody AllCubesRb;
    private bool IsLose;
    private Coroutine shpowCubePlace;
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

    private void Start()
    {
        AllCubesRb = AllCubes.GetComponent<Rigidbody>();
        shpowCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeToPlace != null)
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif

            GameObject newCube = Instantiate(CubeToCreate, CubeToPlace.position,Quaternion.identity) as GameObject;

            newCube.transform.SetParent(AllCubes.transform);
            nowCube.setVector(CubeToPlace.position);
            AllCubePositions.Add(nowCube.GetVector());

            AllCubesRb.isKinematic = true;
            AllCubesRb.isKinematic = false;

            SpawnPositions();
        }

        if (!IsLose && AllCubesRb.velocity.magnitude > 0.1f)
        {
            Destroy(CubeToPlace.gameObject);
            IsLose = true;
            StopCoroutine(shpowCubePlace);
        }
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

        CubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
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
