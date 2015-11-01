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

        IsRenderMeshUpdate();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = _mesh;
        GetComponent<MeshFilter>().sharedMesh.name = "myMesh";
	}

    // Update is called once per frame
    void Update()
    {

        IsRenderMeshUpdate();
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
}
