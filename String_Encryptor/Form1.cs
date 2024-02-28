using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace String_Encryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string frase = textBox1.Text;
            string chave = textBox2.Text;

            string hash = CalcularHashSHA256(chave);

            byte[] key = Encoding.UTF8.GetBytes(hash);

            //byte[] key = Encoding.UTF8.GetBytes("chave-secreta-de-32-bytes-----12");

            //string encrypted_string = Encoding.UTF8.GetString(Encrypt(frase, key));
            byte[] encrypted_string = Encrypt(frase, key);



            textBox3.Text = Convert.ToBase64String(encrypted_string);
            //textBox3.Text = Encoding.UTF8.GetString(encrypted_string);

        }


        private void button2_Click(object sender, EventArgs e)
        {

            //byte[] iv = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFE, 0xDC, 0xBA, 0x98, 0x76, 0x54, 0x32, 0x10 };
            byte[] iv = Encoding.UTF8.GetBytes("chave-secreta-de");

            string frase_encriptada = textBox6.Text;


            string chave = textBox2.Text;

            string hash = CalcularHashSHA256(chave);

            byte[] key = Encoding.UTF8.GetBytes(hash);

            byte[] frase_em_bytes = Convert.FromBase64String(frase_encriptada);
            //byte[] frase_em_bytes = Encoding.UTF8.GetBytes(frase_encriptada);

            string textoDescriptografado = Decrypt(frase_em_bytes, key, iv);

            textBox4.Text = textoDescriptografado;

        }

        static byte[] Encrypt(string texto, byte[] chave)
        {
           
            //byte[] iv = Encoding.UTF8.GetBytes("chave-secreta-de-32-bytes-----iv");
            byte[] iv = Encoding.UTF8.GetBytes("chave-secreta-de"); // 16 caracteres
            
            AesManaged aes = new AesManaged(); // key size de 128 bits
            aes.KeySize = 256;
            aes.Key = chave; // chave tem q ter 32 caracteres
            aes.IV = iv;

            MemoryStream ms = new MemoryStream();
            CryptoStream crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] data = Encoding.UTF8.GetBytes(texto);

            crypto.Write(data, 0, data.Length);
            crypto.FlushFinalBlock();

            byte[] encrypted = ms.ToArray();




            return encrypted;
        }




        static string Decrypt(byte[] textoCriptografado, byte[] chave, byte[] iv)
        {
          
            AesManaged aes = new AesManaged(); // key size de 128 bits
            aes.KeySize = 256;
            aes.Key = chave; // chave tem q ter 32 caracteres
            aes.IV = iv;


            MemoryStream ms = new MemoryStream();
            CryptoStream crypto = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] data = textoCriptografado;

            crypto.Write(textoCriptografado, 0, textoCriptografado.Length);
            crypto.FlushFinalBlock();

            byte[] decrypted = ms.ToArray();

            return UTF8Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
        }


        static string CalcularHashSHA256(string entrada)
        {
            // Converte a entrada em bytes usando UTF-8
            byte[] bytesEntrada = Encoding.UTF8.GetBytes(entrada);

            // Cria um objeto SHA256
            using (SHA256 sha256 = SHA256.Create())
            {
                // Calcula o hash SHA-256
                byte[] hash = sha256.ComputeHash(bytesEntrada);

                // Converte os primeiros 16 bytes do hash em uma representação hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 16; i++)
                {
                    sb.Append(hash[i].ToString("x2")); // "x2" formata para hexadecimal
                }

                // Retorna os primeiros 16 caracteres do hash
                return sb.ToString();
            }
        }
    }











}

