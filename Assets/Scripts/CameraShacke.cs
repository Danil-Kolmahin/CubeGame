using UnityEngine;

public class CameraShacke : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDur = 1f, shakeAmount = 0.04f, decreaseFactor = 1.5f;

    private Vector3 originPos; 

    void Start()
    {
        camTransform = GetComponent<Transform>();
        originPos = camTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDur > 0)
        {
            camTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFactor;
        } else
        {
            shakeDur = 0;
            camTransform.localPosition = originPos;
        }
    }
}
