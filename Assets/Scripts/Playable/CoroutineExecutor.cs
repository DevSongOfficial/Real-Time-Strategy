using System.Collections;
using UnityEngine;

public class CoroutineExecutor : MonoBehaviour
{
    private IEnumerator currentCoroutine;
    
    public void Execute(IEnumerator enumerator)
    {
        StopCurrentCoroutine();

        currentCoroutine = enumerator;
        StartCoroutine(currentCoroutine);
    }

    public void StopCurrentCoroutine()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }
}
