using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector2 dir;
    [SerializeField] Collider player;
    [SerializeField] SphereCollider self;
    public Transform spawn;
    [SerializeField] bool launch;
    public int key;
    

    private void Awake()
    {
        initialize();
    }
    public void initialize()
    {
        GameManager.balls.Add(this);
        transform.position = spawn.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        CustomPhysics.MoveTransform(transform, speed, dir);
        if(launch)
            transform.position = spawn.position;
        
    }

    public void PlayerChangeDir(Collider col, float maxangle)
    {
        float width = col.bounds.size.x;
        float relativeCollsionPoint = (transform.position.x - GameManager.player.transform.position.x) / (width / 2);
        relativeCollsionPoint = Mathf.Clamp(relativeCollsionPoint, -1, 1);

        float angle = relativeCollsionPoint * maxangle;

        float angleinradians = angle * Mathf.Deg2Rad;
        Vector2 bounceDirection = new Vector2(Mathf.Sin(angleinradians), Mathf.Cos(angleinradians));

        dir = bounceDirection.normalized;
    }


    public void SimpleChangeDir( float x, float y)
    {
        
        Vector2 direction = (CustomPhysics.collisionPoint - (Vector2)transform.position).normalized;

       
        if(direction.x != 0)
        {
            direction.x *= x;
            dir.y *= y;
            dir = new Vector2(direction.x, dir.y);
        }
        if (direction.y != 0) 
        {
            direction.y *= y;
            dir.x *= x;
            dir = new Vector2(dir.x, direction.y);
        }

      
    }
    public void resetPosition(Transform spawn)
    {
        launch = true;
        transform.position = spawn.position;
        dir = new Vector2(0, 0);
    }
    public void resetPositionWithoutLaunch(Transform pos)
    {
        transform.position = pos.position;
        dir = new Vector2(0, 0);
        
    }
    public void launchBall(Vector2 direction)
    {
        launch = false;
        dir = direction;
    }
   
    public SphereCollider BallCollider => self;
    public bool Launch => launch;
}
