using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_PlayerWith = 0.1f;
    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_RotateSwayDistance;
    [SerializeField] private float m_RotateSwayDampSpeed;
    [SerializeField] private GameObject m_PlayerPointer;

    private Controls m_Controls;

    [SerializeField]
    public CollisionResult show;

    private void Awake()
    {
        m_Controls = new Controls();
        m_Controls.Enable();

        Application.targetFrameRate = 144;
    }


    private void FixedUpdate()
    {

        // Update Collision Hit Result
        CollisionResult result = GetHitResult();
        show = result;


        float rotateInput = m_Controls.Player.Rotate.ReadValue<float>();
        float rotateValue = m_RotateSpeed * Time.fixedDeltaTime * -rotateInput;

        if (result.Side == 0)
        {
            transform.Rotate(Vector3.forward, rotateValue);
        }
        else
        {
            transform.Rotate(Vector3.forward, result.Side * -1 * (result.AngleFromEdge - m_PlayerWith));
        }
    }

    private CollisionResult GetHitResult()
    {

        CollisionResult result = new CollisionResult();
        result.InsideRing = false;
        result.Side = 0;

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

            bool bInsideRing = innerEdgeDistanceToPlayerPoint > 0 && outerEdgeDistanceToPlayerPoint < 0;


            // Stop checking obstacles when colliding with one
            if (bInsideRing)
            {
                float dotAwayFromPlayer = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, s.RotationAlpha * 360f) / 180f);
                float angleFromCenter = Mathf.DeltaAngle(transform.eulerAngles.z, s.RotationAlpha * 360f);

                result.InsideRing = true;
                result.AngleFromEdge = Mathf.Max(0, Mathf.Abs(angleFromCenter) - (s.FillAlpha * 180f));
                result.InsideAngle = dotAwayFromPlayer < s.FillAlpha;
                result.Side = angleFromCenter > 0 ? -1: 1;


                // Break out of all obstacles when inside of one
                break;
            }
        }
        
        return result;
    }


    [System.Serializable]
    public struct CollisionResult
    {
        public int Side;
        public bool InsideRing;
        public bool InsideAngle;

        public float AngleFromEdge;
    }

}


