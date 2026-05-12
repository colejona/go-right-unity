using UnityEngine;

public static class AspectRatioLogic
{
    public static Rect ComputeViewport(float targetAspect, float screenWidth, float screenHeight, Rect gameAreaInPortrait)
    {
        float screenAspect = screenWidth / screenHeight;

        float px, py, pw, ph;
        if (screenAspect > targetAspect)
        {
            pw = targetAspect / screenAspect;
            ph = 1f;
            px = (1f - pw) / 2f;
            py = 0f;
        }
        else
        {
            pw = 1f;
            ph = screenAspect / targetAspect;
            px = 0f;
            py = (1f - ph) / 2f;
        }

        return new Rect(
            px + gameAreaInPortrait.x * pw,
            py + gameAreaInPortrait.y * ph,
            gameAreaInPortrait.width * pw,
            gameAreaInPortrait.height * ph
        );
    }
}
