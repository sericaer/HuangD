using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceCountPanel : MonoBehaviour
{
    public Text label;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        label.text = Facade.session.playerCountry.provinces.Count().ToString();
    }
}