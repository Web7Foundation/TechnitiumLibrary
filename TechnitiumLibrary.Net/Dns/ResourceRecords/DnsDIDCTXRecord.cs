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
using System.Net;
using System.Text;
using System.Text.Json;
using TechnitiumLibrary.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DnsDIDCTXRecord : DnsResourceRecordData
    {
        #region variables

        string _didctxTag; // optional primary key
        string _didctxData; // "value" field

        #endregion

        #region constructor

        public DnsDIDCTXRecord(string value)
        {
            _didctxTag = "";
            _didctxData = value;
        }

        public DnsDIDCTXRecord(string didctxTag, string didctxData)
        {
            _didctxTag = didctxTag;
            _didctxData = didctxData;
        }

        public DnsDIDCTXRecord(Stream s)
            : base(s)
        { }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            int length = s.ReadByte();
            if (length < 0)
                throw new EndOfStreamException();
            _didctxTag = "";
            if (length > 0) _didctxTag = Encoding.ASCII.GetString(s.ReadBytes(length));

            length = s.ReadByte();
            if (length < 0)
                throw new EndOfStreamException();
            _didctxData = "";
            if (length > 0) _didctxData = Encoding.ASCII.GetString(s.ReadBytes(length));
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            byte[] tagBytes = Encoding.ASCII.GetBytes(_didctxTag);

            s.WriteByte(Convert.ToByte(tagBytes.Length));
            s.Write(tagBytes, 0, tagBytes.Length);

            //s.WriteByte(Convert.ToByte(_didctxTag.Length));
            //if (_didctxTag.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didctxTag));

            byte[] dataBytes = Encoding.ASCII.GetBytes(_didctxData);

            s.WriteByte(Convert.ToByte(dataBytes.Length));
            s.Write(dataBytes, 0, dataBytes.Length);

            //s.WriteByte(Convert.ToByte(_didctxData.Length));
            //if (_didctxData.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didctxData));
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            DnsDIDCTXRecord other = obj as DnsDIDCTXRecord;
            if (other == null)
                return false;

            if (this._didctxTag.Length > 0)
            {
                if (!this._didctxTag.Equals(other._didctxTag, StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            else
            {
                if (this._didctxData != other._didctxData)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _didctxData.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_didctxTag + "=" + _didctxData);
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            jsonWriter.WriteString("Tag", _didctxTag);
            jsonWriter.WriteString("Data", _didctxData);

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public string Tag
        { get { return _didctxTag; } }
        public string Data
        { get { return _didctxData; } }

        public override ushort UncompressedLength
        {
            get
            {
                ushort dataLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didctxData.Length / 255d)) + _didctxData.Length);
                ushort tagLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didctxTag.Length / 255d)) + _didctxTag.Length);
                return (ushort)(dataLength + tagLength);
            }

            #endregion
        }
    }
}
