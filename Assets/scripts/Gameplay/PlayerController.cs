using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Face Controls")] [Tooltip("Degrees rotated in +/- Y direction per turn")]
    public int spinDegrees = 90;

    [Tooltip("Speed of turning")] public float spinSpeed = 15.0f;

    private Transform _initTransform;
    private Quaternion _targetRotation;

    // Start is called before the first frame update
    private void Start()
    {
        _initTransform = gameObject.transform;
        _targetRotation = _initTransform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        if (Input.GetKeyDown(KeyCode.A)) /* Left Turn */
        {
            SetRotation(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D)) /* Right Turn */
        {
            SetRotation(1);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, spinSpeed * Time.deltaTime);
    }

    private void SetRotation(int direction)
    {
        _targetRotation *= Quaternion.AngleAxis(spinDegrees * direction, Vector3.up);
    }
}
