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

namespace InglesFerramenta {
    public partial class Form1 : Form {

        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter; 

        public Form1() {
            InitializeComponent();
                connection = new SqlConnection(Environment.GetEnvironmentVariable("HKEY_CURRENT_USER\\Environment", EnvironmentVariableTarget.User));
                command = new SqlCommand();
                adapter = new SqlDataAdapter();
        }

        private void btnCriar_Click(object sender, EventArgs e) {
            try {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO USUARIO (NOME,IDETIFICADOR) VALUES (@NOME,@IDETIFICADOR)";
                if(!string.IsNullOrEmpty(txtNomeCadastro.Text) && !string.IsNullOrEmpty(txtIdentificadorCadastro.Text)) {
                    command.Parameters.AddWithValue("@NOME", txtNomeCadastro.Text.ToLower());
                    command.Parameters.AddWithValue("@IDETIFICADOR", txtIdentificadorCadastro.Text.ToLower());
                    command.ExecuteNonQuery();
                    MessageBox.Show("Usuário Adicionado", "Adicionado");
                }
                else {
                    MessageBox.Show("Preencha os campos vazios","Avviso");
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
                command.Connection = connection;
                command.CommandText = "SELECT NOME,IDETIFICADOR FROM USUARIO WHERE NOME = @NOME AND IDETIFICADOR = @COD";
                if(!string.IsNullOrEmpty(txtNomeLogin.Text) && !string.IsNullOrEmpty(txtIdentificadoLogin.Text)) {
                    command.Parameters.AddWithValue("@NOME", txtNomeLogin.Text);
                    command.Parameters.AddWithValue("@COD", txtIdentificadoLogin.Text);
                    command.ExecuteNonQuery();
                    adapter.SelectCommand = command;
                }
                DataTable table =  new DataTable();
                adapter.Fill(table);

                foreach(DataRow row in table.Rows) {
                    if (row["Nome"].ToString() == txtNomeLogin.Text.ToLower() && row["IDETIFICADOR"].ToString() == txtIdentificadoLogin.Text.ToLower())  {
                        Close();
                        Thread form = new Thread(() => Application.Run(new FormPrincipal()));
                        form.Start();   
                    }
                    else {
                        MessageBox.Show("eRRADO");
                    }


                }
               
                
                
            }catch(SqlException ex) {
                MessageBox.Show(ex.Message);
            } finally {
                connection.Close();
                command.Dispose();
            }
        }
    }
}
