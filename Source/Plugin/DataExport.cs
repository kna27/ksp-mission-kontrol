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
        public static string startData = @"/GameData/KSPMissionKontrol/data/startData.txt";
        public static string allData = @"/GameData/KSPMissionKontrol/data/allData.csv";
        public static string batPath = @"/GameData/KSPMissionKontrol/makeServer.bat";
        public static string setupPath = @"/GameData/KSPMissionKontrol/setup.bat";
        public static string dataPath = @"/GameData/KSPMissionKontrol/data";
        public static string expressDir = @"/GameData/KSPMissionKontrol/node_modules";
        public int lineCount = 0;
        public static float waitTime = 1f;
        public static int maxData = 20;
        public static bool isLogging = true;
        public static bool canLog;
        public static bool correctVesselType;
        public static bool hasControl = true;
        private int lastLoggedTime = 0;
        public static string serverURL;
        public static Vessel actVess;

        void Start()
        {
            if (!EventsHolder.alreadyStarted)
            {
                appPath = appPath.Substring(0, appPath.Length - 13);
                CSVpath = appPath + CSVpath;
                batPath = appPath + batPath;
                setupPath = appPath + setupPath;
                dataPath = appPath + dataPath;
                expressDir = appPath + expressDir;
                startData = appPath + startData;
                EventsHolder.alreadyStarted = true;
            }

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
                file.WriteLine("Time,SurfaceVelocity,Altitude,Apoapsis,Periapsis,Inclination,GForce,Acceleration");
            }

            if (!File.Exists(startData))
            {
                File.Create(startData);
            }
            File.Delete(startData);
            using (FileStream fs = File.Create(startData));
            using (StreamWriter file = new StreamWriter(startData, true))
            {
                file.WriteLine(actVess.vesselName);
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
            Debug.Log(serverURL);
            serverURL = "http://" + IPAddress;
        }

        public static void OpenServer()
        {
            if(Window.serverStatusText == "Open Server")
            {
                Application.OpenURL(serverURL);
                Debug.Log(serverURL);
                System.Diagnostics.Process.Start(batPath);
            }
            else
            {
                System.Diagnostics.Process.Start(setupPath);
            }

        }
        void FixedUpdate()
        {
            actVess = FlightGlobals.ActiveVessel;
            CheckCanLog();

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            if (!File.Exists(CSVpath))
            {
                File.Create(CSVpath);
            }
            if (isLogging && canLog)
            {
                if (Mathf.RoundToInt((float)actVess.missionTime) >= lastLoggedTime + waitTime)
                {
                    VesselDeltaV e = VesselDeltaV.Create(actVess);
                    AddData(
                        Mathf.RoundToInt((float)actVess.missionTime),
                        Mathf.RoundToInt((float)actVess.srf_velocity.magnitude),
                        Math.Max(0, Mathf.RoundToInt((float)actVess.altitude)),
                        Math.Max(0,Mathf.RoundToInt((float)actVess.orbit.ApA)),
                        Math.Max(0, Mathf.RoundToInt((float)actVess.orbit.PeA)),
                        Math.Round(FlightGlobals.ship_orbit.inclination, 2),
                        Math.Round(actVess.geeForce, 2),
                        Math.Round(actVess.acceleration.magnitude, 2),
                        CSVpath,
                        lineCount,
                        maxData);
                    lastLoggedTime = Mathf.RoundToInt((float)actVess.missionTime);
                }
            }

            
        }

        public static void AddData(
            int elapsedTime,
            int srfVel,
            int altitude,
            int apoapsis,
            int periapsis,
            double inclination,
            double gForce,
            double acceleration,
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
                    file.WriteLine(elapsedTime + "," + srfVel + "," + altitude + "," + apoapsis + "," + periapsis + "," + inclination + "," + gForce + "," + acceleration);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error: ", ex);
            }

        }

        public static void CheckCanLog()
        {
            correctVesselType = actVess.vesselType == VesselType.Base || actVess.vesselType == VesselType.Lander || actVess.vesselType == VesselType.Plane || actVess.vesselType == VesselType.Probe || actVess.vesselType == VesselType.Relay || actVess.vesselType == VesselType.Rover || actVess.vesselType == VesselType.Ship || actVess.vesselType == VesselType.Station;
           // hasControl = check if vessel has a connection to the KSC;
            canLog = correctVesselType && hasControl;
        }
    }
}
