using UnityEngine;

public class RxBehaviour : MonoBehaviour
{
    protected ReactiveUI rxUI = new ReactiveUI();

    private void OnDestroy()
    {
        rxUI.Dispose();
    }
}
