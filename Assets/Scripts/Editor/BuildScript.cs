using UnityEditor;

public class BuildScript
{	
	static string[] scenes = { "Assets/Scenes/MenuScene.unity", "Assets/Scenes/GameScene.unity" };
	
	public static void PerformBuildWindows()
    {
        BuildPipeline.BuildPlayer(scenes, "./builds/Windows/wildVinland.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
	
	public static void PerformBuildLinux()
    {
        BuildPipeline.BuildPlayer(scenes, "./builds/Linux/wildVinland", BuildTarget.StandaloneLinux64, BuildOptions.None);
    }
	
	 public static void PerformBuildWebGL()
    {
		BuildPipeline.BuildPlayer(scenes, "./builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
    }
	
	public static void PerformBuildMac()
    {
        BuildPipeline.BuildPlayer(scenes, "./builds/Mac/wildVinland.app", BuildTarget.StandaloneOSX, BuildOptions.None);
    }
	
}
