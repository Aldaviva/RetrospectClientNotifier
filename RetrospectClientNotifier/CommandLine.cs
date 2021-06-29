using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable

namespace RetrospectClientNotifier {

    /// https://stackoverflow.com/a/749653/979493
    internal static class CommandLine {

        public static IEnumerable<string> commandLineToArgs(string commandLine) {
            IntPtr argv = CommandLineToArgvW(commandLine, out int argc);
            if (argv == IntPtr.Zero) {
                throw new Win32Exception();
            }

            try {
                string[] args = new string[argc];
                for (int i = 0; i < args.Length; i++) {
                    IntPtr p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p)!;
                }

                return args;
            } finally {
                Marshal.FreeHGlobal(argv);
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

    }

}