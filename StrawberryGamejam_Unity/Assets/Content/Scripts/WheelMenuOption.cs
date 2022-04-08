using UnityEngine;
using TMPro;
using ntw.CurvedTextMeshPro;

public class WheelMenuOption : MonoBehaviour
{



    [SerializeField] private TextMeshPro m_Text;
    [SerializeField] private Animator m_Animator;
    private const string m_AnimatorAlphaParameter = "Alpha";



    public void SetAlpha(float alpha)
    {
        m_Animator.SetFloat(m_AnimatorAlphaParameter, alpha);

        Debug.LogError("Text mesh pro updating in animation overwrites other script :(");
    }


    public string OptionName
    {
        set
        {
            m_Text.text = value;
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
