using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapCamera : MonoBehaviour
{
    public Camera mapCamera;
    public MapRender mapRender;

    public UnityEvent OnMoved;
    public UnityEvent<float> OnZoom;

    public int maxOrthoSize = 20;
    public int minOrthoSize = 6;

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

    public void MoveTo(Vector3 pos)
    {
        mapCamera.transform.position = mapCamera.transform.position + pos;
        OnMoved.Invoke();
    }

    public void MoveOffset(Vector3 offset)
    {
        var real = CalcMoveOffset(offset);
        MoveTo(real);
    }

    private void OnScrollWheel(bool flag)
    {
        var newSize = mapCamera.orthographicSize + 0.25f * (flag ? 1 : -1);

        newSize = Mathf.Min(newSize, maxOrthoSize);
        newSize = Mathf.Max(newSize, minOrthoSize);

        mapCamera.orthographicSize = newSize;

        OnZoom.Invoke((newSize - minOrthoSize) / (maxOrthoSize - minOrthoSize));
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
