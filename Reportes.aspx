<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="ComputacionFCQ.Reportes"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Computadoras</title>
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-pprn3073KE6tl6bjs2QrFaJGz5/SUsLqktiwsUTF55Jfv3qYSDhgCecCxMW52nD2" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-0evHe/X+R7YkIZDRvuzKMRqM+OrBnVFBL6DOitfPri4tjfHxaWutUpFmBp4vmVor" crossorigin="anonymous" />
    <link rel="stylesheet" href="styles.css" type="text/css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css"/>
    <script type="module" src="https://unpkg.com/ionicons@5.5.2/dist/ionicons/ionicons.esm.js"></script>
</head>

<body>
    <form id="body" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <!-- Panel que se oscurese cuando se abre el Side bar-->
        <div id="back" onclick="CerrarSide();"></div>
        <!-- Side bar-->
        <div id="side" hidden="">
            <!-- Header del side bar-->
            <div style="user-select: none; margin:15px; display:flex; align-items:center;">
                <img src="https://seeklogo.com/images/F/facultad-de-ciencias-quimicas-logo-82ADF31672-seeklogo.com.png"
                    style="height: 50px;" />
                <a style="font-family: Arial; font-size: 22px; color:rgb(29,29,29); text-align:center; flex-grow:1;">Sala de computación</a>
            </div> 
            <!-- Elementos genericos del side bar-->
            <button class="SideBtn" onclick="document.getElementById('btn_sesiones').click(); return false;"><ion-icon name="home" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Sesiones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_reservaciones').click(); return false;"><ion-icon name="calendar" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Reservaciones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_programas').click(); return false;"><ion-icon name="apps" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Programas</button>
            <button class="SideBtn" onclick="document.getElementById('btn_computadoras').click(); return false;"><i class="bi bi-pc-display" style="font-size: 16px;margin-right:15px ; color:var(--fcq); align-self:center"></i>Computadoras</button>
            <button class="SideBtn" onclick="document.getElementById('btn_reportes').click(); return false;"><i class="bi bi-table" style="font-size: 16px;margin-right:15px ; color:var(--fcq); align-self:center"></i>Reportes</button>
            <!-- Elementos ASP para redireccionar-->
            <asp:Button runat="server" ID="btn_sesiones" Text="Inicio" OnClick="Direccionar" Hidden=""/>
            <asp:Button runat="server" ID="btn_reservaciones" Text="Reservaciones" OnClick="Direccionar" Hidden=""/>
            <asp:Button runat="server" ID="btn_programas" Text="Programas" OnClick="Direccionar" Hidden=""/>
            <asp:Button runat="server" ID="btn_computadoras" Text="Computadoras" OnClick="Direccionar" Hidden=""/>
            <asp:Button runat="server" ID="btn_reportes" Text="Reportes" OnClick="Direccionar" Hidden=""/>
        </div>

        <!-- NAV-->
        <nav style="background:rgb(45,45,45); height:65px; display:flex; vertical-align:middle; border:none;">
            <button onclick="AbrirSide(); return false;" style="background:none; border:none; margin:15px;">
                <ion-icon name="menu-outline" style="font-size: 32px; color:whitesmoke; align-self:center;"></ion-icon>
            </button>

            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:0px; margin-top:10px;"></div>

            <i class="bi bi-table" style="font-size: 18px; margin-left: 15px;margin-right:5px ; color:var(--fcq); align-self:center"></i>
            <a style="font-family: Arial; font-size:18px; margin-left:15px; color: whitesmoke; align-self:center;">Reportes</a>
        </nav>

        <asp:Label runat="server" ID="fecha_inicio" Text="" hidden=""></asp:Label>
        <asp:Label runat="server" ID="fecha_fin" Text="" hidden=""></asp:Label>

        <div style="display:flex; width:100%; justify-content:center; margin-top:15px;">
            <div style="margin-right:10px;">
                <div style="height:25px; display: block;">Tipo de reporte</div>
                <asp:DropDownList runat="server" ID="lista" style="height:35px;">
                    <asp:ListItem>Sesiones</asp:ListItem>
                    <asp:ListItem>Reservaciones</asp:ListItem>
                    <asp:ListItem>Programas</asp:ListItem>
                    <asp:ListItem>Computadoras</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div style="margin-right:10px;">
                <div style="height:25px; display: block;">Desde:</div>
                <div class="dropdown" id="dd_desde">
                    <button class="btn btn-secondary" type="button" data-bs-toggle="dropdown" aria-expanded="false" id="btn_desde" style="display:flex; width:200px; text-align:left; height:35px; align-items:center;">
                        Seleccionar fecha
                        <i class="bi bi-caret-down-fill" style="font-size:12px; right:10px; position:absolute; align-self:center;"></i>
                    </button>
                    <ul class="dropdown-menu" id="desde" style="padding: 10px; width: 250px; box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);"></ul>
                </div>
            </div>

            <div style="margin-right:10px;">
                <div style="height:25px; display: block;">Hasta:</div>
                <div class="dropdown" id="dd_hasta">
                    <button class="btn btn-secondary" type="button" data-bs-toggle="dropdown" aria-expanded="false" id="btn_hasta" style="display:flex; width:200px; text-align:left; height:35px; align-items:center;">
                        Seleccionar fecha
                        <i class="bi bi-caret-down-fill" style="font-size:12px; right:10px; position:absolute; align-self:center;"></i>
                    </button>
                    <ul class="dropdown-menu" id="hasta" style="padding: 10px; width: 250px; box-shadow: 0 2px 4px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);"></ul>
                </div>
            </div>

            <div>
                <div style="height: 25px; display: block;"></div>
                <button class="btn btn-primary" onclick="Descargar(); return false;" style="display: flex; justify-content: center; align-items: center; height: 35px;">
                    <i class="bi bi-download" style="font-size: 14px; margin-right: 7px;"></i>
                    Descargar reporte
                </button>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        console.log('Se carga ClientSide');


        function Descargar() {
            var desde = document.getElementById('btn_desde').textContent;
            if (desde.trim().split(" ")[0] == 'Seleccionar') {
                desde = 'null';
            }
            else {
                desde = desde.trim().split(" ")[0];
                desde = desde.split("/")[0] + "-" + getMes(desde.split("/")[1]) + "-" + desde.split("/")[2];
            }

            var hasta = document.getElementById('btn_hasta').textContent;
            if (hasta.trim().split(" ")[0] == 'Seleccionar') {
                hasta = 'null';
            }
            else {
                hasta = hasta.trim().split(" ")[0];
                hasta = hasta.split("/")[0] + "-" + getMes(hasta.split("/")[1]) + "-" + hasta.split("/")[2];
            }

            var tipo = document.getElementById('lista').options[document.getElementById('lista').selectedIndex].value;

            location.href = 'Reportes.aspx?tipo='+tipo+'&desde=' + desde + '&hasta=' + hasta;
        }

        ActualizarTabla('desde','')
        ActualizarTabla('hasta','')

        $('#dd_desde .dropdown-menu').on({
            "click": function (e) {
                e.stopPropagation();
            }
        });

        $('#dd_desde').on('hidden.bs.dropdown', function () {
            if (document.getElementById('btn_desde').innerText.trim() == 'Seleccionar fecha') {
                document.getElementById('desde_mes').innerText = getMes(new Date().getMonth() + 1);
                document.getElementById('desde_ano').innerText = new Date().getFullYear();
            }
            else {
                document.getElementById('desde_mes').innerText = getMes((document.getElementById('btn_desde').innerText.split("/"))[1]);
                document.getElementById('desde_ano').innerText = ((document.getElementById('btn_desde').innerText.split("/"))[2]).substring(0,5);
                ActualizarTabla('desde', '');
            }
        })

        $('#dd_hasta .dropdown-menu').on({
            "click": function (e) {
                e.stopPropagation();
            }
        });

        $('#dd_hasta').on('hidden.bs.dropdown', function () {
            if (document.getElementById('btn_hasta').innerText.trim() == 'Seleccionar fecha') {
                document.getElementById('hasta_mes').innerText = getMes(new Date().getMonth() + 1);
                document.getElementById('desde_ano').innerText = new Date().getFullYear();
            }
            else {
                document.getElementById('hasta_mes').innerText = getMes((document.getElementById('btn_hasta').innerText.split("/"))[1]);
                document.getElementById('hasta_ano').innerText = ((document.getElementById('btn_hasta').innerText.split("/"))[2]).substring(0,5);
                ActualizarTabla('hasta', '');
            }
        })

        function ActualizarTabla(id, accion) {
            var mes, ano, mes_tx;
            //Si es la primera vez que se actualiza (cuando se crea)
            if (!document.getElementById(id+'_mes')) {
                mes = new Date().getMonth() + 1;
                ano = new Date().getFullYear();
                mes_tx = getMes(mes);
            }
            //si solo es actualizacion
            else {
                ano = document.getElementById(id + '_ano').textContent;
                if (accion=='') {
                    mes = getMes((document.getElementById('btn_' + id).innerText.split("/"))[1]);
                    mes_tx = getMes(mes);
                }
                else {
                    mes_tx = document.getElementById(id + '_mes').innerText;
                    mes = getMes(mes_tx);
                }
            }

            if (accion == 'next') {
                if (mes == 12) {
                    mes = 1;
                    ano++;
                }
                else {
                    mes++;
                }
                mes_tx = getMes(mes);
            }
            if (accion == 'prev') {
                if (mes == 1) {
                    mes = 12;
                    ano--;
                }
                else {
                    mes--;
                }
                mes_tx = getMes(mes);
            }

            var dt = new Date(ano + '-' + mes+'-1');
            const diasTotal = new Date(ano,mes,0).getDate();
            var iteracion = 1, fila = 0;
            var empezarEn = dt.getDay() + 1;
            var fecha_tx;

            var texto;
            if (id === 'desde') {
                texto = `Desde el inicio del semestre`;
                fecha_tx = document.getElementById('fecha_inicio').innerText;
            }
            else {
                texto = `Hasta el final del semestre`;
                fecha_tx = document.getElementById('fecha_fin').innerText
            }

            document.getElementById(id).innerHTML = ``
                + '<div style="display:flex; justify-content:center; margin-top:0;">'
                + `<button class="fecha" onclick="ActualizarTabla('${id}','prev'); return false;" style=" position:absolute; width:30px; height:30px; left:0px; margin-left:10px; top:5px;"><</button> `
                + `<a id="${id}_mes" style="padding-bottom:5px; margin-right:7px; font-weight:bold;">${mes_tx}</a><a id="${id}_ano" style="font-weight:bold;">${ano}</a>`
                + `<button class="fecha" onclick="ActualizarTabla('${id}','next'); return false;" style=" position:absolute; width:30px; height:30px; right:0px; margin-right:10px; top:5px;">></button></div >`

                + '<table class="" style="width:100%;"><thead style="background-color:lightgray; "><tr>'
                + '<th style="text-align: center; font-weight:normal;">Do</th>'
                + '<th style="text-align: center; font-weight:normal;">Lu</th>'
                + '<th style="text-align: center; font-weight:normal;">Ma</th>'
                + '<th style="text-align: center; font-weight:normal;">Mi</th>'
                + '<th style="text-align: center; font-weight:normal;">Ju</th>'
                + '<th style="text-align: center; font-weight:normal;">Vi</th>'
                + '<th style="text-align: center; font-weight:normal;">Sa</th>'
                + '</thead><tbody id = "' + id + '_body"></tbody></table> <hr style="margin-top:10px; margin-bottom:10px;">'
                + `<button class="btn btn-primary" style="font-size:15px; display:flex; align-items: center; justify-content:center; width:100%;"`
                + `OnClick="document.getElementById('btn_${id}').innerText='${fecha_tx}'; $(${id}).dropdown('toggle'); ActualizarTabla('${id}','next'); ActualizarTabla('${id}','prev');return false;">${texto}</button>`;

            while (iteracion < diasTotal + dt.getDay()) {

                if (iteracion == 1 || iteracion % 7 == 1) {
                    fila++;
                    document.getElementById(id + '_body').innerHTML += '<tr style="height:30px;" id="' + id + '_fila_' + fila + '"></tr>';

                    for (let i = 1; i <= 7; i++) {
                        if ((iteracion >= empezarEn) && (iteracion <= (new Date(ano, mes, 0).getDate() + empezarEn - 1))) {
                            if (((iteracion - empezarEn + 1) + '/' + getMes(mes) + '/' + ano) == document.getElementById('btn_' + id).innerHTML.trim().split("<")[0]) {
                                document.getElementById(id + '_fila_' + fila).innerHTML += `<td style="padding:0; margin:0; border:1px lightgray solid;">`
                                    + `<button style="height:30px; background-color:dimgrey; border-radius:50%; border:none; color:white;" class="fecha" `
                                    + `OnClick="document.getElementById('btn_${id}').innerText='${(iteracion - empezarEn + 1)}/${getMes(mes)}/${ano}'; $(${id}).dropdown('toggle'); `
                                    + `ActualizarTabla('${id}','next'); ActualizarTabla('${id}','prev');return false;"> ${(iteracion - empezarEn + 1)} </button></td>`;
                            }
                            else {
                                document.getElementById(id + '_fila_' + fila).innerHTML += `<td style="padding:0; margin:0; border:1px lightgray solid;">`
                                    + `<button style="height:30px;" class="fecha" OnClick="document.getElementById('btn_${id}').innerText='${(iteracion - empezarEn + 1)}/${getMes(mes)}/${ano}'; $(${id}).dropdown('toggle'); `
                                    + `ActualizarTabla('${id}','next'); ActualizarTabla('${id}','prev');return false;"> ${(iteracion - empezarEn + 1)} </button></td>`;
                            }
                        }
                        else {
                            document.getElementById(id + '_fila_' + fila).innerHTML += '<td></td>';
                        }
                        iteracion++;
                    }
                }
            }
            if (document.getElementById('btn_' + id).innerHTML.trim().split(" ")[0] != "Seleccionar")
            document.getElementById('btn_' + id).innerHTML = (document.getElementById('btn_' + id).innerHTML.split(" "))[0] + '<i class="bi bi-caret-down-fill" style="font-size:12px; right:10px; position:absolute; align-self:center;"></i>';
        }

        function getMes(mes) {
            if (mes == 1) return 'Enero';
            if (mes == 2) return 'Febrero';
            if (mes == 3) return 'Marzo';
            if (mes == 4) return 'Abril';
            if (mes == 5) return 'Mayo';
            if (mes == 6) return 'Junio';
            if (mes == 7) return 'Julio';
            if (mes == 8) return 'Agosto';
            if (mes == 9) return 'Septiembre';
            if (mes == 10) return 'Octubre';
            if (mes == 11) return 'Noviembre';
            if (mes == 12) return 'Diciembre';
            if (mes == 'Enero') return 1;
            if (mes == 'Febrero') return 2;
            if (mes == 'Marzo') return 3;
            if (mes == 'Abril') return 4;
            if (mes == 'Mayo') return 5;
            if (mes == 'Junio') return 6;
            if (mes == 'Julio') return 7;
            if (mes == 'Agosto') return 8;
            if (mes == 'Septiembre') return 9;
            if (mes == 'Octubre') return 10;
            if (mes == 'Noviembre') return 11;
            if (mes == 'Diciembre') return 12;
        }

        function AbrirSide() {
            document.getElementById('body').style.overflow = "hidden";
            document.getElementById('back').className = "background"
            document.getElementById('side').hidden = false;
            document.getElementById('side').className = "SideShown";
        }

        function CerrarSide() {
            document.getElementById('body').style.overflow = "";
            document.getElementById('back').className = "backgroundblured";
            document.getElementById('side').className = "SideHidden";
        }
    </script>

</body>
</html>
