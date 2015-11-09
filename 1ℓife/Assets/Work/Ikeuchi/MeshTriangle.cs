using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MeshTriangle : MonoBehaviour {

    [SerializeField]
    GameObject _watersObjManager;

    [SerializeField]
    GameObject _waterObj;

    Mesh _mesh;

    // Use this for initialization
    void Start () {
        _mesh = new Mesh();

        //// 頂点座標指定
        //_mesh.vertices = new Vector3[]
        //{
        //    new Vector3(-2.0f, 0.0f,  2.0f),
        //    new Vector3(-2.0f, 0.0f, -2.0f),
        //    new Vector3( 2.0f, 0.0f, -2.0f),
        //    new Vector3( 2.0f, 0.0f,  2.0f),
        //};

        //// UVの指定
        //_mesh.uv = new Vector2[]
        //{
        //    new Vector2(0.0f, 0.0f),
        //    new Vector2(0.0f, 1.0f),
        //    new Vector2(1.0f, 1.0f),
        //    new Vector2(1.0f, 0.0f),
        //};

        //// 三角形の頂点インデックスの指定
        //_mesh.triangles = new int[]
        //{
        //    2, 1, 0,
        //    0, 3, 2,
        //};

        MeshUpdate();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
	}

    // Update is called once per frame
    void Update()
    {
        //MeshVerticesUpdate();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //RayMeshUpdate();
        //    IsRenderMeshUpdate();
        //}

        IsRenderMeshUpdate();
    }

    void MeshVerticesUpdate()
    {
        var waters = GameObject.Find(_watersObjManager.name).GetComponentsInChildren<WaterMove>();
        //int multiplication = waters.Length * waters.Length;
        //int createNum = multiplication * multiplication;
        if (waters != null)
        {
            Vector3[] vertices = new Vector3[waters.Length];

            // 頂点座標指定
            for (int i = 0; i < waters.Length; i += 4)
            {
                vertices[i] += waters[i]._GET_POSITION;
                vertices[i + 1] += waters[i + 1]._GET_POSITION;
                vertices[i + 2] += waters[i + 2]._GET_POSITION;
                vertices[i + 3] += waters[i + 3]._GET_POSITION;
            }
            _mesh.vertices = vertices;
        }
    }

    void MeshUpdate()
    {
        var waters = GameObject.Find(_watersObjManager.name).GetComponentsInChildren<WaterMove>();
        //int multiplication = waters.Length * waters.Length;
        //int createNum = multiplication * multiplication;
        if (waters != null)
        {
            Vector3[] vertices = new Vector3[waters.Length];
            Vector2[] uv = new Vector2[waters.Length];
            int triangleNum = waters.Length / 4;
            int[] triangles = new int[triangleNum * 6];

            // 頂点座標指定
            for (int i = 0; i < waters.Length; i += 4)
            {
                vertices[i] += waters[i]._GET_POSITION;
                vertices[i + 1] += waters[i + 1]._GET_POSITION;
                vertices[i + 2] += waters[i + 2]._GET_POSITION;
                vertices[i + 3] += waters[i + 3]._GET_POSITION;
            }

            // UVの指定
            for (int i = 0; i < waters.Length; i += 4)
            {
                uv[i] = new Vector2(0.0f, 0.0f);
                uv[i + 1] = new Vector2(0.0f, 1.0f);
                uv[i + 2] = new Vector2(1.0f, 1.0f);
                uv[i + 3] = new Vector2(1.0f, 0.0f);
            }

            // 三角形の頂点インデックスの指定
            for (int i = 0; i < triangleNum; i += 6)
            {
                triangles[i] = i + 2;
                triangles[i + 1] = i + 1;
                triangles[i + 2] = i;
                triangles[i + 3] = i;
                triangles[i + 4] = i + 3;
                triangles[i + 5] = i + 2;
            }
            _mesh.vertices = vertices;
            _mesh.uv = uv;
            _mesh.triangles = triangles;
        }
    }

    void RayMeshUpdate()
    {
        List<GameObject> objList = new List<GameObject>(); 
        //ArrayList objList = new ArrayList();
        for (int x = 0; x < Screen.width; ++x)
        {
            for (int y = 0; y < Screen.height; ++y)
            {
                // screen の全てに Ray を飛ばして当たったオブジェクトを取り出し
                // List に格納していく
                // 同じ gameObject に当たっていたら格納しないようにする
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // まず当たったものが水かどうか確かめる
                    var obj = hit.transform.gameObject;
                    if(obj.name != _waterObj.name) { continue; }

                    // 同じものが格納されていないか確かめる
                    bool addBool = true;
                    for(int i = 0; i < objList.Count; ++i)
                    {
                        // 同じものがあったら飛ばして格納しないようにする
                        if(obj.transform.position != objList[i].transform.position)
                        {
                            addBool = false;
                            break;
                        }
                    }

                    // 同じものがなければ格納する
                    if (addBool)
                    {
                        objList.Add(obj);
                    }
                }
            }
        }

        Debug.Log(objList.Count);
    }

    void IsRenderMeshUpdate()
    {
        // プレイヤーの水を配列で確保
        var waters = GameObject.Find(_watersObjManager.name).GetComponentsInChildren<WaterMove>();

        // 描画に必要な水だけをいれる List
        List<WaterMove> objList = new List<WaterMove>();

        for(int i = 0; i < waters.Length; ++i)
        {
            // 描画すべき水だったら List に追加
            if (waters[i]._IS_RENDERED)
            {
                objList.Add(waters[i]);
            }
        }

        // 配列数が２以下だと△ポリゴンが作れないので抜ける
        int triangleNum = (objList.Count - 2) * 3;
        if (triangleNum <= 2) { return; }

        //Debug.Log(objList.Count);
        //for(int i = 0; i < objList.Count; i++)
        //{
        //    Debug.Log(objList[i]._GET_POSITION.x);
        //}

        Vector3[] vertices = new Vector3[objList.Count];
        Vector2[] uv = new Vector2[objList.Count];
        int[] triangles = new int[triangleNum * 3];

        objList.Sort(delegate (WaterMove a, WaterMove b)
        {
            if (a._GET_POSITION.x > b._GET_POSITION.x)
            {
                return 1;
            }

            if (a._GET_POSITION.x < b._GET_POSITION.x)
            {
                return -1;
            }

            return 0;
        }
        );

        // 頂点座標指定
        for (int i = 0; i < vertices.Length; ++i)
        {
            vertices[i] = objList[i]._GET_POSITION;
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

        // (なんて説明すればいいのかわからない[後で書く])
        objList.Sort(delegate (WaterMove a, WaterMove b)
        {
            if (a._GET_POSITION.y > b._GET_POSITION.y)
            {
                return 1;
            }

            if (a._GET_POSITION.y < b._GET_POSITION.y)
            {
                return -1;
            }

            return 0;
        }
        );
        List<int> objList_y = new List<int>();
        for (int i = 0; i < objList.Count; ++i)
        {
            for (int t = 0; t < vertices.Length; ++t)
            {
                if (objList[i]._GET_POSITION == vertices[t])
                {
                    objList_y.Add(t);
                }
            }
        }
        // (なんて説明すればいいのかわからない[後で書く])
        objList.Sort(delegate (WaterMove a, WaterMove b)
        {
            if (a._GET_POSITION.z > b._GET_POSITION.z)
            {
                return 1;
            }

            if (a._GET_POSITION.z < b._GET_POSITION.z)
            {
                return -1;
            }

            return 0;
        }
        );
        List<int> objList_z = new List<int>();
        for (int i = 0; i < objList.Count; ++i)
        {
            for (int t = 0; t < vertices.Length; ++t)
            {
                if (objList[i]._GET_POSITION == vertices[t])
                {
                    objList_z.Add(t);
                }
            }
        }

        // 三角形の頂点インデックスの指定
        int count = 0;
        for (int i = 0; i < triangles.Length; i += 9)
        {
            //var a = new Vector2(vertices[count].x, vertices[count].y);
            //var b = new Vector2(vertices[count + 1].x, vertices[count + 1].y);
            //var c = new Vector2(vertices[count + 2].x, vertices[count + 2].y);

            //var side1 = b - a;
            //var side2 = c - a;

            //var perp = Vector2Cross(side1, side2);
            
            //if (perp < 0)
            //{
            //    triangles[i] = count;
            //    triangles[i + 1] = count + 1;
            //    triangles[i + 2] = count + 2;
            //}
            //else
            //{
            //    triangles[i] = count;
            //    triangles[i + 1] = count + 2;
            //    triangles[i + 2] = count + 1;
            //}

            triangles[i] = count;
            triangles[i + 1] = count + 1;
            triangles[i + 2] = count + 2;

            triangles[i + 3] = objList_y[count];
            triangles[i + 4] = objList_y[count + 1];
            triangles[i + 5] = objList_y[count + 2];

            triangles[i + 6] = objList_z[count];
            triangles[i + 7] = objList_z[count + 1];
            triangles[i + 8] = objList_z[count + 2];

            count++;
        }

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

    float Vector2Cross(Vector2 lhs, Vector2 rhs)
    {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }
}
