<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reservaciones.aspx.cs" Inherits="ComputacionFCQ.Reservaciones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Calendario</title>
    <link href="styles.css" rel="stylesheet" type="text/css"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/js/bootstrap.bundle.min.js" integrity="sha384-pprn3073KE6tl6bjs2QrFaJGz5/SUsLqktiwsUTF55Jfv3qYSDhgCecCxMW52nD2" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0-beta1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-0evHe/X+R7YkIZDRvuzKMRqM+OrBnVFBL6DOitfPri4tjfHxaWutUpFmBp4vmVor" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.9.1/font/bootstrap-icons.css"/>
    <script type="module" src="https://unpkg.com/ionicons@5.5.2/dist/ionicons/ionicons.esm.js"></script>
   
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server">
        </asp:ScriptManager>

        <nav style="background:rgb(45,45,45); height:65px; display:flex; vertical-align:middle; border:none;">

            <button style="background-color:rgb(45,45,45); border:none; color:whitesmoke;">
                <ion-icon name="home" style="font-size: 16px; margin-left: 15px; color:var(--fcq); align-self:center"></ion-icon> Sesiones
            </button>
            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:15px;margin-top:10px;"></div>

            <button style="background-color:rgb(45,45,45); border:none; color:whitesmoke;">
                <ion-icon name="calendar" style="font-size: 16px; margin-left: 15px; color:var(--fcq); align-self:center"></ion-icon> Reservaciones
            </button>
            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:15px;margin-top:10px;"></div>

            <button style="background-color:rgb(45,45,45); border:none; color:whitesmoke;">
                <ion-icon name="apps" style="font-size: 16px; margin-left: 15px; color:var(--fcq); align-self:center"></ion-icon> Programas
            </button>
            <div class="vr" style="height:45px; background-color:whitesmoke; margin-left:15px;margin-top:10px;"></div>

            <button style="background-color:rgb(45,45,45); border:none; color:whitesmoke;">
                <i class="bi bi-pc-display" style="font-size: 16px; margin-left: 15px;margin-right:5px ; color:var(--fcq); align-self:center"></i>Computadoras  
            </button>

        </nav>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                    <asp:Label ID="dia" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="mes" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="ano" runat="server" Text="" Visible="false"></asp:Label>

                </ContentTemplate>
        </asp:UpdatePanel>

        <!--table -->
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="card" style="border-radius: 15px; background: snow; margin-left:5%; margin-right:5%; margin-top:30px;">
                    <div class="card-body">

                        <!-- Header del card -->
                        <div style="justify-content: center; display: flex; vertical-align:central">

                            <a style="position:absolute; left:20px; margin-top:5px;">Sala: </a>

                            <asp:DropDownList runat="server" ID="calendario_sala" AutoPostBack="true" OnSelectedIndexChanged="ActualizarReservaciones"
                                style="position:absolute; left:60px; margin-top:5px;"></asp:DropDownList>

                            <asp:Label runat="server" Style="margin-left: 6px; font-size:24px; font-weight: 500; font-family: Arial; color: dimgray;"
                                Text="" ID="calendario"></asp:Label>

                            <asp:Button ID="Button6" runat="server" Text="<- Semana anterior" OnClick="SemanaAnterior" class="btn btn-outline-secondary"
                                Style="position: absolute; right: 200px; height:30px; padding-top:0px; padding-bottom:0px" />

                            <asp:Button ID="Button7" runat="server" Text="Semana siguiente ->" OnClick="SemanaSiguiente" class="btn btn-outline-secondary"
                                Style="position: absolute; right: 20px; height:30px; padding-top:0px; padding-bottom:0px" />
                        </div>

                        <!-- Calendario -->
                        <div class="table-responsive">
                            <table cellspacing="1" cellpadding="1" style="width:100%; table-layout:fixed; margin-top:15px;">
                                <thead>
                                    <tr>
                                        <th class="cell-header">Horario</th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HLU"></asp:Label></th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HMA"></asp:Label></th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HMI"></asp:Label></th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HJU"></asp:Label></th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HVI"></asp:Label></th>
                                        <th class="cell-header">
                                            <asp:Label runat="server" ID="HSA"></asp:Label></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="cell-hora">7:00 - 8:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA7" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">8:00 - 9:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA8" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">9:00 - 10:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA9" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">10:00 - 11:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA10" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">11:00 - 12:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA11" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">12:00 - 13:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA12" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">13:00 - 14:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA13" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">14:00 - 15:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA14" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">15:00 - 16:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA15" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">16:00 - 17:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA16" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">17:00 - 18:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA17" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">18:00 - 19:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA18" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">19:00 - 20:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU19" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA19" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI19" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU19" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI19" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA19" CssClass="reserv" Visible="false" /></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">20:00 - 21:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU20" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA20" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI20" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU20" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI20" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA20" CssClass="reserv" Visible="false" /></td>
                                    </tr>
                                    <tr>
                                        <td class="cell-hora">21:00 - 22:00</td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="LU21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MA21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="MI21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="JU21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="VI21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                        <td class="cell-content">
                                            <asp:Button runat="server" ID="SA21" CssClass="reserv" Visible="false" OnClick="ReservacionDetalle"/></td>
                                    </tr>
                                </tbody>

                            </table>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!--Tabla reservaciones -->
        <div class="card" style="border-radius: 15px; background: snow; margin-left:5%; margin-right:5%; margin-top:30px; margin-bottom:30px;">
            <div class="card-body" id="card">

                <asp:UpdatePanel runat="server" style="margin-bottom: 75px;">
                    <ContentTemplate>

                        <div style="margin-left: 6px; font-size: large; font-weight: 500; font-family: Arial; color: dimgray;">Lista de reservaciones</div>

                        <table class="table table-borderless" style="table-layout:fixed;">
                            <tr>
                                <th class="cell" style="padding-left:15px; width:330px;">Desde:</th>
                                <th class="cell" style="padding-left:15px; width:330px">Hasta:</th>
                                <th class="cell" style="padding-left:15px; width:150px;">Salas</th>
                                <th style="padding-left:15px; width:150px;">Tipo</th>
                            </tr>
                            <tr>
                                <td class="cell" style="padding-left:15px;">
                                    <asp:RadioButton runat="server" ID="tabla_desde_inicio" Text="Inicio del semestre" GroupName="t1" Checked="true"/>
                                    <br />
                                    <asp:RadioButton runat="server" ID="RadioButton2" Text="Otra:" GroupName="t1"/>
                                    <asp:DropDownList runat="server" ID="tabla_desde_mes" Enabled="false"></asp:DropDownList><a> / </a>
                                    <asp:DropDownList runat="server" ID="tabla_desde_dia" Enabled="false"></asp:DropDownList><a> / </a>
                                    <asp:TextBox runat="server" type="number" style="width:50px; height:24px;" Enabled="false"></asp:TextBox>
                                </td>
                                <td class="cell" style="padding-left:15px;">
                                    <asp:RadioButton runat="server" ID="tabla_hasta_fin" Text="Fin del semestre" GroupName="t2" Checked="true"/>
                                    <br />
                                    <asp:RadioButton runat="server" ID="tabla_hasta_otra" Text="Otra:" GroupName="t2"/>
                                    <asp:DropDownList runat="server" ID="tabla_hasta_mes" Enabled="false"></asp:DropDownList><a> / </a>
                                    <asp:DropDownList runat="server" ID="tabla_hasta_dia" Enabled="false"></asp:DropDownList><a> / </a>
                                    <asp:TextBox runat="server" type="number" style="width:50px; height:24px;" Enabled="false"></asp:TextBox>
                                </td>
                                <td style="width:100px;padding-left:15px;" class="cell">
                                    <asp:CheckBox runat="server" ID="cb_sala1" Checked="true"/> <asp:Label runat="server" style="padding-left:5px">Sala 1</asp:Label><br />
                                    <asp:CheckBox runat="server" ID="cb_sala2" Checked="true"/> <asp:Label runat="server" style="padding-left:5px">Sala 2</asp:Label><br />
                                    <asp:CheckBox runat="server" ID="cb_sala3" Checked="true"/> <asp:Label runat="server" style="padding-left:5px">Sala 3</asp:Label><br />
                                    <asp:CheckBox runat="server" ID="cb_sala4" Checked="true"/> <asp:Label runat="server" style="padding-left:5px">Sala 4</asp:Label><br />
                                    <asp:CheckBox runat="server" ID="cb_sala5" Checked="true"/> <asp:Label runat="server" style="padding-left:5px">Sala 5</asp:Label><br />
                                </td>
                                <td style="padding-left:15px;">
                                    <asp:CheckBox runat="server" ID="cb_unica" Checked="true"/> <asp:Label runat="server" style="padding-left:5px;">Unicas</asp:Label><br />
                                    <asp:CheckBox runat="server" ID="cb_frecuencia" Checked="true"/> <asp:Label runat="server" style="padding-left:5px;">Frecuencias</asp:Label><br />
                                </td>
                            </tr>
                        </table>

                        <asp:Button runat="server" ID="btn_aplicar" Text="Aplicar filtros" class="btn btn-primary" 
                            style="margin:0px; width:150px; position:absolute; right:180px; height:30px; padding:0px;"/>
                        <asp:Button runat="server" ID="btn_agregar" Text="Crear reservación" class="btn btn-success" OnClick="AbrirModal"
                            style="margin:0px; width:150px; position:absolute; right: 15px; height:30px; padding:0px;"/>

                    </ContentTemplate>
                </asp:UpdatePanel>

                <hr style="margin-bottom:7px;"/>

                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th scope="col" style="font-weight: normal">Curso</th>
                                <th scope="col" style="font-weight: normal">Docente</th>
                                <th scope="col" style="font-weight: normal">Sala</th>
                                <th scope="col" style="font-weight: normal">Fecha</th>
                                <th scope="col" style="font-weight: normal">Estado</th>
                                <th scope="col" style="font-weight: normal"></th>
                            </tr>
                        </thead>
                        <tbody id="tbody">
                        </tbody>
                    </table>
                </div>
            </div>
            <div id="card-footer" style="text-align: center;"></div>
        </div>


        <!-- Modal hacer reservacion-->
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>

                            <div class="modal-header">
                                <asp:Label runat="server" ID="TituloModal" Text="Hacer una reservación" style="font-size:large"></asp:Label>
                                <button id="cerrar_modal" type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div class="modal-body">
                                <a>Matricula:</a>
                                <asp:TextBox ID="tx_matricula" runat="server" type="number" onkeydown="return event.key;"></asp:TextBox>
                                <asp:Button ID="btn_buscar" runat="server" Text="Buscar datos" OnClick="btn_buscar_Click"/>
                                <br />

                                <a>Nombre(s):</a>
                                <asp:TextBox ID="tx_nombres" runat="server" onkeydown="return /[a-zA-Z\s\ñ]/i.test(event.key)"></asp:TextBox>

                                <br />
                                <a>Apellidos:</a>
                                <asp:TextBox ID="tx_apellidos" runat="server" onkeydown="return /[a-zA-Z\s\ñ]/i.test(event.key)"></asp:TextBox>

                                <br />
                                <a>Carrera/Coordinación: </a>
                                <asp:DropDownList ID="list_carreras" runat="server" AutoPostBack="false"></asp:DropDownList>

                                <br />
                                <a>Correo institucional:</a>
                                <asp:TextBox ID="tx_correo" runat="server"></asp:TextBox>

                                <br /><br />
                                Tipo de evento:
                                <asp:RadioButton ID="rad_unico" Text="Único" runat="server" AutoPostBack="true" GroupName="1" Checked="true" OnCheckedChanged="rad_Unico_Checked"/>
                                <asp:RadioButton ID="rad_frecuente" Text="Frecuente" runat="server" AutoPostBack="true" GroupName="1" OnCheckedChanged="rad_Frecuente_Checked"/> 
                                <br />
                                <a>Curso/Materia: </a>
                                <asp:TextBox ID="tx_curso" runat="server"></asp:TextBox>
                                <br />
                                <a>Sala:</a>
                                <asp:DropDownList ID="list_sala" runat="server" AutoPostBack="true" OnSelectedIndexChanged="list_sala_SelectedIndexChanged"></asp:DropDownList>
                                <a>Programa:</a>
                                <asp:DropDownList ID="list_programa" runat="server"></asp:DropDownList>
                                <br />
                                <a>Cantidad de alumnos:</a>
                                <asp:TextBox ID="tx_alumnos" runat="server" type="number" onkeydown="return event.key;"></asp:TextBox>
                                <br /> <br /> 

                                <!-- Panel de tipo de evento UNICO -->
                                <asp:Panel runat="server" id="TipoUnico">
                                    <a>Dia: </a>
                                    <asp:DropDownList ID="lista_dia_unico" runat="server"></asp:DropDownList>
                                    <a>Mes: </a>
                                    <asp:DropDownList ID="lista_mes_unico" runat="server" OnSelectedIndexChanged="lista_mes_unico_onSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <br />
                                    <a>Hora inicio: </a>
                                    <asp:DropDownList ID="hora_inicio_unico" runat="server"></asp:DropDownList>
                                    <a>Hora fin: </a>
                                    <asp:DropDownList ID="hora_fin_unico" runat="server"></asp:DropDownList>
                                </asp:Panel>

                                 <!-- Panel de tipo de evento FRECUENTE-->
                                <asp:Panel runat="server" ID="TipoFrecuente">

                                    <!-- Desde -->
                                    <asp:Label runat="server" style="width:50px; display:inline-block;" Text="Desde:"/>
                                    <asp:RadioButton runat="server" ID="desde_inicio" GroupName="2" Checked="true" onclick="check_fecha(this.id);"/>
                                    <asp:Label runat="server" id="label_inicio" style="margin-left:3px;" Text="Inicio del semestre " /> 
                                    <br />
                                    <a style="width:50px; display:inline-block;"></a>
                                    <asp:RadioButton runat="server" ID="desde_otra" GroupName="2" onclick="check_fecha(this.id);"/>
                                    <a style="margin-left:3px; margin-right:10px;"> Otra fecha</a>
                                    <a>Mes: </a> <asp:DropDownList ID="mes_desde" runat="server" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                                    <a>Dia: </a> <asp:DropDownList ID="dia_desde" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />
                                    <!-- Hasta -->
                                    <asp:Label runat="server" style="width:50px; display:inline-block;" Text="Hasta:"/>
                                    <asp:RadioButton runat="server" ID="hasta_inicio" GroupName="3" Checked="true" onclick="check_fecha(this.id);"/>
                                    <asp:Label runat="server" id="label_final" style="margin-left:3px;" Text="Final del semestre " /> 
                                    <br />
                                    <a style="width:50px; display:inline-block;"></a>
                                    <asp:RadioButton runat="server" ID="hasta_otra" GroupName="3" onclick="check_fecha(this.id);"/>
                                    <a style="margin-left:3px; margin-right:10px;"> Otra fecha</a>
                                    <a>Mes: </a> <asp:DropDownList ID="mes_hasta" runat="server" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                                    <a>Dia: </a> <asp:DropDownList ID="dia_hasta" runat="server" Enabled="false"></asp:DropDownList>
                                    <br /><br />
                                    <!-- Dias-->
                                    <asp:CheckBox ID="cb_lu" runat="server" onclick="check_dias(this.id);" /><a style="width:80px; display:inline-block; margin-left:5px">Lunes</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="lu_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="lu_hora_fin" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />

                                    <asp:CheckBox ID="cb_ma" runat="server" onclick="check_dias(this.id);"/><a style="width:80px; display:inline-block; margin-left:5px">Martes</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="ma_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="ma_hora_fin" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />

                                    <asp:CheckBox ID="cb_mi" runat="server" onclick="check_dias(this.id);"/><a style="width:80px; display:inline-block; margin-left:5px">Miercoles</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="mi_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="mi_hora_fin" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />

                                    <asp:CheckBox ID="cb_ju" runat="server" onclick="check_dias(this.id);"/><a style="width:80px; display:inline-block; margin-left:5px">Jueves</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="ju_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="ju_hora_fin" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />

                                    <asp:CheckBox ID="cb_vi" runat="server" onclick="check_dias(this.id);"/><a style="width:80px; display:inline-block; margin-left:5px">Viernes</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="vi_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="vi_hora_fin" runat="server" Enabled="false"></asp:DropDownList>
                                    <br />

                                    <asp:CheckBox ID="cb_sa" runat="server" onclick="check_dias(this.id);"/><a style="width:80px; display:inline-block; margin-left:5px">Sabados</a>
                                    <a> Hora inicio: </a><asp:DropDownList ID="sa_hora_inicio" runat="server" Enabled="false"></asp:DropDownList>
                                    <a> Hora fin: </a><asp:DropDownList ID="sa_hora_fin" runat="server" Enabled="false"></asp:DropDownList>

                                </asp:Panel>

                                <div class="modal-footer">
                                    <asp:Button ID="Button2" runat="server" Text="Hacer reservación" OnClick="HacerReservacion"/>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>

        <script type="text/javascript">
            var dt = new Date();

            function ClickearBoton(elemento_id) {
                document.getElementById(elemento_id).click();
            }

            function check_fecha(id) {
                if (id == 'desde_inicio') {
                    document.getElementById('mes_desde').disabled = true;
                    document.getElementById('dia_desde').disabled = true;
                }
                if (id == 'desde_otra') {
                    document.getElementById('mes_desde').disabled = false;
                    document.getElementById('dia_desde').disabled = false;
                }
                if (id == 'hasta_inicio') {
                    document.getElementById('mes_hasta').disabled = true;
                    document.getElementById('dia_hasta').disabled = true;
                }
                if (id == 'hasta_otra') {
                    document.getElementById('mes_hasta').disabled = false;
                    document.getElementById('dia_hasta').disabled = false;
                }
            }

            function check_dias(id) {
                var dia = id.substring(3);

                if (document.getElementById(id).checked == true) {
                    document.getElementById(`${dia}_hora_inicio`).disabled = false;
                    document.getElementById(`${dia}_hora_fin`).disabled = false;
                }
                else {
                    document.getElementById(`${dia}_hora_inicio`).disabled = true;
                    document.getElementById(`${dia}_hora_fin`).disabled = true;
                }
            }

            function cerrar_modal(elementid, message) {
                alert(message);
                document.getElementById(elementid).click();
            }

        </script>

    </form>
</body>
</html>
