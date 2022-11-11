using HuangD.Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class LeaderPanel : MonoBehaviour
{
    public Text label;
    public Text officeName;

    public IPerson leader
    {
        get
        {
            return _leader;
        }
        set
        {
            if(_leader == value)
            {
                return;
            }

            _leader = value;
        }
    }

    private IPerson _leader;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        label.text = leader.fullName;
        officeName.text = leader.office.name;
    }
}
