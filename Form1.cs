using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Security.Policy;

namespace InglesFerramenta {
    public partial class Form1 : Form {
        Security srcSecurity;   
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter; 

        public Form1() {
            InitializeComponent();
                connection = new SqlConnection(Environment.GetEnvironmentVariable("HKEY_CURRENT_USER\\Environment", EnvironmentVariableTarget.User));             
                srcSecurity = new Security();                        
        }
        private void btnCriar_Click(object sender, EventArgs e) {
            try {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO USUARIO (NOME,IDENTIFICADOR) VALUES (@NOME,@IDENTIFICADOR)";
                if (!string.IsNullOrEmpty(txtNomeCadastro.Text) && !string.IsNullOrEmpty(txtIdentificadorCadastro.Text)) {
                    command.Parameters.AddWithValue("@NOME", txtNomeCadastro.Text.ToLower());
                    command.Parameters.AddWithValue("@IDENTIFICADOR", srcSecurity.RetorneHash(txtIdentificadorCadastro.Text));
                    command.ExecuteNonQuery();
                    MessageBox.Show("Usuário Adicionado", "Adicionado");
                }
            } catch(SqlException ex) {
                MessageBox.Show(ex.Message, "Erro SQL");
            } finally {
                connection.Close();
                command.Dispose();
                txtIdentificadorCadastro.Text = "";
                txtNomeCadastro.Text = "";
            }        
        }

        private void btnEntrar_Click(object sender, EventArgs e) {
            try {
                connection.Open();
                string query = "SELECT NOME,IDENTIFICADOR FROM USUARIO WHERE NOME = @NAME and IDENTIFICADOR=@COD";
                command = new SqlCommand(query, connection);
                if (!string.IsNullOrEmpty(txtNomeLogin.Text) && !string.IsNullOrEmpty(txtIdentificadoLogin.Text)) {
                    command.Parameters.AddWithValue("@Name", txtNomeLogin.Text.ToLower());
                    command.Parameters.AddWithValue("@cod", srcSecurity.RetorneHash(txtIdentificadoLogin.Text));
                }else
                { MessageBox.Show("Campos Vazios");
                    return;
                }
                SqlDataReader reader = command.ExecuteReader();             
                if(reader.Read()) {
                    string dtNome = reader.GetString(0);
                    string dtSenha = reader.GetString(1);
                    string input = srcSecurity.RetorneHash(txtIdentificadoLogin.Text);
                    if (txtNomeLogin.Text == dtNome && input.Equals(dtSenha,StringComparison.OrdinalIgnoreCase)) {
                        Close();
                        Thread thread = new Thread(() => Application.Run(new FormPrincipal()));
                        thread.Start();
                    }
                    else {
                        MessageBox.Show("Senha Erradas", "Aviso");
                        return;
                    }                  
                }                                                                    
            }catch(SqlException ex) {
                MessageBox.Show(ex.Message);
            } finally {
                connection.Close();
                command.Dispose();
                txtIdentificadoLogin.Text = "";
                txtNomeLogin.Text = "";
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
           
        }
    }
}
