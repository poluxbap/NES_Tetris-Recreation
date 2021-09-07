using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public float interval;
    public void CloseBlinds()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            StartCoroutine(WaitToEnable(transform.GetChild(i).gameObject, i));
        }
    }

    IEnumerator WaitToEnable( GameObject children, int i)
    {
        yield return new WaitForSeconds(interval * i);
        children.SetActive(true);
    }
}
