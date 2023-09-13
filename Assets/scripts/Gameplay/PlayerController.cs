using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Face Controls")] [Tooltip("Degrees rotated in +/- Y direction per turn")]
    public int spinDegrees = 90;

    private Transform _initTransform;

    // Start is called before the first frame update
    void Start()
    {
        _initTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) /* Left Turn */
        {
            TurnPlayer(-1);
        }

        if (Input.GetKeyDown(KeyCode.D)) /* Right Turn */
        {
            TurnPlayer(1);
        }
    }

    void TurnPlayer(int direction)
    {
        gameObject.transform.Rotate(Vector3.up, direction * spinDegrees);
    }
}