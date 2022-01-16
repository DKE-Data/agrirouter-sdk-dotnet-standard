using System;
using System.Threading;

namespace Agrirouter.Test.Helper
{
    /// <summary>
    /// Timer for multiple purpose.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Wait for the AR to process the message.
        /// </summary>
        public static void WaitForTheAgrirouterToProcessTheMessage()
        {
            Thread.Sleep(TimeSpan.FromSeconds(30));
        }
    }
}