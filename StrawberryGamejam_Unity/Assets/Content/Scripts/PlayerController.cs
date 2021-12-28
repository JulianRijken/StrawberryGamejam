using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_RotateSpeed;
    [SerializeField] private float m_RotateSwayDistance;
    [SerializeField] private float m_RotateSwayDampSpeed;
    [SerializeField] private float m_PlayerDistance;
    [SerializeField] private Transform m_PointerTransform;
    [SerializeField] private SpriteRenderer m_PlayerSprite;
    [SerializeField] private ObstacleManager m_ObstacleManager;

    private bool m_Death;

    private Controls m_Controls;
    private Obstacle m_InevitableFatalObstacle;



#if UNITY_EDITOR
    [SerializeField] 
    private bool m_CanDie;
#endif



    private void Start()
    {
        m_Controls = new Controls();
        m_Controls.Enable();
        Debug.Log("ff");
    }


    private void Update()
    {
        CollisionResult result = GetCollisionResult();

        HandlePlayerRotation(result);
        CheckDeath(result);



        // DEBUG
        Obstacle[] obstacles = m_ObstacleManager.ActiveObstacles.ToArray();

        // Reset 
        for (int i = 0; i < obstacles.Length; i++)        
            obstacles[i].GetComponent<SpriteRenderer>().color = Color.white;
        

        if (result.FatalObjstacle)
            result.FatalObjstacle.GetComponent<SpriteRenderer>().color = Color.red;

        if (result.EdgeObstacle)
            result.EdgeObstacle.GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void HandlePlayerRotation(CollisionResult result)
    {
        // Get Rotate Delta
        float rotateInput = -m_Controls.Player.Rotate.ReadValue<float>();
        float rotateDelta = Time.deltaTime * m_RotateSpeed * rotateInput;

        // Clamp Rotate Delta to stop player from going through walls 
        if (result.EdgeObstacle)
        {
            rotateDelta = result.EdgeObstacle_AngleDelta > 0 ? Mathf.Min(result.EdgeObstacle_EdgeDistance, rotateDelta) : Mathf.Max(-result.EdgeObstacle_EdgeDistance, rotateDelta);
        }

        // Apply rotation to player
        transform.Rotate(Vector3.forward, rotateDelta);
    }


    private void CheckDeath(CollisionResult result)
    {
        // Cancel if the player is already dead
        if (m_Death)
            return;


        // Check if the Inevitable Fatal Obstacle is still withing the death angle
        if (m_InevitableFatalObstacle)
        {
            float angleDelta = Mathf.DeltaAngle(transform.eulerAngles.z, m_InevitableFatalObstacle.Rotation);
            bool bInsideObstacleFill = Mathf.Abs(angleDelta) < (m_InevitableFatalObstacle.FillAngle / 2f);

            if (bInsideObstacleFill)
            {
                OnDeath(m_InevitableFatalObstacle);
                return;
            }
            else
            {
                m_InevitableFatalObstacle = null;
            }
        }


        // Get Inevitable Fatal Obstacle for next frame
        if (result.FatalObjstacle)
        {
            float obstacleMoveDelta = result.FatalObjstacle.MoveSpeedMultiplier * m_ObstacleManager.GlobalMoveSpeed * Time.deltaTime;
            if (obstacleMoveDelta > result.FatalObjstacle_DistanceToObstalce)
            {
                m_InevitableFatalObstacle = result.FatalObjstacle;             
            }
        }
    }



    private CollisionResult GetCollisionResult()
    {
        CollisionResult result = new CollisionResult();


        Obstacle[] obstacles = m_ObstacleManager.ActiveObstacles.ToArray();

        for (int i = 0; i < obstacles.Length; i++)
        {

            float outerEdgeDistance = Mathf.Max(0f, obstacles[i].Distance);
            float innerEdgeDistance = Mathf.Max(0f, obstacles[i].Distance - obstacles[i].EdgeWith);


            float angleDelta = Mathf.DeltaAngle(transform.eulerAngles.z, obstacles[i].Rotation);
            bool bInsideObstacleFill = Mathf.Abs(angleDelta) < (obstacles[i].FillAngle / 2f);


            // If player is inside obstacle fill angle
            if (!bInsideObstacleFill)
            {
                bool bBlockedByObstacle = m_PlayerDistance >= innerEdgeDistance && m_PlayerDistance <= outerEdgeDistance;
                if (bBlockedByObstacle)
                {
                    float edgeDistance = Mathf.Max(0f, Mathf.Abs(angleDelta) - (obstacles[i].FillAngle / 2f));

                    if (edgeDistance < result.EdgeObstacle_EdgeDistance || !result.EdgeObstacle)
                    {
                        result.EdgeObstacle = obstacles[i];
                        result.EdgeObstacle_AngleDelta = angleDelta;
                        result.EdgeObstacle_EdgeDistance = edgeDistance;
                    }
                }
            }

            // If player is inside fill angle
            else
            {
                float distanceToObstalce = innerEdgeDistance - m_PlayerDistance;

                if (distanceToObstalce > 0)
                {
                    if (distanceToObstalce < result.FatalObjstacle_DistanceToObstalce || !result.FatalObjstacle)
                    {
                        result.FatalObjstacle = obstacles[i];
                        result.FatalObjstacle_DistanceToObstalce = distanceToObstalce;
                    }
                }
            }
        }

        return result;
    }


    private void OnDeath(Obstacle _instigator)
    {
        m_Death = true;
        Debug.Log("PlayerDied");
        Time.timeScale = 0;
    }

    public struct CollisionResult
    {
        public Obstacle FatalObjstacle;
        public float FatalObjstacle_DistanceToObstalce;

        public Obstacle EdgeObstacle;
        public float EdgeObstacle_AngleDelta;
        public float EdgeObstacle_EdgeDistance;
    }














#if UNITY_EDITOR

    private void OnValidate()
    {
        if (m_PointerTransform)
            m_PointerTransform.position = transform.position + (Vector3.up * m_PlayerDistance);
        else
            Debug.LogWarning("Player Pointer Not Set");
    }
#endif

}







//    private void HandleRotation(CollisionResult collisionResult)
//    {

//        float rotateInput = -m_Controls.Player.Rotate.ReadValue<float>();
//        float rotateDelta = m_RotateSpeed * Time.deltaTime * rotateInput;


//        float rotateEdgeDelta = collisionResult.EdgeObstacleSide * collisionResult.EdgeObstacleAngle;


//        //Clamp rotation delta based on edge distance
//        if (collisionResult.EdgeObstacle && rotateInput != 0)
//        {
//            if (collisionResult.EdgeObstacleSide > 0)
//                rotateDelta = Mathf.Min(rotateEdgeDelta, rotateDelta);
//            else
//                rotateDelta = Mathf.Max(rotateEdgeDelta, rotateDelta);
//        }


//        transform.Rotate(Vector3.forward, rotateDelta);


//        float sway = rotateDelta == 0f ? 0f : rotateDelta > 0f ? 1f : -1f;

//        if (m_PlayerSprite)
//        {
//            Quaternion targetRotation = transform.rotation;
//            targetRotation *= Quaternion.Euler(0, 0, sway * m_RotateSwayDistance);

//            m_PlayerSprite.transform.rotation = Quaternion.Slerp(m_PlayerSprite.transform.rotation, targetRotation, m_RotateSwayDampSpeed * Time.deltaTime);
//        }
//    }


//    private void CheckDeath(CollisionResult collisionResult)
//    {

//#if UNITY_EDITOR
//        if (!m_CanDie)
//            return;
//#endif

//        if (collisionResult.FatalObjstacle)
//        {
//            // DONT FORGET TO ADD GLOBAL MOVE SPEED
//            float obstacleMoveDelta = collisionResult.FatalObjstacle.MoveSpeedMultiplier * Time.deltaTime / 2f;

//            if (collisionResult.FatalObjstacleDistance < 0f && Mathf.Abs(collisionResult.FatalObjstacleDistance) < obstacleMoveDelta)
//            {
//                Debug.Log("Death");           
//            }
//        }
//    }


//    private CollisionResult GetCollisionResult()
//    {

//        CollisionResult result = new CollisionResult();

//        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();


//        foreach (Obstacle obstacle in obstacles)
//        {

//            // Check if player is inside of ring
//            float outerEdgePoint = obstacle.Distance / 2f;
//            float innerEdgePoint = Mathf.Max(0f, outerEdgePoint - (obstacle.EdgeWith / 2f));
//            float playerPoint = m_PointerTransform.localPosition.y;

//            float outerEdgeDistanceToPlayerPoint = playerPoint - outerEdgePoint;
//            float innerEdgeDistanceToPlayerPoint = playerPoint - innerEdgePoint;

//            bool bInsideRing = innerEdgeDistanceToPlayerPoint > 0 && outerEdgeDistanceToPlayerPoint < 0;

//            float dotAwayFromPlayer = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.Rotation * 360f) / 180f);
//            float angleFromCenter = Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.Rotation * 360f);


//            float angleFromEdge = Mathf.Max(0, Mathf.Abs(angleFromCenter) - (obstacle.FillAngle * 180f));
//            bool bInsideAngle = dotAwayFromPlayer < obstacle.FillAngle;
//            int side = angleFromCenter > 0 ? 1 : -1;


//            if(bInsideRing)
//            {
//                if(result.EdgeObstacleAngle > angleFromEdge || !result.EdgeObstacle)
//                {
//                    result.EdgeObstacle = obstacle;

//                    result.EdgeObstacleSide = side;
//                    result.EdgeObstacleAngle = angleFromEdge;
//                }
//            }


//            if (bInsideAngle)
//            {
//                if (result.FatalObjstacleDistance < Mathf.Abs(innerEdgeDistanceToPlayerPoint)  || !result.FatalObjstacle)
//                {
//                    if (outerEdgeDistanceToPlayerPoint < 0)
//                    {
//                        result.FatalObjstacle = obstacle;

//                        result.FatalObjstacleDistance = innerEdgeDistanceToPlayerPoint;
//                    }
//                }
//            }
//        }

//        return result;
//    }


//    [System.Serializable]
//    public struct CollisionResult
//    {
//        public Obstacle FatalObjstacle;
//        public float FatalObjstacleDistance;

//        public Obstacle EdgeObstacle;
//        public float EdgeObstacleAngle;
//        public int EdgeObstacleSide;
//    }












//private void HandleRotation(CollisionResult collisionResult)
//{

//    float rotateInput = -m_Controls.Player.Rotate.ReadValue<float>();
//    float rotateDelta = m_RotateSpeed * Time.deltaTime * rotateInput;


//    float rotateEdgeDelta = collisionResult.EdgeObstacleSide * collisionResult.EdgeObstacleAngle;


//    //Clamp rotation delta based on edge distance
//    if (collisionResult.EdgeObstacle && rotateInput != 0)
//    {
//        if (collisionResult.EdgeObstacleSide > 0)
//            rotateDelta = Mathf.Min(rotateEdgeDelta, rotateDelta);
//        else
//            rotateDelta = Mathf.Max(rotateEdgeDelta, rotateDelta);
//    }


//    transform.Rotate(Vector3.forward, rotateDelta);


//    float sway = rotateDelta == 0f ? 0f : rotateDelta > 0f ? 1f : -1f;

//    if (m_PlayerSprite)
//    {
//        Quaternion targetRotation = transform.rotation;
//        targetRotation *= Quaternion.Euler(0, 0, sway * m_RotateSwayDistance);

//        m_PlayerSprite.transform.rotation = Quaternion.Slerp(m_PlayerSprite.transform.rotation, targetRotation, m_RotateSwayDampSpeed * Time.deltaTime);
//    }
//}


//private void CheckDeath(CollisionResult collisionResult)
//{

//#if UNITY_EDITOR
//    if (!m_CanDie)
//        return;
//#endif

//    if (collisionResult.FatalObjstacle)
//    {
//        // DONT FORGET TO ADD GLOBAL MOVE SPEED
//        float obstacleMoveDelta = collisionResult.FatalObjstacle.MoveSpeedMultiplier * Time.deltaTime / 2f;

//        if (collisionResult.FatalObjstacleDistance < 0f && Mathf.Abs(collisionResult.FatalObjstacleDistance) < obstacleMoveDelta)
//        {
//            OnDeath();
//        }
//    }
//}

//private void OnDeath()
//{
//    Debug.Log("Died");
//    m_PlayerSprite.color = Color.red;
//    Time.timeScale = 0;
//}

//private CollisionResult GetCollisionResult()
//{

//    CollisionResult result = new CollisionResult();

//    Obstacle[] obstacles = FindObjectsOfType<Obstacle>();


//    foreach (Obstacle obstacle in obstacles)
//    {

//        // Check if player is inside of ring
//        float outerEdgePoint = obstacle.Distance / 2f;
//        float innerEdgePoint = Mathf.Max(0f, outerEdgePoint - (obstacle.EdgeWith / 2f));
//        float playerPoint = m_PointerTransform.localPosition.y;

//        float outerEdgeDistanceToPlayerPoint = playerPoint - outerEdgePoint;
//        float innerEdgeDistanceToPlayerPoint = playerPoint - innerEdgePoint;

//        bool bInsideRing = innerEdgeDistanceToPlayerPoint > 0 && outerEdgeDistanceToPlayerPoint < 0;

//        float dotAwayFromPlayer = Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.Rotation * 360f) / 180f);
//        float angleFromCenter = Mathf.DeltaAngle(transform.eulerAngles.z, obstacle.Rotation * 360f);


//        float angleFromEdge = Mathf.Max(0, Mathf.Abs(angleFromCenter) - (obstacle.FillAngle * 180f));
//        bool bInsideAngle = dotAwayFromPlayer < obstacle.FillAngle;
//        int side = angleFromCenter > 0 ? 1 : -1;


//        if (bInsideRing)
//        {
//            if (result.EdgeObstacleAngle > angleFromEdge || !result.EdgeObstacle)
//            {
//                result.EdgeObstacle = obstacle;

//                result.EdgeObstacleSide = side;
//                result.EdgeObstacleAngle = angleFromEdge;
//            }
//        }


//        if (bInsideAngle)
//        {
//            if (result.FatalObjstacleDistance < Mathf.Abs(innerEdgeDistanceToPlayerPoint) || !result.FatalObjstacle)
//            {
//                if (outerEdgeDistanceToPlayerPoint < 0)
//                {
//                    result.FatalObjstacle = obstacle;

//                    result.FatalObjstacleDistance = innerEdgeDistanceToPlayerPoint;
//                }
//            }
//        }
//    }

//    return result;
//}


//[System.Serializable]
//public struct CollisionResult
//{
//    public Obstacle FatalObjstacle;
//    public float FatalObjstacleDistance;

//    public Obstacle EdgeObstacle;
//    public float EdgeObstacleAngle;
//    public int EdgeObstacleSide;
//}