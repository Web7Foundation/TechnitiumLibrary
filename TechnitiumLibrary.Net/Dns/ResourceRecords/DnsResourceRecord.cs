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

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TechnitiumLibrary.IO;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{

    #region DID document components

    // https://www.w3.org/TR/did-core/#dfn-publickeyjwk plus examples
    public class JSONKeyMap
    {
        public string crv;
        public string e;
        public string n;
        public string x;
        public string y;
        public string kty;
        public string kid;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is JSONKeyMap)) return false;

            JSONKeyMap other = (JSONKeyMap)obj;

            return (
                this.crv == other.crv &&
                this.e == other.e &&
                this.n == other.n &&
                this.x == other.x &&
                this.y == other.y &&
                this.kty == other.kty &&
                this.kid == other.kid
                );
        }
    }

    // https://www.w3.org/TR/did-core/#service-properties
    internal class ServiceMapDID
    {
        public string Id { get; set; }
        public string Type_ { get; set; }
        public string ServiceEndpoint { get; set; }
    }

    // https://www.w3.org/TR/did-core/#verification-method-properties
    public class VerificationMethodMapDID
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public string Type_ { get; set; }
        public string Controller { get; set; }
        public string PublicKeyMultibase { get; set; }
        public JSONKeyMap PublicKeyJwk { get; set; }
        public string PublicKeyBase58 { get; set; }
        public string PrivateKeyBase58 { get; set; }

        public void SerializeJson(Utf8JsonWriter jsonWriter, string objectName)
        {
            jsonWriter.WriteStartObject(objectName);

            jsonWriter.WriteString("id", Id);
            jsonWriter.WriteString("comment", Comment);
            jsonWriter.WriteString("controller", Controller);
            jsonWriter.WriteString("type_", Type_);
            jsonWriter.WriteString("publicKeyMultibase", PublicKeyMultibase);
            jsonWriter.WriteString("publicKeyBase58", PublicKeyBase58);
            jsonWriter.WriteString("privateKeyBase58", PrivateKeyBase58);

            jsonWriter.WriteStartObject("publicKeyJwk");
            jsonWriter.WriteString("crv", PublicKeyJwk.crv);
            jsonWriter.WriteString("e", PublicKeyJwk.e);
            jsonWriter.WriteString("n", PublicKeyJwk.n);
            jsonWriter.WriteString("x", PublicKeyJwk.x);
            jsonWriter.WriteString("y", PublicKeyJwk.y);
            jsonWriter.WriteString("kty", PublicKeyJwk.kty);
            jsonWriter.WriteString("kid", PublicKeyJwk.kid);
            jsonWriter.WriteEndObject();

            jsonWriter.WriteEndObject();
        }

        public void Read(Stream s)
        {
            #region read properties

            int len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            Id = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            Comment = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            Type_ = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            Controller = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyMultibase = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyBase58 = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PrivateKeyBase58 = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            #endregion

            #region read json key map properties

            PublicKeyJwk = new JSONKeyMap();

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.crv = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.e = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.n = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.x = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.y = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.kty = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            len = s.ReadByte();
            if (len < 0) throw new EndOfStreamException();
            PublicKeyJwk.kid = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

            #endregion
        }

        public void Write(Stream s)
        {
            #region write properties

            s.WriteByte(Convert.ToByte(Id.Length));
            if (Id.Length > 0) s.Write(Encoding.ASCII.GetBytes(Id));

            s.WriteByte(Convert.ToByte(Comment.Length));
            if (Comment.Length > 0) s.Write(Encoding.ASCII.GetBytes(Comment));

            s.WriteByte(Convert.ToByte(Type_.Length));
            if (Type_.Length > 0) s.Write(Encoding.ASCII.GetBytes(Type_));

            s.WriteByte(Convert.ToByte(Controller.Length));
            if (Controller.Length > 0) s.Write(Encoding.ASCII.GetBytes(Controller));

            s.WriteByte(Convert.ToByte(PublicKeyMultibase.Length));
            if (PublicKeyMultibase.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyMultibase));

            s.WriteByte(Convert.ToByte(PublicKeyBase58.Length));
            if (PublicKeyBase58.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyBase58));

            s.WriteByte(Convert.ToByte(PrivateKeyBase58.Length));
            if (PrivateKeyBase58.Length > 0) s.Write(Encoding.ASCII.GetBytes(PrivateKeyBase58));

            #endregion

            #region write json key map properties

            s.WriteByte(Convert.ToByte(PublicKeyJwk.crv.Length));
            if (PublicKeyJwk.crv.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.crv));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.e.Length));
            if (PublicKeyJwk.e.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.e));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.n.Length));
            if (PublicKeyJwk.n.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.n));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.x.Length));
            if (PublicKeyJwk.x.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.x));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.y.Length));
            if (PublicKeyJwk.y.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.y));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.kty.Length));
            if (PublicKeyJwk.kty.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.kty));

            s.WriteByte(Convert.ToByte(PublicKeyJwk.kid.Length));
            if (PublicKeyJwk.kid.Length > 0) s.Write(Encoding.ASCII.GetBytes(PublicKeyJwk.kid));

            #endregion
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();

            hash.Add(Id);
            hash.Add(Comment);
            hash.Add(Type_);
            hash.Add(Controller);
            hash.Add(PublicKeyMultibase);
            hash.Add(PublicKeyJwk);
            hash.Add(PublicKeyBase58);
            hash.Add(PrivateKeyBase58);

            return hash.ToHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is VerificationMethodMapDID other)
            {
                if (Id != other.Id)
                    return false;

                if (Comment != other.Comment)
                    return false;

                if (Type_ != other.Type_)
                    return false;

                if (Controller != other.Controller)
                    return false;

                if (PublicKeyMultibase != other.PublicKeyMultibase)
                    return false;

                if (PublicKeyBase58 != other.PublicKeyBase58)
                    return false;

                if (PrivateKeyBase58 != other.PrivateKeyBase58)
                    return false;

                if (PublicKeyJwk.Equals(other.PublicKeyJwk))
                    return true;
            }

            return true;
        }
    }

    #endregion

    public enum DnsResourceRecordType : ushort
    {
        #region DNS

        Unknown = 0,
        A = 1,
        NS = 2,
        MD = 3,
        MF = 4,
        CNAME = 5,
        SOA = 6,
        MB = 7,
        MG = 8,
        MR = 9,
        NULL = 10,
        WKS = 11,
        PTR = 12,
        HINFO = 13,
        MINFO = 14,
        MX = 15,
        TXT = 16,
        RP = 17,
        AFSDB = 18,
        X25 = 19,
        ISDN = 20,
        RT = 21,
        NSAP = 22,
        NSAP_PTR = 23,
        SIG = 24,
        KEY = 25,
        PX = 26,
        GPOS = 27,
        AAAA = 28,
        LOC = 29,
        NXT = 30,
        EID = 31,
        NIMLOC = 32,
        SRV = 33,
        ATMA = 34,
        NAPTR = 35,
        KX = 36,
        CERT = 37,
        A6 = 38,
        DNAME = 39,
        SINK = 40,
        OPT = 41,
        APL = 42,
        DS = 43,
        SSHFP = 44,
        IPSECKEY = 45,
        RRSIG = 46,
        NSEC = 47,
        DNSKEY = 48,
        DHCID = 49,
        NSEC3 = 50,
        NSEC3PARAM = 51,
        TLSA = 52,
        SMIMEA = 53,
        HIP = 55,
        NINFO = 56,
        RKEY = 57,
        TALINK = 58,
        CDS = 59,
        CDNSKEY = 60,
        OPENPGPKEY = 61,
        CSYNC = 62,
        ZONEMD = 63,
        SVCB = 64,
        HTTPS = 65,
        SPF = 99,
        UINFO = 100,
        UID = 101,
        GID = 102,
        UNSPEC = 103,
        NID = 104,
        L32 = 105,
        L64 = 106,
        LP = 107,
        EUI48 = 108,
        EUI64 = 109,
        TKEY = 249,
        TSIG = 250,
        IXFR = 251,
        AXFR = 252,
        MAILB = 253,
        MAILA = 254,
        ANY = 255,
        URI = 256,
        CAA = 257,
        AVC = 258,
        TA = 32768,
        DLV = 32769,
        ANAME = 65280, //private use - draft-ietf-dnsop-aname-04
        FWD = 65281, //private use - conditional forwarder
        APP = 65282, //private use - application

        #endregion

        #region DID

        //private use - DID resource record types
        DIDID = 65488,
        DIDPURP = 65489,
        DIDCOMM = 65490,
        DIDCTXT = 65491,
        DIDAKA = 65492,
        DIDCTLR = 65493,
        DIDVM = 65494,
        DIDAUTH = 65495,
        DIDAM = 65496,
        DIDKA = 65500,
        DIDCI = 65501,
        DIDCD = 65502,
        DIDSVC = 65503,
        DIDREL = 65504,

        #endregion

        UUBLAddress = 65472,
    }

    public enum DnsClass : ushort
    {
        IN = 1, //the Internet
        CS = 2, //the CSNET class (Obsolete - used only for examples in some obsolete RFCs)
        CH = 3, //the CHAOS class
        HS = 4, //Hesiod

        NONE = 254,
        ANY = 255
    }

    public enum DnssecStatus : byte
    {
        Unknown = 0,
        Disabled = 1,
        Secure = 2,
        Insecure = 3,
        Bogus = 4,
        Indeterminate = 5
    }

    public sealed class DnsResourceRecord : IComparable<DnsResourceRecord>
    {
        #region variables

        string _name;
        DnsResourceRecordType _type;
        DnsClass _class;
        uint _ttl;
        DnsResourceRecordData _rData;

        readonly int _datagramOffset;

        bool _setExpiry;
        bool _wasExpiryReset;
        DateTime _ttlExpires;
        DateTime _serveStaleTtlExpires;
        DnssecStatus _dnssecStatus;

        #endregion

        #region constructor

        private DnsResourceRecord()
        { }

        public DnsResourceRecord(string name, DnsResourceRecordType type, DnsClass @class, uint ttl, DnsResourceRecordData rData)
        {
            if (DnsClient.IsDomainNameUnicode(name))
                name = DnsClient.ConvertDomainNameToAscii(name);

            DnsClient.IsDomainNameValid(name, true);

            _name = name;
            _type = type;
            _class = @class;
            _ttl = ttl;
            _rData = rData;
        }

        public DnsResourceRecord(Stream s)
        {
            _datagramOffset = Convert.ToInt32(s.Position);

            _name = DnsDatagram.DeserializeDomainName(s);
            _type = (DnsResourceRecordType)DnsDatagram.ReadUInt16NetworkOrder(s);
            _class = (DnsClass)DnsDatagram.ReadUInt16NetworkOrder(s);
            _ttl = DnsDatagram.ReadUInt32NetworkOrder(s);
            _rData = ReadRecordData(s, _type);
        }

        #endregion

        #region static

        public static DnsResourceRecord ReadCacheRecordFrom(BinaryReader bR, Action<DnsResourceRecord> readTagInfo)
        {
            byte version = bR.ReadByte();
            switch (version)
            {
                case 1:
                    DnsResourceRecord record = new DnsResourceRecord();

                    record._name = bR.ReadString();
                    record._type = (DnsResourceRecordType)bR.ReadUInt16();
                    record._class = (DnsClass)bR.ReadUInt16();
                    record._ttl = bR.ReadUInt32();

                    if (bR.ReadBoolean())
                        record._rData = DnsCache.DnsSpecialCacheRecordData.ReadCacheRecordFrom(bR, readTagInfo);
                    else
                        record._rData = ReadRecordData(bR.BaseStream, record._type);

                    record._setExpiry = bR.ReadBoolean();
                    record._wasExpiryReset = bR.ReadBoolean();
                    record._ttlExpires = DateTime.UnixEpoch.AddSeconds(bR.ReadInt64());
                    record._serveStaleTtlExpires = DateTime.UnixEpoch.AddSeconds(bR.ReadInt64());
                    record._dnssecStatus = (DnssecStatus)bR.ReadByte();

                    readTagInfo(record);

                    return record;

                default:
                    throw new InvalidDataException("DnsResorceRecord cache format version not supported.");
            }
        }

        public static Dictionary<string, Dictionary<DnsResourceRecordType, List<DnsResourceRecord>>> GroupRecords(IReadOnlyCollection<DnsResourceRecord> records, bool deduplicate = false)
        {
            Dictionary<string, Dictionary<DnsResourceRecordType, List<DnsResourceRecord>>> groupedByDomainRecords = new Dictionary<string, Dictionary<DnsResourceRecordType, List<DnsResourceRecord>>>();

            foreach (DnsResourceRecord record in records)
            {
                string recordName = record.Name.ToLowerInvariant();

                if (!groupedByDomainRecords.TryGetValue(recordName, out Dictionary<DnsResourceRecordType, List<DnsResourceRecord>> groupedByTypeRecords))
                {
                    groupedByTypeRecords = new Dictionary<DnsResourceRecordType, List<DnsResourceRecord>>();
                    groupedByDomainRecords.Add(recordName, groupedByTypeRecords);
                }

                if (!groupedByTypeRecords.TryGetValue(record.Type, out List<DnsResourceRecord> groupedRecords))
                {
                    groupedRecords = new List<DnsResourceRecord>();
                    groupedByTypeRecords.Add(record.Type, groupedRecords);
                }

                if (deduplicate)
                {
                    if (!groupedRecords.Contains(record))
                        groupedRecords.Add(record);
                }
                else
                {
                    groupedRecords.Add(record);
                }
            }

            return groupedByDomainRecords;
        }

        public static bool IsRRSetExpired(IReadOnlyCollection<DnsResourceRecord> records, bool serveStale)
        {
            foreach (DnsResourceRecord record in records)
            {
                if (record.IsExpired(serveStale))
                    return true;
            }

            return false;
        }

        public static bool IsRRSetStale(IReadOnlyCollection<DnsResourceRecord> records)
        {
            foreach (DnsResourceRecord record in records)
            {
                if (record.IsStale)
                    return true;
            }

            return false;
        }

        #endregion

        #region private

        private static DnsResourceRecordData ReadRecordData(Stream s, DnsResourceRecordType type)
        {
            switch (type)
            {
                #region DNS RRs

                case DnsResourceRecordType.A:
                    return new DnsARecordData(s);

                case DnsResourceRecordType.NS:
                    return new DnsNSRecordData(s);

                case DnsResourceRecordType.CNAME:
                    return new DnsCNAMERecordData(s);

                case DnsResourceRecordType.SOA:
                    return new DnsSOARecordData(s);

                case DnsResourceRecordType.PTR:
                    return new DnsPTRRecordData(s);

                case DnsResourceRecordType.HINFO:
                    return new DnsHINFORecordData(s);

                case DnsResourceRecordType.MX:
                    return new DnsMXRecordData(s);

                case DnsResourceRecordType.TXT:
                    return new DnsTXTRecordData(s);

                case DnsResourceRecordType.AAAA:
                    return new DnsAAAARecordData(s);

                case DnsResourceRecordType.SRV:
                    return new DnsSRVRecordData(s);

                case DnsResourceRecordType.DNAME:
                    return new DnsDNAMERecordData(s);

                case DnsResourceRecordType.OPT:
                    return new DnsOPTRecordData(s);

                case DnsResourceRecordType.DS:
                    return new DnsDSRecordData(s);

                case DnsResourceRecordType.SSHFP:
                    return new DnsSSHFPRecordData(s);

                case DnsResourceRecordType.RRSIG:
                    return new DnsRRSIGRecordData(s);

                case DnsResourceRecordType.NSEC:
                    return new DnsNSECRecordData(s);

                case DnsResourceRecordType.DNSKEY:
                    return new DnsDNSKEYRecordData(s);

                case DnsResourceRecordType.NSEC3:
                    return new DnsNSEC3RecordData(s);

                case DnsResourceRecordType.NSEC3PARAM:
                    return new DnsNSEC3PARAMRecordData(s);

                case DnsResourceRecordType.TLSA:
                    return new DnsTLSARecordData(s);

                case DnsResourceRecordType.TSIG:
                    return new DnsTSIGRecordData(s);

                case DnsResourceRecordType.CAA:
                    return new DnsCAARecordData(s);

                case DnsResourceRecordType.ANAME:
                    return new DnsANAMERecordData(s);

                case DnsResourceRecordType.FWD:
                    return new DnsForwarderRecordData(s);

                case DnsResourceRecordType.APP:
                    return new DnsApplicationRecordData(s);

                #endregion

                #region DID RRs

                // single string value did RR types:
                case DnsResourceRecordType.DIDID:
                    return new DnsDIDIDRecordData(s);

                case DnsResourceRecordType.DIDPURP:
                    return new DnsDIDPURPRecordData(s);

                case DnsResourceRecordType.DIDCOMM:
                    return new DnsDIDCOMMRecordData(s);

                case DnsResourceRecordType.DIDCTXT:
                    return new DnsDIDCTXTRecordData(s);

                case DnsResourceRecordType.DIDAKA:
                    return new DnsDIDAKARecordData(s);

                case DnsResourceRecordType.DIDCTLR:
                    return new DnsDIDCTLRRecordData(s);

                // verification method map did RR types
                case DnsResourceRecordType.DIDVM:
                    return new DnsDIDVMRecordData(s);

                //case DnsResourceRecordType.DIDAUTH:
                //    return new DnsDIDAUTHRecordData(s);

                //case DnsResourceRecordType.DIDAM:
                //    return new DnsDIDAMRecordData(s);

                //case DnsResourceRecordType.DIDKA:
                //    return new DnsDIDKARecordData(s);

                //case DnsResourceRecordType.DIDCI:
                //    return new DnsDIDCIRecordData(s);

                //case DnsResourceRecordType.DIDCD:
                //    return new DnsDIDCDRecordData(s);

                // service map did RR types
                case DnsResourceRecordType.DIDSVC:
                    return new DnsDIDSVCRecordData(s);

                case DnsResourceRecordType.DIDREL:
                    return new DnsDIDRELRecordData(s);

                #endregion

                default:
                    return new DnsUnknownRecordData(s);
            }
        }

        #endregion

        #region internal

        internal void NormalizeName()
        {
            _name = _name.ToLowerInvariant();
            _rData.NormalizeName();
        }

        internal void SetDnssecStatus(DnssecStatus dnssecStatus, bool force = false)
        {
            if ((_dnssecStatus == DnssecStatus.Unknown) || force)
                _dnssecStatus = dnssecStatus;
        }

        internal void FixNameForNSEC(string wildcardName)
        {
            if (_type != DnsResourceRecordType.NSEC)
                throw new InvalidOperationException();

            _name = wildcardName;
        }

        #endregion

        #region public

        public void SetExpiry(uint minimumTtl, uint maximumTtl, uint serveStaleTtl)
        {
            if (_ttl < minimumTtl)
                _ttl = minimumTtl; //to help keep record in cache for a minimum time
            else if (_ttl > maximumTtl)
                _ttl = maximumTtl; //to help remove record from cache early

            _setExpiry = true;
            _wasExpiryReset = false;
            _ttlExpires = DateTime.UtcNow.AddSeconds(_ttl);
            _serveStaleTtlExpires = _ttlExpires.AddSeconds(serveStaleTtl);
        }

        public void ResetExpiry(int seconds)
        {
            if (!_setExpiry)
                throw new InvalidOperationException("Must call SetExpiry() before ResetExpiry().");

            _wasExpiryReset = true;
            _ttlExpires = DateTime.UtcNow.AddSeconds(seconds);
        }

        public void RemoveExpiry()
        {
            _setExpiry = false;
        }

        public bool IsExpired(bool serveStale)
        {
            if (serveStale)
                return TTL < 1u;

            return IsStale;
        }

        public void WriteTo(Stream s)
        {
            WriteTo(s, null);
        }

        public void WriteTo(Stream s, List<DnsDomainOffset> domainEntries)
        {
            DnsDatagram.SerializeDomainName(_name, s, domainEntries);
            DnsDatagram.WriteUInt16NetworkOrder((ushort)_type, s);
            DnsDatagram.WriteUInt16NetworkOrder((ushort)_class, s);
            DnsDatagram.WriteUInt32NetworkOrder(TTL, s);

            _rData.WriteTo(s, domainEntries);
        }

        public void WriteCacheRecordTo(BinaryWriter bW, Action writeTagInfo)
        {
            bW.Write((byte)1); //version

            bW.Write(_name);
            bW.Write((ushort)_type);
            bW.Write((ushort)_class);
            bW.Write(_ttl);

            if (_rData is DnsCache.DnsSpecialCacheRecordData cacheRData)
            {
                bW.Write((byte)1);
                cacheRData.WriteCacheRecordTo(bW, writeTagInfo);
            }
            else
            {
                bW.Write((byte)0);
                _rData.WriteTo(bW.BaseStream);
            }

            bW.Write(_setExpiry);
            bW.Write(_wasExpiryReset);
            bW.Write(Convert.ToInt64((_ttlExpires - DateTime.UnixEpoch).TotalSeconds));
            bW.Write(Convert.ToInt64((_serveStaleTtlExpires - DateTime.UnixEpoch).TotalSeconds));
            bW.Write((byte)_dnssecStatus);

            writeTagInfo();
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj is DnsResourceRecord other)
            {
                if (!_name.Equals(other._name, StringComparison.OrdinalIgnoreCase))
                    return false;

                if (_type != other._type)
                    return false;

                if (_class != other._class)
                    return false;

                if (_ttl != other._ttl)
                    return false;

                return _rData.Equals(other._rData);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_name, _type, _class, _ttl, _rData);
        }

        public int CompareTo(DnsResourceRecord other)
        {
            int value;

            value = DnsNSECRecordData.CanonicalComparison(_name, other._name);
            if (value != 0)
                return value;

            value = _type.CompareTo(other._type);
            if (value != 0)
                return value;

            return _ttl.CompareTo(other._ttl);
        }

        public override string ToString()
        {
            return _name.ToLowerInvariant() + ". " + _type.ToString() + " " + _class.ToString() + " " + _ttl + " " + _rData.ToString();
        }

        public void SerializeTo(Utf8JsonWriter jsonWriter)
        {
            jsonWriter.WriteStartObject();

            jsonWriter.WriteString("Name", _name);

            if (_name.Contains("xn--", StringComparison.OrdinalIgnoreCase))
                jsonWriter.WriteString("NameIDN", DnsClient.ConvertDomainNameToUnicode(_name));

            jsonWriter.WriteString("Type", _type.ToString());
            jsonWriter.WriteString("Class", _class.ToString());
            jsonWriter.WriteString("TTL", _ttl + " (" + WebUtilities.GetFormattedTime((int)_ttl) + ")");
            jsonWriter.WriteString("RDLENGTH", _rData.RDLENGTH + " bytes");

            jsonWriter.WritePropertyName("RDATA");
            _rData.SerializeTo(jsonWriter);

            jsonWriter.WriteString("DnssecStatus", _dnssecStatus.ToString());

            jsonWriter.WriteEndObject();
        }

        #endregion

        #region properties

        public string Name
        { get { return _name; } }

        public DnsResourceRecordType Type
        { get { return _type; } }

        public DnsClass Class
        { get { return _class; } }

        public uint TTL
        {
            get
            {
                if (_setExpiry)
                {
                    DateTime utcNow = DateTime.UtcNow;

                    if (utcNow > _serveStaleTtlExpires)
                        return 0u;

                    if (utcNow > _ttlExpires)
                        return 30u; //stale TTL

                    return Convert.ToUInt32((_ttlExpires - utcNow).TotalSeconds);
                }

                return _ttl;
            }
        }

        public bool IsStale
        {
            get
            {
                if (_setExpiry)
                    return DateTime.UtcNow > _ttlExpires;

                return false;
            }
        }

        public bool WasExpiryReset
        { get { return _wasExpiryReset; } }

        public uint OriginalTtlValue
        { get { return _ttl; } }

        public DnsResourceRecordData RDATA
        { get { return _rData; } }

        public int DatagramOffset
        { get { return _datagramOffset; } }

        public ushort UncompressedLength
        { get { return Convert.ToUInt16(DnsDatagram.GetSerializeDomainNameLength(_name) + 2 + 2 + 4 + 2 + _rData.UncompressedLength); } }

        public object Tag { get; set; }

        public DnssecStatus DnssecStatus
        { get { return _dnssecStatus; } }

        #endregion
    }
}
