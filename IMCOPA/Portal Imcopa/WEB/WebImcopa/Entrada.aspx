<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entrada.aspx.cs" Inherits="WebImcopa.Entrada"
    EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Entradas de Vendas</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">

        <script type="text/javascript">

            function VerificaNumero(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }

            function Limpar(valor, validos) {
                // retira caracteres invalidos da string
                var result = "";
                var aux;
                for (var i = 0; i < valor.length; i++) {
                    aux = validos.indexOf(valor.substring(i, i + 1));
                    if (aux >= 0) {
                        result += aux;
                    }
                }
                return result;
            }

            //Formata número tipo moeda usando o evento onKeyDown

            function Formata(campo, tammax, teclapres, decimal) {
                var tecla = teclapres.keyCode;
                vr = Limpar(campo.value, "0123456789");
                tam = vr.length;
                dec = decimal
                if (tam < tammax && tecla != 8) {
                    tam = vr.length + 1;
                }
                if (tecla == 8) {
                    tam = tam - 1;
                }
                if (tecla == 8 || tecla >= 48 && tecla <= 57 || tecla >= 96 && tecla <= 105) {
                    if (tam <= dec) {
                        campo.value = vr;
                    }
                    if ((tam > dec) && (tam <= 5)) {
                        campo.value = vr.substr(0, tam - 2) + "," + vr.substr(tam - dec, tam);
                    }
                    if ((tam >= 6) && (tam <= 8)) {
                        campo.value = vr.substr(0, tam - 5) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
                    }
                    if ((tam >= 9) && (tam <= 11)) {
                        campo.value = vr.substr(0, tam - 8) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
                    }
                    if ((tam >= 12) && (tam <= 14)) {
                        campo.value = vr.substr(0, tam - 11) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - dec, tam);
                    }
                    if ((tam >= 15) && (tam <= 17)) {
                        campo.value = vr.substr(0, tam - 14) + "." + vr.substr(tam - 14, 3) + "." + vr.substr(tam - 11, 3) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "," + vr.substr(tam - 2, tam);
                    }
                }
            }
        </script>

        <asp:ScriptManager ID="MasterScript" runat="server" EnableScriptGlobalization="True"
            EnableScriptLocalization="True" EnablePageMethods="true" AsyncPostBackTimeout="1800"
            EnablePartialRendering="true" />
        <asp:UpdatePanel ID="uppGeral" runat="server">
            <ContentTemplate>
                <div id="cabecalho" class="background">
                    <img alt="Imcopa" src="Imagens/LogoTeste.png" class="table" />
                </div>
                <div id="menu" class="div_menu">
                    <table>
                        <tr>
                            <td style="width: 40%;">
                                <uc1:cmenu ID="Cmenu1" runat="server" />
                            </td>
                            <td style="width: 20%;"></td>
                            <td style="width: 40%; text-align: right;">
                                <asp:Label ID="lblcdRepresentante" runat="server" CssClass="ocultar"></asp:Label>&nbsp;
                                <asp:Label ID="lblVendedor" runat="server">
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_mensagem">
                    <asp:Panel ID="pnlAvisos" runat="server" CssClass="avisos" Width="100%" Visible="false">
                        <asp:Image ID="imgAvisos" runat="server" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lblAvisos" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                <div id="campos" class="background2" style="height: 100%; padding-left: 5px;">
                    <br />
                    <table class="table" style="text-align: left; width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCNPJ" runat="server" CssClass="font">CNPJ:</asp:Label>
                                <asp:TextBox ID="txtCNPJ" runat="server" CssClass="textBox" Width="150px" OnTextChanged="txtCNPJ_TextChanged"
                                    AutoPostBack="True" TabIndex="1">
                                </asp:TextBox>
                                <asp:Label ID="lblCliente" runat="server" CssClass="font" Style="padding-left: 20px">Cliente:</asp:Label>
                                <asp:TextBox ID="txtCodCliente" runat="server" CssClass="textBox" Width="100px" OnTextChanged="txtCodCliente_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:Label ID="lblPedido" runat="server" CssClass="font" Style="padding-left: 20px">Nº Pedido do Cliente:</asp:Label>
                                <asp:TextBox ID="txtPedido" runat="server" CssClass="textBox" Width="100px" MaxLength="10"
                                    TabIndex="2"></asp:TextBox>

                                <asp:TextBox ID="txtTeste"  hidden runat="server" CssClass="textBox" Width="50px" AutoPostBack="True"></asp:TextBox>
                                <asp:TextBox ID="txtTeste2" hidden runat="server" CssClass="textBox" Width="50px" AutoPostBack="True"></asp:TextBox>
                              
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 15px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Width="102px">Código:</asp:Label>
                                <asp:Label ID="Label4" runat="server" Width="402px">Cliente:</asp:Label>
                                <asp:Label ID="Label5" runat="server" Width="292px">Cidade:</asp:Label>
                                <asp:Label ID="Label6" runat="server" Width="30px">UF:</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="lblCodCliente" runat="server" CssClass="textBox" disabled Width="100px"></asp:TextBox>
                                <asp:TextBox ID="lblNomeCliente" runat="server" CssClass="textBox" disabled Width="400px"></asp:TextBox>
                                <asp:TextBox ID="lblCidade" runat="server" CssClass="textBox" disabled Width="290px"></asp:TextBox>
                                <asp:TextBox ID="lblUF" runat="server" CssClass="textBox" disabled Width="30px"></asp:TextBox>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataRem" runat="server" CssClass="font" Width="80px">Data Entrega:</asp:Label>
                                <asp:TextBox ID="txtDataRem" runat="server" CssClass="textBox" Width="90px" TabIndex="6"></asp:TextBox>
                                <asp:ImageButton ID="btnCalendario" runat="server" AlternateText="Exibir calendário"
                                    ImageAlign="AbsMiddle" ImageUrl="~/Imagens/Calendar.gif" TabIndex="7" Height="18px"
                                    Width="18px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFilEmb" runat="server" CssClass="font" Width="80px">Fil. Embarque:</asp:Label>
                                <asp:DropDownList ID="ddlFilEmb" runat="server" CssClass="textBox" DataTextField="Text"
                                    DataValueField="Id" TabIndex="3" Width="150px" OnSelectedIndexChanged="ddlFilEmb_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="lblFrete" runat="server" CssClass="font" Style="margin-left: 37px;">Frete:</asp:Label>
                                <asp:DropDownList ID="ddlFrete" runat="server" CssClass="textBox" DataTextField="Text"
                                    TabIndex="4">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 0px 1px 0px 1px; height: 5px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" runat="server" CssClass="font" Width="80px">Email envio NF (Comprador):</asp:Label>
                                <asp:TextBox ID="TextBox10" runat="server" CssClass="textBox" Width="300px" TabIndex="6"></asp:TextBox>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlMateriais" runat="server" BorderWidth="4px" BorderColor="transparent"
                                    Visible="False">
                                    <asp:Label ID="Label1" runat="server" CssClass="textBox" Font-Size="12pt" Visible="False"></asp:Label>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="98%"
                                        DataKeyNames="MATNR" OnRowDataBound="GridView1_RowDataBound" TabIndex="9">
                                        <Columns>
                                            <asp:BoundField DataField="MATNR" HeaderText="C&#243;digo">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Quant.">
                                                <ItemStyle CssClass="GridView-Cell" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="textBoxMoney" Width="70px" MaxLength="5"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="MAKTX" HeaderText="Descri&#231;&#227;o">
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Unit. (R$)">
                                                <ItemStyle CssClass="GridView-Cell" />
                                                <ItemTemplate>
                                                    R$<asp:TextBox ID="TextBox2" runat="server" CssClass="textBoxMoney" AutoPostBack="True"
                                                        OnTextChanged="TextBox2_TextChanged" Width="110px" MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total (R$)">
                                                <ItemStyle CssClass="GridView-Cell" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="textBoxMoney" Width="110px" MaxLength="10"
                                                        ReadOnly="True"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="GridView-Row"></RowStyle>
                                        <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                        <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                        <HeaderStyle CssClass="GridView-HeaderRow"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" CssClass="font" Width="132px" Font-Size="10pt">Tot. Quantidade:</asp:Label>
                                <asp:TextBox ID="txtTotQuantidade" style="text-align:right" runat="server" disabled CssClass="textBox" Width="70px" Font-Size="12pt"></asp:TextBox>
                                <asp:Label ID="lblResultado" runat="server" CssClass="font" Font-Size="10pt" Style="padding-left: 845px">Total:</asp:Label>&nbsp;
                                <asp:TextBox ID="txtSubTotal" runat="server" disabled CssClass="textBoxMoney" Font-Size="12pt"
                                    ReadOnly="True" Width="140px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 5px;" colspan="5"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" Width="350px" runat="server" CssClass="font">Informações adicionais (NF):</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAdicionais" runat="server" TextMode="MultiLine" Width="350px"
                                    Height="50px" MaxLength="132" CssClass="textBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 60px; padding-top: 20px; padding-left: 84%;">
                                <asp:Button ID="btnGravar" runat="server" Text="Gravar" CssClass="linkbotao" OnClick="btnGravar_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="subtotal" class="background" style="background-color: transparent;">
                    <asp:Timer ID="Timer1" runat="server" Interval="200" OnTick="Timer1_Tick" Enabled="False">
                    </asp:Timer>
                    <cc1:MaskedEditExtender ID="meeCNPJ" runat="server" MaskType="Number" TargetControlID="txtCNPJ"
                        Mask="99,999,999/9999-99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" ClearMaskOnLostFocus="False" InputDirection="RightToLeft">
                    </cc1:MaskedEditExtender>
                    <cc1:MaskedEditExtender ID="meeDataRem" runat="server" TargetControlID="txtDataRem"
                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="None" ErrorTooltipEnabled="True" />
                    <cc1:CalendarExtender ID="ceDataRem" runat="server" TargetControlID="txtDataRem"
                        PopupButtonID="btnCalendario" />
                </div>
                <div id="rodape" class="rodape">
                    © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                        href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="uppAtualizando" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <div id="div_Tela" class="centralizarBack">
                    <table id="tb_aguarde" class="centralizar">
                        <tr>
                            <td>
                                <img src="Imagens/progressBar.gif" alt="" />
                            </td>
                        </tr>
                        <tr>
                            <td>Por favor, aguarde...
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
