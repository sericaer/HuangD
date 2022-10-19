using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Camera mapCamera;
    public MapRender mapRender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel > 0)
        {
            OnScrollWheel(true);
        }
        if (mouseScrollWheel < 0)
        {
            OnScrollWheel(false);
        }
    }

    public void OnMove(Vector3 pos)
    {
        Vector3 move = CalcMoveOffset(pos);

        mapCamera.transform.position = mapCamera.transform.position + move;
        //mapUIContainer.UpdateItemsPosition();
    }

    private void OnScrollWheel(bool flag)
    {
        var newSize = mapCamera.orthographicSize + 0.25f * (flag ? 1 : -1);

        if (newSize > 6f && newSize < 20f)
        {
            mapCamera.orthographicSize = newSize;
        }
    }

    private Vector3 CalcMoveOffset(Vector3 pos)
    {
        Vector3 move = (pos - new Vector3(0.5f, 0.5f)) * 0.1f;
        Debug.Log(move);
        if (move.x < 0)
        {
            var edgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f));
            if (!mapRender.HasTile(edgeCenter))
            {
                move = new Vector3(0, move.y);
            }
        }
        else if (move.x > 0)
        {
            var edgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f));
            if (!mapRender.HasTile(edgeCenter))
            {
                move = new Vector3(0, move.y);
            }
        }

        if (move.y < 0)
        {
            var edgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f));
            if (!mapRender.HasTile(edgeCenter))
            {
                move = new Vector3(move.x, 0);
            }
        }
        else if (move.y > 0)
        {
            var edgeCenter = mapCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f));
            if (!mapRender.HasTile(edgeCenter))
            {
                move = new Vector3(move.x, 0);
            }
        }

        return move;
    }

}
