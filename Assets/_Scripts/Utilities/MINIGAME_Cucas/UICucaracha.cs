using UnityEngine;

public class UICucaracha : MonoBehaviour
{
    public int ID;
    public bool IsAlive = true;
    public RectTransform rect;

    void Awake() => rect = GetComponent<RectTransform>();

    public void Kill(int actor)
    {
        if (!IsAlive) return;
        IsAlive = false;
        gameObject.SetActive(false);
        // aquí podés sumar puntos al jugador actor
    }

    public void KillSimultaneous()
    {
        if (!IsAlive) return;
        IsAlive = false;
        gameObject.SetActive(false);
        // puntos iguales o animación especial
    }

}
