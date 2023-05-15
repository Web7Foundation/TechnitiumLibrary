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
    public class DnsDIDEXTDATRecordData : DnsResourceRecordData
    {
        #region variables

        string _diddataTag; // optional primary key
        string _diddataDID; // optional secondary key
        string _diddataType;
        string _diddataSource; // "value" field
        string _diddataQuery;
        string _diddataParms;

        #endregion

        #region constructor

        public DnsDIDEXTDATRecordData(string value)
        {
            _diddataTag = "default";
            _diddataDID = "";
            _diddataType = "";
            _diddataSource = value;
            _diddataQuery = "";
            _diddataParms = "";
        }

        public DnsDIDEXTDATRecordData(Stream s)
            : base(s)
        { }

        public DnsDIDEXTDATRecordData(string diddataTag, string diddataDID, string diddataType, string diddataSource, string diddataQUery, string diddataParms)
        {
            _diddataTag = diddataTag;
            _diddataDID = diddataDID;
            _diddataType = diddataType;
            _diddataSource = diddataSource;
            _diddataQuery = diddataQUery;
            _diddataParms = diddataParms;
        }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            int len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataTag = "";
            if (len > 0) _diddataTag = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataDID = "";
            if (len > 0) _diddataDID = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataType = "";
            if (len > 0) _diddataType = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataSource = "";
            if (len > 0) _diddataSource = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataQuery = "";
            if (len > 0) _diddataQuery = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _diddataParms = "";
            if (len > 0) _diddataParms = Encoding.ASCII.GetString(s.ReadBytes(len));
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            s.WriteByte(Convert.ToByte(_diddataTag.Length));
            if (_diddataTag.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataTag));
            s.WriteByte(Convert.ToByte(_diddataDID.Length));
            if (_diddataDID.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataDID));
            s.WriteByte(Convert.ToByte(_diddataType.Length));
            if (_diddataType.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataType));
            s.WriteByte(Convert.ToByte(_diddataSource.Length));
            if (_diddataSource.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataSource));
            s.WriteByte(Convert.ToByte(_diddataQuery.Length));
            if (_diddataQuery.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataQuery));
            s.WriteByte(Convert.ToByte(_diddataParms.Length));
            if (_diddataParms.Length > 0) s.Write(Encoding.ASCII.GetBytes(_diddataParms));
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            DnsDIDEXTDATRecordData other = obj as DnsDIDEXTDATRecordData;
            if (other == null)
                return false;

            return this._diddataTag.Equals(other._diddataTag, StringComparison.OrdinalIgnoreCase); // mwh
        }

        public override int GetHashCode()
        {
            return _diddataTag.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_diddataTag + ":" + _diddataDID + "=" + _diddataType + "," + _diddataSource + ", " + _diddataQuery + ", " + _diddataParms);
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            jsonWriter.WriteString("Tag", _diddataTag);
            jsonWriter.WriteString("DID", _diddataDID);
            jsonWriter.WriteString("Type", _diddataType);
            jsonWriter.WriteString("Source", _diddataSource);
            jsonWriter.WriteString("Query", _diddataQuery);
            jsonWriter.WriteString("Params", _diddataParms);

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public string Tag
        { get { return _diddataTag; } }
        public string DID
        { get { return _diddataDID; } }
        public string Type
        { get { return _diddataType; } }
        public string Source
        { get { return _diddataSource; } }
        public string Query
        { get { return _diddataQuery; } }
        public string Parms
        { get { return _diddataParms; } }

        public override ushort UncompressedLength
        {
            get
            {
                ushort tagLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataTag.Length / 255d)) + _diddataTag.Length);
                ushort didLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataDID.Length / 255d)) + _diddataDID.Length);
                ushort typeLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataType.Length / 255d)) + _diddataType.Length);
                ushort sourceLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataSource.Length / 255d)) + _diddataSource.Length);
                ushort queryLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataQuery.Length / 255d)) + _diddataQuery.Length);
                ushort paramsLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_diddataParms.Length / 255d)) + _diddataParms.Length);
                return (ushort)(tagLength + didLength + typeLength + sourceLength + queryLength + paramsLength);
            }
        }

        #endregion
    }
}
