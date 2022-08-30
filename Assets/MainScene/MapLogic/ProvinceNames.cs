using Math.TileMap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceNames : MonoBehaviour
{
    public Text nameTemplate;
    public Grid mapGrid;

    public IEnumerable<(Text label, (int x, int y) pos)> namesWithPos;

    public Block[] blocks
    {
        set
        {
            foreach (var block in value)
            {
                var pos = GetCenterPos(block.elements.Except(block.edges).ToHashSet());
                var worldPos = mapGrid.CellToWorld(new Vector3Int(pos.x, pos.y));

                var label = Instantiate<Text>(nameTemplate, this.transform);
                label.transform.position = worldPos;
            }
        }
    }

    public IEnumerable<Province> provinces
    {
        set
        {
            namesWithPos = value.Select(x => {
                var pos = GetCenterPos(x.block.elements.Except(x.block.edges).ToHashSet());

                var label = Instantiate<Text>(nameTemplate, this.transform);
                label.text = x.name + "州";
                return (label, pos);
            }).ToArray();


            UpdateNamePosition();

            nameTemplate.gameObject.SetActive(false);

            //foreach (var prov in value)
            //{
            //    var pos = GetCenterPos(prov.block.elements.Except(prov.block.edges).ToHashSet());
            //    var worldPos = mapGrid.CellToWorld(new Vector3Int(pos.x, pos.y));

            //    var label = Instantiate<Text>(nameTemplate, this.transform);
            //    label.text = prov.name + "州";
            //    label.transform.position = worldPos;
            //}


        }
    }

    public void UpdateNamePosition()
    {
        foreach (var elem in namesWithPos)
        {
            var worldPos = mapGrid.CellToWorld(new Vector3Int(elem.pos.x, elem.pos.y));

            elem.label.transform.position = worldPos;
        }

    }

    private (int x, int y) GetCenterPos(HashSet<(int x, int y)> elements)
    {
        IEnumerable<(int x, int y)> rings;

        int max = 0;
        int index = -1;
        for (int i = 0; i < elements.Count; i++)
        {
            var elem = elements.ElementAt(i);
            int distance = 1;
            do
            {
                rings = Hexagon.GetRing(elem, distance);
                distance++;
            }
            while (rings.All(x => elements.Contains(x)));
            if (distance > max)
            {
                index = i;
                max = distance;
            }
        }

        return elements.ElementAt(index);
    }

    //private (int x, int y) GetCenterPos(HashSet<(int x, int y)> elements)
    //{
    //    var xMax =elements.Select(e => e.x).Max();
    //    var yMax = elements.Select(e => e.y).Max();
    //    var xMin = elements.Select(e => e.x).Min();
    //    var yMin = elements.Select(e => e.y).Min();

    //    var xCount = 0;
    //    var xSelect = -1;
    //    for(int i=xMin; i<=xMax; i++)
    //    {
    //        var currCount = elements.Count(e => e.x == i);
    //        if(currCount > xCount)
    //        {
    //            xCount = currCount;
    //            xSelect = i;
    //        }
    //    }

    //    var yCount = 0;
    //    var ySelect = -1;
    //    for (int i = yMin; i <= yMax; i++)
    //    {
    //        var currCount = elements.Count(e => e.y == i);
    //        if (currCount > yCount)
    //        {
    //            yCount = currCount;
    //            ySelect = i;
    //        }
    //    }

    //    if(yCount >= xCount)
    //    {
    //        var xRCount = 0;
    //        var xRSelect = -1;

    //        for (int i = xMin; i <= xMax; i++)
    //        {
    //            if (!elements.Contains((i, ySelect)))
    //            {
    //                continue;
    //            }

    //            var currCount = elements.Count(e => e.x == i);
    //            if (currCount > xRCount)
    //            {
    //                xRCount = currCount;
    //                xRSelect = i;
    //            }
    //        }

    //        return (xRSelect, ySelect);
    //    }
    //    else
    //    {
    //        var yRCount = 0;
    //        var yRSelect = -1;

    //        for (int i = yMin; i <= yMax; i++)
    //        {
    //            if (!elements.Contains((xSelect, i)))
    //            {
    //                continue;
    //            }

    //            var currCount = elements.Count(e => e.y == i);
    //            if (currCount > yRCount)
    //            {
    //                yRCount = currCount;
    //                yRSelect = i;
    //            }
    //        }

    //        return (xSelect, yRSelect);
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
