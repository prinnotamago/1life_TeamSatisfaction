using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MeshTriangle : MonoBehaviour {

    [SerializeField]
    GameObject _watersObjManager;

    Mesh _mesh;

    // Use this for initialization
    void Start () {
        _mesh = new Mesh();

        Is2DRenderMeshUpdate();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
	}

    // Update is called once per frame
    void Update()
    {
        Is2DRenderMeshUpdate();
    }

    void IsRenderMeshUpdate()
    {
        // プレイヤーの水を配列で確保
        var waters = GameObject.Find(_watersObjManager.name).GetComponentsInChildren<WaterMove>();

        // 描画に必要な水だけをいれる List
        List<WaterMove> objList = new List<WaterMove>();

        for (int i = 0; i < waters.Length; ++i)
        {
            // 描画すべき水だったら List に追加
            if (waters[i]._IS_RENDERED)
            {
                objList.Add(waters[i]);
            }
        }

        // 配列数が２以下だと△ポリゴンが作れないので抜ける(３つ頂点が必要)
        int triangleNum = (objList.Count - 2) * 3;
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

        Vector3[] vertices = new Vector3[objList.Count];
        Vector2[] uv = new Vector2[objList.Count];
        // ソートしたX,Y,Zでつないでいるため * 3 している
        int[] triangles = new int[triangleNum * 3];

        // 順番につなげていくために X座標 でソート
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

        //--------------------------------------------------------------

        // 順番につなげていくために Y座標 でソート
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
        // その頂点が X座標 でソートした時の何番目かを調べて格納
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

        // Z座標 も同様にソート
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
        // その頂点が X座標 でソートした時の何番目かを調べて格納
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

        //--------------------------------------------------------------

        // 三角形の頂点インデックスの指定
        int count = 0;  // 0,1,2 → 1,2,3 → 2,3,4　とつなげるためのカウント
        for (int i = 0; i < triangles.Length; i += 9)
        {
            // ソートした X座標 の順番でつなぐ
            triangles[i] = count;
            triangles[i + 1] = count + 1;
            triangles[i + 2] = count + 2;

            // ソートした Y座標 の順番でつなぐ
            triangles[i + 3] = objList_y[count];
            triangles[i + 4] = objList_y[count + 1];
            triangles[i + 5] = objList_y[count + 2];

            // ソートした Z座標 の順番でつなぐ
            triangles[i + 6] = objList_z[count];
            triangles[i + 7] = objList_z[count + 1];
            triangles[i + 8] = objList_z[count + 2];

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

    float Vector2Cross(Vector2 lhs, Vector2 rhs)
    {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }

    void Is2DRenderMeshUpdate()
    {
        // プレイヤーの水を配列で確保
        var waters = GameObject.Find(_watersObjManager.name).GetComponentsInChildren<WaterMove>();

        // 描画に必要な水だけをいれる List
        List<WaterMove> objList = new List<WaterMove>();

        for (int i = 0; i < waters.Length; ++i)
        {
            // 描画すべき水だったら List に追加
            if (waters[i]._IS_RENDERED)
            {
                objList.Add(waters[i]);
            }
        }

        // 配列数が２以下だと△ポリゴンが作れないので抜ける(３つ頂点が必要)
        int triangleNum = (objList.Count - 2) * 3;
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

        Vector3 SumPosition = Vector3.zero;
        for(int i = 0; i < objList.Count; ++i)
        {
            SumPosition += objList[i].transform.position;
        }
        Vector3 AveragePosition = SumPosition / objList.Count;
        //Vector3 AveragePosition = GameObject.Find("Player").transform.position;

        // + 1 は AveragePosition の分
        Vector3[] vertices = new Vector3[objList.Count + 1];
        Vector2[] uv = new Vector2[objList.Count + 1];
        // 0,1,2 → 0,2,3 → 0,3,4 と要素数３つずつでつないでいるため * 3 している
        int[] triangles = new int[((objList.Count - 1) * (objList.Count - 1)) * 3];

        //Debug.Log(triangles.Length / 3);

        // AveragePosition と距離が遠い順にソート
        objList.Sort(delegate (WaterMove a, WaterMove b)
        {
            Vector3 aPos = a.transform.position - AveragePosition;
            Vector3 bPos = b.transform.position - AveragePosition;
            float aValue =
            aPos.x * aPos.x +
            aPos.y * aPos.y +
            aPos.z * aPos.z;
            float bValue =
            bPos.x * bPos.x +
            bPos.y * bPos.y +
            bPos.z * bPos.z;
            if (aValue > bValue)
            {
                return 1;
            }
            else if (aValue < bValue)
            {
                return -1;
            }

            return 0;
        }
        );

        // 頂点座標指定
        vertices[0] = AveragePosition;
        for (int i = 1; i < vertices.Length; ++i)
        {
            vertices[i] = objList[i - 1]._GET_POSITION;
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
        // count は 0,1,2 → 0,2,3 → 0,3,4　とつなげるためのカウント
        // - 1 は要素数外にでないように
        int count = 0;
        //for (int i = 0; i < triangles.Length; i += 3)
        //{
        //    triangles[i] = 0;
        //    triangles[i + 1] = count + 1;
        //    triangles[i + 2] = count + 2;
        //    count++;
        //}

        for (int i = 0; i < objList.Count - 1; ++i)
        {
            for (int t = 0; t < objList.Count - 1; ++t)
            {
                triangles[count] = 0;
                triangles[count + 1] = i;
                triangles[count + 2] = t;
                count += 3;
            }
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
