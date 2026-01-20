using System;
using Sharp7;

namespace Omnicheck360
{
    public class PLCData
    {
        // Properties to store PLC data
        public uint TotalThroughput { get; set; }
        public uint TotalRejects { get; set; }
        public uint TotalUpstreamRejects { get; set; }
        public uint TotalDownstreamRejects { get; set; }
        public uint PPS { get; set; }
        public uint MeasuredTriggerWidth { get; set; }
        public uint MeasuredContainerWidth { get; set; }
        public uint CPS { get; set; }
        public uint UpstreamToTriggerGap { get; set; }
        public uint TriggerToInspectionGap { get; set; }
        public uint InspectionToRejectionGap { get; set; }
        public uint DownToRejectGap { get; set; }
        public uint RejectPulseDuration { get; set; }
        public uint ProductWidth { get; set; }
        public uint DownBottleWidth { get; set; }
        public uint ConsecutiveBads { get; set; }
        public uint ConsecutiveUpstreamBads { get; set; }
        public uint ConsecutiveDownstreamBads { get; set; }
        public int CameraTriggerDuration { get; set; }
        public int ResultTime { get; set; }
        public int Version { get; set; }
        public bool ReadyBusy { get; set; }
        public bool BadGood { get; set; }
        public bool DisableReject { get; set; }
        public bool IgnoreMoving { get; set; }
        public bool IgnoreInterlock { get; set; }
        public bool RejectorDiverter { get; set; }
        public bool Inspect2Enable { get; set; }
        public bool DownBottleEnable { get; set; }
        public bool ZeroSystem { get; set; }
        public bool Moving { get; set; }
        public bool Measure { get; set; }
        public bool ResetConsecutiveBad { get; set; }
        public bool ResetConsecitiveUpstream { get; set; }
        public bool ResetConsecutiveDownstream { get; set; }
        public bool ResetTotalThroughput { get; set; }
        public bool ResetTotalReject { get; set; }
        public bool ResetTotalUpstreamReject { get; set; }
        public bool ResetTotalDownstreamReject { get; set; }

        // S7Client instance for PLC communication
        public S7Client client1;

        // Constructor that accepts an S7Client instance
        public PLCData(S7Client client1)
        {
            if (client1 == null)
            {
                throw new ArgumentNullException(nameof(client1), "S7Client instance cannot be null.");
            }
            this.client1 = client1;
            int result1 = client1.ConnectTo("192.168.0.21", 0, 1); // Update IP, rack, slot as needed
            if (result1 == 0)
            {
                Console.WriteLine("Connected to PLC 1");
            }
            else
            {
                throw new Exception($"Failed to connect to PLC 1: {client1.ErrorText(result1)}");
            }
        }

        // Default constructor for convenience
        public PLCData()
        {
            client1 = new S7Client();
            int result1 = client1.ConnectTo("192.168.0.21", 0, 1); // Update IP, rack, slot as needed
            if (result1 == 0)
            {
                Console.WriteLine("Connected to PLC 1");
            }
            else
            {
                throw new Exception($"Failed to connect to PLC 1: {client1.ErrorText(result1)}");
            }
        }

        // Method to read data from PLC (DB4)
        public void ReadDataPLC1_DB4()
        {
            byte[] buffer = new byte[24]; // Adjust buffer size to match PLC memory layout
            int result = client1.DBRead(4, 0, buffer.Length, buffer);
            if (result == 0)
            {
                try
                {
                    TotalThroughput = S7.GetDWordAt(buffer, 0);
                    TotalRejects = S7.GetDWordAt(buffer, 4);
                    TotalUpstreamRejects = S7.GetDWordAt(buffer, 8);
                    TotalDownstreamRejects = S7.GetDWordAt(buffer, 12);
                    PPS = S7.GetDWordAt(buffer, 16);
                    MeasuredTriggerWidth = S7.GetDWordAt(buffer, 20);
                    MeasuredContainerWidth = S7.GetDWordAt(buffer, 24);

                    CPS = S7.GetUIntAt(buffer, 28);
                    UpstreamToTriggerGap = S7.GetUIntAt(buffer, 30);
                    TriggerToInspectionGap = S7.GetUIntAt(buffer, 32);
                    InspectionToRejectionGap = S7.GetUIntAt(buffer, 34);
                    DownToRejectGap = S7.GetUIntAt(buffer, 36);
                    RejectPulseDuration = S7.GetUIntAt(buffer, 42);
                    ProductWidth = S7.GetUIntAt(buffer, 44);
                    DownBottleWidth = S7.GetUIntAt(buffer, 46);
                    ConsecutiveBads = S7.GetUIntAt(buffer, 48);
                    ConsecutiveUpstreamBads = S7.GetUIntAt(buffer, 50);
                    ConsecutiveDownstreamBads = S7.GetUIntAt(buffer, 52);

                    CameraTriggerDuration = S7.GetDIntAt(buffer, 54);
                    ResultTime = S7.GetDIntAt(buffer, 58);
                    Version = S7.GetDIntAt(buffer, 70);

                    ReadyBusy = S7.GetBitAt(buffer, 104, 0);
                    BadGood = S7.GetBitAt(buffer, 104, 1);
                    DisableReject = S7.GetBitAt(buffer, 104, 2);
                    IgnoreMoving = S7.GetBitAt(buffer, 104, 3);
                    IgnoreInterlock = S7.GetBitAt(buffer, 104, 4);
                    RejectorDiverter = S7.GetBitAt(buffer, 104, 5);
                    Inspect2Enable = S7.GetBitAt(buffer, 104, 6);
                    DownBottleEnable = S7.GetBitAt(buffer, 104, 7);
                    ZeroSystem = S7.GetBitAt(buffer, 105, 0);
                    Moving = S7.GetBitAt(buffer, 105, 1);
                    Measure = S7.GetBitAt(buffer, 105, 2);
                    ResetConsecutiveBad = S7.GetBitAt(buffer, 105, 3);
                    ResetConsecitiveUpstream = S7.GetBitAt(buffer, 105, 4);
                    ResetConsecutiveDownstream = S7.GetBitAt(buffer, 105, 5);
                    ResetTotalThroughput = S7.GetBitAt(buffer, 105, 6);
                    ResetTotalReject = S7.GetBitAt(buffer, 105, 7);
                    ResetTotalUpstreamReject = S7.GetBitAt(buffer, 106, 0);
                    ResetTotalDownstreamReject = S7.GetBitAt(buffer, 106, 1);

                    Console.WriteLine("Buffer content:");
                    Console.WriteLine(BitConverter.ToString(buffer));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while parsing buffer: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Error reading DB4: {client1.ErrorText(result)} (Error Code: {result})");
            }
        }

        // Method to write data to PLC (DB4)
        public void WriteDataPLC1_DB4(int newValue)
        {
            byte[] buffer = new byte[124]; // Adjust buffer size to match PLC memory layout
            S7.SetDIntAt(buffer, 0, newValue); // Example: Write a DInt value at offset 0

            int result = client1.DBWrite(4, 0, buffer.Length, buffer);
            if (result != 0)
            {
                Console.WriteLine("Failed to write data to PLC 1. Error: " + client1.ErrorText(result));
            }
            else
            {
                Console.WriteLine("Value written to PLC 1 successfully!");
            }
        }
    }
}