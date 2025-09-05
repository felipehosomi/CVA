<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" Codebehind="Configuracao.aspx.cs"
    Inherits="WebImcopa.Configuracao" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Configuração</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="1800"
            EnablePartialRendering="false" />
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
                            <td style="width: 20%;">
                            </td>
                            <td style="width: 40%; text-align: right;">
                                <asp:Label ID="lblcdRepresentante" runat="server" CssClass="ocultar"></asp:Label>&nbsp;
                                <asp:Label ID="lblVendedor" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_mensagem">
                    <asp:Panel ID="pnlAvisos" runat="server" CssClass="avisos" Width="100%" Visible="false">
                        <asp:Image ID="imgAvisos" runat="server" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lblAvisos" runat="server">
                        </asp:Label>
                    </asp:Panel>
                </div>
                <div id="campos" class="background">
                    <br />
                    <div class="center">
                        <center>
                            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" Width="95%"
                                Height="300px" OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
                                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="VAL">
                                    <HeaderTemplate>
                                        Filtros Adicionais
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <br />
                                        <center>
                                            <table class="table" style="width: 80%" cellspacing="3">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAutho" runat="server" Text="AUTHO - Usuário de teste"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAutho" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:Label ID="lblBrco" runat="server" Text="(Deixar em branco)"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblFemb" runat="server" Text="FEMB - Filial de Embarque"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtFemb" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtFemb"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblFrete" runat="server" Text="FRETE - Tipo de Frete"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtFrete" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtFrete"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPagto" runat="server" Text="PAGTO - Forma de Pagamento"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtPagto" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtPagto"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        &nbsp;<asp:Button ID="btnOK03" runat="server" Text="Salvar" CssClass="linkbotao"
                                                            OnClick="btnOK03_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="SAP">
                                    <HeaderTemplate>
                                        Conexão SAP
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <br />
                                        <center>
                                            <table class="table" style="width: 55%" cellspacing="3">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label1" runat="server" Text="Cliente"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtCliente" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCliente"
                                                            ErrorMessage="*" CssClass="textBox"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label2" runat="server" Text="Usuário"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUsuario"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label3" runat="server" Text="Senha"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtSenha" runat="server" CssClass="textBox" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSenha"
                                                            ErrorMessage="*" EnableClientScript="False"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label4" runat="server" Text="Lang"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtLang" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLang"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label5" runat="server" Text="Host"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtHost" runat="server" CssClass="textBox" Width="300px"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtHost"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label6" runat="server" Text="SysNr"></asp:Label></td>
                                                    <td align="left" style="width: 342px">
                                                        <asp:TextBox ID="txtSysnr" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtSysnr"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" style="height: 21px">
                                                        &nbsp;<asp:Button ID="btnOK01" runat="server" Text="Salvar" CssClass="linkbotao"
                                                            OnClick="btnOK01a_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="ADM">
                                    <HeaderTemplate>
                                        Active Directory
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <br />
                                        <center>
                                            <table class="table" style="width: 35%" cellspacing="3">
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label7" runat="server" Text="User"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAdministradorAD" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtAdministradorAD"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="Label8" runat="server" Text="Password"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtSenhaAD" runat="server" CssClass="textBox" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtSenhaAD"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblADServer" runat="server" Text="Server"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtADServer" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtADServer"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblADPath" runat="server" Text="Path"></asp:Label></td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtADPath" runat="server" CssClass="textBox"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtADPath"
                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        &nbsp;<asp:Button ID="btnOK02" runat="server" Text="Salvar" CssClass="linkbotao"
                                                            OnClick="btnOK02_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </center>
                    </div>
                    <br />
                </div>
                <div id="rodape" class="rodape">
                    © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                        href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
