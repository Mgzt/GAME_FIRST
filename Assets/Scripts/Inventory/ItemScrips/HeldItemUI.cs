using UnityEngine;
using UnityEngine.UI;

public class HeldItemUI : MonoBehaviour
{
    public static HeldItemUI Instance;

    public Image icon;
    public Text countText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg != null)
            cg.blocksRaycasts = false;

        Hide();
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void Show(InventorySlot slot)
    {
        Debug.Log("SHOW CALLED");

        if (slot == null || slot.IsEmpty || slot.itemData == null)
        {
            Debug.Log("SHOW FAILED: SLOT INVALID");
            Hide();
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.itemData.icon;

        countText.text = slot.quantity > 1
            ? slot.quantity.ToString()
            : "";
    }

    public void Hide()
    {
        icon.enabled = false;
        countText.text = "";
        gameObject.SetActive(false);
    }
}
