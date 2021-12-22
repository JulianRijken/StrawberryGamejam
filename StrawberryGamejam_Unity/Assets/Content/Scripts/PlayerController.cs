using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_RotateSwayDistance;
    [SerializeField] private float m_RotateSwayDampSpeed;
    [SerializeField] private GameObject m_PlayerPointer;
    [SerializeField] private SpriteRenderer m_PlayerSprite;
    [SerializeField] private bool m_CanDie;

    private Controls m_Controls;

    private void Start()
    {
        m_Controls = new Controls();
        m_Controls.Enable();
    }

    // Remove
    private CollisionResult last;

    private void Update()
    {
        if(last.FatalObjstacle)
        {
            last.FatalObjstacle.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        // Update Collision Hit Result
        CollisionResult collisionResult = GetCollisionResult();
        last = collisionResult;
        if (collisionResult.FatalObjstacle)
        {
            collisionResult.FatalObjstacle.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }




        HandleRotation(collisionResult);

        if(m_CanDie)
            CheckDeath(collisionResult);
    }



    private void HandleRotation(CollisionResult collisionResult)
    {

        float rotateInput = -m_Controls.Player.Rotate.ReadValue<float>();
        float rotateInputDelta = m_RotateSpeed * Time.deltaTime * rotateInput;

        float rotateEdgeDelta = collisionResult.EdgeObstacleSide * collisionResult.EdgeObstacleAngle;


        //Clamp rotation delta based on edge distance
        if (collisionResult.EdgeObstacle && rotateInput != 0)
        {
            if (collisionResult.EdgeObstacleSide > 0)
                rotateInputDelta = Mathf.Min(rotateEdgeDelta, rotateInputDelta);
            else
                rotateInputDelta = Mathf.Max(rotateEdgeDelta, rotateInputDelta);
        }

        transform.Rotate(Vector3.forward, rotateInputDelta);


        float sway = rotateInputDelta == 0f ? 0f : rotateInputDelta > 0f ? 1f : -1f;

        if (m_PlayerSprite)
        {
            Quaternion targetRotation = transform.rotation;
            targetRotation *= Quaternion.Euler(0, 0, sway * m_RotateSwayDistance);

            m_PlayerSprite.transform.rotation = Quaternion.Slerp(m_PlayerSprite.transform.rotation, targetRotation, m_RotateSwayDampSpeed * Time.deltaTime);
        }
    }


    private void CheckDeath(CollisionResult collisionResult)
    {
        if (collisionResult.FatalObjstacle)
        {
            float obstacleMoveDelta = collisionResult.FatalObjstacle.MoveSpeed * Time.deltaTime / 2f;

            if (collisionResult.FatalObjstacleDistance < 0f && Mathf.Abs(collisionResult.FatalObjstacleDistance) < obstacleMoveDelta)
            {
                OnDeath();
            }
        }
    }

    private void OnDeath()
    {
        Debug.Log("Died");
        Time.timeScale = 0;
    }

    private CollisionResult GetCollisionResult()
    {

        CollisionResult result = new CollisionResult();

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();


        foreach (Obstacle obstacle in obstacles)
        {

            // Check if player is inside of ring
            float outerEdgePoint = obstacle.Distance / 2f;
            float innerEdgePoint = Mathf.Max(0f, outerEdgePoint - (obstacle.EdgeSize / 2f));
            float playerPoint = m_PlayerPointer.transform.localPosition.y;

            float outerEdgeDistanceToPlayerPoint = playerPoint - outerEdgePoint;
            float innerEdgeDistanceToPlayerPoint = playerPoint - innerEdgePoint;

            bool bInsideRing = innerEdgeDistanceToPlayerPoint > 0 && outerEdgeDistanceToPlayerPoint < 0;

            float dotAwayFromPlayer = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.RotationAlpha * 360f) / 180f);
            float angleFromCenter = Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.RotationAlpha * 360f);


            float angleFromEdge = Mathf.Max(0, Mathf.Abs(angleFromCenter) - (obstacle.FillAlpha * 180f));
            bool bInsideAngle = dotAwayFromPlayer < obstacle.FillAlpha;
            int side = angleFromCenter > 0 ? 1 : -1;


            if(bInsideRing)
            {
                if(result.EdgeObstacleAngle > angleFromEdge || !result.EdgeObstacle)
                {
                    result.EdgeObstacle = obstacle;

                    result.EdgeObstacleSide = side;
                    result.EdgeObstacleAngle = angleFromEdge;
                }
            }


            if (bInsideAngle)
            {
                if (result.FatalObjstacleDistance < Mathf.Abs(innerEdgeDistanceToPlayerPoint)  || !result.FatalObjstacle)
                {
                    if (outerEdgeDistanceToPlayerPoint < 0)
                    {
                        result.FatalObjstacle = obstacle;

                        result.FatalObjstacleDistance = innerEdgeDistanceToPlayerPoint;
                    }
                }
            }
        }

        return result;
    }


    [System.Serializable]
    public struct CollisionResult
    {
        public Obstacle FatalObjstacle;
        public float FatalObjstacleDistance;

        public Obstacle EdgeObstacle;
        public float EdgeObstacleAngle;
        public int EdgeObstacleSide;
    }

}


