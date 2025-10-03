using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public SphereCollider selfColl;
    public Collider[] players;
    public Transform spawnpoint;


    private void Update()
    {
        CustomPhysics.MoveTransform(transform, speed, dir);

        for (int i = 0; i < players.Length; i++)
        {
            CustomPhysics.CircleRectangleCollision(players[i], selfColl);
        }
    }

    public void PlayerChangeDir(Collider col, float maxAngle)
    {
        float height = col.bounds.size.y;
        Pallet palletCollide = col.gameObject.GetComponent<Pallet>();
        float relativeCollisionPoint = (transform.position.y - palletCollide.transform.position.y) / (height / 2);
        relativeCollisionPoint = Mathf.Clamp(relativeCollisionPoint, -1, 1);

        float angle = relativeCollisionPoint = maxAngle;

        float angleRadiands = angle * Mathf.Deg2Rad;
        Vector2 bounceDir = new Vector2(Mathf.Sin(angleRadiands), Mathf.Cos(angleRadiands));

        dir = bounceDir.normalized;
    }

    public void SimpleChangeDir(float x, float y)
    {
        Vector2 direction = (CustomPhysics.collisionPoint - (Vector2)transform.position).normalized;

        if(direction.x != 0)
        {
            direction.x *= x;
            dir.y *= y;
            dir = new Vector2(direction.x , dir.y);
        }
        if(direction.y != 0)
        {
            direction.y *= y;
            dir.x *= x;
            dir = new Vector2(dir.x , direction.y);
        }
    }
}
