/*
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

using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    internal class DnsDIDRELRecordData : DnsResourceRecordData
    {
        public DnsDIDRELRecordData(Stream s) : base(s)
        {
        }

        public override ushort UncompressedLength => throw new System.NotImplementedException();

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public override void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            throw new System.NotImplementedException();
        }

        protected override void ReadRecordData(Stream s)
        {
            throw new System.NotImplementedException();
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries, bool canonicalForm)
        {
            throw new System.NotImplementedException();
        }
    }
}