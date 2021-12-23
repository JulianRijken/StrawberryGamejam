using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private float m_CameraRotatateSpeed;
    private float[] m_SpeedSet = new float[] { 25f, 50f, 100f };
    private float[] m_TransitonSpeedSet = new float[] { 0.5f, 1f, 3f };

    private void Start()
    {
        StartCoroutine(HandeCameraRotation());
    }

    private void LateUpdate()
    {
        //transform.RotateAround(Vector3.zero,)

        transform.Rotate(transform.forward, m_CameraRotatateSpeed * Time.deltaTime);
    }


    private IEnumerator HandeCameraRotation()
    {
        while (true)
        {
            float speedRange = m_SpeedSet[Random.Range(0, m_SpeedSet.Length)];
            float targetRotationSpeed = m_CameraRotatateSpeed > 0f ? -speedRange : speedRange;
            float oldRotationSpeed = m_CameraRotatateSpeed;

            float transitionSpeed = m_TransitonSpeedSet[Random.Range(0, m_TransitonSpeedSet.Length)];
            float timer = 0f;

            while (timer < 1f)
            {
                yield return new WaitForEndOfFrame();

                timer += Time.deltaTime / transitionSpeed;

                m_CameraRotatateSpeed = Mathf.Lerp(oldRotationSpeed, targetRotationSpeed, timer);
            }

            m_CameraRotatateSpeed = targetRotationSpeed;

            yield return new WaitForSeconds(Random.Range(4f, 8f));
        }
    }
}
