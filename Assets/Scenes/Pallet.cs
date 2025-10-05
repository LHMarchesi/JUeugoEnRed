using UnityEngine;

public class Pallet : MonoBehaviour
{
    public KeyCode Up;
    public KeyCode Down;
    public float speed;
    public Vector2 direction;
    public Collider bottnWall;
    public Collider topWall;
    public Collider selfColl;

    private void Update()
    {
        KeyPress();
    }
    void KeyPress()
    {
        if (Input.GetKey(Up) && !CustomPhysics.RectangleCollision(topWall, selfColl))
        {
            Move(speed, direction);
        }
        if (Input.GetKey(Down) && !CustomPhysics.RectangleCollision(bottnWall, selfColl))
        {
            Move(-speed, direction);
        }
    }

    private void Move(float speed, Vector2 dir)
    {
        CustomPhysics.MoveTransform(transform, speed, dir);
    }
}
