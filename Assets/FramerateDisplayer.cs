using UnityEngine;

public class FramerateDisplayer : MonoBehaviour
{
    float deltaTime = 0.0f;
    float timeStamp = 0.0f;
    //Font font;

    float framesavtick = 0;
    float framesav = 0.0f;

    int frameCount = 0;
    private void Start()
    {
        timeStamp = Time.time;
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

    }

    public void ClearFPSAverage()
    {
        timeStamp = Time.time;
        frameCount = 0;
        framesav = 0.0f;
        framesavtick = 0;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle guiFPS = new GUIStyle();

        Rect rect = new Rect(10, h * 1 / 40, w, h * 2 / 100);
        guiFPS.alignment = TextAnchor.UpperLeft;
        guiFPS.fontSize = h * 1 / 30;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        if (fps < 15)
        {
            guiFPS.normal.textColor = new Color(0.8679245f, 0.0f, 0f, 1.0f);
        }
        else if (fps < 29)
        {
            guiFPS.normal.textColor = new Color(0.8666667f, 0.4943416f, 0f, 1.0f);
        }
        else if (fps < 60)
        {
            guiFPS.normal.textColor = new Color(0.7581673f, 0.8666667f, 0f, 1.0f);
        }
        else
        {
            guiFPS.normal.textColor = new Color(0.4881453f, 0.8666667f, 0f, 1.0f);
        }
        string fpsavg = "Calculating...";

        if ((Time.time - timeStamp) > 5 && frameCount > 50) // ignore 50 first frame, calculate avg on 50 frames min
        {
            ++framesavtick;
            framesav += fps;
            float fpsav = framesav / framesavtick;
            if (frameCount > 100)
            {
                fpsavg = fpsav.ToString();
            }
        }

        string text = string.Format("FPS : {0:0.} ({1:0.0} ms)", fps, msec);
        //string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, guiFPS);


        GUIStyle guiMem = new GUIStyle();
        Rect rect2 = new Rect(10, h * 1 / 15, w, h * 2 / 100);
        guiMem.alignment = TextAnchor.UpperLeft;
        guiMem.fontSize = h * 1 / 40;
        guiMem.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        //string textMem = string.Format("Average FPS : {0}\nTotal Allocated Memory : {1} MB\nTotal Reserved Memory : {2} MB", fpsavg, Profiler.GetTotalAllocatedMemoryLong() / 1000000, Profiler.GetTotalReservedMemoryLong() / 1000000);
        string textMem = string.Format("Average FPS : {0}\nTotal Allocated Memory : {1} MB", fpsavg, UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1000000);
        GUI.Label(rect2, textMem, guiMem);
        frameCount++;
    }
}
