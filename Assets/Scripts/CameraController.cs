using UnityEngine;

public class CameraController: MonoBehaviour
{
    public float WheelSensitivity = 5f;
    public float KeyboardSensitivity = 1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            transform.localPosition += new Vector3(0, -transform.up.y * KeyboardSensitivity, 0);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.localPosition -= new Vector3(0, -transform.up.y * KeyboardSensitivity, 0);

        if (Input.GetKey(KeyCode.RightArrow))
            transform.localPosition += new Vector3(-transform.right.x * KeyboardSensitivity, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.localPosition -= new Vector3(-transform.right.x * KeyboardSensitivity, 0, 0);

        float translateZ = Input.GetAxis("Mouse ScrollWheel") * WheelSensitivity;
        transform.Translate(0, 0, translateZ * Time.deltaTime);
    }
}
