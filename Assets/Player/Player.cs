using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Texture2D PlayerBoarder;
    public Texture2D PlayerSprite;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = InputManager.MoveAxis * 5;
        float Degree = RotationHelper.VectorToDeg(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Degree > 180)
        {
            Degree -= 180;
            Degree = 180 - Degree;
            Degree *= -1;
        }
        int Offset = Mathf.RoundToInt(Degree / 11.25f);
        Texture2D RenderedSprite = new Texture2D(PlayerBoarder.width, PlayerBoarder.height);
        for (int w = 0; w < PlayerBoarder.width; w++)
        {
            for (int h = 0; h < PlayerBoarder.height; h++)
            {
                RenderedSprite.SetPixel(w, h, PlayerBoarder.GetPixel(w, h));
            }
        }
        for (int w = 0; w < PlayerBoarder.width; w++)
        {
            for (int h = 0; h < PlayerBoarder.height; h++)
            {
                if (PlayerBoarder.GetPixel(w, h) == Color.white)
                {
                    if (w - Offset < 0)
                    {
                        RenderedSprite.SetPixel(w, h, PlayerSprite.GetPixel(w - Offset + PlayerSprite.width, h));
                    }
                    else if (w - Offset >= PlayerSprite.width)
                    {
                        RenderedSprite.SetPixel(w, h, PlayerSprite.GetPixel(w - Offset - PlayerSprite.width, h));
                    }
                    else
                    {
                        RenderedSprite.SetPixel(w, h, PlayerSprite.GetPixel(w - Offset, h));
                    }
                }
            }
        }
        RenderedSprite.filterMode = FilterMode.Point;
        RenderedSprite.Apply();
        sr.sprite = Sprite.Create(RenderedSprite, new Rect(0, 0, RenderedSprite.width, RenderedSprite.height), new Vector2(0.5f, 0.5f), Mathf.Min(RenderedSprite.width, RenderedSprite.height));
        sr.sprite.name = "RenderedSprite";
    }
}