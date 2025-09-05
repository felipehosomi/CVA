<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Login.aspx.cs" Inherits="WebImcopa.Login"
    EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Efetuar Login</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" EnableScriptGlobalization="True"
            EnableScriptLocalization="True" EnablePageMethods="true" AsyncPostBackTimeout="1800"
            EnablePartialRendering="false" />
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
                            <td>
                                Por favor, aguarde...
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="uppGeral" runat="server">
            <ContentTemplate>
                <div id="cabecalho" class="background">
                    <img alt="Imcopa" src="Imagens/LogoTeste.png" class="table" />
                </div>
                <div id="menu" class="div_menu">
                    <table>
                        <tr>
                            <td style="width: 40%;">
                            </td>
                            <td style="width: 20%;">
                            </td>
                            <td style="width: 40%; text-align: right;">
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="div_mensagem">
                    <asp:Panel ID="pnlAvisos" runat="server" CssClass="avisos" Width="100%" Visible="false">
                        <asp:Image ID="imgAvisos" runat="server" ImageAlign="AbsMiddle" />
                        <asp:Label ID="lblAvisos" runat="server"></asp:Label></asp:Panel>
                </div>
                
                <center>
                    <div id="campos" class="background2">
                        <br />
                        <asp:Panel ID="login" runat="server" BackColor="white" Width="300px" >
                            <asp:Label ID="pnl_Titulo" runat="server" CssClass="label" Width="100%" Text="Login" style="line-height:32px;font-size:14px;font-weight:600;border-top-left-radius:7px;border-top-right-radius:7px;"></asp:Label>
                            <br />
                            <table border="0" cellpadding="0" width="100%">
                                <tr class="tr-formulario">
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" ForeColor="#339966">Usuário:</asp:Label>&nbsp;
                                        &nbsp;</td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="UserName" runat="server" CssClass="fontes_lowerLogin" Width="150px"
                                            TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            ErrorMessage="Usuário requerido." ToolTip="Usuário requerido." ValidationGroup="Login">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="tr-formulario">
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" ForeColor="#339966">Senha:</asp:Label>&nbsp;
                                        &nbsp;</td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="150px" TabIndex="2"></asp:TextBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <center>
                                            <br />
                                            <asp:LinkButton ID="lkbNovaSenha" runat="server" Text="Solicitar nova senha" 
                                                style="color:Blue" onclick="lkbNovaSenha_Click"></asp:LinkButton>
                                        </center>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <center>
                                            <br />
                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" OnClick="LoginButton_Click"
                                                Text="Entrar..." ValidationGroup="Login" CssClass="linkbotao" ToolTip="Digite seu login e senha do sistema."
                                                TabIndex="3" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                    </div>
                </center>
                
                <div id="rodape" class="rodape">
                    © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                        href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
