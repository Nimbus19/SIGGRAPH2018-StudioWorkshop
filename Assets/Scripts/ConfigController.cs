using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ConfigController : MonoBehaviour
{
    bool showMenu = false;
    bool showSysInfo = false;
    int fontSize = 24;
    int fpsSelIndex = 0;
    string sysInfo;
    readonly Dictionary<string, Rect> locations = new Dictionary<string, Rect>();

    void OnEnable()
    {

    }

    void Start()
    {
        setUILoactions();
        getSystemInfo();

    }

    private void setUILoactions()
    {
        fontSize = Screen.width / 50;

        float xAnchor = Screen.width - fontSize * 12 - 10;
        float yAnchor = 30;

        locations.Add("Config", new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;

        locations.Add("FPS", new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;

        locations.Add("Fullscreen", new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;

        locations.Add("SysInfo", new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        locations.Add("SysInfoDetail", new Rect(fontSize * 15, 30, fontSize * 20, fontSize * 20));
    }

    private void getSystemInfo()
    {
        var sb = new StringBuilder(500);
        sb.AppendLine($"Device: {SystemInfo.deviceName}");
        sb.AppendLine($"OS: {SystemInfo.operatingSystem}");
        sb.AppendLine($"CPU: {SystemInfo.processorType}");
        sb.AppendLine($"GPU: {SystemInfo.graphicsDeviceVendor} {SystemInfo.graphicsDeviceName}");
        sb.AppendLine($"GPU Memory: {SystemInfo.graphicsMemorySize} MB");
        sb.AppendLine($"GPU API: {SystemInfo.graphicsDeviceVersion}");
        sb.AppendLine($"GPU Multi-Threaded: {SystemInfo.graphicsMultiThreaded}");
        sb.AppendLine($"GPU Shader Level: {SystemInfo.graphicsShaderLevel}");

        Resolution[] resolutions = Screen.resolutions;
        var res = resolutions[resolutions.Length - 1];
        sb.AppendLine($"Res: {res.width}x{res.height}@{res.refreshRate}");
        sb.AppendLine($"Window: {Screen.width}x{Screen.height}");

        sysInfo = sb.ToString();
    }


    void OnGUI()
    {
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.textArea.fontSize = fontSize;

        if (GUI.Button(locations["Config"], "Config", GUI.skin.button))
        {
            showMenu = !showMenu;
        }

        if (showMenu)
        {
            string[] selStrings = new string[] { "FPS", "60", "30", "10" };
            switch (GUI.SelectionGrid(locations["FPS"], fpsSelIndex, selStrings, 4, GUI.skin.button))
            {
                case 1:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = 60;
                    fpsSelIndex = 1;
                    break;
                case 2:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = 30;
                    fpsSelIndex = 2;
                    break;
                case 3:
                    QualitySettings.vSyncCount = 0;
                    Application.targetFrameRate = 10;
                    fpsSelIndex = 3;
                    break;
                default:
                    QualitySettings.vSyncCount = 1;
                    Application.targetFrameRate = -1;
                    fpsSelIndex = 0;
                    break;
            }

            if (GUI.Button(locations["Fullscreen"], "Fullscreen", GUI.skin.button))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }

            if (GUI.Button(locations["SysInfo"], "SysInfo", GUI.skin.button))
            {
                showSysInfo = !showSysInfo;
            }

            if (showSysInfo)
            {
                GUI.Label(locations["SysInfoDetail"], sysInfo, GUI.skin.textArea);
            }

        }

    }
}
