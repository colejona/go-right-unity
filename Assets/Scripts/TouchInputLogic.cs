public static class TouchInputLogic
{
    public static int GetDirection(bool hasTouch, float touchX, float screenWidth)
    {
        if (!hasTouch) return 0;
        return touchX < screenWidth / 2f ? -1 : 1;
    }
}
