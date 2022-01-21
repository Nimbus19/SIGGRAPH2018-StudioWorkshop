using System.IO;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

public class ProfilerController : MonoBehaviour
{ 
    bool showMenu = true;
    int fontSize = 24;
    List<Rect> locations = new List<Rect>();

    string statsText;

    // Internal
    ProfilerRecorder mainThreadTimeRecorder;
    ProfilerRecorder vSyncWaitForFPSRecorder;
    ProfilerRecorder gfxPresentFrameRecorder;

    // Memory
    ProfilerRecorder systemMemoryRecorder;
    ProfilerRecorder gcMemoryRecorder;  
    ProfilerRecorder gfxMemoryRecorder;    

    // Render
    ProfilerRecorder drawCallsCountRecorder;
    ProfilerRecorder setPassCallsCountRecorder;
    ProfilerRecorder verticesCountRecorder;

    internal struct StatInfo
    {
        public ProfilerCategory Cat;
        public string Name;
        public ProfilerMarkerDataUnit Unit;
    }

    static private void EnumerateProfilerStats()
    {
        var availableStatHandles = new List<ProfilerRecorderHandle>();
        ProfilerRecorderHandle.GetAvailable(availableStatHandles);

        var availableStats = new List<StatInfo>(availableStatHandles.Count);
        foreach (var h in availableStatHandles)
        {
            var statDesc = ProfilerRecorderHandle.GetDescription(h);
            var statInfo = new StatInfo()
            {
                Cat = statDesc.Category,
                Name = statDesc.Name,
                Unit = statDesc.UnitType
            };
            availableStats.Add(statInfo);
        }
        availableStats.Sort((a, b) =>
        {
            var result = string.Compare(a.Cat.ToString(), b.Cat.ToString());
            if (result != 0)
                return result;

            return string.Compare(a.Name, b.Name);
        });

        var sb = new StringBuilder("Available stats:\n");
        foreach (var s in availableStats)
        {
            sb.AppendLine($"{s.Cat.Name}({(int)s.Cat})\t\t - {s.Name}\t\t - {s.Unit}");
        }

        Debug.Log(sb.ToString());
        //File.WriteAllText("ProfilerStats.txt", sb.ToString());
    }

    private double GetRecorderFrameAverage(ProfilerRecorder recorder)
    {
        var samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;

        var samples = new List<ProfilerRecorderSample>(samplesCount);
        recorder.CopyTo(samples);
        for (var i = 0; i < samples.Count; ++i)
            r += samples[i].Value;
        r /= samplesCount;

        return r;
    }    

    void OnEnable()
    {
        mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
        vSyncWaitForFPSRecorder = ProfilerRecorder.StartNew(new ProfilerCategory("VSync"), "WaitForTargetFPS", 15);
        gfxPresentFrameRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Gfx.PresentFrame", 15);
        systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        gfxMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory");
        drawCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        setPassCallsCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
        verticesCountRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

        //EnumerateProfilerStats();
    }

    void OnDisable()
    {
        mainThreadTimeRecorder.Dispose();
        vSyncWaitForFPSRecorder.Dispose();
        gfxPresentFrameRecorder.Dispose();
        systemMemoryRecorder.Dispose();
        gcMemoryRecorder.Dispose();
        gfxMemoryRecorder.Dispose();
        drawCallsCountRecorder.Dispose();
        setPassCallsCountRecorder.Dispose();
        verticesCountRecorder.Dispose();
    }

    void Update()
    {
        var sb = new StringBuilder(500);
        sb.AppendLine($"Frame Time: {GetRecorderFrameAverage(mainThreadTimeRecorder) * (1e-6f):F1} ms");
        sb.AppendLine($"vSync Time: {GetRecorderFrameAverage(vSyncWaitForFPSRecorder) * (1e-6f):F1} ms");
        sb.AppendLine($"Present Time: {GetRecorderFrameAverage(gfxPresentFrameRecorder) * (1e-6f):F1} ms");
        sb.AppendLine($"GC Memory: {gcMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"System Memory: {systemMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"GFX Memory: {gfxMemoryRecorder.LastValue / (1024 * 1024)} MB");
        sb.AppendLine($"Draw Calls: {drawCallsCountRecorder.LastValue}");
        sb.AppendLine($"SetPass Calls: {setPassCallsCountRecorder.LastValue}");
        sb.AppendLine($"Vertices: {verticesCountRecorder.LastValue}");

        statsText = sb.ToString();
    }

    void Start()
    {
        fontSize = Screen.width / 50;
        locations.Add(new Rect(10, 30, fontSize * 12, fontSize * 1.5f));
        locations.Add(new Rect(10, fontSize * 1.5f + 30, fontSize * 12, fontSize * 15));
    }

    void OnGUI()
    {
        GUI.skin.button.fontSize = fontSize;
        GUI.skin.textArea.fontSize = fontSize;

        if (GUI.Button(locations[0], "Profiler", GUI.skin.button))
        {
            showMenu = !showMenu;
        }

        if (showMenu)
        {
            GUI.Label(locations[1], statsText, GUI.skin.textArea);
        }

    }

}
