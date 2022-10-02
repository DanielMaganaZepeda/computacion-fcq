using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputacionFCQ
{
    public partial class Programas : System.Web.UI.Page
    {
        readonly static SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                ActualizarTabla();
            }
            else
            {
            }
        }

        protected void ActualizarTabla()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select * from programa where activo=1 order by nombre", conexion);
                conexion.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int i=0;
                string template;

                ScriptManager.RegisterStartupScript(this, this.GetType(), $"borrar", $"document.getElementById('tbody').innerHTML='';", true);
                while (reader.Read())
                {
                    template = $"<tr style=\"vertical-align:middle;\"><td>{reader["nombre"]}</td>" +
                        $"<td style=\"text-align: center;\"><input type=\"checkbox\" id=\"1_{reader["id"]}\" onclick=\"CB_Click(this.id);\" class=\"cb\"/></td>" +
                        $"<td style=\"text-align: center;\"><input type=\"checkbox\" id=\"2_{reader["id"]}\" onclick=\"CB_Click(this.id);\" class=\"cb\"/></td>" +
                        $"<td style=\"text-align: center;\"><input type=\"checkbox\" id=\"3_{reader["id"]}\" onclick=\"CB_Click(this.id);\" class=\"cb\"/></td>" +
                        $"<td style=\"text-align: center;\"><input type=\"checkbox\" id=\"4_{reader["id"]}\" onclick=\"CB_Click(this.id);\" class=\"cb\"/></td>" +
                        $"<td style=\"text-align: center;\"><input type=\"checkbox\" id=\"5_{reader["id"]}\" onclick=\"CB_Click(this.id);\" class=\"cb\"/></td>" +

                        $"<td style=\"text-align:center;\">" +

                        $"<button id=\"{reader["id"]}\" onclick=\"EliminarPrograma(this.id); return false;\" class=\"btn btn-outline-danger\" " +
                        $"style=\"height:35px; padding:0px; padding-left:10px; padding-right:10px; display:flex; align-items:center; margin:0px auto;\">" +

                        $"<ion-icon name=\"trash-outline\" style=\"font-size:20px; text-align:center;\"></ion-icon>" +

                        $"<a style=\"font-family: Arial; font-size: 16px; text-align:center; flex-grow:1; margin-left:10px\">Eliminar</a></button></td></tr>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), $"agregar{i}", $"document.getElementById('tbody').innerHTML+='{template}';", true);
                    i++;
                }
                reader.Close();

                cmd = new SqlCommand($"select * from ProgramaSala", conexion);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), $"agregar{i}", $"document.getElementById('{reader["sala_id"]}_{reader["programa_id"]}').checked=true;", true);
                    i++;
                }
                reader.Close();

                conexion.Close();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
            }
        }

        protected string Consultar(string query, string campo)
        {
            try
            {
                string output;
                SqlCommand cmd = new SqlCommand(query, conexion);

                if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                    cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    output = reader[campo].ToString();
                else
                    output = "";
                cmd.Connection.Close();
                return output;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log(\"{ex.Message}\");", true);
                if (conexion.State == System.Data.ConnectionState.Open)
                    conexion.Close();
                return "";
            }
        }

        [System.Web.Services.WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public static void EliminarPrograma(string id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand($"update Programa set activo=0 where id={id}", conexion);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"delete from ProgramaSala where programa_id={id}", conexion);
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch {  }
        }

        [System.Web.Services.WebMethod()]
        [System.Web.Script.Services.ScriptMethod()]
        public static void Prueba(List<string> ids)
        {
            try
            {
                SqlCommand cmd = new SqlCommand($"SP_ActualizarProgramaSala", conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sala_id", System.Data.SqlDbType.Int);
                cmd.Parameters.Add("@programa_id", System.Data.SqlDbType.Int);
                cmd.Connection.Open();

                foreach (string id in ids)
                {
                    cmd.Parameters["@sala_id"].Value = id.Substring(0, 1);
                    cmd.Parameters["@programa_id"].Value = id.Substring(2);
                    cmd.ExecuteNonQuery();
                }

                cmd.Connection.Close();
            }
            catch
            {
            }
        }

        protected void CB_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"console.log('{((Button)sender).Text}');", true);
        }

        protected void btn_cerrar_modal_Click(object sender, EventArgs e)
        {
            try
            {
                if(Consultar($"select * from programa where nombre = '{tx_nombre.Text}' and activo=1", "nombre") != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"alert('ERROR: Ya hay un programa registrado con este nombre');", true);
                    return;
                }

                SqlCommand cmd = new SqlCommand("SP_AgregarPrograma", conexion);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar).Value = tx_nombre.Text;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "2", $"alert('El programa ha sido agregado con exito'); location.reload();", true);
            }
            catch { }
        }

        protected void btn_agregar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"$('#modalAgregarPrograma').modal('show');", true);
            tx_nombre.Text = "";
        }

        protected void btn_sesiones_Click(object sender, EventArgs e)
        {
            Response.Redirect("Inicio.aspx");
        }
        protected void btn_reservaciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reservaciones.aspx");
        }
        protected void btn_programas_Click(object sender, EventArgs e)
        {
            Response.Redirect("Programas.aspx");
        }
    }
}