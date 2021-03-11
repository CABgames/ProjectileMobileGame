/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: Prediction.cs 
//Author: Charlie Bullock, based upon and using help from CT5009 intro to Unity physics tutorial
///Description: Prediction will predict the cannon projectile paths
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prediction : MonoBehaviour
{
    //Variables
    [SerializeField]
    private float fTimeToPredict;
    private Rigidbody2D rigidbody2D;
    private LineRenderer lineRenderer;

    //Start function getting components and setting line renderer
    void Start()
    {
        //Get the components 
        rigidbody2D = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        //Reset the line renderer
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    //In this function the launch arc for projectiles is predicted and then sent to the line renderer
    public void Predict(Vector2 inputVelocity)
    {
        //Get the proposed timestep (or how long for each simulation)
        float fTimeStep = (Time.fixedDeltaTime / 0.5f);
        //Our new predicted positions starts here... (or at the current transform position)
        Vector3 predictedPosition = transform.position;
        //Add gravity by time to velocity each frame
        Vector3 gravity = Physics.gravity * fTimeStep; 
        //Casts vector2 to vector3 for 3d positions
        Vector3 velocity = inputVelocity; 
        //To score positions (we can use array but in suggested tasks we will not know it's length)
        List<Vector3> positons = new List<Vector3>();
        for (float t = fTimeStep; t <= fTimeToPredict; t += fTimeStep)
        {
            //Add gravity
            velocity += gravity;
            //Add velocity based on time
            predictedPosition += velocity * fTimeStep;
            //Add the predicted position
            positons.Add(predictedPosition);
        }
        //Convert to array for the line renderer
        Vector3[] positionsArray = positons.ToArray();
        lineRenderer.positionCount = positionsArray.Length;
        lineRenderer.SetPositions(positionsArray);
    }
}
