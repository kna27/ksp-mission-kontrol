using System;
using System.Linq;
using UnityEngine;
using System.IO;
using System.Net;
namespace KSPMissionControl
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DataExport : MonoBehaviour
    {
        public static string appPath = Application.dataPath;
        public static string CSVpath = @"/GameData/KSPMissionKontrol/data/data.csv";
        public static string batPath = @"/GameData/KSPMissionKontrol/makeServer.bat";
        public static string setupPath = @"/GameData/KSPMissionKontrol/setup.bat";
        public static string dataPath = @"/GameData/KSPMissionKontrol/data";
        public int lineCount = 0;
        public static float waitTime = 1f;
        public static int maxData = 20;
        public static bool isLogging = true;
        private int lastLoggedTime = 0;
        private static string serverURL;

        void Start()
        {
            appPath = appPath.Substring(0, appPath.Length - 13);
            CSVpath = appPath + CSVpath;
            batPath = appPath + batPath;
            setupPath = appPath + setupPath;
            dataPath = appPath + dataPath;
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            if (!File.Exists(CSVpath))
            {
                File.Create(CSVpath);
            }

            File.Delete(CSVpath);
            using (FileStream fs = File.Create(CSVpath));
            using (StreamWriter file = new StreamWriter(CSVpath, true))
            {
                file.WriteLine("Time,SurfaceVelocity,Altitude");
            }


            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            serverURL = "http://" + IPAddress;
        }

        public static void OpenServer()
        {
            if(Window.serverStatusText == "Open Server")
            {
                Application.OpenURL(serverURL);
                System.Diagnostics.Process.Start(batPath);
            }
            else
            {
                System.Diagnostics.Process.Start(setupPath);
            }

        }
        void FixedUpdate()
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            if (!File.Exists(CSVpath))
            {
                File.Create(CSVpath);
            }
            if (isLogging)
            {


                if (Mathf.RoundToInt((float)FlightGlobals.ActiveVessel.missionTime) >= lastLoggedTime + waitTime)
                {
                    AddData(
                        Mathf.RoundToInt((float)FlightGlobals.ActiveVessel.missionTime),
                        Mathf.RoundToInt((float)FlightGlobals.ActiveVessel.srf_velocity.magnitude),
                        Mathf.RoundToInt((float)FlightGlobals.ActiveVessel.altitude),
                        CSVpath,
                        lineCount,
                        maxData);
                    lastLoggedTime = Mathf.RoundToInt((float)FlightGlobals.ActiveVessel.missionTime);
                }
            }

            
        }

        public static void AddData(
            int elapsedTime,
            int srfVel,
            int altitude,
            string path,
            int lc,
            int maxData)
        {
            lc = File.ReadLines(path).Count();
            while (lc > maxData)
            {
                var lines = File.ReadAllLines(path);
                var header = lines.Take(1);
                var records = lines.Skip(2);
                var merge = header.Concat(records);
                File.WriteAllLines(path, merge.ToArray());
                lc = File.ReadLines(path).Count();
            }
            try
            {
                using (StreamWriter file = new StreamWriter(path, true))
                {
                    file.WriteLine(elapsedTime + "," + srfVel + "," + altitude);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: ", ex);
            }

        }
    }
}
