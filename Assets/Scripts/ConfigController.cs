using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : MonoBehaviour
{
    bool showDetail = false;
    int fontSize = 24;
    readonly List<Rect> locations = new List<Rect>();

    int fpsSelIndex = 0;

    void Start()
    {
        fontSize = Screen.width / 50;

        float xAnchor = Screen.width - fontSize * 12 - 10;
        float yAnchor = 30;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.textArea.fontSize = fontSize;

        if (GUI.Button(locations[0], "Config", GUI.skin.button))
        {
            showDetail = !showDetail;
        }

        if (showDetail)
        {
            string[] selStrings = new string[] { "FPS", "60", "30", "10" };
            switch (GUI.SelectionGrid(locations[1], fpsSelIndex, selStrings, 4))
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
                    break;
            }

            
        }

    }
}
