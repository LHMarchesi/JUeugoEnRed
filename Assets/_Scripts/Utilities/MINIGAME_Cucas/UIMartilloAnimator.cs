using System.Collections.Generic;
using UnityEngine;

public class UIMartilloAnimator : MonoBehaviour
{
    public static UIMartilloAnimator Instance;

  //  public GameObject martilloPrefab;

    private void Awake() => Instance = this;

    public void HitGround(int actor, Vector2 pos)
    {
      //  SpawnMartillo(actor, pos);
        // partículas de piso
        Debug.Log("Hit ground at " + pos);
    }

    public void HitCucaracha(int actor, Vector2 pos)
    {
        // SpawnMartillo(actor, pos);
        Debug.Log("Hit Cuca at " + pos);

        // animación de aplastar
    }

    public void HitSimultaneous(List<int> actors, Vector2 pos)
    {
        // foreach (int actor in actors)
        //   SpawnMartillo(actor, pos);
        Debug.Log("Hit Simultanous at " + pos);
        // animación de martillos chocando
    }

    private void SpawnMartillo(int actor, Vector2 pos)
    {
     //   GameObject m = Instantiate(martilloPrefab, transform);
      //  m.GetComponent<RectTransform>().anchoredPosition = pos;
        // reproducir animación diferente según el jugador
    }
}