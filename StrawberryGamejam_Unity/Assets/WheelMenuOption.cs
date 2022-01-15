using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WheelMenuOption : MonoBehaviour
{

    //[SerializeField] private float m_RotateDistance;
    //[SerializeField] private float m_TransitionTime;
    [SerializeField] private TextMeshPro m_Text;
    //[SerializeField] private AnimationCurve m_MotionTimeCurve;

    public string OptionName
    {
        set
        {
            m_Text.text = value;
        }
    }

    public float ColorAlpha
    {
        set
        {
            Color color = m_Text.color;
            color.a = value;
            m_Text.color = color;
        }

    }

    //public void Show(int direction)
    //{
    //    StartCoroutine(TransitionAnimation(direction, false));
    //}



    //public void Hide(int direction)
    //{
    //    StartCoroutine(TransitionAnimation(direction, true));
    //}


    //private void UpdateAnimation(float motionTime)
    //{

    //    float curvedMotionTime = m_MotionTimeCurve.Evaluate(motionTime);

    //    transform.localRotation = Quaternion.Euler(Vector3.forward * curvedMotionTime * m_RotateDistance);

    //    Color color = m_Text.color;

    //    color.a = 1f - Mathf.Abs(curvedMotionTime);

    //    m_Text.color = color;
    //}


    //private IEnumerator TransitionAnimation(int rotateDirection, bool reverse)
    //{
    //    float alpha = 0f;

    //    for (; ; )
    //    {
    //        float animationAlpha = !reverse ? Mathf.Lerp(1f, 0f, alpha) : alpha;

    //        UpdateAnimation(animationAlpha * rotateDirection);


    //        if (alpha >= 1f) break;

    //        yield return new WaitForEndOfFrame();
    //        alpha = Mathf.Clamp01(alpha + (Time.deltaTime * (1 / m_TransitionTime)));
    //    }
    //}





    //private IEnumerator TransitionAnimation(int rotateDirection)
    //{
    //    float alpha = 0f;

    //    for (; ; )
    //    {

    //        float alphaOverCuve = m_AlphaCurve.Evaluate(alpha);
    //        transform.localRotation = Quaternion.Euler(Vector3.forward * alphaOverCuve * m_RotateDistance * rotateDirection);

    //        Color color = m_Text.color;

    //        color.a = Mathf.Lerp(1f, 0f, alphaOverCuve);

    //        m_Text.color = color;


    //        if (alpha >= 1f) break;

    //        yield return new WaitForEndOfFrame();
    //        alpha = Mathf.Clamp01(alpha + (Time.deltaTime * (1 / m_TransitionTime)));
    //    }
    //}

}
