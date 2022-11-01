using UnityEngine;

public class PanelFactory : MonoBehaviour
{
    public ReactiveTest reactiveTestTemplate;
    public ReactiveListTest reactiveListTestTemplate;

    private GameObject currentInstance;

    private Reactive<int> reactiveInt;
    private ReactiveList<Data> reactiveIntList;

    public void Start()
    {
        reactiveTestTemplate.gameObject.SetActive(false);
        reactiveListTestTemplate.gameObject.SetActive(false);
    }

    public void OnDestroyButton()
    {
        Destroy(currentInstance);
    }

    public void OnCreateRxIntButton()
    {
        if(reactiveInt == null)
        {
            reactiveInt = new Reactive<int>(1000);
        }

        var currRx = Instantiate(reactiveTestTemplate, reactiveTestTemplate.transform.parent);
        currRx.SetDataSource(reactiveInt);

        currentInstance = currRx.gameObject;

        currentInstance.SetActive(true);
        currentInstance.transform.position = reactiveTestTemplate.transform.position;
    }

    public void OnCreateRxListButton()
    {
        if(reactiveIntList == null)
        {
            reactiveIntList = new ReactiveList<Data>(new Data[] { new Data(1000), new Data(2000) });
        }


        var currRx = Instantiate(reactiveListTestTemplate, reactiveListTestTemplate.transform.parent);
        currRx.SetDataSource(reactiveIntList);

        currentInstance = currRx.gameObject;

        currentInstance.SetActive(true);
        currentInstance.transform.position = reactiveListTestTemplate.transform.position;
    }

    int count = 0;
    private void Update()
    {
        count++;

        if (count%100 != 0)
        {
            return;
        }

        if(reactiveInt != null)
        {
            reactiveInt.SetValue(reactiveInt.GetValue()+1);
        }

        if (reactiveIntList != null)
        {
            foreach(var item in reactiveIntList)
            {
                item.Value.SetValue(item.Value.GetValue()+1);
            }
        }
    }
}