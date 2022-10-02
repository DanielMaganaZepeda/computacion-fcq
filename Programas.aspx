<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Programas.aspx.cs" Inherits="ComputacionFCQ.Programas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Programas</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
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

        <div id="back" onclick="CerrarSide();"></div>

        <div id="side" hidden="">

            <div style="user-select: none; margin:15px; display:flex; align-items:center;">
                <img src="https://seeklogo.com/images/F/facultad-de-ciencias-quimicas-logo-82ADF31672-seeklogo.com.png"
                    style="height: 50px;" />
                <a style="font-family: Arial; font-size: 22px; color:rgb(29,29,29); text-align:center; flex-grow:1;">Sala de computación</a>
            </div> 

            <button class="SideBtn" onclick="document.getElementById('btn_sesiones').click(); return false;"><ion-icon name="home" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Sesiones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_reservaciones').click(); return false;"><ion-icon name="calendar" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Reservaciones</button>
            <button class="SideBtn" onclick="document.getElementById('btn_programas').click(); return false;"><ion-icon name="apps" style="font-size: 16px; margin-right: 15px; color:var(--fcq);"></ion-icon>Programas</button>

            <asp:Button runat="server" ID="btn_sesiones" OnClick="btn_sesiones_Click" Hidden=""/>
            <asp:Button runat="server" ID="btn_reservaciones" OnClick="btn_reservaciones_Click" Hidden=""/>
            <asp:Button runat="server" ID="btn_programas" OnClick="btn_programas_Click" Hidden=""/>
        </div>

        <nav style="background:rgb(45,45,45); height:65px; display:flex; vertical-align:middle; border:none;">
            <button onclick="AbrirSide(); return false;" style="background:none; border:none; margin:15px;">
                <ion-icon name="menu-outline" style="font-size: 32px; color:whitesmoke; align-self:center;"></ion-icon>
            </button>

            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:0px; margin-top:10px;"></div>

            <ion-icon name="apps" style="font-size: 20px; margin-left: 15px; color:var(--fcq); align-self:center"></ion-icon>
            <a style="font-family: Arial; font-size:18px; margin-left:15px; color: whitesmoke; align-self:center;">Programas</a>
        </nav>

        <div class="card" style="margin: 30px; border-radius: 15px; background: white">
            <div class="card-body" id="card">

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>

                        <div style="display: flex; background-color: none; height:40px;">
                            <a style="font-size: 18px; font-weight: 500; font-family: Arial; color: dimgray; align-self: center">Lista de programas</a>

                            <button id="btn_editar" class="btn btn-primary" style="height: 35px; align-self: center; right: 210px; position: absolute; display: flex; text-align: center;" onclick="btn_editar_Click(); return false;">
                                <div style="display: flex; align-self: center; background-color: none;">
                                    <ion-icon name="create-outline" style="font-size: 24px; text-align: center; align-self: center; margin-right: 5px;"></ion-icon>
                                    <a style="font-family: Arial; font-size: 16px; align-self: center">Editar</a>
                                </div>
                            </button>

                            <button  id="btn_agregar" class="btn btn-success" style="height: 35px; align-self: center; right: 15px; position: absolute; display: flex; text-align: center;" onclick="btn_agregar_Click(); return false;">
                                <div style="display: flex; align-self: center; background-color: none;">
                                    <ion-icon name="add-outline" style="font-size: 24px; text-align: center; align-self: center; margin-right: 5px;"></ion-icon>
                                    <a style="font-family: Arial; font-size: 16px; align-self: center">Agregar programa</a>
                                </div>
                            </button>

                            <button  id="btn_cancelar" class="btn btn-danger" style="height: 35px; align-self: center; right: 200px; position: absolute; display: flex; text-align: center;" hidden="" onclick="btn_cancelar_Click(); return false;">
                                <div style="display: flex; align-self: center; background-color: none;">
                                    <ion-icon name="close-outline" style="font-size: 24px; text-align: center; align-self: center; margin-right: 5px;"></ion-icon>
                                    <a style="font-family: Arial; font-size: 16px; align-self: center">Cancelar</a>
                                </div>
                            </button>

                            <button  id="btn_aplicar" class="btn btn-success" style="height: 35px; align-self: center; right: 15px; position: absolute; display: flex; text-align: center;" hidden="" onclick="btn_aplicar_Click(); return false;">
                                <div style="display: flex; align-self: center; background-color: none;">
                                    <ion-icon name="checkmark-outline" style="font-size: 24px; text-align: center; align-self: center; margin-right: 5px;"></ion-icon>
                                    <a style="font-family: Arial; font-size: 16px; align-self: center">Aplicar cambios</a>
                                </div>
                            </button>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <hr style="margin-bottom:0px;"/>

                <div class="table-responsive">
                    <table class="table table-hover" style="margin-top:0px; margin-bottom:0px;">
                            <tr>
                                <th scope="col" style="font-weight: normal">Nombre</th>
                                <th scope="col" style="font-weight: normal; text-align:center;">Sala 1</th>
                                <th scope="col" style="font-weight: normal; text-align:center;">Sala 2</th>
                                <th scope="col" style="font-weight: normal; text-align:center;">Sala 3</th>
                                <th scope="col" style="font-weight: normal; text-align:center;">Sala 4</th>
                                <th scope="col" style="font-weight: normal; text-align:center;">Sala 5</th>
                                <th scope="col" style="font-weight: normal; text-align:center;"></th>
                            </tr>
                        
                        <tbody id="tbody">
                        </tbody>

                    </table>
                </div>
            </div>
            <div id="card-footer" style="text-align: center;"></div>
        </div>

        <!-- Modal cerrar sesion-->
        <div class="modal fade" id="modalAgregarPrograma" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <div class="modal-header">
                                <h5 class="modal-title">Agregar un programa</h5>
                                <button id="cerrar_modal_cerrar_sesion" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <a>Nombre del programa:</a>
                                <asp:TextBox ID="tx_nombre" runat="server"/>
                                <br />
                                <br />
                                <div class="modal-footer">
                                    <asp:Button ID="btn_cerrar_modal" runat="server" Text="Agregar programa" OnClick="btn_cerrar_modal_Click" />
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">

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

        var editable = false;
        var ids = new Array;

        function EliminarPrograma(id) {
            if (window.confirm("¿Está seguro de que desea eliminar este programa?")) {
                PageMethods.EliminarPrograma(id);
                alert('El programa ha sido eliminado con exito');
                location.reload();
            }
        }

        function btn_editar_Click() {
            document.getElementById('btn_editar').hidden = true;
            document.getElementById('btn_agregar').hidden = true;
            document.getElementById('btn_cancelar').hidden = false;
            document.getElementById('btn_aplicar').hidden = false;
            editable = true;
            Hover();
        }

        function btn_agregar_Click() {
            $("#modalAgregarPrograma").modal('show');
        }

        function btn_cancelar_Click() {
            document.getElementById('btn_editar').hidden = false;
            document.getElementById('btn_agregar').hidden = false;
            document.getElementById('btn_cancelar').hidden = true;
            document.getElementById('btn_aplicar').hidden = true;
            ids = new Array;
            editable = false;
            Hover();
            alert('Los cambios no se han aplicado');
        }

        function btn_aplicar_Click() {
            if (ids.length == 0) {
                alert('ERROR: No se ha seleccionado ningun programa para aplicar cambios');
            }
            else {
                document.getElementById('btn_editar').hidden = false;
                document.getElementById('btn_agregar').hidden = false;
                document.getElementById('btn_cancelar').hidden = true;
                document.getElementById('btn_aplicar').hidden = true;
                PageMethods.Prueba(ids);
                alert('Los cambios se han realizado con exito');
            }
        }

        function Hover() {
            const cbs = document.getElementsByTagName('input');

            for (i = 0; i< cbs.length; i++) {
                if (cbs[i].type == 'checkbox') {
                    if (cbs[i].className == "cb")
                        cbs[i].className = "";
                    else
                        cbs[i].className = "cb";
                }
            }
        }

        function CB_Click(id) {
            if (editable) {
                if (ids.includes(id)) {
                    ids = ids.filter((item) => item != id);
                }
                else {
                    ids.push(id);
                }
            }
            else {
                alert('Para hacer modificaciones debe habilitar la edición');
                document.getElementById(id).checked = !document.getElementById(id).checked;
            }
        }
    </script>
</body>
</html>
