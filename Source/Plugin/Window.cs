using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.IO;
namespace KSPMissionControl
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Window : MonoBehaviour
    {
        public bool showGUI = false;
        public Rect windowRect = new Rect(20, 20, 200, 250);
        public Rect buttonRect = new Rect(50, 25, 100, 22);
        public Rect inptRect = new Rect(75, 25, 50, 20);
        public Rect inpButtons = new Rect(75, 25, 20, 20);
        public string onText;
        public static string serverStatusText;
        public string autoText;
        public string logRate;
        public string maxData;
        private string expressDir = @"/GameData/KSPMissionKontrol/node_modules";
        public static string appPath = Application.dataPath;
        void Start()
        {
            appPath = appPath.Substring(0, appPath.Length - 13);
            expressDir = appPath + expressDir;

            logRate = DataExport.waitTime.ToString();
            maxData = DataExport.maxData.ToString();
            showGUI = false;
            onText = DataExport.isLogging == true ? "Turn Off" : "Turn On";
            serverStatusText = Directory.Exists(expressDir) == true ? "Open Server" : "Initial Setup";


        }
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.N))
            {
                showGUI = !showGUI;
            }
            serverStatusText = Directory.Exists(expressDir) == true ? "Open Server" : "Initial Setup";
        }

        void OnGUI()
        {
            if (showGUI)
            {
                windowRect = GUI.Window(0, windowRect, MakeWindow, "Mission Kontrol");
            }
        }

        void MakeWindow(int windowID)
        {
            if (GUI.Button(buttonRect, onText))
            {
                DataExport.isLogging = !DataExport.isLogging;
                onText = DataExport.isLogging == true ? "Turn Off" : "Turn On";
            }
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y + 25, buttonRect.width, buttonRect.height), serverStatusText))
            {
                DataExport.OpenServer();
            } /*
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y + 50, buttonRect.width, buttonRect.height), "Auto"))
            {
                Debug.Log("auto");
            } */
            GUI.Box(new Rect(buttonRect.x - 25, buttonRect.y + 85, buttonRect.width + 50, buttonRect.height), "Log Rate");
            /*
            if (GUI.Button(new Rect(inpButtons.x - 25, inpButtons.y + 110, inpButtons.width, inpButtons.height), "-"))
            {
                DataExport.waitTime -= 1f;
            }
            if (GUI.Button(new Rect(inpButtons.x + 55, inpButtons.y + 110, inpButtons.width, inpButtons.height), "+"))
            {
                DataExport.waitTime += 1f;
            }
            */
            logRate = GUI.TextField(new Rect(inptRect.x, inptRect.y + 110, inptRect.width, inptRect.height), logRate, 3);

            GUI.Box(new Rect(buttonRect.x - 25, buttonRect.y + 135, buttonRect.width + 50, buttonRect.height), "Max Data");
            /*
            if (GUI.Button(new Rect(inpButtons.x - 25, inpButtons.y + 160, inpButtons.width, inpButtons.height), "-"))
            {
                DataExport.maxData -= 2;
            }
            if (GUI.Button(new Rect(inpButtons.x + 55, inpButtons.y + 160, inpButtons.width, inpButtons.height), "+"))
            {
                DataExport.maxData += 2;
            }
            */
            maxData = GUI.TextField(new Rect(inptRect.x, inptRect.y + 160, inptRect.width, inptRect.height), maxData, 3);
            logRate = Regex.Replace(logRate, "[^0-9]", "");
            maxData = Regex.Replace(maxData, "[^0-9]", "");
            DataExport.waitTime = Int32.Parse(logRate);
            DataExport.maxData = Int32.Parse(maxData);
            if (GUI.Button(new Rect(buttonRect.x, buttonRect.y + 200, buttonRect.width, buttonRect.height), "Help"))
            {
                Application.OpenURL("https://github.com/kna27/ksp-mission-kontrol/wiki/How-To-Use");
            }
            GUI.DragWindow(new Rect(0, 0, 10000, 50000));
        }

    }
}
