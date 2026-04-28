public static class HpBarLogic
{
    public static float FillFraction(int currentHp, int maxHp)
    {
        float t = (float)currentHp / maxHp;
        return t < 0f ? 0f : t > 1f ? 1f : t;
    }
}
