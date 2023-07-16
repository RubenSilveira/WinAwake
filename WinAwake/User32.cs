using System;
using System.Runtime.InteropServices;

class User32
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort Vk;
        public ushort Scan;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    public enum MOUSEEVENTF : uint
    {
        MOUSEEVENTF_ABSOLUTE = 0x8000u,
        MOUSEEVENTF_HWHEEL = 0x1000u,
        MOUSEEVENTF_MOVE = 0x1u,
        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000u,
        MOUSEEVENTF_LEFTDOWN = 0x2u,
        MOUSEEVENTF_LEFTUP = 0x4u,
        MOUSEEVENTF_RIGHTDOWN = 0x8u,
        MOUSEEVENTF_RIGHTUP = 0x10u,
        MOUSEEVENTF_MIDDLEDOWN = 0x20u,
        MOUSEEVENTF_MIDDLEUP = 0x40u,
        MOUSEEVENTF_VIRTUALDESK = 0x4000u,
        MOUSEEVENTF_WHEEL = 0x800u,
        MOUSEEVENTF_XDOWN = 0x80u,
        MOUSEEVENTF_XUP = 0x100u
    }

    public enum INPUTTYPE : uint
    {
        INPUT_MOUSE = 0u,
        INPUT_KEYBOARD = 1u,
        INPUT_HARDWARE = 2u
    }
}