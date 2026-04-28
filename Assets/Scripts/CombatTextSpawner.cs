using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatTextSpawner : MonoBehaviour
{
    [SerializeField] float duration = 0.8f;
    [SerializeField] float floatDistance = 1.2f;
    [SerializeField] float fontSize = 6f;

    readonly List<(TextMeshPro tmp, FloatingTextLogic logic, Vector3 spawnPos)> _active = new();

    public void Show(Vector3 worldPos, int amount)
    {
        var go = new GameObject("CombatText");
        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = amount.ToString();
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.sortingOrder = 10;
        go.transform.position = worldPos;

        _active.Add((tmp, new FloatingTextLogic(duration, floatDistance), worldPos));
    }

    void Update()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var (tmp, logic, spawnPos) = _active[i];
            logic.Tick(Time.deltaTime);

            tmp.transform.position = new Vector3(spawnPos.x, spawnPos.y + logic.YOffset, spawnPos.z);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, logic.Alpha);

            if (logic.IsExpired)
            {
                Destroy(tmp.gameObject);
                _active.RemoveAt(i);
            }
        }
    }
}
