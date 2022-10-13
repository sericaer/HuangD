using UnityEngine;
using UnityEngine.UI;

public class LeaderPanel : MonoBehaviour
{
    public Text label;
    public Text officeName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //label.text = Facade.session.playerCountry.officeGroup.leaderOffice.person.fullName;
        //officeName.text = Facade.session.playerCountry.officeGroup.leaderOffice.name;
    }
}
