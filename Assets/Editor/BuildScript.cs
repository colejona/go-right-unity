using UnityEditor;

public static class BuildScript
{
    [MenuItem("Build/WebGL")]
    public static void BuildWebGL()
    {
        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = "Builds/WebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None,
        });
    }
}
