using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    Canvas _canvas;
    TextMeshProUGUI _label;

    void Awake()
    {
        _canvas = gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 100;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        var bg = new GameObject("DeathScreen_BG");
        bg.transform.SetParent(_canvas.transform, false);
        var bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0f, 0f, 0f, 0.6f);
        var bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        var textGo = new GameObject("DeathScreen_Text");
        textGo.transform.SetParent(_canvas.transform, false);
        _label = textGo.AddComponent<TextMeshProUGUI>();
        _label.text = "YOU DIED";
        _label.fontSize = 72;
        _label.alignment = TextAlignmentOptions.Center;
        _label.color = new Color(0.85f, 0.1f, 0.1f);
        var textRect = textGo.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 0.4f);
        textRect.anchorMax = new Vector2(1f, 0.6f);
        textRect.sizeDelta = Vector2.zero;

        SetVisible(false);
    }

    public void SetVisible(bool visible) => _canvas.gameObject.SetActive(visible);
}
