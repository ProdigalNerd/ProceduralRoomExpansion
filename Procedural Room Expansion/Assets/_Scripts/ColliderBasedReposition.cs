using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBasedReposition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        this.gameObject.transform.position += new Vector3(0, 1, 0);
        other.gameObject.transform.position -= new Vector3(0, 1, 0);
    }
}
