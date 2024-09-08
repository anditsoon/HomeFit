using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform knee;
    public Transform foot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aa = knee.position - foot.position;

        transform.position = foot.position + Vector3.Cross(transform.right, aa) * 0.2f;
    }
}
