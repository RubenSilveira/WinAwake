using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Timers;

class Worker : Timer
{
#if DEBUG
    private static readonly Random Random = new Random();
#endif

    public Worker()
    {
        Interval =
#if DEBUG
            5
#else
            50
#endif
        ;
        AutoReset = false;
        Interval *= 1000;
        Elapsed += Awake;
    }

    private void Awake(object sender, ElapsedEventArgs e)
    {
        try
        {
            User32.SendInput(
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
                Marshal.SizeOf(typeof(User32.INPUT)));
        }
        finally
        {
            Enabled = true;
        }
    }
}