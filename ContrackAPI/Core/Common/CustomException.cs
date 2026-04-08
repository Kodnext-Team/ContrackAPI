using System.Diagnostics;

namespace ContrackAPI
{
    public class CustomException
    {
        internal void RecordException(Exception ex)
        {
            try
            {
                var stackTrace = new StackTrace();

                // Frame 1 -> caller of RecordException
                var frame = stackTrace.GetFrame(1);
                var method = frame.GetMethod();
                var className = method.DeclaringType?.Name;
                var methodName = method.Name;

                Console.WriteLine($"Class: {className}");
                Console.WriteLine($"Method: {methodName}");
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex1)
            {

            }

        }

    }
}
