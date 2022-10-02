using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Xaml;

namespace ComputacionFCQ
{
    public partial class Reservaciones : System.Web.UI.Page
    {
        readonly SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
        string[] meses = new string[12];
        protected void Page_Load(object sender, EventArgs e)
        {
            meses[0] = "Enero";
            meses[1] = "Febrero";
            meses[2] = "Marzo";
            meses[3] = "Abril";
            meses[4] = "Mayo";
            meses[5] = "Junio";
            meses[6] = "Julio";
            meses[7] = "Agosto";
            meses[8] = "Septiembre";
            meses[9] = "Octubre";
            meses[10] = "Noviembre";
            meses[11] = "Diciembre";
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

                    //Cargar salas
                    cmd = new SqlCommand("select id from sala", conexion);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list_sala.Items.Add(reader["id"].ToString());
                        calendario_sala.Items.Add(reader["id"].ToString());
                    }
                    reader.Close();

                    cmd.Connection.Close();

                    actualizarProgramas();

                    DateTime dtTabla = DateTime.Now;
                    if (dtTabla.DayOfWeek != DayOfWeek.Monday)
                    {
                        while (dtTabla.DayOfWeek != DayOfWeek.Monday)
                        {
                            dtTabla = dtTabla.Subtract(new TimeSpan(1, 0, 0, 0));
                        }
                        dia.Text = dtTabla.Day.ToString();
                        mes.Text = dtTabla.Month.ToString();
                        ano.Text = dtTabla.Year.ToString();
                    }
                    else
                    {
                        dia.Text = dtTabla.Day.ToString();
                        mes.Text = dtTabla.Month.ToString();
                        ano.Text = dtTabla.Year.ToString();
                    }
                    ActualizarFechas();
                    ActualizarReservaciones(null, null);

                    //Agregar opciones a las listas de fecha de agregar evento
                    foreach (string i in meses)
                    {
                        lista_mes_unico.Items.Add(i);
                        mes_desde.Items.Add(i);
                        mes_hasta.Items.Add(i);
                        tabla_desde_mes.Items.Add(i);
                        tabla_hasta_mes.Items.Add(i);
                    }
                    ActualizarDias(tabla_desde_dia);
                    ActualizarDias(tabla_hasta_dia);
                    ActualizarDias(lista_dia_unico);
                    ActualizarDias(dia_hasta);
                    ActualizarDias(dia_desde);
                    lista_mes_unico.SelectedIndex = DateTime.Now.Month-1;

                    List<DropDownList> coleccion_inicio = new List<DropDownList>();
                    List<DropDownList> coleccion_fin = new List<DropDownList>();
                    coleccion_inicio.Add(hora_inicio_unico); coleccion_fin.Add(hora_fin_unico);
                    coleccion_inicio.Add(lu_hora_inicio); coleccion_fin.Add(lu_hora_fin);
                    coleccion_inicio.Add(ma_hora_inicio); coleccion_fin.Add(ma_hora_fin);
                    coleccion_inicio.Add(mi_hora_inicio); coleccion_fin.Add(mi_hora_fin);
                    coleccion_inicio.Add(ju_hora_inicio); coleccion_fin.Add(ju_hora_fin);
                    coleccion_inicio.Add(vi_hora_inicio); coleccion_fin.Add(vi_hora_fin);
                    coleccion_inicio.Add(sa_hora_inicio); coleccion_fin.Add(sa_hora_fin);

                    for (int i=7; i<=22; i++)
                    {
                        string str = $"{i}:00";
                        if (i == 7)
                        {
                            foreach (DropDownList j in coleccion_inicio) j.Items.Add(str);
                        }
                        if (i == 22)
                        {
                            foreach (DropDownList j in coleccion_fin) j.Items.Add(str);
                        }
                        if(i!=7 && i != 22)
                        {
                            foreach (DropDownList j in coleccion_inicio) j.Items.Add(str);
                            foreach (DropDownList j in coleccion_fin) j.Items.Add(str);
                        }
                        
                    }

                    DateTime aux = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'inicio'", "fecha"));
                    label_inicio.Text += $"({aux.Day}/{meses[aux.Month-1]}/{aux.Year})";
                    tabla_desde_inicio.Text += $" ({aux.Day}/{meses[aux.Month - 1]}/{aux.Year})";
                    aux = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'final'", "fecha"));
                    label_final.Text += $"({aux.Day}/{meses[aux.Month-1]}/{aux.Year})";
                    tabla_hasta_fin.Text += $" ({aux.Day}/{meses[aux.Month-1]}/{aux.Year})";

                    rad_Unico_Checked(null, null);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
                }
            }
            else
            {
                if (desde_otra.Checked) { mes_desde.Enabled = true; dia_desde.Enabled = true; } else { mes_desde.Enabled = false; dia_desde.Enabled = false; }
                if (hasta_otra.Checked) { mes_hasta.Enabled = true; dia_hasta.Enabled = true; } else { mes_hasta.Enabled = false; dia_hasta.Enabled = false; }
                if (cb_lu.Checked == true) { lu_hora_inicio.Enabled = true; lu_hora_fin.Enabled = true; } else { lu_hora_inicio.Enabled = false; lu_hora_fin.Enabled = false; }
                if (cb_ma.Checked == true) { ma_hora_inicio.Enabled = true; ma_hora_fin.Enabled = true; } else { ma_hora_inicio.Enabled = false; ma_hora_fin.Enabled = false; }
                if (cb_mi.Checked == true) { mi_hora_inicio.Enabled = true; mi_hora_fin.Enabled = true; } else { mi_hora_inicio.Enabled = false; mi_hora_fin.Enabled = false; }
                if (cb_ju.Checked == true) { ju_hora_inicio.Enabled = true; ju_hora_fin.Enabled = true; } else { ju_hora_inicio.Enabled = false; ju_hora_fin.Enabled = false; }
                if (cb_vi.Checked == true) { vi_hora_inicio.Enabled = true; vi_hora_fin.Enabled = true; } else { vi_hora_inicio.Enabled = false; vi_hora_fin.Enabled = false; }
                if (cb_sa.Checked == true) { sa_hora_inicio.Enabled = true; sa_hora_fin.Enabled = true; } else { sa_hora_inicio.Enabled = false; sa_hora_fin.Enabled = false; }
            }
        }

        protected void AbrirModal(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"$('#exampleModal').modal('show');", true);
                TituloModal.Text = "Hacer reservación";
                tx_matricula.Text = "";
                tx_nombres.Text = "";
                tx_apellidos.Text = "";
                list_carreras.SelectedIndex = 0;
                tx_correo.Text = "";
                rad_unico.Checked = true;
                TipoUnico.Visible = true;
                TipoFrecuente.Visible = false;
                tx_curso.Text = "";
                list_sala.SelectedIndex = 0;
                list_programa.SelectedIndex = 0;
                tx_alumnos.Text = "";
                lista_mes_unico.SelectedValue = meses[DateTime.Now.Month-1];
                lista_dia_unico.SelectedValue = DateTime.Now.Day.ToString();
                hora_inicio_unico.SelectedIndex = 0;
                hora_fin_unico.SelectedIndex = 0;
                desde_inicio.Checked = true;
                mes_desde.SelectedIndex = 0;
                dia_desde.SelectedIndex = 0;
                hasta_inicio.Checked = true;
                mes_hasta.SelectedIndex = 0;
                dia_hasta.SelectedIndex = 0;
                cb_lu.Checked = false; lu_hora_inicio.SelectedIndex = 0; lu_hora_fin.SelectedIndex = 0;
                cb_ma.Checked = false; ma_hora_inicio.SelectedIndex = 0; ma_hora_fin.SelectedIndex = 0;
                cb_mi.Checked = false; mi_hora_inicio.SelectedIndex = 0; mi_hora_fin.SelectedIndex = 0;
                cb_ju.Checked = false; ju_hora_inicio.SelectedIndex = 0; ju_hora_fin.SelectedIndex = 0;
                cb_vi.Checked = false; vi_hora_inicio.SelectedIndex = 0; vi_hora_fin.SelectedIndex = 0;
                cb_sa.Checked = false; sa_hora_inicio.SelectedIndex = 0; sa_hora_fin.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", $"console.log('{ex.Message}');", true);
            }
        }

        protected void ReservacionDetalle(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "test", $"console.log('{((Button)sender).CommandArgument}');", true);
            try
            {
                DateTime inicio_sem = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'inicio'","fecha"));
                DateTime fin_sem = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'final'", "fecha"));

                string reservacion_id = ((Button)sender).CommandArgument;

                SqlCommand cmd = new SqlCommand($"select *,reservacion.curso as reservacion_curso,frecuencia.curso as frecuencia_curso,programa.nombre as programa_nombre " +
                    $"from reservacion left join usuario on reservacion.usuario_id = usuario.id left join frecuencia on reservacion.frecuencia_id = frecuencia.id left join " +
                    $"programa on reservacion.programa_id = programa.id where reservacion.id={reservacion_id}",conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"$('#exampleModal').modal('show');", true);
                TituloModal.Text = "Detalles de la reservacion";
                tx_matricula.Text = reader["matricula"].ToString();
                tx_nombres.Text = reader["nombre"].ToString();
                tx_apellidos.Text = reader["apellidos"].ToString();
                list_carreras.SelectedIndex = Convert.ToInt32(reader["carrera_id"]) - 1;
                tx_correo.Text = reader["correo"].ToString();
                tx_alumnos.Text = reader["cantidad_alumnos"].ToString();
                list_sala.SelectedValue = reader["sala_id"].ToString();

                if (list_programa.Items.FindByText(reader["programa_nombre"].ToString()) == null)
                {
                    list_programa.Items.Add(reader["programa_nombre"].ToString());
                }
                list_programa.SelectedValue = reader["programa_nombre"].ToString();

                //Si es unica
                if (reader["frecuencia_id"] == DBNull.Value)
                {
                    rad_unico.Checked = true;
                    TipoFrecuente.Visible = false;
                    TipoUnico.Visible = true;
                    DateTime fecha_inicio = Convert.ToDateTime(reader["fecha_inicio"]);
                    DateTime fecha_fin = Convert.ToDateTime(reader["fecha_fin"]);

                    tx_curso.Text = reader["reservacion_curso"].ToString();               
                    lista_dia_unico.SelectedValue = fecha_inicio.Day.ToString();
                    lista_mes_unico.SelectedValue = meses[fecha_inicio.Month-1];
                    hora_inicio_unico.SelectedValue = $"{fecha_inicio.Hour}:00";
                    hora_fin_unico.SelectedValue = $"{fecha_fin.Hour}:00";
                }
                //Si es frecuencial
                else
                {
                    TipoFrecuente.Visible = true;
                    TipoUnico.Visible = false;
                    rad_frecuente.Checked = true;
                    DateTime periodo_inicio = Convert.ToDateTime(reader["periodo_inicio"]);
                    DateTime periodo_fin = Convert.ToDateTime(reader["periodo_fin"]);
                    tx_curso.Text = reader["frecuencia_curso"].ToString();

                    if (periodo_inicio.Date != inicio_sem.Date)
                    {
                        desde_otra.Checked = true;
                        mes_desde.SelectedValue = meses[periodo_inicio.Month-1];
                        dia_desde.SelectedValue = periodo_inicio.Day.ToString();
                    }
                    if (periodo_fin.Date != fin_sem.Date)
                    {
                        hasta_otra.Checked = true;
                        mes_hasta.SelectedValue = meses[periodo_fin.Month - 1];
                        dia_hasta.SelectedValue = periodo_fin.Day.ToString();
                    }
                    periodo_fin = periodo_inicio.AddDays(6);
                    string query = $"select fecha_inicio,fecha_fin from reservacion where frecuencia_id={reader["frecuencia_id"]} and fecha_inicio >= " +
                        $"'{periodo_inicio.Year}-{periodo_inicio.Month}-{periodo_inicio.Day} 00:00:00' and fecha_inicio<='{periodo_fin.Year}-{periodo_fin.Month}-{periodo_fin.Day} 23:00:00'";
                    reader.Close();
                    cmd = new SqlCommand(query, conexion);
                    reader = cmd.ExecuteReader();

                    DateTime fecha_inicio,fecha_fin;
                    while (reader.Read())
                    {
                        fecha_inicio = Convert.ToDateTime(reader["fecha_inicio"]);
                        fecha_fin = Convert.ToDateTime(reader["fecha_fin"]);

                        if(fecha_inicio.DayOfWeek == DayOfWeek.Monday)
                        {
                            cb_lu.Checked = true;
                            lu_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            lu_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                        if (fecha_inicio.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            cb_ma.Checked = true;
                            ma_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            ma_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                        if (fecha_inicio.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            cb_mi.Checked = true;
                            mi_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            mi_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                        if (fecha_inicio.DayOfWeek == DayOfWeek.Thursday)
                        {
                            cb_ju.Checked = true;
                            ju_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            ju_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                        if (fecha_inicio.DayOfWeek == DayOfWeek.Friday)
                        {
                            cb_vi.Checked = true;
                            vi_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            vi_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                        if (fecha_inicio.DayOfWeek == DayOfWeek.Saturday)
                        {
                            cb_sa.Checked = true;
                            sa_hora_inicio.SelectedValue = $"{fecha_inicio.Hour}:00";
                            sa_hora_fin.SelectedValue = $"{fecha_fin.Hour}:00";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error1", $"console.log('{ex.Message}');", true);
            }
        }

        protected void rad_Unico_Checked(object sender, EventArgs e)
        {
            TipoFrecuente.Visible = false;
            TipoUnico.Visible = true;
        }

        protected void rad_Frecuente_Checked(object sender, EventArgs e)
        {
            TipoUnico.Visible = false;
            TipoFrecuente.Visible = true;
        }

        protected void ActualizarReservaciones(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test1", $"console.log('actualizacion');", true);
                List<Button> lista = new List<Button>();
                List<string> dias = new List<string>() { "LU", "MA", "MI", "JU", "VI", "SA" };
                foreach(string str in dias)
                {
                    for(int j = 7; j<=21; j++)
                    {
                        lista.Add((Button)FindControl(str + j.ToString()));
                    }
                }
                foreach (Button btn in lista) btn.Visible = false;

                DateTime dt = new DateTime(Convert.ToInt32(ano.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(dia.Text));
                string fecha_inicio = $"{dt.Year}-{dt.Month}-{dt.Day} 00:00:00";
                dt=dt.AddDays(5);
                string fecha_fin = $"{dt.Year}-{dt.Month}-{dt.Day} 23:00:00";

                SqlCommand cmd = new SqlCommand("select reservacion.id,nombre,apellidos,reservacion.curso as reservacion_curso,frecuencia.curso as frecuencia_curso,fecha_inicio,fecha_fin " +
                    $"from reservacion join usuario on reservacion.usuario_id = usuario.id left join Frecuencia on Reservacion.frecuencia_id = frecuencia.id " +
                    $"where fecha_inicio > '{fecha_inicio}' and fecha_inicio < '{fecha_fin}' and activa=1 and sala_id={calendario_sala.SelectedValue}", conexion);

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 1;
                while (reader.Read())
                {
                    string elemento_id = "";
                    DateTime inicio = Convert.ToDateTime(reader["fecha_inicio"].ToString());
                    DateTime fin = Convert.ToDateTime(reader["fecha_fin"].ToString());

                    if (inicio.DayOfWeek == DayOfWeek.Monday) elemento_id = "LU";
                    if (inicio.DayOfWeek == DayOfWeek.Tuesday) elemento_id = "MA";
                    if (inicio.DayOfWeek == DayOfWeek.Wednesday) elemento_id = "MI";
                    if (inicio.DayOfWeek == DayOfWeek.Thursday) elemento_id = "JU";
                    if (inicio.DayOfWeek == DayOfWeek.Friday) elemento_id = "VI";
                    if (inicio.DayOfWeek == DayOfWeek.Saturday) elemento_id = "SA";
                    
                    while(inicio < fin)
                    {
                        ((Button)FindControl(elemento_id + inicio.Hour)).Visible = true;
                        ((Button)FindControl(elemento_id + inicio.Hour)).Text = $"Nombre:\n{reader["nombre"]} {reader["apellidos"]} \n\n";
                        if (reader["reservacion_curso"]==DBNull.Value)
                            ((Button)FindControl(elemento_id + inicio.Hour)).Text += "Curso:\n"+reader["frecuencia_curso"].ToString();
                        else
                            ((Button)FindControl(elemento_id + inicio.Hour)).Text += "Curso:\n"+reader["reservacion_curso"].ToString();
                        ((Button)FindControl(elemento_id + inicio.Hour)).Style.Add("width", "100%");
                        //((Button)FindControl(elemento_id + inicio.Hour)).Style.Add("white-space", "normal");
                        ((Button)FindControl(elemento_id + inicio.Hour)).CommandArgument=reader["id"].ToString();
                        inicio = inicio.AddHours(1);
                        i++;
                    }
                }
                reader.Close();
                cmd.Connection.Close();
                return;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error1", $"console.log('{ex.Message}');", true);
            }
        }

        protected void HacerReservacion(object sender, EventArgs e)
        {
            string result_valid = valid_campos();
            if (result_valid == "")
            {
                if (rad_unico.Checked)
                {
                    try
                    {
                        //Dando de alta o actualizando informacion del usuario con la matricula escrita
                        string carrera_id = Consultar($"select id from carrera where nombre = '{list_carreras.Text}'", "id");

                        SqlCommand cmd;
                        if (Consultar($"select * from usuario where matricula = '{tx_matricula.Text}'", "id") != "")
                            cmd = new SqlCommand("SP_ModificarUsuario", conexion);
                        else
                            cmd = new SqlCommand("SP_AgregarUsuario", conexion);

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@matricula", System.Data.SqlDbType.VarChar).Value = tx_matricula.Text;
                        cmd.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar).Value = tx_nombres.Text;
                        cmd.Parameters.Add("@apellidos", System.Data.SqlDbType.VarChar).Value = tx_apellidos.Text;
                        cmd.Parameters.Add("@correo", System.Data.SqlDbType.VarChar).Value = tx_correo.Text;
                        cmd.Parameters.Add("@carrera_id", System.Data.SqlDbType.Int).Value = carrera_id;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();

                        cmd = new SqlCommand($"select usuario.id as usuario_id,programa.id as programa_id from usuario,programa where " +
                            $"usuario.matricula='{tx_matricula.Text}' and programa.nombre='{list_programa.Text}'", conexion);
                        cmd.Connection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        SqlCommand cmd2 = new SqlCommand("SP_AgregarReservacion", conexion);
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd2.Parameters.Add("@usuario_id", System.Data.SqlDbType.VarChar).Value = reader["usuario_id"].ToString();
                        cmd2.Parameters.Add("@fecha_inicio", System.Data.SqlDbType.DateTime).Value = DateTime.Now.Year.ToString() + "-" + (lista_mes_unico.SelectedIndex + 1).ToString() +
                            "-" + (lista_dia_unico.SelectedIndex + 1).ToString() + " " + hora_inicio_unico.Text + ":00";
                        cmd2.Parameters.Add("@fecha_fin", System.Data.SqlDbType.DateTime).Value = DateTime.Now.Year.ToString() + "-" + (lista_mes_unico.SelectedIndex + 1).ToString() +
                            "-" + (lista_dia_unico.SelectedIndex + 1).ToString() + " " + hora_fin_unico.Text + ":00";
                        cmd2.Parameters.Add("@sala_id", System.Data.SqlDbType.Int).Value = list_sala.Text;
                        cmd2.Parameters.Add("@programa_id", System.Data.SqlDbType.VarChar).Value = reader["programa_id"].ToString();
                        cmd2.Parameters.Add("@cantidad_alumnos", System.Data.SqlDbType.VarChar).Value = tx_alumnos.Text;
                        cmd2.Parameters.Add("@curso", System.Data.SqlDbType.VarChar).Value = tx_curso.Text;
                        cmd2.Parameters.Add("@frecuencia_id", System.Data.SqlDbType.Int).Value = DBNull.Value;

                        reader.Close();
                        cmd2.ExecuteNonQuery();
                        cmd.Connection.Close();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"cerrar_modal('cerrar_modal','La reservación ha sido creada con exito');", true);
                        ActualizarReservaciones(null, null);
                        return;
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "3", $"console.log(\"{ex.Message}\");", true);
                    }
                }
                else
                {
                    DateTime desde, hasta;
                    //Obteniendo fecha de inicio de periodo
                    if (desde_inicio.Checked == true)
                        desde = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'inicio'", "fecha"));
                    else
                        desde = new DateTime(DateTime.Now.Year, mes_desde.SelectedIndex + 1, Convert.ToInt32(dia_desde.SelectedValue));
                    //Obteniendo fecha de fin de periodo
                    if (hasta_inicio.Checked == true)
                        hasta = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'final'", "fecha"));
                    else
                        hasta = new DateTime(DateTime.Now.Year, mes_hasta.SelectedIndex + 1, Convert.ToInt32(dia_hasta.SelectedValue));
                    
                    SqlCommand cmd2 = new SqlCommand("SP_AgregarFrecuencia", conexion);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure; 
                    cmd2.Parameters.Add("@periodo_inicio", System.Data.SqlDbType.DateTime).Value = $"{desde.Year}-{desde.Month}-{desde.Day} 00:00:00";
                    cmd2.Parameters.Add("@periodo_fin", System.Data.SqlDbType.DateTime).Value = $"{hasta.Year}-{hasta.Month}-{hasta.Day} 00:00:00";
                    cmd2.Parameters.Add("@curso", System.Data.SqlDbType.VarChar).Value = tx_curso.Text;
                    cmd2.Connection.Open();
                    cmd2.ExecuteNonQuery();
                    cmd2.Connection.Close();

                    string frecuencia_id = Consultar("select top 1 id from frecuencia", "id");

                    //Se seleccionan los datos del usuario y programa para ahcer la reservacion
                    SqlCommand cmd = new SqlCommand($"select usuario.id as usuario_id,programa.id as programa_id from usuario,programa where " +
                            $"usuario.matricula='{tx_matricula.Text}' and programa.nombre='{list_programa.Text}'", conexion);
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    cmd2 = new SqlCommand("SP_AgregarReservacion", conexion);
                    cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd2.Parameters.Add("@usuario_id", System.Data.SqlDbType.Int).Value = reader["usuario_id"].ToString();
                    cmd2.Parameters.Add("@fecha_inicio", System.Data.SqlDbType.DateTime);
                    cmd2.Parameters.Add("@fecha_fin", System.Data.SqlDbType.DateTime);
                    cmd2.Parameters.Add("@sala_id", System.Data.SqlDbType.Int).Value = list_sala.Text;
                    cmd2.Parameters.Add("@programa_id", System.Data.SqlDbType.VarChar).Value = reader["programa_id"].ToString();
                    cmd2.Parameters.Add("@cantidad_alumnos", System.Data.SqlDbType.VarChar).Value = tx_alumnos.Text;
                    cmd2.Parameters.Add("@curso", System.Data.SqlDbType.VarChar).Value = DBNull.Value;
                    cmd2.Parameters.Add("@frecuencia_id", System.Data.SqlDbType.Int).Value = frecuencia_id;

                    reader.Close();

                    while (desde <= hasta)
                    {
                        if (cb_lu.Checked && desde.DayOfWeek == DayOfWeek.Monday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value =$"{desde.Year}-{desde.Month}-{desde.Day} {lu_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {lu_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        if (cb_ma.Checked && desde.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {ma_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {ma_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        if (cb_mi.Checked && desde.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {mi_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {mi_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        if (cb_ju.Checked && desde.DayOfWeek == DayOfWeek.Thursday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {ju_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {ju_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        if (cb_vi.Checked && desde.DayOfWeek == DayOfWeek.Friday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {vi_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {vi_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        if (cb_sa.Checked && desde.DayOfWeek == DayOfWeek.Saturday)
                        {
                            cmd2.Parameters["@fecha_inicio"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {sa_hora_inicio.SelectedValue}:00";
                            cmd2.Parameters["@fecha_fin"].Value = $"{desde.Year}-{desde.Month}-{desde.Day} {sa_hora_fin.SelectedValue}:00";
                            cmd2.ExecuteNonQuery();
                        }
                        desde = desde.AddDays(1);
                    }

                    cmd.Connection.Close();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"cerrar_modal('cerrar_modal','La reservación ha sido creada con exito');", true);
                    ActualizarReservaciones(null, null);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "3", $"alert('{result_valid}');", true);
            }
        }

        private string ValidarDisponibilidad(DateTime desde,DropDownList elemento1,DropDownList elemento2)
        {
            string str1 = $"{desde.Year}-{desde.Month}-{desde.Day} {elemento1.SelectedValue}:00";
            string str2 = $"{desde.Year}-{desde.Month}-{desde.Day} {elemento2.SelectedValue}:00";
            string reservacion_id = Consultar($"select id from reservacion where sala_id={list_sala.SelectedValue} and activa=1 and " +
                                   $"((fecha_inicio>='{str1}' and fecha_inicio<'{str2}') or " +
                                   $"(fecha_fin>'{str1}' and fecha_fin<='{str2}') or (fecha_inicio<'{str1}' and fecha_fin>'{str2}'))", "id");
            if (reservacion_id != "")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select nombre,apellidos,fecha_inicio,fecha_fin,curso from reservacion join usuario on " +
                                                   $"reservacion.usuario_id=usuario.id where reservacion.id={reservacion_id}", conexion);
                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    DateTime inicio = Convert.ToDateTime(reader["fecha_inicio"].ToString());
                    DateTime fin = Convert.ToDateTime(reader["fecha_fin"].ToString());

                    return $"No se pudo realizar la reservacion: Se empalma con la reservación hecha por {reader["nombre"]} {reader["apellidos"]} con la descripción " +
                           $"\"{reader["curso"]}\" con fecha {inicio.Day}/{inicio.Month}/{inicio.Year} de {inicio.Hour}:00 a {fin.Hour}:00";
                }
                catch(Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
                }
            }
            return reservacion_id;
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
            if (tx_curso.Text == "") return "Se requiere introducir el nombre del curso";
            if (tx_alumnos.Text == "") return "Se requiere introducir la cantidad de alumnos";

            try
            {
                MailAddress aux = new MailAddress(tx_correo.Text);
            }
            catch { return "Se requiere introducir un correo válido"; }

            //Validacion para reservaciones UNICAS
            if (rad_unico.Checked == true)
            {
                try
                {
                    DateTime inicio = new DateTime(DateTime.Now.Year, lista_mes_unico.SelectedIndex + 1, Convert.ToInt32(lista_dia_unico.SelectedValue),
                        Convert.ToInt32(hora_inicio_unico.SelectedValue.Substring(0, hora_inicio_unico.SelectedValue.IndexOf(':'))), 0, 0);

                    DateTime fin = new DateTime(DateTime.Now.Year, lista_mes_unico.SelectedIndex + 1, Convert.ToInt32(lista_dia_unico.SelectedValue),
                        Convert.ToInt32(hora_fin_unico.SelectedValue.Substring(0, hora_fin_unico.SelectedValue.IndexOf(':'))), 0, 0);

                    if (inicio < DateTime.Now) return "La fecha de inicio de la reservación es pasada";
                    if (fin <= inicio) return "La fecha de fin de la reservación debe ser futura a la fecha de inicio";
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log(\"{ex.Message}\");", true);
                    return "error";
                }
            }
            //Valudacion para reservaciones FRECUENTES
            else
            {
                try
                {
                    if(cb_lu.Checked==false && cb_ma.Checked==false && cb_mi.Checked==false && cb_ju.Checked==false && cb_vi.Checked==false && cb_sa.Checked == false)
                        return "Debe seleccionar al menos un dia de la semana en la frecuencia";

                    DateTime desde, hasta;
                    //Obteniendo fecha de inicio de periodo
                    if (desde_inicio.Checked == true)
                        desde = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'inicio'","fecha"));
                    else
                        desde = new DateTime(DateTime.Now.Year, mes_desde.SelectedIndex + 1, Convert.ToInt32(dia_desde.SelectedValue));
                    //Obteniendo fecha de fin de periodo
                    if (hasta_inicio.Checked == true)
                        hasta = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'final'", "fecha"));
                    else
                        hasta = new DateTime(DateTime.Now.Year, mes_hasta.SelectedIndex + 1, Convert.ToInt32(dia_hasta.SelectedValue));
                    
                    if(hasta <= desde)
                        return "La fecha de fin de la frecuencia debe ser futura a la fecha de inicio";

                    //Validacion de fechas particulares
                    DropDownList elemento1=new DropDownList(), elemento2=new DropDownList();
                    string aux="";
                    if (cb_lu.Checked == true)
                    {
                        elemento1 = lu_hora_inicio; elemento2 = lu_hora_fin; aux = "Lunes";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    if (cb_ma.Checked == true)
                    {
                        elemento1 = ma_hora_inicio; elemento2 = ma_hora_fin; aux = "Martes";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    if (cb_mi.Checked == true)
                    {
                        elemento1 = mi_hora_inicio; elemento2 = mi_hora_fin; aux = "Miercoles";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    if (cb_ju.Checked == true)
                    { 
                        elemento1 = ju_hora_inicio; elemento2 = ju_hora_fin; aux = "Jueves";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    if (cb_vi.Checked == true) 
                    { 
                        elemento1 = vi_hora_inicio; elemento2 = vi_hora_fin; aux = "Viernes";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    if (cb_sa.Checked == true) 
                    { 
                        elemento1 = sa_hora_inicio; elemento2 = sa_hora_fin; aux = "Sabado";
                        aux = validarDiaIndividual(elemento1, elemento2, aux); if (aux != "") return aux;
                    }
                    //Validando que no haya reservaciones en los dias establecidos
                    //Obteniendo fecha de inicio de periodo
                    if (desde_inicio.Checked == true)
                        desde = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'inicio'", "fecha"));
                    else
                        desde = new DateTime(DateTime.Now.Year, mes_desde.SelectedIndex + 1, Convert.ToInt32(dia_desde.SelectedValue));
                    //Obteniendo fecha de fin de periodo
                    if (hasta_inicio.Checked == true)
                        hasta = Convert.ToDateTime(Consultar("select fecha from fecha where nombre = 'final'", "fecha"));
                    else
                        hasta = new DateTime(DateTime.Now.Year, mes_hasta.SelectedIndex + 1, Convert.ToInt32(dia_hasta.SelectedValue));

                    while (desde <= hasta)
                    {
                        if (cb_lu.Checked && desde.DayOfWeek == DayOfWeek.Monday)
                        {
                            aux = ValidarDisponibilidad(desde, lu_hora_inicio, lu_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        if (cb_ma.Checked && desde.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            aux = ValidarDisponibilidad(desde, ma_hora_inicio, ma_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        if (cb_mi.Checked && desde.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            aux = ValidarDisponibilidad(desde, mi_hora_inicio, mi_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        if (cb_ju.Checked && desde.DayOfWeek == DayOfWeek.Thursday)
                        {
                            aux = ValidarDisponibilidad(desde, ju_hora_inicio, ju_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        if (cb_vi.Checked && desde.DayOfWeek == DayOfWeek.Friday)
                        {
                            aux = ValidarDisponibilidad(desde, vi_hora_inicio, vi_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        if (cb_sa.Checked && desde.DayOfWeek == DayOfWeek.Saturday)
                        {
                            aux = ValidarDisponibilidad(desde, sa_hora_inicio, sa_hora_fin);
                            if (aux != "")
                                return aux;
                        }
                        desde = desde.AddDays(1);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
                    return "error";
                }
            }

            return "";
        }

        private string validarDiaIndividual(DropDownList elemento1, DropDownList elemento2, string aux)
        {
            DateTime inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                            Convert.ToInt32(elemento1.SelectedValue.Substring(0, elemento1.SelectedValue.IndexOf(':'))), 0, 0);
            DateTime fin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                Convert.ToInt32(elemento2.SelectedValue.Substring(0, elemento2.SelectedValue.IndexOf(':'))), 0, 0);
            if (fin <= inicio) return $"{aux}: La hora de fin de la reservacion debe ser futura a la fecha de inicio";
            else return "";
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

        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tx_matricula.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"alert('Se requiere introducir una matricula');", true);
                    return;
                }

                SqlCommand cmd = new SqlCommand($"select * from usuario where matricula = {tx_matricula.Text}", conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tx_nombres.Text = reader["nombre"].ToString();
                    tx_apellidos.Text = reader["apellidos"].ToString();
                    tx_correo.Text = reader["correo"].ToString();

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

        protected void lista_mes_unico_onSelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarDias(lista_dia_unico);
        }

        protected void ActualizarDias(DropDownList lista)
        {
            lista.Items.Clear();
            int mes_actual;
            DateTime dt = new DateTime(DateTime.Now.Year,lista_mes_unico.SelectedIndex + 1, 1);
            mes_actual = lista_mes_unico.SelectedIndex + 1;

            while (dt.Month == mes_actual)
            {
               lista.Items.Add(dt.Day.ToString());
               dt=dt.AddDays(1);
            }
        }

        protected void ActualizarFechas ()
        {
            try
            {
                DateTime aux = new DateTime(Convert.ToInt32(ano.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(dia.Text));
                calendario.Text = meses[aux.Month-1];
                HLU.Text = "Lunes " + aux.Day; aux=aux.AddDays(1);
                HMA.Text = "Martes " + aux.Day; aux=aux.AddDays(1);
                HMI.Text = "Miercoles " + aux.Day; aux =aux.AddDays(1);
                HJU.Text = "Jueves " + aux.Day; aux=aux.AddDays(1);
                HVI.Text = "Viernes " + aux.Day; aux=aux.AddDays(1);
                HSA.Text = "Sabado " + aux.Day; 
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
            }
        }

        protected void SemanaSiguiente(object sender, EventArgs e)
        {
            try
            {
                DateTime dtTabla = new DateTime(Convert.ToInt32(ano.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(dia.Text));
                dtTabla = dtTabla.AddDays(7);

                dia.Text = dtTabla.Day.ToString();
                mes.Text = dtTabla.Month.ToString();
                ano.Text = dtTabla.Year.ToString();

                ActualizarFechas();
                ActualizarReservaciones(null,null);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "2", $"console.log('{ex.Message}');", true);
            }
        }

        protected void SemanaAnterior(object sender, EventArgs e)
        {
            try
            {
                DateTime dtTabla = new DateTime(Convert.ToInt32(ano.Text), Convert.ToInt32(mes.Text), Convert.ToInt32(dia.Text));
                for (int i = 0; i < 7; i++)
                {
                    dtTabla = dtTabla.Subtract(new TimeSpan(1, 0, 0, 0));
                }
                
                dia.Text = dtTabla.Day.ToString();
                mes.Text = dtTabla.Month.ToString();
                ano.Text = dtTabla.Year.ToString();

                ActualizarFechas();
                ActualizarReservaciones(null, null);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "2", $"console.log('{ex.Message}');", true);
            }
        }

        protected void list_sala_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizarProgramas();
        }

        protected void actualizarProgramas()
        {
            try
            {
                list_programa.Items.Clear();
                SqlCommand cmd = new SqlCommand($"select nombre from Programa join ProgramaSala on Programa.id = ProgramaSala.programa_id where sala_id={list_sala.Text} and activo=1", conexion);
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", $"console.log('{ex.Message}');", true);
            }
        }
    }
}