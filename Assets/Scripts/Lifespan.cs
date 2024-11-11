using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    [SerializeField]
    float lifespan;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine("Die", lifespan);
    }

    private IEnumerator Die(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }


}
