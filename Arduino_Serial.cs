using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; // Serial stuff in here.
using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;


namespace Serial
{
    public class Program
    {
	  public static SerialPort port;
	  public static void Main(string[] args)
	  {
		int baud;
		string name;

        SerialPort.GetPortNames().Count() = SerialCount;

		Console.WriteLine("Enter your parameters to begin");
		Console.WriteLine(" ");
        Console.WriteLine("If no ports are displayed below, check your connection to the serial device");
		Console.WriteLine("Available ports:");
        /* Looks to see if there are any SerialPorts open, then 
         * proceeds to print the names of any and all serial ports
         * it finds to be open until it can't list any more
         */
        
		if (SerialCount >= 0)
		{
		    foreach (string p in SerialPort.GetPortNames())
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
		Console.WriteLine("Baud rate:");
		baud = GetBaudRate();

		Console.WriteLine(" ");
		Console.WriteLine("Beginning Serial...");
		BeginSerial(baud, name);
		port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
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

	  static void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
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
    }
}
 
