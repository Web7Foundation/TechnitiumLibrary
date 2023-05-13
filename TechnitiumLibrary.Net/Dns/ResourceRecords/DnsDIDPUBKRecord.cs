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
    public class DnsDIDPUBKRecord : DnsResourceRecordData
    {
        #region variables

        string _didpubkTag; // optional primary key
        string _didpubkDID; // optional secondary key
        string _didpubkType;
        string _didpubkSubjectPublicKey; // "value" value
        string _didpubkControllerDID;

        #endregion

        #region constructor

        public DnsDIDPUBKRecord(string value)
        {
            _didpubkTag = "";
            _didpubkDID = "";
            _didpubkType = "";
            _didpubkSubjectPublicKey = value;           
            _didpubkControllerDID = "";
        }

        public DnsDIDPUBKRecord(Stream s)
            : base(s)
        { }

        public DnsDIDPUBKRecord(string didpubkTag, string didpubkDID, string didpubkType, string didpubkSubjectPublicKey, string didpubkController)
        {
            _didpubkTag = didpubkTag;
            _didpubkDID = didpubkDID;
            _didpubkType = didpubkType;
            _didpubkSubjectPublicKey = didpubkSubjectPublicKey;
            _didpubkControllerDID = didpubkController;
        }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            int len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _didpubkTag = "";
            if (len > 0) _didpubkTag = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _didpubkDID = "";
            if (len > 0) _didpubkDID = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _didpubkType = "";
            if (len > 0) _didpubkType = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _didpubkSubjectPublicKey = "";
            if (len > 0) _didpubkSubjectPublicKey = Encoding.ASCII.GetString(s.ReadBytes(len));

            len = s.ReadByte();
            if (len < 0)
                throw new EndOfStreamException();
            _didpubkControllerDID = "";
            if (len > 0) _didpubkControllerDID = Encoding.ASCII.GetString(s.ReadBytes(len));
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool cannonicalForm)
        {
            s.WriteByte(Convert.ToByte(_didpubkTag.Length));
            if (_didpubkTag.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didpubkTag));

            s.WriteByte(Convert.ToByte(_didpubkDID.Length));
            if (_didpubkDID.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didpubkDID));

            s.WriteByte(Convert.ToByte(_didpubkType.Length));
            if (_didpubkType.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didpubkType));

            s.WriteByte(Convert.ToByte(_didpubkSubjectPublicKey.Length));
            if (_didpubkSubjectPublicKey.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didpubkSubjectPublicKey));

            s.WriteByte(Convert.ToByte(_didpubkControllerDID.Length));
            if (_didpubkControllerDID.Length > 0) s.Write(Encoding.ASCII.GetBytes(_didpubkControllerDID));
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            DnsDIDPUBKRecord other = obj as DnsDIDPUBKRecord;
            if (other == null)
                return false;

            if (this._didpubkTag.Length > 0)
            {
                if (this._didpubkTag != other._didpubkTag)
                    return false;
            }
            else if(this._didpubkDID.Length > 0)
            {
                if (this._didpubkDID != other._didpubkDID)
                    return false;
            }
            else
            {
                if ((this._didpubkType.Length > 0) && (this._didpubkType != other._didpubkType))
                    return false;
                if (this._didpubkSubjectPublicKey != other._didpubkSubjectPublicKey)
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return _didpubkSubjectPublicKey.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_didpubkTag + ":" + _didpubkDID + "=" + _didpubkType + "," + _didpubkSubjectPublicKey + ", " + _didpubkControllerDID);
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            jsonWriter.WriteString("Tag", _didpubkTag);
            jsonWriter.WriteString("DID", _didpubkDID);
            jsonWriter.WriteString("Type", _didpubkType);
            jsonWriter.WriteString("SubjectPublicKey", _didpubkSubjectPublicKey);
            jsonWriter.WriteString("ControllerDID", _didpubkControllerDID);

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public string Tag
        { get { return _didpubkTag; } }
        public string DID
        { get { return _didpubkDID; } }
        public string Type
        { get { return _didpubkType; } }
        public string SubjectPublicKey
        { get { return _didpubkSubjectPublicKey; } }
        public string ControllerDID
        { get { return _didpubkControllerDID; } }

        public override ushort UncompressedLength 
        { 
            get
            {
                ushort tagLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didpubkTag.Length / 255d)) + _didpubkTag.Length);
                ushort didLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didpubkDID.Length / 255d)) + _didpubkDID.Length);
                ushort typeLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didpubkType.Length / 255d)) + _didpubkType.Length);
                ushort pubkeyLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didpubkSubjectPublicKey.Length / 255d)) + _didpubkSubjectPublicKey.Length);
                ushort controllerLength = Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_didpubkControllerDID.Length / 255d)) + _didpubkControllerDID.Length);
                return (ushort)(tagLength + didLength + typeLength + pubkeyLength + controllerLength);
            }
        }

        #endregion
    }
}
