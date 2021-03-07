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
        public static string CSVpath = @"/GameData/KSPMissionControl/data/data.csv";
        public static string batPath = @"/GameData/KSPMissionControl/makeServer.bat";
        public int lineCount = 0;
        public static float waitTime = 1f;
        public static int maxData = 20;
        public static bool isLogging = true;
        private int lastLoggedTime = 0;
        private static string IPA;

        void Start()
        {
            appPath = appPath.Substring(0, appPath.Length - 13);
            CSVpath = appPath + CSVpath;
            batPath = appPath + batPath;
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
            IPA = IPAddress;

        }

        public static void OpenServer()
        {
            System.Diagnostics.Process.Start(batPath);
            Application.OpenURL("http://" + IPA);
        }
        void FixedUpdate()
        {

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
