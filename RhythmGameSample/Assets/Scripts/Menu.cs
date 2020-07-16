using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private readonly Vector3 MAX_POS = new Vector3(36.6f, 2, 0);
    private readonly Vector3 MIN_POS = new Vector3(29.6f, 2, 0);
    private const float speed = 0.01f;

    private bool isDo = false;
    private bool isShow = false;

    public void OnClickMenuButton()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (isDo) yield break;


        float moved = 0;
        Vector3 endPos = (isShow) ? MAX_POS : MIN_POS;
        isDo = true;

        while (Vector2.Distance(endPos, transform.position) != 0)
        {
            yield return null;
            moved += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(endPos, transform.position, moved);
        }

        isShow = !isShow;
        isDo = false;
    }

}
