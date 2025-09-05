<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebImcopa._Default"
    EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Início</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" EnablePartialRendering="false" />
        <asp:UpdatePanel ID="uppGeral" runat="server">
            <ContentTemplate>
                <div>
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
                                    <asp:Label ID="lblVendedor" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="campos" class="background2">
                        <table id="Table1" cellspacing="1" cellpadding="3" border="0" style="text-align: left;">
                            <tr>
                                <td>
                                    <div>
                                        <p class="font" style="font-size: 12pt">
                                            <b>Seja bem-vindo ao Portal de Vendas Imcopa!</b>
                                        </p>
                                        <p class="font" style="font-size: 12pt">
                                            <span lang="PT-BR">Esta ferramenta foi desenvolvida com o objetivo de melhorar a comunicação com os representantes e otimizar o processo de atendimento aos pedidos de venda de nossos clientes.</span>
                                        </p>
                                        <p class="font" style="font-size: 12pt">
                                            <span lang="PT-BR">Aqui será possível realizar as seguintes atividades:</span>
                                        </p>
                                        <ul class="font" type="disc" style="font-size: 12pt; margin-top: 0in">
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Criação do pedido de compras</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Consulta individual do pedido e impressão</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Cadastro de clientes</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Consultar status do cliente</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Consultar metas de vendas</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Histórico de vendas ao cliente</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Consulta de títulos em aberto</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Listagem de faturamento</span></li>
                                            <li class="font"><span lang="PT-BR" style="font-size: 12pt">Relatórios podem ser exportados para o Excel</span></li>
                                        </ul>
                                        <p class="font" style="font-size: 12pt">
                                            <span lang="PT-BR">Para mais informações, entre em contato com a equipe de suporte:</span>
                                            <ul class="font" type="disc" style="font-size: 12pt; margin-top: 0in">
                                                <li class="font"><span lang="PT-BR" style="font-size: 12pt">(41) 2141-8023</span></li>
                                                <li class="font"><span lang="PT-BR" style="font-size: 12pt">(41) 2141-9676</span></li>
                                                <li class="font"><span lang="PT-BR" style="font-size: 12pt">(41) 2141-9691</span></li>
                                                <li class="font"><span lang="PT-BR" style="font-size: 12pt">(41) 2141-9692</span></li>
                                                <li class="font"><span lang="PT-BR" style="font-size: 12pt">E-mail: <a href="comercialpet@imcopa.com.br" class="branco">comercialpet@imcopa.com.br</a></span></li>
                                            </ul>

                                        <%--    <span class="style12">
                                                <br>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fone:&nbsp;(41) 2141-8000</span>
                                            <span class="style12">
                                                <br>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Fax:  &nbsp;(41) 2141-8001</span>
                                            <span class="style12">
                                                <br>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;E-mail: <a href="mailto:support.it@imcopa.com.br" class="branco">support.it@imcopa.com.br</a></span>--%>
                                        </p>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div id="rodape" class="rodape">
                        © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                            href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
