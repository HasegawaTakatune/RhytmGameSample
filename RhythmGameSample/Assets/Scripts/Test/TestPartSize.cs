using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPartSize : MonoBehaviour
{
    public Vector2 baseSize = new Vector2(0, 0);
    public Vector2 size = new Vector2(1, 1);

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        baseSize = renderer.bounds.size * 0.5f;

        renderer.size = size;
        collider.size = size;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y - ((baseSize.y * size.y) - baseSize.y));
        transform.position = pos;
    }
}
