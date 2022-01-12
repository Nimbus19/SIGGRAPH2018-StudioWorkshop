using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : MonoBehaviour
{
    bool showDetail = false;
    int fontSize = 24;
    List<Rect> locations = new List<Rect>();

    void Start()
    {
        fontSize = Screen.width / 50;

        float xAnchor = Screen.width - fontSize * 12 - 10;
        float yAnchor = 30;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
        yAnchor += fontSize * 1.5f + 10;
        locations.Add(new Rect(xAnchor, yAnchor, fontSize * 12, fontSize * 1.5f));
    }

    void OnGUI()
    {
        fontSize = Screen.width / 50;
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.textArea.fontSize = fontSize;

        if (GUI.Button(locations[0], "Config", GUI.skin.button))
        {
            showDetail = !showDetail;
        }

        if (showDetail)
        {
            if (GUI.Button(locations[1], "FPS 60", GUI.skin.button))
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 60;
            }

            if (GUI.Button(locations[2], "FPS 30", GUI.skin.button))
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 30;
            }

            if (GUI.Button(locations[3], "FPS 10", GUI.skin.button))
            {
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 10;
            }
        }

    }
}
