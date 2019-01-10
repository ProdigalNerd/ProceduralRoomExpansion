using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBasedReposition : MonoBehaviour
{
    private Vector3 previousPosition;
    private int matchCount = 0;

    [HideInInspector]
    public bool doneMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!doneMoving)
        {
            Vector3 curPos = this.gameObject.transform.position;
            if (curPos == previousPosition)
            {
                matchCount++;

                if (matchCount == 5)
                {
                    doneMoving = true;
                }
            }
            else
            {
                matchCount = 0;
                previousPosition = curPos;
            }
        }
    }
}
