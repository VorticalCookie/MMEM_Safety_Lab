using System.Collections;
using UnityEngine;

public class TestCapsule : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Relocate());
    }

    public IEnumerator Relocate()
    {
        yield return new WaitForSeconds(3f);
        transform.position = new Vector3(0, 2, 5);
        //new Vector3(1, 2, 5);
    }
}
