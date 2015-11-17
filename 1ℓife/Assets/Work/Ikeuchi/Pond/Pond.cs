using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Pond : MonoBehaviour {

    [SerializeField]
    GameObject _createPondStopWater = null;
    [SerializeField]
    GameObject _createPondMoveWater = null;

    [SerializeField]
    int _createPondWaterWidthNum = 5;

    List<GameObject> _pondWaterList = new List<GameObject>();

    Mesh _mesh;

    // Use this for initialization
    void Start ()
    {
        var objs = GameObject.FindGameObjectsWithTag("Pond");

        CreatePondWater(objs);
        SetupMesh();
    }

    void CreatePondWater(GameObject[] objs)
    {
        Vector3 lenght1 = objs[1].transform.position - objs[0].transform.position;
        Vector3 lenght2 = objs[2].transform.position - objs[3].transform.position;
        _pondWaterList.Add(objs[0]);
        _pondWaterList.Add(objs[3]);
        for (int i = 0; i < _createPondWaterWidthNum; ++i)
        {
            var obj1 = GameObject.Instantiate(_createPondStopWater);
            Vector3 interval1 = lenght1 / _createPondWaterWidthNum;
            obj1.transform.position = objs[0].transform.position + interval1 * i + interval1 / 2.0f;
            obj1.transform.parent = transform;
            obj1.transform.localScale = new Vector3(interval1.x, interval1.x, interval1.x);
            obj1.name = _createPondStopWater.name + i;

            var obj2 = GameObject.Instantiate(_createPondMoveWater);
            Vector3 interval2 = lenght2 / _createPondWaterWidthNum;
            obj2.transform.position = objs[3].transform.position + interval2 * i + interval2 / 2.0f;
            obj2.transform.parent = transform;
            obj2.transform.localScale = new Vector3(interval2.x, interval2.x, interval2.x);
            obj2.name = _createPondMoveWater.name + i;
            obj2.GetComponent<PondWater>()._TargetWaterPos = obj2.transform.position;

            _pondWaterList.Add(obj1);
            _pondWaterList.Add(obj2);
        }
        _pondWaterList.Add(objs[1]);
        _pondWaterList.Add(objs[2]);

        //foreach(var obj in _pondWaterList)
        //{
        //    Debug.Log(obj.name);
        //}
    }

    void SetupMesh()
    {
        _mesh = new Mesh();

        IsRenderMeshUpdate();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }

    // Update is called once per frame
    void Update () {
        IsRenderMeshUpdate();
    }

    void IsRenderMeshUpdate()
    {
        // 配列数が２以下だと△ポリゴンが作れないので抜ける(３つ頂点が必要)
        int triangleNum = (_pondWaterList.Count - 2) * 3;
        if (triangleNum <= 2)
        {
            // 前に描画した水が残るので _mesh を空にする
            Mesh resetMesh = new Mesh();
            _mesh = resetMesh;

            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            GetComponent<MeshFilter>().sharedMesh = _mesh;
            GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
            return;
        }

        Vector3[] vertices = new Vector3[_pondWaterList.Count];
        Vector2[] uv = new Vector2[_pondWaterList.Count];      
        int[] triangles = new int[triangleNum];

        // 頂点座標指定
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 offset = new Vector3(0.0f, _pondWaterList[2].transform.localScale.y / 2, 0.0f);
            if (i % 2 == 0)
            {              
                vertices[i] = _pondWaterList[i].transform.position;
            }
            else
            {
                vertices[i] = _pondWaterList[i].transform.position + offset;
            }
            
        }

        // UVの指定
        Vector2[] uvs =
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
        };
        for (int i = 0; i < uv.Length; ++i)
        {
            uv[i] = uvs[i % 4];
        }

        // 三角形の頂点インデックスの指定
        int count = 0;  // 0,1,2 → 1,2,3 → 2,3,4　とつなげるためのカウント
        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (count % 2 == 0)
            {
                triangles[i] = count;
                triangles[i + 1] = count + 1;
                triangles[i + 2] = count + 2;
            }
            else
            {
                triangles[i + 2] = count;
                triangles[i + 1] = count + 1;
                triangles[i] = count + 2;
            }

            count++;
        }

        // いきなり _mesh にデータを入れるとエラーがでる
        // _mesh.vertices = vertices;　をすると
        // uv と vertices の要素数が一致しないといわれるため
        // 新しい Mesh を作り _mesh に入れる

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        _mesh = mesh;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
    }
}
