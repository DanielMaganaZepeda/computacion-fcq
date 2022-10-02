using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputacionFCQ
{
    public partial class Computadoras : System.Web.UI.Page
    {
        readonly static SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select id from sala", conexion);
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list_salas.Items.Add(reader["id"].ToString());
                    }
                    reader.Close();
                    cmd.Connection.Close();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('{ex.Message}');", true);
                }
            }
            else
            {
                ActualizarComputadoras(null,null);
            }
        }

        protected void ComputadoraDetalle(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrir_modal", "$('#modal_detalle').modal('show');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('{ex.Message}');", true);
            }
        }

        protected void ActualizarReportes(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error4", $"console.log('Entro Reportes');", true);
            try
            {
                SqlCommand cmd;
                SqlDataReader reader;
                int i = 0;
                string template;
                DateTime dt;

                cmd = new SqlCommand("select sala_id,numero,detalle,fecha_creacion from reporte join computadora on reporte.computadora_id=computadora.id where fecha_solucion is null", conexion);
                cmd.Connection.Open();
                reader = cmd.ExecuteReader();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "borrar_tabla", $"document.getElementById('rep_body').innerHTML = '';", true);
                while (reader.Read())
                {
                    dt = DateTime.Parse(reader["fecha_creacion"].ToString());
                    template = $"<tr style=\"background-color:lightpink\"><td style=\"text-align:center; font-weight:500; max-heigth:200px;\">{reader["sala_id"]}</td>" +
                        $"<td style=\"text-align:center; font-weight:500;\">{reader["numero"]}</td>" +
                        $"<td style=\"max-height:100px; overflow:hidden; display:block;\">{reader["detalle"]}</td>" +
                        $"<td style=\"text-align:center;\"><a style=\"margin-right:10px;\">{dt.Day}/{dt.Month}/{dt.Year}</a><a>{dt.Hour}:{dt.Minute.ToString("D2")}</a></td></tr>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), $"agregar_rep{i}", $"document.getElementById('rep_body').innerHTML += '{template}';", true);
                    i++;
                }
                reader.Close();
                cmd.Connection.Close();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('{ex.Message} Reportes');", true);
            }
        }

        protected void ActualizarComputadoras(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('Entro Computadoras');", true);
            try
            {
                int i = 0, total = 0, no_habilitadas = 0, j;
                SqlCommand cmd;
                SqlDataReader reader;
                string tabla="10";

                string template;
                string disponible = "<div style=\"background-color: lightgreen; width:100%px; height: 30px; display: flex; justify-content:center; align-items:center; border-radius:5px;\">" +
                    "<i class=\"bi bi-check-circle\" style=\"margin-right:5px; font-size:14px;\"></i><a>Habilitada</a></div>";
                string no_disponible = "<div style=\"background-color: LightPink; width:100%px; height: 30px; display: flex; justify-content:center; align-items:center; border-radius:5px;\">" +
                    "<i class=\"bi bi-x-circle\" style=\"margin-right:5px; font-size:14px;\"></i><a>No habilitada<a/></div>";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "10", $"document.getElementById('10').innerHTML='';", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "20", $"document.getElementById('20').innerHTML='';", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "30", $"document.getElementById('30').innerHTML='';", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "40", $"document.getElementById('40').innerHTML='';", true);
                //Se consultan las computadoras en la sala seleccionada

                cmd = new SqlCommand($"select * from computadora where sala_id={list_salas.SelectedValue} order by numero",conexion);
                cmd.Connection.Open();
                reader = cmd.ExecuteReader();
                j = 0;
                while (reader.Read())
                {
                    j++;
                    if (j > 10)
                    {
                        j = 1;
                        tabla = (Convert.ToInt32(tabla) + 10).ToString();
                    }

                    string detalles = $"<button id=\"{reader["id"]}\" class=\"btn btn-outline-primary\" style=\"height: 30px; width:100%; display: flex; justify-content: center; align-items: center;\" " +
                        $"onclick=\"ComputadoraDetalle(this.id); return false;\">" +
                    "<i name=\"information-circle-outline\" class=\"bi bi-gear-wide-connected align-middle\" style =\"font-size: 16px;\"></i></button>";


                    if (reader["funcional"].ToString() == "True")
                    {
                        /*
                        template = $"<button id=\"{reader["id"]}\"  class=\"habilitada\" " +
                        $"onclick=\"ComputadoraDetalle(this.id); return false;\">" +
                        $"<i class=\"bi bi-check-circle align-middle\" name=\"information-circle-outline\" style =\"font-size: 16px; margin-right: 15px;\"></i>" +
                        $"<a>{reader["numero"]}</a></button>";
                        */
                        template = $"<tr><td style=\"vertical-align:middle;\">{reader["numero"]}</td><td style=\"width:80%; text-align:center; padding-left:5px;\">{disponible}</td><td>{detalles}</td style=\"width:10%; text-align:center;\"></tr>";
                    }
                    else
                    {
                        /*
                        template = $"<button id=\"{reader["id"]}\"  class=\"no-habilitada\" " +
                        $"onclick=\"ComputadoraDetalle(this.id); return false;\">" +
                        $"<i class=\"bi bi-x-circle align-middle\" name=\"information-circle-outline\" style =\"font-size: 16px; margin-right: 15px;\"></i>" +
                        $"<a>{reader["numero"]}</a></button>";
                        */
                        template = $"<tr><td style=\"vertical-align:middle;\">{reader["numero"]}</td><td style=\"width:80%; align:center; padding-left:5px;\">{no_disponible}</td><td>{detalles}</td style=\"width:10%; text-align:center;\"></tr>";
                        no_habilitadas++;
                    }
                    //template = $"<tr><td style=\"width:100%; padding:0px; display: flex; height:40px;\">{template}</tr></td>";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), $"agregar{i}", $"document.getElementById('{tabla}').innerHTML+='{template}'", true);
                    i++;
                    total++;
                }
                lbl_hab.Text = (total - no_habilitadas).ToString() + " Habilitadas";
                lbl_no_hab.Text = (no_habilitadas).ToString() + " No habilitadas";
                reader.Close();
                cmd.Connection.Close();
                ActualizarReportes(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error2", $"console.log('Qp');", true);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('{ex.Message} Computadoras');", true);
            }
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

        protected void list_salas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarComputadoras(null,null);
        }
    }
}