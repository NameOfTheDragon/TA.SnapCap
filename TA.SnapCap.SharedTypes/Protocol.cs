// This file is part of the TA.SnapCap project
//
// Copyright © 2016-2020 Tigra Astronomy, all rights reserved.
//
// File: Protocol.cs  Last modified: 2020-03-10@22:08 by Tim Long

namespace TA.SnapCap.SharedTypes
    {
    public static class Protocol
        {
        public const char ElpOff = 'D';
        public const char ElpOn = 'L';
        public const char GetBrightness = 'J';
        public const char GetFirmwareVersion = 'V';
        public const char GetStatus = 'S';
        public const char SetBrightness = 'B';
        public const char OpenCover = 'O';
        public const char CloseCover = 'C';
        public const char CommandInitiator = '>';
        public const char CommandTerminator = '\n';

        public static string GetCommandString(char command) => $"{CommandInitiator}{command}\r\n";
        public static string GetCommandString(char command, ushort payload) => $"{CommandInitiator}{command}{payload:D3}\r\n";
        }
    }