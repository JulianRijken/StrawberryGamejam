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

    }


    private void Update()
    {

        // Update Collision Hit Result
        CollisionResult result = GetHitResult();
        show = result;



        // Handle rotation \\        

        float rotateInput = -m_Controls.Player.Rotate.ReadValue<float>();
        float rotateInputDelta = m_RotateSpeed * Time.deltaTime * rotateInput;

        float rotateEdgeDelta = result.Side * (result.AngleFromEdge - m_PlayerWith);


        // Clamp rotation delta based on edge distance
        if(result.Side != 0 && rotateInput != 0)
        {
            if (result.Side > 0)
                rotateInputDelta = Mathf.Min(rotateEdgeDelta, rotateInputDelta);
            else 
                rotateInputDelta = Mathf.Max(rotateEdgeDelta, rotateInputDelta);
        }

        transform.Rotate(Vector3.forward, rotateInputDelta);





        // Handle Position \\ 

        Debug.Log("Distance to edge: " + result.InnerEdgeDistanceToPlayerPoint + " | " + " Move Speed: " + (Time.deltaTime * 50f));

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
                result.Side = angleFromCenter > 0 ? 1: -1;
                result.collision = obstacle;
                result.InnerEdgeDistanceToPlayerPoint = innerEdgeDistanceToPlayerPoint;
                // Break out of all obstacles when inside of one
                break;
            }
        }
        
        return result;
    }


    [System.Serializable]
    public struct CollisionResult
    {
        public Obstacle collision;
        public int Side;
        public bool InsideRing;
        public bool InsideAngle;

        public float AngleFromEdge;
        public float InnerEdgeDistanceToPlayerPoint;
    }

}


