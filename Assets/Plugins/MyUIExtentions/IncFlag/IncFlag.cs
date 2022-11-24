using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IncFlag : MonoBehaviour
{
    public bool isInc
    {
        get
        {
            return _isInc;
        }
        set
        {
            _isInc = value;

            Inc.gameObject.SetActive(isInc);
            Dec.gameObject.SetActive(!isInc);
        }
    }

    private bool _isInc = true;

    private Image Inc => GetComponentsInChildren<Image>(true).SingleOrDefault(x => x.name == "Inc");
    private Image Dec => GetComponentsInChildren<Image>(true).SingleOrDefault(x => x.name == "Dec");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
