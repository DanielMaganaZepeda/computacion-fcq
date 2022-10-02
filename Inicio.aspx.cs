using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputacionFCQ
{
    public partial class Inicio : System.Web.UI.Page
    {
        readonly SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ServerSide", $"console.log('Se cargar ServerSide');", true);
            
            if (!IsPostBack)
            {
                try
                {
                    //Cargar carreras
                    SqlCommand cmd = new SqlCommand("select nombre from carrera order by id", conexion);
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list_carreras.Items.Add(reader["nombre"].ToString());
                    }
                    reader.Close();
                    cmd.Connection.Close();

                    ActualizarSalas(null,null);
                    actualizarComputadoras();
                    actualizarProgramas();
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"console.log('{ex.Message}');", true);
                }
            }
            else
            {
                if (Request.QueryString.ToString() != "")
                {
                    if (Request.QueryString["tipo"] == "finalizar")
                    {
                        //FinalizarSesion(Request.QueryString["matricula"]);
                        
                        return;
                    }
                }
                actualizarInfoSalas();
            }
        }

        protected void FinalizarSesion(object sender, EventArgs e)
        {
            try
            {
                string matricula = field_matricula.Value;
                string usuario_id = Consultar($"select id from usuario where matricula='{matricula}'", "id");
                if (Consultar($"select id from sesion where usuario_id='{usuario_id}' and fecha_fin is null", "id") == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "no_existe", $"alert('ERROR: El usuario con la matricula introducida no tiene una sesión activa');", true);
                    return;
                }
                SqlCommand cmd = new SqlCommand($"update sesion set fecha_fin=getdate() where usuario_id='{usuario_id}' and fecha_fin is null", conexion);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                actualizar_sesiones(null,null);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "finalizada", $"alert('La sesión del usuario con la matricula {matricula} se ha finalizado con éxito');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", $"console.log('{ex.Message}');", true);
            }
        }

        //Carga las salas disponbiles en el DropDownList
        protected void ActualizarSalas(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select id from sala where id not in (select sala.id from reservacion join sala on sala.id=reservacion.sala_id where " +
                        "fecha_inicio <= getdate() and fecha_fin > getdate())", conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                list_sala.Items.Clear();
                while (reader.Read())
                {
                    list_sala.Items.Add(reader["id"].ToString());
                }
                reader.Close();
                cmd.Connection.Close();
                actualizarComputadoras();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "errorActualizarSalas", $"console.log('{ex.Message}');", true);
            }
        }

        protected void actualizarInfoSalas()
        {
            try
            {
                DateTime dt;
                List<Label> lista = new List<Label>();
                string[] s = new string[5];
                lista.Add(info1); lista.Add(info2); lista.Add(info3); lista.Add(info4); lista.Add(info5);

                int i = 1;
                foreach(Label info in lista)
                {
                    try
                    {
                        dt = Convert.ToDateTime(Consultar($"select fecha_inicio from reservacion where fecha_inicio > '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 00:00:00' " +
                            $"and fecha_inicio < '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 23:59:59' and sala_id={i} and activa=1 order by fecha_inicio","fecha_inicio"));

                        //SI LA FECHA DE INICIO ES PASADA
                        if(dt <= DateTime.Now)
                        {
                            dt = Convert.ToDateTime(Consultar($"select fecha_fin from reservacion where fecha_inicio > '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 00:00:00' " +
                            $"and fecha_inicio < '{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day} 23:59:59' and sala_id={i}  and activa=1 order by fecha_inicio", "fecha_fin"));
                            // SI LA FECHA DE FIN ES FUTURA ENTONCES ESTA OCUPADA TODAVIA
                            if (dt > DateTime.Now)
                            {
                                s[i - 1] = "background:LightPink; color:Brown;";
                                if (dt.Hour == 22)
                                    info.Text = $"CERRADA: Disponible nuevamente hasta mañana";
                                else
                                    info.Text = $"CERRADA: Disponible nuevamente hasta las {dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";
                            }
                            //SI LA FECHA DE FIN ES PASADA ENTONCES YA NO HAY SESIONES EN EL RESTO DEL DIA
                            else
                            {
                                s[i - 1] = "background:lightgreen; color:darkgreen;";
                                info.Text = "Disponible por lo que resta del día";
                            }
                        }
                        else
                        {
                            //SI FALTA UNA HORA O MENOS
                            if (dt - DateTime.Now <= new TimeSpan(1, 0, 0))
                            {

                                s[i - 1] = "background:PaleGoldenRod; color:Brown;";
                                info.Text = $"Disponible hasta las {dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";
                            }
                            else
                            {
                                s[i - 1] = "background:lightgreen; color:darkgreen;";
                                info.Text = $"Disponible hasta las {dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";
                            }
                        }
                    }
                    catch
                    {
                        s[i-1] = "background:lightgreen; color:darkgreen;";
                        info.Text = "Disponible por lo que resta del día";
                    }

                    i++;
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"agregarInfoSala('{s[0]}','{s[1]}','{s[2]}','{s[3]}','{s[4]}')", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"console.log('{ex.Message}');", true);
            }
        }

        protected void actualizar_sesiones(object sender, EventArgs e)
        {
            actualizarInfoSalas();
            try
            {
                //Se vacia el contenido de la tabla
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CS_CrearTabla", $"document.getElementById('tbody').innerHTML = '';", true);
                //Se muestra en el label "No hay sesiones activas"
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CS_OcultarLbl", "document.getElementById('lbl_no').hidden=true;", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CS_MostrarTabla", "document.getElementById('tabla_sesiones').hidden=false;", true);

                //Se buscan las sesiones
                string query = "select sesion.id,usuario.matricula,computadora.sala_id,computadora.numero,sesion.fecha_inicio from sesion " +
                    "inner join usuario on sesion.usuario_id=usuario.id " +
                    "inner join computadora on sesion.computadora_id=computadora.id " +
                    "where sesion.fecha_fin is null " +
                    "order by fecha_inicio desc";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 1;
                string template = "";
                DateTime dt;
                while (reader.Read())
                {
                    dt = Convert.ToDateTime(reader["fecha_inicio"]);
                    template = $"<tr style=\"vertical-align: middle\"><td>{reader["matricula"]}</td><td>{reader["sala_id"]}</td><td>{reader["numero"]}</td>" +
                        $"<td><a style=\"height:35px; margin-right:10px;\">{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year}</a><a>{dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}</a></td>" +
                        $"<td><button id=\"{reader["matricula"]}\" class=\"btn btn-outline-danger\" style=\"display:flex; justify-content:center; align-items:center;\" onclick=\"finalizarSesion(this.id); return false;\">"+
                        "<i class=\"bi bi-x-circle\" style=\"margin-right:10px; font-size:14px;\" onclick=\"finalizarSesion(this.id); return false;\"></i><a>Finalizar esta sesión</a></button></td></tr>";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), $"agregar{i}", $"document.getElementById('tbody').innerHTML+='{template}'", true);
                    i++;
                }
                reader.Close();
                if (i == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CS_OcultarTabla", "document.getElementById('tabla_sesiones').hidden=true;", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CS_MostrarLbl", "document.getElementById('lbl_no').hidden=false;", true);
                }
                cmd.Connection.Close(); 
                return;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "6", $"console.log('{ex.Message}');", true);
            }
        }

        protected void validar_sesiones(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log('ERROR: No hay sesiones activas');", true);
            try
            {
                if (Consultar("select id from sesion where fecha_fin is null", "id") == "")
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "2", $"alert('ERROR: No hay sesiones activas');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "3", $"clickearBoton('btn_aux_cerrar');", true);
                return;
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "4", $"console.log('{ex.Message}');", true);
            }
        }

        protected void iniciar_sesion(object sender, EventArgs e)
        {
            string result_valid = valid_campos();
            if (result_valid == "")
            {
                try
                {
                    //Dando de alta o actualizando informacion del usuario con la matricula escrita
                    string carrera_id = Consultar($"select id from carrera where nombre = '{list_carreras.Text}'","id");

                    SqlCommand cmd;
                    if(Consultar($"select * from usuario where matricula = '{tx_matricula.Text}'", "id") != "") 
                         cmd = new SqlCommand("SP_ModificarUsuario",conexion);
                    else
                        cmd = new SqlCommand("SP_AgregarUsuario", conexion);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add("@matricula", System.Data.SqlDbType.VarChar).Value = tx_matricula.Text;
                    cmd.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar).Value = tx_nombres.Text;
                    cmd.Parameters.Add("@apellidos", System.Data.SqlDbType.VarChar).Value = tx_apellidos.Text;
                    cmd.Parameters.Add("@correo", System.Data.SqlDbType.VarChar).Value = tx_correo.Text;
                    cmd.Parameters.Add("@carrera_id", System.Data.SqlDbType.Int).Value = carrera_id;

                    if (rad_alumno.Checked)
                        cmd.Parameters.Add("@es_alumno", System.Data.SqlDbType.Bit).Value = 1;
                    else
                        cmd.Parameters.Add("@es_alumno", System.Data.SqlDbType.Bit).Value = 0;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    //Para este punto ya se actualizo la informacion en la tabla Usuario

                    string computadora_id = Consultar($"select id from computadora where numero={list_computadora.Text} and sala_id={list_sala.Text}", "id");
                    string programa_id = Consultar($"select id from programa where nombre='{list_programa.Text}'", "id");
                    string usuario_id = Consultar($"select id from usuario where matricula='{tx_matricula.Text}'", "id");

                    cmd = new SqlCommand("SP_IniciarSesion", conexion);
                    cmd.Parameters.Add("@usuario_id", System.Data.SqlDbType.VarChar).Value = usuario_id;
                    cmd.Parameters.Add("@computadora_id", System.Data.SqlDbType.VarChar).Value = computadora_id;
                    cmd.Parameters.Add("@programa_id", System.Data.SqlDbType.VarChar).Value = programa_id;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "CerrarModal", $"cerrar_modal('cerrar_modal','La sesión ha iniciado con éxito');", true);
                    actualizar_sesiones(null, null);
                    return;
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"alert(\"{ex.Message}\");", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "NoValido", $"alert(\"{result_valid}\");", true);
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

        protected string valid_campos()
        {
            tx_nombres.Text.Trim();
            while (tx_nombres.Text.Contains("  ")) tx_nombres.Text.Replace("  ", " ");

            tx_nombres.Text.Trim();
            while (tx_nombres.Text.Contains("  ")) tx_nombres.Text.Replace("  ", " ");

            if (tx_matricula.Text == "") return "Se requiere introducir una matricula";
            if (tx_nombres.Text == "") return "Se requiere introducir un nombre";
            if (tx_apellidos.Text == "") return "Se requiere introducir los apellidos";

            try
            {
                SqlCommand cmd = new SqlCommand("select sala.id from reservacion join sala on sala.id=reservacion.sala_id " +
                    "where fecha_inicio <= getdate() and fecha_fin > getdate()", conexion);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (list_sala.Text == reader["id"].ToString()) return "Esta sala no está disponible por el momento";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"console.log('{ex.Message}');", true);
            }

            try
            {
                MailAddress aux = new MailAddress(tx_correo.Text);
            }
            catch{ return "Se requiere introducir un correo válido"; }

            //Validando que el usuario no se encuentre ya en una sesion activa
            string str = Consultar($"select id from usuario where matricula = '{tx_matricula.Text}'", "id");
            if (Consultar($"select id from sesion where usuario_id={str} and fecha_fin is null", "id") != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"alert('ERROR: El usuario con la matricula {tx_matricula.Text} ya cuenta con una sesión activa');", true);
                return "ERROR: Este usuario ya cuenta con una sesion activa";
            }

            return "";
        }

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tx_matricula.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"alert('Se requiere introducir una matricula');", true);
                    return;
                }

                SqlCommand cmd = new SqlCommand($"select * from usuario where matricula = {tx_matricula.Text}",conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tx_nombres.Text = reader["nombre"].ToString();
                    tx_apellidos.Text = reader["apellidos"].ToString();
                    tx_correo.Text = reader["correo"].ToString();

                    if (reader["es_alumno"].ToString() == "1") rad_alumno.Checked = true;
                    else rad_docente.Checked = true;

                    SqlCommand cmd2 = new SqlCommand($"select nombre from carrera where id = {reader["carrera_id"]}", conexion);
                    reader.Close();
                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    reader2.Read();
                    list_carreras.SelectedValue = reader2["nombre"].ToString();
                    reader2.Close();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"alert('No se encontraron datos para esta matricula, favor de registrar al usuario');", true);
                    tx_nombres.Focus();
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"console.log('{ex.Message}');", true);
            }
        }

        protected void list_sala_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarComputadoras();
            actualizarProgramas();
        }

        protected void actualizarComputadoras()
        {
            try
            {
                list_computadora.Items.Clear();
                SqlCommand cmd = new SqlCommand($"select numero from computadora where " +
                                                $"id not in (select computadora_id from sesion where fecha_fin is null) and " +
                                                $"sala_id={list_sala.Text} and funcional=1", conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list_computadora.Items.Add(reader["numero"].ToString());
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "errorActualizarComputadoras", $"console.log('{ex.Message}');", true);
            }
        }

        protected void actualizarProgramas()
        {
            try
            {
                list_programa.Items.Clear();
                SqlCommand cmd = new SqlCommand($"select nombre from Programa join ProgramaSala on Programa.id = ProgramaSala.programa_id where sala_id={list_sala.Text} and activo=1",conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list_programa.Items.Add(reader["nombre"].ToString());
                }
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "errorActualizarProgramas", $"console.log('{ex.Message}');", true);
            }
        }

        protected void Direccionar(object sender, EventArgs e)
        {
            Response.Redirect(((Button)sender).Text + ".aspx");
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if(Timer1.Interval != 30000)
            Timer1.Interval = 30000;

            actualizar_sesiones(null,null);
        }
    }
}