﻿/*
Technitium Library
Copyright (C) 2023  Shreyas Zare (shreyas@technitium.com)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TechnitiumLibrary.IO;

namespace TechnitiumLibrary.Net
{
    public static class EndPointExtensions
    {
        #region static

        public static EndPoint ReadFrom(BinaryReader bR)
        {
            switch (bR.ReadByte())
            {
                case 1:
                    return new IPEndPoint(new IPAddress(bR.ReadBytes(4)), bR.ReadUInt16());

                case 2:
                    return new IPEndPoint(new IPAddress(bR.ReadBytes(16)), bR.ReadUInt16());

                case 3:
                    return new DomainEndPoint(bR.ReadShortString(), bR.ReadUInt16());

                default:
                    throw new NotSupportedException("AddressFamily not supported.");
            }
        }

        public static void WriteTo(this EndPoint ep, BinaryWriter bW)
        {
            switch (ep.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    bW.Write((byte)1);
                    bW.Write((ep as IPEndPoint).Address.GetAddressBytes());
                    bW.Write(Convert.ToUInt16((ep as IPEndPoint).Port));
                    break;

                case AddressFamily.InterNetworkV6:
                    bW.Write((byte)2);
                    bW.Write((ep as IPEndPoint).Address.GetAddressBytes());
                    bW.Write(Convert.ToUInt16((ep as IPEndPoint).Port));
                    break;

                case AddressFamily.Unspecified: //domain end point
                    bW.Write((byte)3);
                    bW.WriteShortString((ep as DomainEndPoint).Address);
                    bW.Write(Convert.ToUInt16((ep as DomainEndPoint).Port));
                    break;

                default:
                    throw new NotSupportedException("AddressFamily not supported.");
            }
        }

        public static string GetAddress(this EndPoint ep)
        {
            switch (ep.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                case AddressFamily.InterNetworkV6:
                    return (ep as IPEndPoint).Address.ToString();

                case AddressFamily.Unspecified:
                    return (ep as DomainEndPoint).Address;

                default:
                    throw new NotSupportedException("AddressFamily not supported.");
            }
        }

        public static int GetPort(this EndPoint ep)
        {
            switch (ep.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                case AddressFamily.InterNetworkV6:
                    return (ep as IPEndPoint).Port;

                case AddressFamily.Unspecified:
                    return (ep as DomainEndPoint).Port;

                default:
                    throw new NotSupportedException("AddressFamily not supported.");
            }
        }

        public static async Task<IPEndPoint> GetIPEndPointAsync(this EndPoint ep, AddressFamily family = AddressFamily.Unspecified)
        {
            switch (ep.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                case AddressFamily.InterNetworkV6:
                    return ep as IPEndPoint;

                case AddressFamily.Unspecified:
                    if (ep is not DomainEndPoint dep)
                        throw new NotSupportedException("AddressFamily not supported.");

                    IPAddress[] ipAddresses = await System.Net.Dns.GetHostAddressesAsync(dep.Address);
                    if (ipAddresses.Length == 0)
                        throw new SocketException((int)SocketError.HostNotFound);

                    switch (family)
                    {
                        case AddressFamily.InterNetwork:
                        case AddressFamily.InterNetworkV6:
                            foreach (IPAddress ipAddress in ipAddresses)
                            {
                                if (ipAddress.AddressFamily == family)
                                    return new IPEndPoint(ipAddress, dep.Port);
                            }

                            throw new SocketException((int)SocketError.NetworkUnreachable);

                        default:
                            return new IPEndPoint(ipAddresses[0], dep.Port);
                    }

                default:
                    throw new NotSupportedException("AddressFamily not supported.");
            }
        }

        public static EndPoint GetEndPoint(string address, int port)
        {
            if (IPAddress.TryParse(address, out IPAddress ipAddress))
                return new IPEndPoint(ipAddress, port);
            else
                return new DomainEndPoint(address, port);
        }

        #endregion
    }
}
