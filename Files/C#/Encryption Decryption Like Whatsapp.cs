using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncDecLikeWhatsApp
{
    internal class Program
    {
        static void Main()
        {
            // Generate RSA keys
            using (RSA rsa = RSA.Create())
            {
                // Export public and private keys
                RSAParameters publicKeyParams = rsa.ExportParameters(false);
                RSAParameters privateKeyParams = rsa.ExportParameters(true);

                // Convert RSAParameters to base64 strings
                string publicKey = Convert.ToBase64String(ExportParametersToBytes(publicKeyParams, false));
                string privateKey = Convert.ToBase64String(ExportParametersToBytes(privateKeyParams, true));

                // Message to encrypt
                string message = "Hello, secure world!";
                Console.WriteLine("Original Message: " + message);

                // Encrypt the message using the public key
                string encryptedMessage = EncryptMessage(message, publicKey);
                Console.WriteLine("Encrypted Message: " + encryptedMessage);

                // Decrypt the message using the private key
                string decryptedMessage = DecryptMessage(encryptedMessage, privateKey);
                Console.WriteLine("Decrypted Message: " + decryptedMessage);
            }
        }

        static string EncryptMessage(string message, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(ImportParametersFromBytes(Convert.FromBase64String(publicKey), false));
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                // Use PKCS#1 v1.5 padding as a fallback
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        static string DecryptMessage(string encryptedMessage, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportParameters(ImportParametersFromBytes(Convert.FromBase64String(privateKey), true));
                byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);

                // Use PKCS#1 v1.5 padding as a fallback
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        static byte[] ExportParametersToBytes(RSAParameters parameters, bool includePrivateParameters)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(parameters.Exponent?.Length ?? 0);
                    if (parameters.Exponent != null) writer.Write(parameters.Exponent);
                    writer.Write(parameters.Modulus?.Length ?? 0);
                    if (parameters.Modulus != null) writer.Write(parameters.Modulus);

                    if (includePrivateParameters)
                    {
                        writer.Write(parameters.D?.Length ?? 0);
                        if (parameters.D != null) writer.Write(parameters.D);
                        writer.Write(parameters.P?.Length ?? 0);
                        if (parameters.P != null) writer.Write(parameters.P);
                        writer.Write(parameters.Q?.Length ?? 0);
                        if (parameters.Q != null) writer.Write(parameters.Q);
                        writer.Write(parameters.DP?.Length ?? 0);
                        if (parameters.DP != null) writer.Write(parameters.DP);
                        writer.Write(parameters.DQ?.Length ?? 0);
                        if (parameters.DQ != null) writer.Write(parameters.DQ);
                        writer.Write(parameters.InverseQ?.Length ?? 0);
                        if (parameters.InverseQ != null) writer.Write(parameters.InverseQ);
                    }
                }
                return ms.ToArray();
            }
        }

        static RSAParameters ImportParametersFromBytes(byte[] bytes, bool includePrivateParameters)
        {
            RSAParameters parameters = new RSAParameters();
            using (var ms = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(ms))
                {
                    int length = reader.ReadInt32();
                    parameters.Exponent = length > 0 ? reader.ReadBytes(length) : null;
                    length = reader.ReadInt32();
                    parameters.Modulus = length > 0 ? reader.ReadBytes(length) : null;

                    if (includePrivateParameters)
                    {
                        length = reader.ReadInt32();
                        parameters.D = length > 0 ? reader.ReadBytes(length) : null;
                        length = reader.ReadInt32();
                        parameters.P = length > 0 ? reader.ReadBytes(length) : null;
                        length = reader.ReadInt32();
                        parameters.Q = length > 0 ? reader.ReadBytes(length) : null;
                        length = reader.ReadInt32();
                        parameters.DP = length > 0 ? reader.ReadBytes(length) : null;
                        length = reader.ReadInt32();
                        parameters.DQ = length > 0 ? reader.ReadBytes(length) : null;
                        length = reader.ReadInt32();
                        parameters.InverseQ = length > 0 ? reader.ReadBytes(length) : null;
                    }
                }
            }
            return parameters;
        }
    }
}
