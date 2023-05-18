/*
Technitium Library
Copyright (C) 2019  Shreyas Zare (shreyas@technitium.com)

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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TechnitiumLibrary.IO;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DnsDIDCTLRRecordData : DnsResourceRecordData
    {
        #region variables

        string _controller;

        byte[] _rData;

        #endregion

        #region constructor

        public DnsDIDCTLRRecordData(string value)
        {
            _controller = value; 
        }

        public DnsDIDCTLRRecordData(Stream s)
            : base(s)
        { }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            _rData = s.ReadBytes(_rdLength);

            using (MemoryStream mS = new MemoryStream(_rData))
            {
                int bytesRead = 0;
                int length;

                while (bytesRead < _rdLength)
                {
                    length = mS.ReadByte();
                    if (length < 0)
                        throw new EndOfStreamException();

                    if (_controller == null)
                        _controller = Encoding.ASCII.GetString(mS.ReadBytes(length));
                    else
                        _controller += Encoding.ASCII.GetString(mS.ReadBytes(length));

                    bytesRead += length + 1;
                }
            }
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            if (_rData is null)
            {
                using (MemoryStream mS = new MemoryStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(_controller);
                    int offset = 0;
                    int length;

                    do
                    {
                        length = data.Length - offset;
                        if (length > 255)
                            length = 255;

                        mS.WriteByte(Convert.ToByte(length));
                        mS.Write(data, offset, length);

                        offset += length;
                    }
                    while (offset < data.Length);

                    _rData = mS.ToArray();
                }
            }

            s.Write(_rData);
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is DnsDIDCTLRRecordData other)
                return _controller.Equals(other._controller);

            return false;
        }

        public override int GetHashCode()
        {
            return _controller.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_controller);
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            jsonWriter.WriteString("Controller", _controller);

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public string Controller
        { get { return _controller; } }

        public override ushort UncompressedLength
        { get { return Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_controller.Length / 255d)) + _controller.Length); } }


        #endregion
    }
}
