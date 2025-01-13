using UnityEngine;

public class SpriteScroller : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public float scrollSpeed = 1f; 
    public float scrollDuration = 2f;

    private Material material; 
    private float elapsedTime = 0f;
    private bool isScrolling = false;

    void Start()
    {
        material = spriteRenderer.material;
    }

    public void StartScroll()
    {
        if (!isScrolling)
        {
            elapsedTime = 0f;
            isScrolling = true;
        }
    }

    void Update()
    {
        if (isScrolling)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < scrollDuration)
            {
                float offset = elapsedTime * scrollSpeed;

                material.mainTextureOffset = new Vector2(offset, 0f);
            }
            else
            {
                isScrolling = false;

                material.mainTextureOffset = Vector2.zero;
            }
        }
    }
}
