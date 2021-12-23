using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    //[SerializeField] private float m_angle;
    //[SerializeField] private float m_AngleDist;

    //[ColorUsage(false)]
    //[SerializeField] private Color[] m_ColorOptions;
    //private Color m_color;


    [SerializeField] private float m_CameraDistance;
    [SerializeField] private float[] m_SpeedSet = new float[] { 25f, 50f, 100f };
    [SerializeField] private float[] m_TransitonSpeedSet = new float[] { 0.5f, 1f, 3f };

    private float m_CameraRotatateSpeed;



    private void Start()
    {
        //volume = FindObjectOfType<Volume>();
        //volume.profile.TryGet(out colorAjustment);

        StartCoroutine(HandeCameraRotation());
    }

    private void LateUpdate()
    {

        // Camera Rotation\\

        //Vector2 direction = new Vector2(Mathf.Cos(m_angle * Mathf.Deg2Rad), Mathf.Sin(m_angle * Mathf.Deg2Rad));
        //direction.Normalize();
        //transform.rotation = Quaternion.Euler(direction.x * m_AngleDist, direction.y * m_AngleDist, transform.rotation.z);

        transform.Rotate(Vector3.forward, m_CameraRotatateSpeed * Time.deltaTime);
        //transform.RotateAround(Vector3.zero, transform.forward, m_CameraRotatateSpeed * Time.deltaTime);

        //transform.position = Vector3.zero + transform.forward * m_CameraDistance;








        

        //colorAjustment.colorFilter.value = m_color;


    }

    //private Volume volume;
    //private ColorAdjustments colorAjustment;




    private IEnumerator HandeCameraRotation()
    {
        while (true)
        {
            float speedRange = m_SpeedSet[Random.Range(0, m_SpeedSet.Length)];
            float targetRotationSpeed = m_CameraRotatateSpeed > 0f ? -speedRange : speedRange;
            float oldRotationSpeed = m_CameraRotatateSpeed;

            float transitionSpeed = m_TransitonSpeedSet[Random.Range(0, m_TransitonSpeedSet.Length)];
            float timer = 0f;

            //Color oldColor = m_color;
            //Color targetColor = m_ColorOptions[Random.Range(0, m_ColorOptions.Length)];
            
            while (timer < 1f)
            {
                yield return new WaitForEndOfFrame();

                timer += Time.deltaTime / transitionSpeed;

                m_CameraRotatateSpeed = Mathf.Lerp(oldRotationSpeed, targetRotationSpeed, timer);
                //m_color = Color.Lerp(oldColor, targetColor, timer);
            }


            m_CameraRotatateSpeed = targetRotationSpeed;

            //m_color = targetColor;

            yield return new WaitForSeconds(Random.Range(4f, 8f));
        }
    }
}
