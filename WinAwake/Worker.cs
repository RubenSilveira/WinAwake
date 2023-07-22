using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

class Worker : Timer
{
#if DEBUG
    private static readonly Random Random = new Random();
#endif

    internal Worker()
    {
        Interval =
#if DEBUG
            5
#else
            50
#endif
            * 1000;
        AutoReset = false;
        Elapsed += Worker_Elapsed;
    }

    private void Worker_Elapsed(object sender, ElapsedEventArgs e)
    {
        try
        {
            if (1u != User32.SendInput(
                        1u,
                        new[] {
                            new User32.INPUT
                            {
                                Type = (uint)User32.INPUTTYPE.INPUT_MOUSE,
                                Data = new User32.MOUSEKEYBDHARDWAREINPUT
                                {
                                    Mouse = new User32.MOUSEINPUT
                                    {
                                        X =
#if DEBUG
                                            Random.Next(15, 31) * (Random.Next(0, 2) * 2 - 1)
#else
                                            0
#endif
                                        ,
                                        Y =
#if DEBUG
                                            Random.Next(15, 31) * (Random.Next(0, 2) * 2 - 1)
#else
                                            0
#endif
                                        ,
                                        MouseData = 0u,
                                        Flags = (uint)User32.MOUSEEVENTF.MOUSEEVENTF_MOVE,
                                        Time = 0u,
                                        ExtraInfo = IntPtr.Zero,
                                    }
                                }
                            }
                        },
                        Marshal.SizeOf(typeof(User32.INPUT))))
            {
                throw new ApplicationException($"SendInput failed with error 0x{Marshal.GetLastWin32Error():x8}");
            }

            Debug.WriteLine("Awoke");
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Awake failed: " + ex.Message);
        }
        finally
        {
            Enabled = true;
        }
    }
}