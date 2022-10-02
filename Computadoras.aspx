<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Computadoras.aspx.cs" Inherits="ComputacionFCQ.Computadoras" %>

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

<body id="body">
    <form id="form1" runat="server">
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
            <button class="SideBtn" onclick="document.getElementById('btn_programas').click(); return false;"><i class="bi bi-pc-display" style="font-size: 16px;margin-right:15px ; color:var(--fcq); align-self:center"></i>Computadoras</button>
            <!-- Elementos ASP para redireccionar-->
            <asp:Button runat="server" ID="btn_sesiones" OnClick="btn_sesiones_Click" Hidden=""/>
            <asp:Button runat="server" ID="btn_reservaciones" OnClick="btn_reservaciones_Click" Hidden=""/>
            <asp:Button runat="server" ID="btn_programas" OnClick="btn_programas_Click" Hidden=""/>
        </div>

        <!-- NAV-->
        <nav style="background:rgb(45,45,45); height:65px; display:flex; vertical-align:middle; border:none;">
            <button onclick="AbrirSide(); return false;" style="background:none; border:none; margin:15px;">
                <ion-icon name="menu-outline" style="font-size: 32px; color:whitesmoke; align-self:center;"></ion-icon>
            </button>

            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:0px; margin-top:10px;"></div>

            <i class="bi bi-pc-display" style="font-size: 18px; margin-left: 15px;margin-right:5px ; color:var(--fcq); align-self:center"></i>
            <a style="font-family: Arial; font-size:18px; margin-left:15px; color: whitesmoke; align-self:center;">Computadoras</a>
        </nav>

        <!-- Tabla de reportes-->
        <div class="card" style="margin: 30px; border-radius: 15px; background: white;">
            <div class="card-body">
                <div class="card-title" style="font-size: 18px; font-family: Arial; color: rgb(29,29,29);">Reportes</div>
                <hr style="margin-bottom:0px; margin-top:0px; color:gray;"/>
                <table class="table" style="margin: 0; border-radius: 5px; overflow: hidden; font-size:15px;">
                    <thead>
                        <tr>
                            <td style="width:50px;">Sala</td>
                            <td style="width:100px;">Computadora</td>
                            <td>Detalle</td>
                            <td style="width:200px; text-align:center;">Fecha del reporte</td>
                        </tr>
                    </thead>
                    <tbody id="rep_body"></tbody>
                </table>
            </div>
        </div>

        <!-- Tabla de computadoras -->
        <div class="card" style="border-radius: 15px; background: white; margin: 30px; margin:30px;">
            <div class="card-body">

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <div class="card-title" style="font-size: 18px; font-family: Arial; color: rgb(29,29,29); display: flex;">
                            <a style="margin-right: 15px;">Lista de computadoras</a>
                            <div class="vr" style="margin-right: 15px; height: 100%;"></div>
                            <a style="margin-right: 10px;">Sala</a>
                            <asp:DropDownList runat="server" ID="list_salas" AutoPostBack="true" Style="margin-right: 15px;" OnSelectedIndexChanged="ActualizarComputadoras"></asp:DropDownList>

                            <div style="position: absolute; right: 0px; margin-right: 15px;">
                                <i class="bi bi-check-circle-fill" style="color: lightgreen; font-size: 16px;"></i>
                                <asp:Label runat="server" ID="lbl_hab" Style="font-size: 16px; margin-right: 15px;" Text=" habilitadas"></asp:Label>
                                <i class="bi bi-x-circle-fill" style="color: lightpink; font-size: 16px;"></i>
                                <asp:Label runat="server" ID="lbl_no_hab" Style="font-size: 16px;" Text=" no habilitadas"></asp:Label>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <table class="table table-bordered">
                                <tbody id="tbody">
                                    <tr>
                                        <td style="margin: 0px; padding: 0px; width: calc(100% / 4);">
                                            <table class="table table-hover" style="margin: 0px;">
                                                <tbody id="10"></tbody>
                                            </table>
                                        </td>
                                        <td style="margin: 0px; padding: 0px; width: calc(100% / 4);">
                                            <table class="table table-hover" style="margin: 0px;">
                                                <tbody id="20"></tbody>
                                            </table>
                                        </td>
                                        <td style="margin: 0px; padding: 0px; width: calc(100% / 4);">
                                            <table class="table table-hover" style="margin: 0px;">
                                                <tbody id="30"></tbody>
                                            </table>
                                        </td>
                                        <td style="margin: 0px; padding: 0px; width: calc(100% / 4);">
                                            <table class="table table-hover" style="margin: 0px;">
                                                <tbody id="40"></tbody>
                                            </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Modal cerrar sesion-->
        <div class="modal fade" id="modal_detalle" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <div class="modal-header">
                                <button id="btn_cerrar_modal_top" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                test
                                <div class="modal-footer">
                                    <asp:Button ID="btn_cerrar_modal" runat="server" Text="Agregar programa"/>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <!--botones aux-->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Button runat="server" ID="detalle_aux" hidden="" OnClick="ComputadoraDetalle"/>
                <asp:Button runat="server" ID="aux" hidden="" OnClick="ActualizarComputadoras" />
                <asp:Button runat="server" ID="rep_aux" hidden="" OnClick="ActualizarReportes" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>


    <script type="text/javascript">
        function ComputadoraDetalle(id) {
            history.pushState(null, "", "Computadoras.aspx/"+id);
            document.getElementById('detalle_aux').click();
        }

        $('#modal_detalle').on('hide.bs.modal', function () {
            history.back();
        });

        document.getElementById('aux').click();

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
