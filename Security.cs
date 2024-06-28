using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace InglesFerramenta {
    internal class Security {
        MD5 md5;

  
        

        private bool VerificacaoLogin(string input,string senha) {         
            if (input.Equals(senha, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
            else {
                return false;
            }
        }
        public bool Confirmacao(string input,string senhaMD5) {
            string senha = RetorneHash(input);
            return VerificacaoLogin(senha,senhaMD5);
        }
      

        public string RetorneHash(string input) {
            using( md5 = MD5.Create()) {
                return GetHash(input,md5);
            }
        }
        public string GetHash(string input,MD5 hash) {
            byte[] dados = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder();
             
            foreach (byte b in dados) {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }



    }
}
