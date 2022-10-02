using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComputacionFCQ
{
    public partial class Reportes : System.Web.UI.Page
    {
        readonly static SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (Request.QueryString.ToString() != "")
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"alert('{Request.QueryString.ToString()}');", true);
                GenerarReporte(Request.QueryString["tipo"],Request.QueryString["desde"],Request.QueryString["hasta"]);
            }
            if (!IsPostBack)
            {
                string[] meses = new string[]{"Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre"};

                SqlCommand cmd = new SqlCommand("select fecha from fecha where nombre = 'inicio'", conexion);
                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                DateTime dt = Convert.ToDateTime(reader["fecha"]);
                fecha_inicio.Text = $"{dt.Day}/{meses[dt.Month-1]}/{dt.Year}";
                reader.Close();
                cmd = new SqlCommand("select fecha from fecha where nombre = 'final'", conexion);
                reader = cmd.ExecuteReader();
                reader.Read();
                dt = Convert.ToDateTime(reader["fecha"]);
                fecha_fin.Text = $"{dt.Day}/{meses[dt.Month - 1]}/{dt.Year}";
                reader.Close();
                cmd.Connection.Close();
            }
        }

        protected void GenerarReporte(string tipo,string desde,string hasta)
        {
            try
            {
                string[] meses = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
                SqlCommand cmd;
                SqlDataReader reader;
                DateTime now = DateTime.Now;
                DateTime dt, dt_desde=DateTime.Now,dt_hasta=DateTime.Now;
                int i = 3;
                string args = "", query="";

                if (desde != "null")
                {
                    dt_desde = new DateTime(Convert.ToInt32(desde.Split('-')[2]), Convert.ToInt32(desde.Split('-')[1]), Convert.ToInt32(desde.Split('-')[0]));
                }
                if (hasta != "null")
                {
                    dt_hasta = new DateTime(Convert.ToInt32(hasta.Split('-')[2]), Convert.ToInt32(hasta.Split('-')[1]), Convert.ToInt32(hasta.Split('-')[0]));
                }

                string fileName = "Reporte";
                using (ExcelPackage package = new ExcelPackage(fileName))
                {
                    if (tipo == "Sesiones")
                    {
                        fileName = $"Reporte_Sesiones_{now.Day}-{now.Month}-{now.Year}";
                        package.Workbook.Worksheets.Add("Sesiones");
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Sesiones"];

                        query = "select matricula, usuario.nombre as usuario_nombre, apellidos, Carrera.nombre as carrera, correo, " +
                            "es_alumno,sala_id,numero,fecha_inicio,fecha_fin,programa.nombre as programa_nombre " +
                            "from sesion join usuario on sesion.usuario_id=usuario.id join computadora on sesion.computadora_id=computadora.id " +
                            "join programa on sesion.programa_id=programa.id join carrera on usuario.carrera_id=carrera.id " +
                            "where fecha_fin is not null ";

                        if (desde != "null")
                        {
                            sheet.Cells[1, 1].Value = $"Sesiones de uso desde {dt_desde.Day}/{meses[dt_desde.Month - 1]}/{dt_desde.Year} hasta ";
                            query += $"and fecha_inicio >= '{dt_desde.Year}-{dt_desde.Month.ToString("D2")}-{dt_desde.Day.ToString("D2")} 00:00:00'";
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value = $"Sesiones de uso desde el primer registro hasta ";
                        }
                        if (hasta != "null")
                        {
                            sheet.Cells[1, 1].Value += $"{dt_hasta.Day}/{meses[dt_hasta.Month - 1]}/{dt_hasta.Year}";
                            query += $"and fecha_inicio <= '{dt_hasta.Year}-{dt_hasta.Month.ToString("D2")}-{dt_hasta.Day.ToString("D2")} 23:59:59'";
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value += $"actual";
                        }

                        sheet.Cells[1, 1].Style.Font.Italic = true;
                        sheet.Cells[1, 1].Style.Font.Bold = true;
                        sheet.Cells[2, 1].Value = "Matricula";            sheet.Column(1).Width =10;
                        sheet.Cells[2, 2].Value = "Nombre";               sheet.Column(2).Width = 20;
                        sheet.Cells[2, 3].Value = "Apellidos";            sheet.Column(3).Width = 20;
                        sheet.Cells[2, 4].Value = "Carrera";              sheet.Column(4).Width = 20;
                        sheet.Cells[2, 5].Value = "Correo universitario"; sheet.Column(5).Width = 30;
                        sheet.Cells[2, 6].Value = "Tipo";                 sheet.Column(6).Width = 10;
                        sheet.Cells[2, 7].Value = "Sala";                 sheet.Column(7).Width = 5;
                        sheet.Cells[2, 8].Value = "Computadora";          sheet.Column(8).Width = 5;
                        sheet.Cells[2, 9].Value = "Fecha de inicio";      sheet.Column(9).Width = 20;
                        sheet.Cells[2, 10].Value = "Fecha de fin";        sheet.Column(10).Width = 20;
                        sheet.Cells[2, 11].Value = "Programa utilizado";  sheet.Column(11).Width = 30;
           
                        sheet.Cells[1, 2, 1, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[1, 2, 1, 11].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        cmd = new SqlCommand(query, conexion);
                        cmd.Connection.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sheet.Cells[i, 1].Value = Convert.ToInt32(reader["matricula"]);
                            sheet.Cells[i, 2].Value = reader["usuario_nombre"].ToString();
                            sheet.Cells[i, 3].Value = reader["apellidos"].ToString();
                            sheet.Cells[i, 4].Value = reader["carrera"].ToString();
                            sheet.Cells[i, 5].Value = reader["correo"].ToString();

                            sheet.Cells[1, 1, 1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[1, 1, 1, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                            if (reader["es_alumno"].ToString() == "True")
                                sheet.Cells[i, 6].Value = "Alumno";
                            else
                                sheet.Cells[i, 6].Value = "Docente";

                            sheet.Cells[i, 7].Value = reader["sala_id"];
                            sheet.Cells[i, 8].Value = reader["numero"];

                            dt = Convert.ToDateTime(reader["fecha_inicio"]);
                            sheet.Cells[i, 9].Value = $"{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year} {dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";
                            dt = Convert.ToDateTime(reader["fecha_fin"]);
                            sheet.Cells[i, 10].Value = $"{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year} {dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";

                            sheet.Cells[i, 11].Value = reader["programa_nombre"].ToString();

                            i++;
                        }
                        reader.Close();
                        cmd.Connection.Close();
                        if (i == 3)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error_ses", $"alert('ERROR: No existen sesiones en las fechas seleccioandas');", true);
                            return;
                        }

                        sheet.Cells[1, 1, i, 11].Style.Font.Name = "Arial";
                        sheet.Cells[1, 1, i, 11].Style.Font.Size = 11;
                        sheet.Cells[1, 1, i, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    }
                    if (tipo == "Reservaciones")
                    {
                        fileName = $"Reporte_Reservaciones_{now.Day}-{now.Month}-{now.Year}";
                        package.Workbook.Worksheets.Add("Reservaciones");
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Reservaciones"];

                        List<string> list_args = new List<string>();

                        if (desde != "null")
                        {
                            sheet.Cells[1, 1].Value = $"Reservaciones desde {dt_desde.Day}/{meses[dt_desde.Month - 1]}/{dt_desde.Year} hasta ";
                            list_args.Add($"and fecha_inicio >= '{dt_desde.Year}-{dt_desde.Month.ToString("D2")}-{dt_desde.Day.ToString("D2")} 00:00:00'");
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value = $"Reservaciones desde el primer registro hasta";
                        }
                        if (hasta != "null")
                        {
                            sheet.Cells[1, 1].Value += $"{dt_hasta.Day}/{meses[dt_hasta.Month - 1]}/{dt_hasta.Year}";
                            list_args.Add($"and fecha_inicio <= '{dt_hasta.Year}-{dt_hasta.Month.ToString("D2")}-{dt_hasta.Day.ToString("D2")} 23:59:59'");
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value += $"actual";
                        }

                        if (list_args.Count == 0) args = "";
                        if (list_args.Count == 1) args = "where " + list_args[0];
                        if (list_args.Count == 2) args = "where " + list_args[0] + " and " + list_args[1];

                        query = "select matricula,Usuario.nombre as usuario_nombre,apellidos,correo,Carrera.nombre as carrera,sala_id,fecha_inicio,fecha_fin," +
                            "case when reservacion.curso is null then frecuencia.curso else reservacion.curso end as curso,programa.nombre as programa_nombre,cantidad_alumnos," +
                            "frecuencia_id,periodo_inicio,periodo_fin,activa from Reservacion join Usuario on Reservacion.usuario_id=Usuario.id left join Frecuencia on " +
                            "Reservacion.frecuencia_id=Frecuencia.id join Carrera on Usuario.carrera_id=Carrera.id join Programa on Reservacion.programa_id=Programa.id "
                            + args + "order by fecha_inicio ";

                        sheet.Cells[1, 1].Style.Font.Italic = true;
                        sheet.Cells[1, 1].Style.Font.Bold = true;
                        sheet.Cells[2, 1].Value = "Matricula";            sheet.Column(1).Width = 10;
                        sheet.Cells[2, 2].Value = "Nombre";               sheet.Column(2).Width = 20;
                        sheet.Cells[2, 3].Value = "Apellidos";            sheet.Column(3).Width = 20;
                        sheet.Cells[2, 4].Value = "Carrera";              sheet.Column(4).Width = 20;
                        sheet.Cells[2, 5].Value = "Correo universitario"; sheet.Column(5).Width = 35;
                        sheet.Cells[2, 6].Value = "Sala";                 sheet.Column(6).Width = 5;
                        sheet.Cells[2, 7].Value = "Fecha";                sheet.Column(7).Width = 15;
                        sheet.Cells[2, 8].Value = "Hora inicio";          sheet.Column(8).Width = 10;
                        sheet.Cells[2, 9].Value = "Hora fin";             sheet.Column(9).Width = 10;
                        sheet.Cells[2, 10].Value = "Curso";               sheet.Column(10).Width = 30;
                        sheet.Cells[2, 11].Value = "Programa";            sheet.Column(11).Width = 30;
                        sheet.Cells[2, 12].Value = "Cantidad alumnos";    sheet.Column(12).Width = 5;
                        sheet.Cells[2, 13].Value = "Tipo";                sheet.Column(13).Width = 15;
                        sheet.Cells[2, 14].Value = "Frecuencia";          sheet.Column(14).Width = 5;
                        sheet.Cells[2, 15].Value = "Desde";               sheet.Column(15).Width = 15;
                        sheet.Cells[2, 16].Value = "Hasta";               sheet.Column(16).Width = 15;
                        sheet.Cells[2, 17].Value = "Estado";              sheet.Column(17).Width = 10;
   
                        sheet.Cells[1, 1, 2, 17].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[1, 1, 2, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        cmd = new SqlCommand(query, conexion);
                        cmd.Connection.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sheet.Cells[i, 1].Value = Convert.ToInt32(reader["matricula"]);
                            sheet.Cells[i, 2].Value = reader["usuario_nombre"];
                            sheet.Cells[i, 3].Value = reader["apellidos"];
                            sheet.Cells[i, 4].Value = reader["carrera"];
                            sheet.Cells[i, 5].Value = reader["correo"];
                            sheet.Cells[i, 6].Value = Convert.ToInt32(reader["sala_id"]);

                            dt = Convert.ToDateTime(reader["fecha_inicio"]);
                            sheet.Cells[i, 7].Value = $"{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year}";
                            sheet.Cells[i, 8].Value = $"{dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";
                            dt = Convert.ToDateTime(reader["fecha_fin"]);
                            sheet.Cells[i, 9].Value = $"{dt.Hour.ToString("D2")}:{dt.Minute.ToString("D2")}";

                            sheet.Cells[i, 10].Value = reader["curso"];
                            sheet.Cells[i, 11].Value = reader["programa_nombre"];
                            sheet.Cells[i, 12].Value = Convert.ToInt32(reader["cantidad_alumnos"]);

                            if (reader["frecuencia_id"] == DBNull.Value)
                            {
                                sheet.Cells[i, 13].Value = "Unica";
                                sheet.Cells[i, 14].Value = "-";
                                sheet.Cells[i, 15].Value = "-";
                                sheet.Cells[i, 16].Value = "-";
                            }
                            else
                            {
                                sheet.Cells[i, 13].Value = "Frecuencial";
                                sheet.Cells[i, 14].Value = Convert.ToInt32(reader["frecuencia_id"]);
                                dt = Convert.ToDateTime(reader["periodo_inicio"]);
                                sheet.Cells[i, 15].Value = $"{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year}";
                                dt = Convert.ToDateTime(reader["periodo_fin"]);
                                sheet.Cells[i, 16].Value = $"{dt.Day.ToString("D2")}/{dt.Month.ToString("D2")}/{dt.Year}";
                            }
                            if (reader["activa"].ToString() == "True")
                                sheet.Cells[i, 17].Value = "Activa";
                            else
                                sheet.Cells[i, 17].Value = "Activa";

                            i++;
                        }
                        reader.Close();
                        cmd.Connection.Close();
                        if (i == 3)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error_res", $"alert('ERROR: No existen reservaciones en las fechas seleccioandas');", true);
                            return;
                        }

                        sheet.Cells[1, 1, i, 17].Style.Font.Name = "Arial";
                        sheet.Cells[1, 1, i, 17].Style.Font.Size = 11;
                        sheet.Cells[1, 1, i, 17].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    if (tipo == "Programas")
                    {
                        fileName = $"Reporte_Programas_{now.Day}-{now.Month}-{now.Year}";
                        package.Workbook.Worksheets.Add("Programas");
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Programas"];

                        if(desde=="null" && hasta == "null")
                        {
                            sheet.Cells[1, 1].Value = $"Programas y su uso desde el primer registro hasta actual";
                            query = "select id,nombre,activo,(select isnull((select count(programa_id) from VistaReservaciones " +
                                "where (programa_id=programa.id and frecuencia_id is null) or (programa_id=programa.id and frecuencia_id is not null) " +
                                "group by programa_id),0)) as cantidad_res,(select count(programa_id) from sesion where programa_id=programa.id) " +
                                "as cantidad_ses from Programa order by nombre";
                        }
                        if(desde!="null" && hasta != "null")
                        {
                            sheet.Cells[1, 1].Value = $"Programas y su uso desde {dt_desde.Day}/{dt_desde.Month}/{dt_desde.Year} hasta {dt_hasta.Day}/{dt_hasta.Month}/{dt_hasta.Year}";
                            desde = $"'{dt_desde.Year}-{dt_desde.Month.ToString("D2")}-{dt_desde.Day.ToString("D2")} 00:00:00'";
                            hasta = $"'{dt_hasta.Year}-{dt_hasta.Month.ToString("D2")}-{dt_hasta.Day.ToString("D2")} 23:59:59'";

                            query = $"select id,nombre,activo,(select isnull((select count(programa_id) from VistaReservaciones where " +
                                $"(programa_id=programa.id and frecuencia_id is null and fecha_inicio>={desde} and fecha_inicio<={hasta}) " +
                                $"or (programa_id=programa.id and frecuencia_id is not null and (({desde}>=fecha_inicio and {desde}<=fecha_fin) " +
                                $"or ({hasta}>=fecha_inicio and {hasta}<=fecha_fin))) group by programa_id),0)) as cantidad_res,(select count(programa_id) " +
                                $"from sesion where programa_id=programa.id and fecha_inicio>={desde} and fecha_inicio<={hasta}) as cantidad_ses " +
                                $"from Programa order by nombre";
                        }
                        if(desde!="null" && hasta == "null")
                        {
                            sheet.Cells[1, 1].Value = $"Programas y su uso desde {dt_desde.Day}/{dt_desde.Month}/{dt_desde.Year} hasta actual";
                            desde = $"'{dt_desde.Year}-{dt_desde.Month.ToString("D2")}-{dt_desde.Day.ToString("D2")} 00:00:00'";

                            query = $"select id,nombre,activo,(select isnull((select count(programa_id) from VistaReservaciones where " +
                                $"(programa_id=programa.id and frecuencia_id is null and fecha_inicio>={desde}) " +
                                $"or (programa_id=programa.id and frecuencia_id is not null and ({desde}>=fecha_inicio and {desde}<=fecha_fin)) " +
                                $"group by programa_id),0)) as cantidad_res,(select count(programa_id) " +
                                $"from sesion where programa_id=programa.id and fecha_inicio>={desde}) as cantidad_ses " +
                                $"from Programa order by nombre";
                        }
                        if(desde=="null" && hasta != "null")
                        {
                            sheet.Cells[1, 1].Value = $"Programas y su uso desde el primer registro hasta {dt_hasta.Day}/{dt_hasta.Month}/{dt_hasta.Year}";
                            hasta = $"'{dt_hasta.Year}-{dt_hasta.Month.ToString("D2")}-{dt_hasta.Day.ToString("D2")} 23:59:59'";

                            query = $"select id,nombre,activo,(select isnull((select count(programa_id) from VistaReservaciones where " +
                                $"(programa_id=programa.id and frecuencia_id is null and fecha_inicio<={hasta}) " +
                                $"or (programa_id=programa.id and frecuencia_id is not null and ({hasta}>=fecha_inicio and {hasta}<=fecha_fin)) " +
                                $"group by programa_id),0)) as cantidad_res,(select count(programa_id) " +
                                $"from sesion where programa_id=programa.id and fecha_inicio<={hasta}) as cantidad_ses " +
                                $"from Programa order by nombre";
                        }

                        sheet.Cells[1, 1].Style.Font.Italic = true;
                        sheet.Cells[1, 1].Style.Font.Bold = true;
                        sheet.Cells[2, 1].Value = "Nombre";               sheet.Column(1).Width = 30;
                        sheet.Cells[2, 2].Value = "Estado";               sheet.Column(2).Width = 20;
                        sheet.Cells[2, 3].Value = "Uso en sesiones";      sheet.Column(3).Width = 20;
                        sheet.Cells[2, 4].Value = "Uso en reservaciones"; sheet.Column(4).Width = 20;

                        sheet.Cells[1, 1, 2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[1, 1, 2, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        cmd = new SqlCommand(query, conexion);

                        cmd.Connection.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sheet.Cells[i, 1].Value = reader["nombre"];

                            if (reader["activo"].ToString() == "True")
                                sheet.Cells[i, 2].Value = "Disponible";
                            else
                                sheet.Cells[i, 2].Value = "No disponible";

                            sheet.Cells[i, 3].Value = Convert.ToInt32(reader["cantidad_ses"]);
                            sheet.Cells[i, 4].Value = Convert.ToInt32(reader["cantidad_res"]);

                            i++;
                        }
                        reader.Close();
                        cmd.Connection.Close();

                        sheet.Cells[1, 1, i, 4].Style.Font.Name = "Arial";
                        sheet.Cells[1, 1, i, 4].Style.Font.Size = 11;
                        sheet.Cells[1, 1, i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    if (tipo == "Computadoras")
                    {
                        fileName = $"Reporte_Computadoras_{now.Day}-{now.Month}-{now.Year}";
                        package.Workbook.Worksheets.Add("Computadoras");
                        ExcelWorksheet sheet = package.Workbook.Worksheets["Computadoras"];

                        if (desde != "null")
                        {
                            sheet.Cells[1, 1].Value = $"Computadoras y su uso desde {dt_desde.Day}/{meses[dt_desde.Month - 1]}/{dt_desde.Year} hasta ";
                            args += $"and fecha_inicio >= '{dt_desde.Year}-{dt_desde.Month.ToString("D2")}-{dt_desde.Day.ToString("D2")} 00:00:00'";
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value = $"Computadoras y su uso desde el primer registro hasta";
                        }
                        if (hasta != "null")
                        {
                            sheet.Cells[1, 1].Value += $"{dt_hasta.Day}/{meses[dt_hasta.Month - 1]}/{dt_hasta.Year}";
                            args += $"and fecha_inicio <= '{dt_hasta.Year}-{dt_hasta.Month.ToString("D2")}-{dt_hasta.Day.ToString("D2")} 00:00:00'";
                        }
                        else
                        {
                            sheet.Cells[1, 1].Value += $"actual";
                        }

                        sheet.Cells[1, 1].Style.Font.Italic = true;
                        sheet.Cells[1, 1].Style.Font.Bold = true;
                        sheet.Cells[2, 1].Value = "Sala";            sheet.Column(1).Width = 15;
                        sheet.Cells[2, 2].Value = "Numero";          sheet.Column(2).Width = 15;
                        sheet.Cells[2, 3].Value = "Estado";          sheet.Column(3).Width = 15;
                        sheet.Cells[2, 4].Value = "Uso en sesiones"; sheet.Column(4).Width = 15;

                        sheet.Cells[1, 1, 2, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[1, 1, 2, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        cmd = new SqlCommand("select *,(select count(sesion.id) from sesion where computadora_id=computadora.id "+args+") as cantidad from computadora", conexion);
                        cmd.Connection.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sheet.Cells[i, 1].Value = Convert.ToInt32(reader["sala_id"]);
                            sheet.Cells[i, 2].Value = Convert.ToInt32(reader["numero"]);

                            if (reader["funcional"].ToString() == "True")
                                sheet.Cells[i, 3].Value = "Disponible";
                            else
                                sheet.Cells[i, 3].Value = "No disponible";

                            sheet.Cells[i, 4].Value = Convert.ToInt32(reader["cantidad"]);

                            i++;
                        }
                        reader.Close();
                        cmd.Connection.Close();

                        sheet.Cells[1, 1, i, 4].Style.Font.Name = "Arial";
                        sheet.Cells[1, 1, i, 4].Style.Font.Size = 11;
                        sheet.Cells[1, 1, i, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }
                    Download(package,fileName);
                }
            }
            catch(Exception ex)
            {
                if (conexion.State == System.Data.ConnectionState.Open) conexion.Close();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "1", $"console.log('{ex.Message}');", true);
            }
        }

        void Download(ExcelPackage package, string fileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            TextWriter textWriter = new StreamWriter(memoryStream);
            textWriter.WriteLine("Something");
            textWriter.Flush(); // added this line
            byte[] bytesInStream = package.GetAsByteArray(); // simpler way of converting to array
            memoryStream.Close();

            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", $"attachment;    filename={fileName}.xlsx");
            Response.BinaryWrite(bytesInStream);
            Response.End();
        }

        protected void Direccionar(object sender, EventArgs e)
        {
            Response.Redirect(((Button)sender).Text +".aspx");
        }
    }
}