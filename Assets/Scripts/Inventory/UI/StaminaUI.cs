using UnityEngine;
using UnityEngine.UI;

public class StaminaSpriteUI : MonoBehaviour
{
    public Image staminaImage;
    public Sprite[] staminaSprites; // size = 6 (0→5)
    PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.Instance;
    }

    void Update()
    {
        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (stats == null) return;

        int index = Mathf.RoundToInt(
            (float)stats.stamina / stats.maxStamina * (staminaSprites.Length - 1)
        );

        index = Mathf.Clamp(index, 0, staminaSprites.Length - 1);
        staminaImage.sprite = staminaSprites[index];
    }
}
