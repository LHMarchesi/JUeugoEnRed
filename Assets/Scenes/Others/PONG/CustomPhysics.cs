using System.Collections;
using UnityEngine;

public static class CustomPhysics 
{
    public static Vector2 collisionPoint;
    public static Vector2 collidedSide;
    public static void MoveTransform(Transform transform, float speed, Vector2 dir)
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public static bool RectangleCollision(Collider collider1, Collider collider2)
    {
        if (collider1.bounds.min.y < collider2.bounds.max.y &&
            collider2.bounds.min.y < collider1.bounds.max.y &&
            collider1.bounds.min.y < collider2.bounds.max.y &&
            collider2.bounds.min.y < collider1.bounds.max.y)
            return true;

        return false;
    }
    public static bool CircleRectangleCollision(Collider rectangle, SphereCollider circle)
    {
        Vector2 closestPoint = circle.transform.position;

        if (closestPoint.x < rectangle.bounds.min.x) closestPoint.x = rectangle.bounds.min.x;
        if (closestPoint.x > rectangle.bounds.max.x) closestPoint.x = rectangle.bounds.max.x;
        if (closestPoint.y < rectangle.bounds.min.y) closestPoint.y = rectangle.bounds.min.y;
        if (closestPoint.y > rectangle.bounds.max.y) closestPoint.y = rectangle.bounds.max.y;

        if (closestPoint.x < 0) collidedSide.x = 1;
        if (closestPoint.x > 0) collidedSide.x = -1;
        if (closestPoint.y < 0) collidedSide.y = 1;
        if (closestPoint.y > 0) collidedSide.y = -1;

        collisionPoint = closestPoint;
        return Vector2.Distance(closestPoint, circle.transform.position) < circle.radius;
    }

    public static bool CircleCollision(SphereCollider circle1, SphereCollider circle2)
    {
        return Vector2.Distance(circle1.transform.position, circle2.transform.position) < circle1.radius * circle1.transform.lossyScale.x + circle2.radius * circle2.transform.lossyScale.x;
    }
}
