using System;
using System.IO.Ports;
using System.Linq;
namespace Serial
{
    public class Program
    {
        public static System.IO.Ports.SerialPort port;
        public static int baud_rate;
        //public static int s;//User can choose which baud rate from letters
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
                               "1. 300\n" +
                               "2. 1200\n" +
                               "3. 2400\n" +
                               "4. 4800\n" +
                               "5. 9600\n" +
                               "6. 14400\n" +
                               "7. 19200\n" +
                               "8. 28800\n" +
                               "9. 38400\n" +
                               "10. 57600\n" +
                               "11. 115200\n");
           /* char A, B, C, D, E, F, G, H, I, J, K = a;
            switch(baud_rate)
            {
                case 1: baud = 300; break;
                case 2: baud = 1200; break;
                case 3: baud = 2400; break;
                case 4: baud = 4800; break;
                case 5: baud = 9600; break;
                case 6: baud = 14400; break;
                case 7: baud = 19200; break;
                case 8: baud = 28800; break;
                case 9: baud = 38400; break;
                case 10: baud = 57600; break;
                case 11: baud = 115200; break;
                default: Console.WriteLine("Setting baud rate to 9600 as default\n"); baud = 9600;
            } */

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

