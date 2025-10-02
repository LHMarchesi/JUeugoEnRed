using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode launch;
    [SerializeField] float speed;
    [SerializeField] Vector2 dir;
    [SerializeField] Collider leftWall;
    [SerializeField] Collider rightWall;
    [SerializeField] float maxbounceangle;
    [SerializeField] Collider self;
    [SerializeField] Transform resetPosition;
    bool canmove;
    private void Awake()
    {
        canmove = false;
        GameManager.player = this;
    }
    

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < GameManager.balls.Count; i++)
        {
            keyPressed(GameManager.balls[i]);
            if (CustomPhysics.CircleRectangleCollision(self, GameManager.balls[i].BallCollider))
            {
                
                GameManager.balls[i].PlayerChangeDir(self, maxbounceangle);
                
            }
        }
        
            
    }

    void move(float speed, Vector2 dir)
    {
        CustomPhysics.MoveTransform(transform, speed, dir);
    }

    void keyPressed(ball ball)
    {
        float x = 0;
        if (!canmove && Input.GetKey(right))
        {
            x = 1;
        }
        if (!canmove && Input.GetKey(left))
        {
            x = -1;
        }
        if (canmove && Input.GetKey(left) && !CustomPhysics.RectangleCollision(leftWall, self)) 
        {
            move(-speed, dir);
            
        }

        if (canmove && Input.GetKey(right) && !CustomPhysics.RectangleCollision(rightWall, self)) 
        {
            move(speed, dir);
        }
        if (Input.GetKey(launch) && ball.Launch)
        {
            ball.launchBall(new Vector2(x, 1));
            canmove = true;
        }
        
        
    }

    public void ResetPosition()
    {
        transform.position = resetPosition.position;
        canmove = false;
    }


    public Collider PlayerCollider => self;
}
