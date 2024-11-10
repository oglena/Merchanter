using System.Collections;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Merchanter.Classes.Settings {
    public class DBSetting :IEnumerable {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string group_name { get; set; }
        public string? description { get; set; }
        public DateTime update_date { get; set; }

        public static string Decrypt( string _text ) {
            string decryption_key = "420A8A65DA156D24EE2A093277530142"; //ConfigurationManager.AppSettings[ "Secret" ] ?? throw new InvalidOperationException( "Decryption key not found in configuration." );
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash( Encoding.UTF8.GetBytes( decryption_key ) );
            byte[] iv = new byte[ 16 ] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            return DecryptString( _text, key, iv );
        }

        private static string DecryptString( string cipherText, byte[] key, byte[] iv ) {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;

            // Set key and IV
            encryptor.Key = key;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream( memoryStream, aesDecryptor, CryptoStreamMode.Write );

            // Will contain decrypted plaintext
            string plainText = string.Empty;

            try {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String( cipherText );

                // Decrypt the input ciphertext string
                cryptoStream.Write( cipherBytes, 0, cipherBytes.Length );

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.UTF8.GetString( plainBytes, 0, plainBytes.Length );
            } finally {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
        }

        public static string Encrypt( string plainText ) {
            string encryption_key = "420A8A65DA156D24EE2A093277530142"; //ConfigurationManager.AppSettings[ "Secret" ] ?? throw new InvalidOperationException( "Decryption key not found in configuration." );
            SHA256 mySHA256 = SHA256.Create();
            byte[] key = mySHA256.ComputeHash( Encoding.UTF8.GetBytes( encryption_key ) );
            byte[] iv = new byte[ 16 ] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
            return EncryptString( plainText, key, iv );
        }

        private static string EncryptString( string plainText, byte[] key, byte[] iv ) {
            using( Aes encryptor = Aes.Create() ) {
                encryptor.Mode = CipherMode.CBC;
                encryptor.Key = key;
                encryptor.IV = iv;

                using( MemoryStream memoryStream = new MemoryStream() ) {
                    using( ICryptoTransform aesEncryptor = encryptor.CreateEncryptor() )
                    using( CryptoStream cryptoStream = new CryptoStream( memoryStream, aesEncryptor, CryptoStreamMode.Write ) ) {
                        byte[] plainBytes = Encoding.UTF8.GetBytes( plainText );
                        cryptoStream.Write( plainBytes, 0, plainBytes.Length );
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String( memoryStream.ToArray() );
                    }
                }
            }
        }

        public IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
