using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteParticles());
    }

    // Delete the particle after a few seconds
    private IEnumerator DeleteParticles()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
