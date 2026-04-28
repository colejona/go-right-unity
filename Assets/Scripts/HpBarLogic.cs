using UnityEngine;

public static class HpBarLogic
{
    public static readonly Color ColorHealthy  = new Color(0.2f,  0.8f,  0.2f);
    public static readonly Color ColorCaution  = new Color(0.9f,  0.75f, 0.1f);
    public static readonly Color ColorCritical = new Color(0.85f, 0.15f, 0.15f);

    public static float FillFraction(int currentHp, int maxHp)
    {
        float t = (float)currentHp / maxHp;
        return t < 0f ? 0f : t > 1f ? 1f : t;
    }

    public static Color BarColor(float fraction)
    {
        if (fraction >= 0.75f) return ColorHealthy;
        if (fraction >= 0.25f) return ColorCaution;
        return ColorCritical;
    }
}
