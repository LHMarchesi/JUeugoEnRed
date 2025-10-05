using UnityEngine;

public class PongWall : MonoBehaviour
{
    public Collider self;
    public SphereCollider ballColl;
    public Ball ballScript;
    public float x;
    public float y;


    private void Update()
    {
        if (CustomPhysics.CircleRectangleCollision(self, ballColl)) 
            ballScript.SimpleChangeDir(x, y);
    }
}
