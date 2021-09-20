using SCKRM.Window;
using UnityEngine;

public class windowMoveTest : MonoBehaviour
{
    int i = 6;
    bool b = false;
    float x = 0;
    float y = 0;
    float rotation = 0;

    bool enable = false;

    void Start()
    {
        x = Screen.currentResolution.width * 0.5f;
        y = 0;
    }

    void Update()
    {
        /*if (RhythmManager.CurrentBeat > NextBeat && b)
        {
            NextBeat += 1;
            x += x;
            b = false;
        }
        else if (RhythmManager.CurrentBeat > NextBeat && !b)
        {
            NextBeat += 1;
            x += x;
            b = true;
        }
        
        x = Mathf.Lerp(x, 100, 0.15f * 60 * Time.deltaTime);

        if (!Input.GetKey(KeyCode.Space))
        {
            if (b)
                WindowManager.SetWindowPosition(x, y, WindowManager.datumPoint.LeftCenter, WindowManager.datumPoint.LeftCenter);
            else
                WindowManager.SetWindowPosition(-x, y, WindowManager.datumPoint.RightCenter, WindowManager.datumPoint.RightCenter);
        }*/

        if (Input.GetKeyDown(KeyCode.LeftControl))
            enable = !enable;

        if (enable)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                x = 0;
                y = 0;
                i++;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
                b = !b;

            if (i == 0)
            {
                rotation -= 0.025f * 60 * Time.deltaTime;
                x = Mathf.Sin(rotation) * 150;
                y = Mathf.Cos(rotation) * 150;

                WindowManager.SetWindowRect(new Rect(x, y, Screen.currentResolution.width / 1.5f, Screen.currentResolution.height / 1.5f), Vector2.one * 0.5f, Vector2.one * 0.5f, true, b);
            }
            else if (i == 1)
            {
                x += 20 * 60 * Time.deltaTime;
                y = 0;
                Vector2 pos = WindowManager.GetWindowPos(new Vector2(0, 0.5f), new Vector2(0, 0.5f));

                if (pos.x > Screen.currentResolution.width)
                    x = 0;

                WindowManager.SetWindowRect(new Rect(x, y, Screen.currentResolution.width / 1.5f, Screen.currentResolution.height / 1.5f), new Vector2(1, 0.5f), new Vector2(0, 0.5f), true, b);
            }
            else if (i == 2)
            {
                x = 0;
                y += 20 * 60 * Time.deltaTime;
                Vector2 pos = WindowManager.GetWindowPos(new Vector2(0.5f, 0), new Vector2(0.5f, 0));
                
                if (pos.y > Screen.currentResolution.height)
                    y = 0;

                WindowManager.SetWindowRect(new Rect(x, y, Screen.currentResolution.width / 1.5f, Screen.currentResolution.height / 1.5f), new Vector2(0.5f, 1), new Vector2(0.5f, 0), true, b);
            }
            else if (i == 3)
            {
                x -= 20 * 60 * Time.deltaTime;
                y = 0;
                Vector2 pos = WindowManager.GetWindowPos(new Vector2(1, 0.5f), new Vector2(1, 0.5f));

                if (pos.x < -Screen.currentResolution.width)
                    x = 0;

                WindowManager.SetWindowRect(new Rect(x, y, Screen.currentResolution.width / 1.5f, Screen.currentResolution.height / 1.5f), new Vector2(0, 0.5f), new Vector2(1, 0.5f), true, b);
            }
            else if (i == 4)
            {
                x = 0;
                y -= 20 * 60 * Time.deltaTime;
                Vector2 pos = WindowManager.GetWindowPos(new Vector2(0.5f, 1), new Vector2(0.5f, 1));
                
                if (pos.y < -Screen.currentResolution.height)
                    y = 0;

                WindowManager.SetWindowRect(new Rect(x, y, 1280, 720), new Vector2(0.5f, 0), new Vector2(0.5f, 1), true, b);
            }
            else if (i == 5)
                WindowManager.SetWindowRect(new Rect(x, y, 1280, 720), Vector2.one * 0.5f, Vector2.one * 0.5f, true, b);
            else
                i = 0;
        }
    }
}
