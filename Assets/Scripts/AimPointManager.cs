using UnityEngine;
using UnityEngine.UI;

public class AimPointManager : MonoBehaviour
{
    [SerializeField] private Vector2 nonSpriteSize, spriteSize;
    
    private Image image;
    private RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (image.sprite == null) {
            rectTransform.sizeDelta = nonSpriteSize;
        } else {
            rectTransform.sizeDelta = spriteSize;
        }
    }
}
