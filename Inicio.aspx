<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="ComputacionFCQ.Inicio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Inicio</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-pprn3073KE6tl6bjs2QrFaJGz5/SUsLqktiwsUTF55Jfv3qYSDhgCecCxMW52nD2" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-0evHe/X+R7YkIZDRvuzKMRqM+OrBnVFBL6DOitfPri4tjfHxaWutUpFmBp4vmVor" crossorigin="anonymous" />
    <link rel="stylesheet" href="styles.css" type="text/css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css" />
    <script type="module" src="https://unpkg.com/ionicons@5.5.2/dist/ionicons/ionicons.esm.js"></script>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="" />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@100;300;400;500;700&display=swap" rel="stylesheet" />
</head>

<body id="body" style="font-family: 'Roboto', sans-serif;">

    <form id="inicio" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <!-- Panel que se oscurese cuando se abre el Side bar-->
        <div id="back" onclick="CerrarSide();"></div>
        <!-- Side bar-->
        <div id="side" hidden="">
            <!-- Header del side bar-->
            <div style="user-select: none; margin: 15px; display: flex; align-items: center;">
                <img src="https://seeklogo.com/images/F/facultad-de-ciencias-quimicas-logo-82ADF31672-seeklogo.com.png"
                    style="height: 50px;" />
                <a style="font-family: Arial; font-size: 22px; color: rgb(29,29,29); text-align: center; flex-grow: 1;">Sala de computación</a>
            </div>
            <!-- Elementos genericos del side bar-->
            <button class="SideBtn" onclick="document.getElementById('btn_sesiones').click(); return false;">
                <ion-icon name="home" style="font-size: 16px; margin-right: 15px; color: var(--fcq);"></ion-icon>
                Sesiones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_reservaciones').click(); return false;">
                <ion-icon name="calendar" style="font-size: 16px; margin-right: 15px; color: var(--fcq);"></ion-icon>
                Reservaciones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_programas').click(); return false;">
                <ion-icon name="apps" style="font-size: 16px; margin-right: 15px; color: var(--fcq);"></ion-icon>
                Programas</button>
            <button class="SideBtn" onclick="document.getElementById('btn_computadoras').click(); return false;"><i class="bi bi-pc-display" style="font-size: 16px; margin-right: 15px; color: var(--fcq); align-self: center"></i>Computadoras</button>
            <button class="SideBtn" onclick="document.getElementById('btn_reportes').click(); return false;"><i class="bi bi-table" style="font-size: 16px; margin-right: 15px; color: var(--fcq); align-self: center"></i>Reportes</button>
            <!-- Elementos ASP para redireccionar-->
            <asp:Button runat="server" ID="btn_sesiones" Text="Inicio" OnClick="Direccionar" Hidden="" />
            <asp:Button runat="server" ID="btn_reservaciones" Text="Reservaciones" OnClick="Direccionar" Hidden="" />
            <asp:Button runat="server" ID="btn_programas" Text="Programas" OnClick="Direccionar" Hidden="" />
            <asp:Button runat="server" ID="btn_computadoras" Text="Computadoras" OnClick="Direccionar" Hidden="" />
            <asp:Button runat="server" ID="btn_reportes" Text="Reportes" OnClick="Direccionar" Hidden="" />
        </div>

        <!-- NAV-->
        <nav style="background: rgb(45,45,45); height: 65px; display: flex; vertical-align: middle; border: none;">
            <button onclick="AbrirSide(); return false;" style="background: none; border: none; margin: 15px;">
                <ion-icon name="menu-outline" style="font-size: 32px; color: whitesmoke; align-self: center;"></ion-icon>
            </button>

            <div class="vr" style="height: 45px; background-color: whitesmoke; margin-left: 0px; margin-top: 10px;"></div>

            <ion-icon name="home" style="font-size: 20px; margin-left: 15px; color: var(--fcq); align-self: center"></ion-icon>
            <a style="font-family: Arial; font-size: 18px; margin-left: 15px; color: whitesmoke; align-self: center;">Sesiones</a>
        </nav>

        <!-- NO MOVERLE A ESTO -->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="1" OnTick="Timer1_Tick"></asp:Timer>

                <asp:Button runat="server" ID="aux_iniciar" hidden="" OnClick="ActualizarSalas"/>

                <asp:Button runat="server" ID="btn_finalizar_sesion" OnClick="FinalizarSesion" hidden=""/>
                <asp:HiddenField runat="server" ID="field_matricula"/>
            </ContentTemplate>
        </asp:UpdatePanel>
        <!-- Modal iniciar sesion-->
        <div class="modal fade" id="modalIniciar" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel">Iniciar una sesión</h5>
                                <button id="cerrar_modal" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div class="modal-body">
                                <a>Matricula:</a>
                                <asp:TextBox ID="tx_matricula" runat="server" type="number" onkeydown="return event.key;"></asp:TextBox>
                                <asp:Button ID="btn_buscar" runat="server" Text="Buscar datos" OnClick="btn_buscar_Click" />
                                <br />

                                <a>Nombre(s):</a>
                                <asp:TextBox ID="tx_nombres" runat="server"></asp:TextBox>

                                <br />
                                <a>Apellidos:</a>
                                <asp:TextBox ID="tx_apellidos" runat="server"></asp:TextBox>

                                <br />
                                <a>Carrera/Coordinación: </a>
                                <asp:DropDownList ID="list_carreras" runat="server" AutoPostBack="false"></asp:DropDownList>

                                <br />

                                <asp:RadioButton ID="rad_alumno" Text="Alumno" runat="server" AutoPostBack="true" GroupName="1" Checked="true" />
                                <asp:RadioButton ID="rad_docente" Text="Docente" runat="server" AutoPostBack="true" GroupName="1" />

                                <br />
                                <a>Correo institucional:</a>
                                <asp:TextBox ID="tx_correo" runat="server"></asp:TextBox>

                                <br />
                                <a>Sala:</a>
                                <asp:DropDownList ID="list_sala" runat="server" AutoPostBack="true" OnSelectedIndexChanged="list_sala_SelectedIndexChanged"></asp:DropDownList>
                                <a>Computadora:</a>
                                <asp:DropDownList ID="list_computadora" runat="server"></asp:DropDownList>

                                <br />
                                <a>Programa:</a>
                                <asp:DropDownList ID="list_programa" runat="server"></asp:DropDownList>

                                <div class="modal-footer">
                                    <asp:Button ID="Button1" runat="server" Text="Iniciar sesión" OnClick="iniciar_sesion" />
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>

        <!-- Modal cerrar sesion-->
        <div class="modal fade" id="modalFinalizar" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <div class="modal-header">
                                <h5 class="modal-title">Cerrar una sesión</h5>
                                <button id="cerrar_modal_cerrar_sesion" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <a>Matricula:</a>
                                <asp:TextBox ID="tx_matricula_cerrar" runat="server" type="number" />
                                <br />
                                <br />
                                <div class="modal-footer">
                                    
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <!-- Tabla estados-->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="card" style="margin: 30px; border-radius: 15px; background: white; box-shadow: 0 0 0 0 rgba(0, 0, 0, 0.2), 0 3px 10px 0 rgba(0, 0, 0, 0.1);">
                    <div class="card-body">
                        <div class="card-title" style="font-size: 18px; font-family: Arial; color: rgb(29,29,29);">Estado de las salas</div>
                        <table class="table" style="margin: 0; border-radius: 5px; overflow: hidden;">
                            <tbody id="card-info-body">
                            </tbody>

                            <tr id="rowSala1">
                                <td style="width: 80px; font-weight: 500;">Sala 1</td>
                                <td>
                                    <asp:Label runat="server" ID="info1" Text="" /></td>
                            </tr>
                            <tr id="rowSala2">
                                <td style="width: 80px; font-weight: 500;">Sala 2</td>
                                <td>
                                    <asp:Label runat="server" ID="info2" Text="" /></td>
                            </tr>
                            <tr id="rowSala3">
                                <td style="width: 80px; font-weight: 500;">Sala 3</td>
                                <td>
                                    <asp:Label runat="server" ID="info3" Text="" /></td>
                            </tr>
                            <tr id="rowSala4">
                                <td style="width: 80px; font-weight: 500;">Sala 4</td>
                                <td>
                                    <asp:Label runat="server" ID="info4" Text="" /></td>
                            </tr>
                            <tr id="rowSala5">
                                <td style="width: 80px; font-weight: 500;">Sala 5</td>
                                <td>
                                    <asp:Label runat="server" ID="info5" Text="" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Tabla sesiones-->
        <div class="card" style="margin: 30px; border-radius: 15px; background: white">
            <div class="card-body" id="card">

                <div class="card-header" style="background-color: white; padding: 0px; display: flex; border: none; align-items: center;">
                    <a style="font-size: 18px; color: rgb(29,29,29);">Sesiones activas</a>

                    <button class="btn btn-success" style="display: flex; justify-content: center; align-items: center; right: 200px; position: absolute; margin-right: 15px; height: 35px;"
                        onclick="document.getElementById('aux_iniciar').click(); $('#modalIniciar').modal('show'); return false;">
                        <i class="bi bi-triangle-fill" style="margin-right: 10px; font-size: 10px; transform: rotate(90deg);"></i>
                        <a>Iniciar una sesión</a>
                    </button>

                    <button class="btn btn-danger" style="display: flex; justify-content: center; align-items: center; right: 0px; position: absolute; margin-right: 15px; height: 35px;"
                        onclick="if(document.getElementById('lbl_no').hidden==false){alert('ERROR: No hay sesiones activas'); return false;} 
                        document.getElementById('tx_matricula_cerrar').value=''; $('#modalFinalizar').modal('show'); return false;">
                        <i class="bi bi-x-circle" style="margin-right: 10px; font-size: 14px;"></i>
                        <a>Finalizar una sesión</a>
                    </button>

                </div>
                <hr style="margin-bottom:10px; margin-top:20px;"/>
                <div id="lbl_no" style="display:flex; justify-content:center; align-items:center;">
                    <a style="justify-self:center;">No hay sesiones activas por el momento</a>
                </div>
                <div class="table-responsive table-hover" id="tabla_sesiones" hidden="">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th scope="col" style="font-weight: normal">Matricula</th>
                                <th scope="col" style="font-weight: normal">Sala</th>
                                <th scope="col" style="font-weight: normal;">Computadora</th>
                                <th scope="col" style="font-weight: normal">Fecha de inicio</th>
                                <th scope="col" style="font-weight: normal;">Acciones</th>
                            </tr>
                        </thead>
                        <tbody id="tbody">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

    <script type="text/javascript">

        function agregarInfoSala(sala1, sala2, sala3, sala4, sala5) {
            document.getElementById('rowSala1').style.cssText = sala1;
            document.getElementById('rowSala2').style.cssText = sala2;
            document.getElementById('rowSala3').style.cssText = sala3;
            document.getElementById('rowSala4').style.cssText = sala4;
            document.getElementById('rowSala5').style.cssText = sala5;
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

        function finalizarSesion(matricula) {
            if (matricula == "") {
                alert('ERROR: Se debe introducir una matricula');
                return;
            }
            if (confirm('Está seguro de que desea finalizar la sesión del usuario con la matricula ' + matricula + '?') == true) {
                //location.href = location.pathname.substring(1) + '?tipo=finalizar&matricula=' + matricula;
                document.getElementById('<%= field_matricula.ClientID %>').value = matricula;
                document.getElementById('btn_finalizar_sesion').click();
            }
        }

        function clickearBoton(elementid) {
            document.getElementById(elementid).click();
        }

        $('#exampleModal').on('show.bs.modal', function (e) {
            $(this)
                .find("input[type=text],input[type=number]")
                .val('')
                .end()

            document.getElementById('rad_alumno').checked = true;
            document.getElementById('list_computadora').selectedIndex = "0";
            document.getElementById('list_programa').selectedIndex = "0";
            document.getElementById('list_carreras').selectedIndex = "0";
        })

        $('#modalFinalizar').on('shown.bs.modal', function (e) {
            document.getElementById('tx_matricula_cerrar').focus();
        })

        function cerrar_modal(elementid, message) {
            alert(message);
            document.getElementById(elementid).click();
        }

    </script>

        </form>
</body>
</html>
