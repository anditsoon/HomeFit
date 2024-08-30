using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_PlayerMoveTest : MonoBehaviour
{
    float v;
    float h;
    Vector3 dir;
    float moveSpeed = 7;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        dir = new Vector3(h, 0, v);

        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
}
