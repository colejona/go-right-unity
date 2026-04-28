using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class StatMenuScreen : MonoBehaviour
{
    Canvas _canvas;
    TextMeshProUGUI _pointsLabel;
    StatAllocationLogic _allocation;

    readonly TextMeshProUGUI[] _pendingLabels = new TextMeshProUGUI[5];

    public event Action<StatAllocationLogic.Allocation> OnCommit;
    public event Action OnCancel;

    static readonly string[] StatNames = { "STR", "AGI", "LUK", "INT", "HP" };

    void Awake()
    {
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var esGo = new GameObject("EventSystem");
            esGo.AddComponent<EventSystem>();
            esGo.AddComponent<InputSystemUIInputModule>();
        }

        _canvas = gameObject.AddComponent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        _canvas.sortingOrder = 90;
        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        var panel = CreatePanel();
        CreateTitle(panel);
        _pointsLabel = CreateLabel(panel, "", 14, TextAlignmentOptions.Center);
        SetAnchors(_pointsLabel.rectTransform, new Vector2(0f, 0.76f), new Vector2(1f, 0.85f));
        for (int i = 0; i < 5; i++)
            _pendingLabels[i] = CreateStatRow(panel, i);
        CreateFooterButtons(panel);

        SetVisible(false);
    }

    GameObject CreatePanel()
    {
        var go = new GameObject("StatMenu_Panel");
        go.transform.SetParent(_canvas.transform, false);
        var img = go.AddComponent<Image>();
        img.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.3f, 0.2f);
        rt.anchorMax = new Vector2(0.7f, 0.8f);
        rt.sizeDelta = Vector2.zero;
        return go;
    }

    void CreateTitle(GameObject parent)
    {
        var label = CreateLabel(parent, "ALLOCATE POINTS", 20, TextAlignmentOptions.Center);
        var rt = label.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0.85f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.sizeDelta = Vector2.zero;
    }

    TextMeshProUGUI CreateStatRow(GameObject parent, int index)
    {
        float rowHeight = 0.12f;
        float top = 0.74f - index * rowHeight;

        var nameLabel = CreateLabel(parent, StatNames[index], 14, TextAlignmentOptions.Left);
        SetAnchors(nameLabel.rectTransform, new Vector2(0.05f, top - rowHeight), new Vector2(0.35f, top));

        var pendingLabel = CreateLabel(parent, "+0", 14, TextAlignmentOptions.Center);
        SetAnchors(pendingLabel.rectTransform, new Vector2(0.35f, top - rowHeight), new Vector2(0.65f, top));

        var btnMinus = CreateButton(parent, "-", () => { Decrease((StatAllocationLogic.Stat)index); });
        SetAnchors(btnMinus.GetComponent<RectTransform>(), new Vector2(0.65f, top - rowHeight), new Vector2(0.80f, top));

        var btnPlus = CreateButton(parent, "+", () => { Increase((StatAllocationLogic.Stat)index); });
        SetAnchors(btnPlus.GetComponent<RectTransform>(), new Vector2(0.82f, top - rowHeight), new Vector2(0.97f, top));

        return pendingLabel;
    }

    void CreateFooterButtons(GameObject parent)
    {
        var cancel = CreateButton(parent, "Cancel", () => OnCancel?.Invoke());
        SetAnchors(cancel.GetComponent<RectTransform>(), new Vector2(0.05f, 0.02f), new Vector2(0.45f, 0.12f));

        var ok = CreateButton(parent, "OK", () => { if (_allocation != null) OnCommit?.Invoke(_allocation.Commit()); });
        SetAnchors(ok.GetComponent<RectTransform>(), new Vector2(0.55f, 0.02f), new Vector2(0.95f, 0.12f));
    }

    static TextMeshProUGUI CreateLabel(GameObject parent, string text, float fontSize, TextAlignmentOptions alignment)
    {
        var go = new GameObject("Label");
        go.transform.SetParent(parent.transform, false);
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.color = Color.white;
        return tmp;
    }

    static Button CreateButton(GameObject parent, string label, Action onClick)
    {
        var go = new GameObject("Btn_" + label);
        go.transform.SetParent(parent.transform, false);
        var img = go.AddComponent<Image>();
        img.color = new Color(0.25f, 0.25f, 0.35f);
        var btn = go.AddComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(() => onClick());

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);
        var tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 13;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        var textRt = textGo.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.sizeDelta = Vector2.zero;

        return btn;
    }

    static void SetAnchors(RectTransform rt, Vector2 min, Vector2 max)
    {
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }

    void Increase(StatAllocationLogic.Stat stat)
    {
        if (_allocation == null) return;
        if (_allocation.Increase(stat)) Refresh();
    }

    void Decrease(StatAllocationLogic.Stat stat)
    {
        if (_allocation == null) return;
        if (_allocation.Decrease(stat)) Refresh();
    }

    void Refresh()
    {
        if (_allocation == null) return;
        _pointsLabel.text = $"Points remaining: {_allocation.PointsRemaining}";
        for (int i = 0; i < 5; i++)
        {
            int p = _allocation.Pending((StatAllocationLogic.Stat)i);
            _pendingLabels[i].text = p > 0 ? $"+{p}" : "–";
        }
    }

    public void Open(int availablePoints)
    {
        _allocation = new StatAllocationLogic(availablePoints);
        Refresh();
        SetVisible(true);
    }

    public void SetVisible(bool visible) => _canvas.gameObject.SetActive(visible);
}
