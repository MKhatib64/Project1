using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

class IPC
{
    static void Main()
    {
        Console.WriteLine("Starting IPC Simulation in a Single Window...");

        // Create a pipe for communication
        using (var pipe = new AnonymousPipeServerStream(PipeDirection.Out))
        using (var pipeReader = new AnonymousPipeClientStream(PipeDirection.In, pipe.ClientSafePipeHandle))
        {
            // Start the consumer thread
            Thread consumerThread = new Thread(() => Consumer(pipeReader));
            consumerThread.Start();

            // Run the producer
            Producer(pipe);

            // Wait for the consumer thread to finish
            consumerThread.Join();
        }

        Console.WriteLine("IPC Simulation Complete.");
    }

    static void Producer(AnonymousPipeServerStream pipe)
    {
        Console.WriteLine("Producer started.");

        using (var writer = new StreamWriter(pipe))
        {
            for (int i = 0; i < 10; i++)
            {
                string message = $"Message {i + 1} from producer.";
                Console.WriteLine($"Producer sending: {message}");
                writer.WriteLine(message); // Send data via pipe
                writer.Flush(); // Ensure data is sent immediately
                Thread.Sleep(500); // Simulate delay
            }

            // Signal the consumer to exit
            writer.WriteLine("exit");
            writer.Flush();
        }

        Console.WriteLine("Producer finished.");
    }

    static void Consumer(AnonymousPipeClientStream pipeReader)
    {
        Console.WriteLine("Consumer started.");

        using (var reader = new StreamReader(pipeReader))
        {
            while (true)
            {
                string message = reader.ReadLine(); // Read data from the pipe
                if (message == "exit") // Exit signal
                {
                    break;
                }

                // Process the data (e.g., print to console)
                Console.WriteLine($"Consumer received: {message}");
            }
        }

        Console.WriteLine("Consumer finished.");
    }
}