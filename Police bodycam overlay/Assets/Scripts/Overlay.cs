using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class E : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    private struct MARGINS
    {
        public int cxLeftWidth;

        public int cxRightWidth;

        public int cyTopHeight;

        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_EXSTYLE= -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    //*******************

    public TMP_Text date;
    public TMP_Text time;
    public TMP_Text random;

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR_
        IntPtr hWnd = GetActiveWindow();

        MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);

        SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);

        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
#endif
        //*******************
        DateTimeFunction();
        Xrand();
    }

    // Update is called once per frame
    void Update()
    {
        DateTimeFunction();
    }

    public void DateTimeFunction()
    {
        DateTime dateNow = DateTime.Today;
        string dateYear = dateNow.Year.ToString();
        string dateMonth = "";
        string dateDay = "";
        if (dateNow.Month >= 10)
        {
            dateMonth = dateNow.Month.ToString();
        }
        else
        {
            dateMonth = "0" + dateNow.Month.ToString();
        }

        if (dateNow.Day >= 10)
        {
            dateDay = dateNow.Day.ToString();
        }
        else
        {
            dateDay = "0" + dateNow.Day.ToString();
        }

        DateTime timeNow = DateTime.Now;
        string timeHour = timeNow.Hour.ToString();
        string timeMinute = timeNow.Minute.ToString();
        string timeSecond = timeNow.Second.ToString();
        date.text = dateYear + "-" + dateMonth + "-" + dateDay;
        time.text = timeHour + ":" + timeMinute + ":" + timeSecond;
    }

    public void Xrand()
    {
        if (!PlayerPrefs.HasKey("Xrand"))
        {
            System.Random rnd = new System.Random();
            int randNumber = rnd.Next(10000000, 99999999);
            random.text = "X" + randNumber.ToString();
            PlayerPrefs.SetString("Xrand", random.text);
        }
        else
        {
            random.text = PlayerPrefs.GetString("Xrand");
        }
    }
}