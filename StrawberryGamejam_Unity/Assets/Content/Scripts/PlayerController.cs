using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [FoldoutGroup("PlayerMovement"), SerializeField]
    private float m_RotateSpeed;


    [FoldoutGroup("General"), SerializeField]
    private float m_RotateSwayDistance;

    [FoldoutGroup("General"), SerializeField]
    private float m_RotateSwayDampSpeed;

    [FoldoutGroup("General"), SerializeField]
    private float m_PlayerDistance;


    [FoldoutGroup("Components"), SerializeField]
    private Transform m_PointerTransform;

    [FoldoutGroup("Components"), SerializeField]
    private SpriteRenderer m_PlayerSprite;

    [FoldoutGroup("Components"), SerializeField]
    private ObstacleManager m_ObstacleManager;



    private bool m_Death;
    private Controls m_Controls;
    private Obstacle m_InevitableFatalObstacle;



#if UNITY_EDITOR
    [FoldoutGroup("Debug"), SerializeField]
    private bool m_DInvincible;

    [FoldoutGroup("Debug"), SerializeField]
    private bool m_DShowCollision;
#endif



    private void Start()
    {
        m_Controls = new Controls();
        m_Controls.Enable();
    }


    private void Update()
    {
        CollisionResult result = GetCollisionResult();

        HandlePlayerRotation(result);
        CheckDeath(result);

#if UNITY_EDITOR

        // DEBUG
        Obstacle[] obstacles = m_ObstacleManager.ActiveObstacles.ToArray();

        // Reset 
        for (int i = 0; i < obstacles.Length; i++)
            obstacles[i].GetComponent<SpriteRenderer>().color = Color.white;


        if (result.FatalObjstacle)
            result.FatalObjstacle.GetComponent<SpriteRenderer>().color = Color.red;

        if (result.EdgeObstacle)
            result.EdgeObstacle.GetComponent<SpriteRenderer>().color = Color.green;
#endif
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




        // Get Rotate Sway Direction
        float rotateSway = rotateDelta == 0f ? 0f : rotateDelta > 0f ? 1f : -1f;

        if (m_PlayerSprite)
        {
            Quaternion targetRotation = transform.rotation;
            targetRotation *= Quaternion.Euler(0, 0, rotateSway * m_RotateSwayDistance);

            m_PlayerSprite.transform.rotation = Quaternion.Slerp(m_PlayerSprite.transform.rotation, targetRotation, m_RotateSwayDampSpeed * Time.deltaTime);
        }
    }

    private void CheckDeath(CollisionResult result)
    {


#if UNITY_EDITOR
        if (m_DInvincible)
            return;
#endif

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
                float offsetDelta = m_InevitableFatalObstacle.Distance - m_InevitableFatalObstacle.EdgeWith - m_PlayerDistance;
                m_ObstacleManager.MoveObstacles(Mathf.Max(0f,-offsetDelta));

                OnDeath(m_InevitableFatalObstacle);
                return;
            }
            else
            {
                Debug.Log("Death Escaped");
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


    private void OnDeath(Obstacle _instigator)
    {
        m_Death = true;
        Time.timeScale = 0;
    }


    private CollisionResult GetCollisionResult()
    {
        CollisionResult result = new CollisionResult();


        Obstacle[] obstacles = m_ObstacleManager.ActiveObstacles.ToArray();

        for (int i = 0; i < obstacles.Length; i++)
        {
            // Global Input Data \\
            float outerEdgeDistance = Mathf.Max(0f, obstacles[i].Distance);
            float innerEdgeDistance = Mathf.Max(0f, obstacles[i].Distance - obstacles[i].EdgeWith);
            float angleDelta = Mathf.DeltaAngle(transform.eulerAngles.z, obstacles[i].Rotation);


            // Get Fatal Obstacle \\

            bool bInsideObstacleFill = Mathf.Abs(angleDelta) < (obstacles[i].FillAngle / 2f);

            if (bInsideObstacleFill)
            {
                float distanceToObstalce = innerEdgeDistance - m_PlayerDistance;

                if (distanceToObstalce > 0)
                {
                    if (distanceToObstalce < result.FatalObjstacle_DistanceToObstalce || !result.FatalObjstacle)
                    {
                        result.FatalObjstacle = obstacles[i];
                        result.FatalObjstacle_DistanceToObstalce = distanceToObstalce;
                        continue;
                    }
                }
            }



            // Get Edge Obstacle \\

            bool bBlockedByObstacle = m_PlayerDistance >= innerEdgeDistance && m_PlayerDistance <= outerEdgeDistance;

            if (bBlockedByObstacle)
            {
                float edgeDistance = Mathf.Max(0f, Mathf.Abs(angleDelta) - (obstacles[i].FillAngle / 2f));

                if (edgeDistance < result.EdgeObstacle_EdgeDistance || !result.EdgeObstacle)
                {
                    result.EdgeObstacle = obstacles[i];
                    result.EdgeObstacle_AngleDelta = angleDelta;
                    result.EdgeObstacle_EdgeDistance = edgeDistance;
                    continue;
                }
            }

        }

        return result;
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
