using System;
using System.IO.Ports;
using System.Linq;
//This program is to replace Arduino_Serial.cs
namespace Serial
{
    public class Program
    {
        public static System.IO.Ports.SerialPort port;
        public static void Main(string[] args)
        {
            int baud;
            string name;

            System.IO.Ports.SerialPort.GetPortNames().Count();//counts available ports

            Console.WriteLine("Enter your parameters to begin");
            Console.WriteLine(" ");
            Console.WriteLine("If no ports are displayed below, check your connection to the serial device");
            Console.WriteLine("Available ports:");
            /* Looks to see if there are any SerialPorts open, then 
             * proceeds to print the names of any and all serial ports
             * it finds to be open until it can't list any more
             */

            if (System.IO.Ports.SerialPort.GetPortNames().Count() >= 0)
            {
                foreach (string p in System.IO.Ports.SerialPort.GetPortNames())
                {
                    Console.WriteLine(p);
                }
            }
            else //If there are no ports avail, quit program
            {
                Console.WriteLine("No Ports available, press any key to exit.");
                Console.ReadLine();
                // Quit
                return;
            }
            Console.WriteLine("Port Name:");
            name = Console.ReadLine();
            Console.WriteLine(" ");
            Console.WriteLine("Baud rate:\n" +
                               "A. 300\n" +
                               "B. 1200\n" +
                               "C. 2400\n" +
                               "D. 4800\n" +
                               "E. 9600\n" +
                               "F. 14400\n" +
                               "G. 19200\n" +
                               "H. 28800\n" +
                               "I. 38400\n" +
                               "J. 57600\n" +
                               "K. 115200\n");
            baud = GetBaudRate();

            Console.WriteLine(" ");
            Console.WriteLine("Beginning Serial...");
            BeginSerial(baud, name);
            port.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
            port.Open();
            Console.WriteLine("Serial Started.");
            Console.WriteLine(" ");
            Console.WriteLine("Ctrl+C to exit program");
            Console.WriteLine("Send:");

            for (;;)
            {
                Console.WriteLine(" ");
                Console.WriteLine("> ");
                port.WriteLine(Console.ReadLine());
            }
        }

        static void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            for (int i = 0; i < (10000 * port.BytesToRead) / port.BaudRate; i++)
                ;	 //Delay a bit for the serial to catch up
            Console.Write(port.ReadExisting());
            Console.WriteLine("");
            Console.WriteLine("> ");
        }

        static void BeginSerial(int baud, string name)
        {
            port = new SerialPort(name, baud);
        }

        static int GetBaudRate()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid integer.  Please try again:");
                return GetBaudRate();
            }
        }

        public static int SerialCount { get; set; }
    }
}

