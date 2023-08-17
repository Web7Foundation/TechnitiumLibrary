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
using static TechnitiumLibrary.Net.Dns.ResourceRecords.DIDCommComponents;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DnsDIDSVCRecordData : DnsResourceRecordData
    {
        #region variables

        ServiceMap _sm;

        #endregion

        #region constructor

        public DnsDIDSVCRecordData(ServiceMap sm)
        {
            _sm = sm;
        }

        public DnsDIDSVCRecordData(Stream s)
            : base(s)
        { }

        #endregion

        #region protected

        protected override void ReadRecordData(Stream s)
        {
            _sm = new ServiceMap();

            _sm.Read(s);
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            _sm.Write(s);
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is DnsDIDSVCRecordData other)
                return _sm.Equals(other._sm);

            return false;
        }

        public override int GetHashCode()
        {
            return _sm.GetHashCode();
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_sm.ToString());
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            _sm.SerializeJson(jsonWriter);

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public ServiceMap ServiceMap { get => _sm; set => _sm = value; }

        public override ushort UncompressedLength
        { get { return Convert.ToUInt16(Convert.ToInt32(Math.Ceiling(_sm.ToString().Length / 255d)) + _sm.ToString().Length); } }

        #endregion
    }
}
