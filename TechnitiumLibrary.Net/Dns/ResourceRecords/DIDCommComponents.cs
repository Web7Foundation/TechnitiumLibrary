using System;
using System.IO;
using System.Text;
using System.Text.Json;
using TechnitiumLibrary.IO;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DIDCommComponents
    {
        #region DID document components

        // https://www.w3.org/TR/did-core/#service-properties
        public class ServiceMap
        {
            #region properties

            public string Id { get; set; }
            public string Comment { get; set; }
            public string Type_ { get; set; }
            public string ServiceEndpoint { get; set; }

            #endregion

            #region internal

            internal void Read(Stream s)
            {
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
                ServiceEndpoint = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;
            }

            internal void Write(Stream s)
            {
                s.WriteByte(Convert.ToByte(Id.Length));
                if (Id.Length > 0) s.Write(Encoding.ASCII.GetBytes(Id));

                s.WriteByte(Convert.ToByte(Comment.Length));
                if (Comment.Length > 0) s.Write(Encoding.ASCII.GetBytes(Comment));

                s.WriteByte(Convert.ToByte(Type_.Length));
                if (Type_.Length > 0) s.Write(Encoding.ASCII.GetBytes(Type_));

                s.WriteByte(Convert.ToByte(ServiceEndpoint.Length));
                if (ServiceEndpoint.Length > 0) s.Write(Encoding.ASCII.GetBytes(ServiceEndpoint));
            }

            #endregion

            #region public

            public void SerializeJson(Utf8JsonWriter jsonWriter)
            {
                jsonWriter.WriteStartObject("serviceMap");

                jsonWriter.WriteString("id", Id);
                jsonWriter.WriteString("comment", Comment);
                jsonWriter.WriteString("type_", Type_);
                jsonWriter.WriteString("serviceEndpoint", ServiceEndpoint);

                jsonWriter.WriteEndObject();
            }

            public override int GetHashCode()
            {
                HashCode hash = new HashCode();

                hash.Add(Id);
                hash.Add(Comment);
                hash.Add(Type_);
                hash.Add(ServiceEndpoint);

                return hash.ToHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is ServiceMap other)
                {
                    if (Id != other.Id)
                        return false;

                    if (Comment != other.Comment)
                        return false;

                    if (Type_ != other.Type_)
                        return false;

                    if (ServiceEndpoint != other.ServiceEndpoint)
                        return false;
                }

                return true;
            }

            public override string ToString()
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            }

            #endregion
        }

        // https://www.w3.org/TR/did-core/#verification-method-properties
        public class VerificationMethodMap
        {
            #region properties

            public string Id { get; set; }
            public string Comment { get; set; }
            public string Type_ { get; set; }
            public string Controller { get; set; }
            public string keyPublicJsonWebKey { get; set; }            // STRING (Json Text) Web7.TrustLibrary.Did.DIDDocumenter() - JsonWebKeyDotnet6
            public string keyPublicJsonWebKeyString { get; set; }      // STRING (Json Text)
            public string publicKeyMultibase { get; set; }             // STRING (Json Text)
            public string publicKeyJwk { get; set; }                   // STRING (Json Text) - JSONKeyMap

            #endregion

            #region internal

            internal void Read(Stream s)
            {
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
                keyPublicJsonWebKey = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;
                
                len = s.ReadByte();
                if (len < 0) throw new EndOfStreamException();
                keyPublicJsonWebKeyString = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

                len = s.ReadByte();
                if (len < 0) throw new EndOfStreamException();
                publicKeyMultibase = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;

                len = s.ReadByte();
                if (len < 0) throw new EndOfStreamException();
                publicKeyJwk = len > 0 ? Encoding.ASCII.GetString(s.ReadBytes(len)) : string.Empty;
            }

            internal void Write(Stream s)
            {
                s.WriteByte(Convert.ToByte(Id.Length));
                if (Id.Length > 0) s.Write(Encoding.ASCII.GetBytes(Id));

                s.WriteByte(Convert.ToByte(Comment.Length));
                if (Comment.Length > 0) s.Write(Encoding.ASCII.GetBytes(Comment));

                s.WriteByte(Convert.ToByte(Type_.Length));
                if (Type_.Length > 0) s.Write(Encoding.ASCII.GetBytes(Type_));

                s.WriteByte(Convert.ToByte(Controller.Length));
                if (Controller.Length > 0) s.Write(Encoding.ASCII.GetBytes(Controller));

                s.WriteByte(Convert.ToByte(keyPublicJsonWebKey.Length));
                if (keyPublicJsonWebKey.Length > 0) s.Write(Encoding.ASCII.GetBytes(keyPublicJsonWebKey));

                s.WriteByte(Convert.ToByte(keyPublicJsonWebKeyString.Length));
                if (keyPublicJsonWebKeyString.Length > 0) s.Write(Encoding.ASCII.GetBytes(keyPublicJsonWebKeyString));

                s.WriteByte(Convert.ToByte(publicKeyMultibase.Length));
                if (publicKeyMultibase.Length > 0) s.Write(Encoding.ASCII.GetBytes(publicKeyMultibase));

                s.WriteByte(Convert.ToByte(publicKeyJwk.Length));
                if (publicKeyJwk.Length > 0) s.Write(Encoding.ASCII.GetBytes(publicKeyJwk));


            }

            #endregion

            #region public

            public void SerializeJson(Utf8JsonWriter jsonWriter)
            {
                jsonWriter.WriteStartObject("verificationMethodMap");

                jsonWriter.WriteString("id", Id);
                jsonWriter.WriteString("comment", Comment);
                jsonWriter.WriteString("controller", Controller);
                jsonWriter.WriteString("type_", Type_);
                jsonWriter.WriteString("keyPublicJsonWebKey", keyPublicJsonWebKey);
                jsonWriter.WriteString("keyPublicJsonWebKeyString", keyPublicJsonWebKeyString);
                jsonWriter.WriteString("publicKeyMultibase", publicKeyMultibase);
                jsonWriter.WriteString("publicKeyJwk", publicKeyJwk);

                jsonWriter.WriteEndObject();
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
                hash.Add(keyPublicJsonWebKey);
                hash.Add(keyPublicJsonWebKeyString);
                hash.Add(publicKeyMultibase);
                hash.Add(publicKeyJwk);

                return hash.ToHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is VerificationMethodMap other)
                {
                    if (Id != other.Id)
                        return false;

                    if (Comment != other.Comment)
                        return false;

                    if (Type_ != other.Type_)
                        return false;

                    if (Controller != other.Controller)
                        return false;

                    if (keyPublicJsonWebKey != other.keyPublicJsonWebKey)
                        return false;

                    if (keyPublicJsonWebKeyString != other.keyPublicJsonWebKeyString)
                        return false;

                    if (publicKeyMultibase != other.publicKeyMultibase)
                        return false;

                    if (publicKeyJwk != other.publicKeyJwk)
                        return false;
                }

                return true;
            }

            #endregion
        }

        #endregion
    }
}
