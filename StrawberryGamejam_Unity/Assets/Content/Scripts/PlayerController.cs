using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_PlayerWith = 0.1f;
    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_RotateSwayDistance;
    [SerializeField] private float m_RotateSwayDampSpeed;
    [SerializeField] private GameObject m_PlayerPointer;

    private Controls m_Controls;

    private HitResult m_HitResult;

    private void Awake()
    {
        m_Controls = new Controls();
        m_Controls.Enable();

        Application.targetFrameRate = 144;
    }


    private void FixedUpdate()
    {

        // Update Collision Hit Result
        m_HitResult = GetHitResult();


        //float min = m_HitResult.Equals(HitResult.Left) ? 0f : -1f;
        //float max = m_HitResult.Equals(HitResult.Right) ? 0f : 1f;
        //float rotateInput = Mathf.Clamp(m_Controls.Player.Rotate.ReadValue<float>(), min, max);
        float rotateInput = m_Controls.Player.Rotate.ReadValue<float>();


        float rotateValue = m_RotateSpeed * Time.fixedDeltaTime * -rotateInput;

        Debug.Log(rotateValue);

        transform.Rotate(Vector3.forward, rotateValue);

        // Update Collision Hit Result
        m_HitResult = GetHitResult();

        if(m_HitResult.Equals(HitResult.hit))
        {
            transform.Rotate(Vector3.forward, -rotateValue);
        }
    }

    private HitResult GetHitResult()
    {

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (Obstacle obstacle in obstacles)
        {

            HalfCircleSettings s = obstacle.HalfCircleSettings;

            // Check if player is inside of ring
            float outerEdgePoint = obstacle.Distance / 2f;
            float innerEdgePoint = Mathf.Max(0f, outerEdgePoint - (s.EdgeSize / 2f));
            float playerPoint = m_PlayerPointer.transform.localPosition.y;

            float outerEdgeDistanceToPlayerPoint = playerPoint - outerEdgePoint;
            float innerEdgeDistanceToPlayerPoint = playerPoint - innerEdgePoint;


            // Check If Inside of ring
            if (innerEdgeDistanceToPlayerPoint > 0 && outerEdgeDistanceToPlayerPoint < 0)
            {

                float dotAwayFromPlayer = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, s.RotationAlpha * 360f) / 180f);

                // Check if player is within the angle of the ring
                if (dotAwayFromPlayer < s.FillAlpha)
                {
                    return HitResult.hit;
                }
                else
                {
                    float angleFromCenter = Mathf.DeltaAngle(transform.eulerAngles.z, s.RotationAlpha * 360f);
                    float angleFromEdge = Mathf.Max(0, Mathf.Abs(angleFromCenter) - (s.FillAlpha * 180f));

                    if (angleFromEdge < m_PlayerWith)
                    {
                        return angleFromCenter > 0 ? HitResult.Left : HitResult.Right;
                    }
                }
            }
        }


        return HitResult.none;
    }

    public enum HitResult
    {
        none,
        hit,
        Left,
        Right
    }


}


