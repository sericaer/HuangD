using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Population : MonoBehaviour
{
    public Text count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count.text = Facade.session.playerCountry.provinces.Sum(x => x.population).ToString();
    }
}
