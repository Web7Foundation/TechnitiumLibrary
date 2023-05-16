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
using System.Text.Json;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DnsDIDAMRecordData : DnsResourceRecordData
    {
        #region variables

        VerificationMethodMapDID _vmm;

        #endregion

        #region constructor

        public DnsDIDAMRecordData(VerificationMethodMapDID vmm)
        {
            _vmm = vmm; 
        }

        public DnsDIDAMRecordData(Stream s)
            : base(s)
        { }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            _vmm = new VerificationMethodMapDID();

            _vmm.Read(s);
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            _vmm.Write(s);
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is DnsDIDAMRecordData other)
                return _vmm.Equals(other._vmm);

            return false;
        }

        public override int GetHashCode()
        {
            return _vmm.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_vmm.ToString());
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            _vmm.SerializeJson(jsonWriter, "assertionMethod");

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public VerificationMethodMapDID VerificationMethodMap { get => _vmm; set => _vmm = value; }

        public override ushort UncompressedLength
        { get { return Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_vmm.ToString().Length / 255d)) + _vmm.ToString().Length); } }

        #endregion
    }
}
