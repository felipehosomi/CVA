<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Detalhe.aspx.cs" Inherits="WebImcopa.Detalhe" %>

<%@ Register Assembly="skmMenu" Namespace="skmMenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Detalhe</title>
    <link rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
</head>
<body class="body">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" EnableScriptGlobalization="True"
            EnableScriptLocalization="True" EnablePageMethods="true" AsyncPostBackTimeout="1800" />
        <asp:UpdatePanel ID="uppGeral" runat="server">
            <ContentTemplate>
                <!-- Início tabela principal -->
                <table class="table" style="background-color: #ffffff;" width="100%">
                    <tr>
                        <!-- Coluna em vermelho do lado esquerdo-->
                        <!-- Fim da coluna em vermelho do lado esquerdo-->
                        <td>
                            <!-- Início tabela das colunas do site-->
                            <!-- Fim tabela das colunas do site -->
                            <!-- Inicio tabela dos elementos -->
                            <table class="background" style="width: 80%" align="center">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Imagens/logo_imcopan2.jpg">
                                        </asp:ImageButton></td>
                                </tr>
                                <tr>
                                    <td style="text-align: center">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="text-align: right; font-family: Verdana;" colspan="2">
                                                    <asp:Label ID="lblcdRepresentante" runat="server" Visible="False" CssClass="font"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblVendedor" runat="server" CssClass="font"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <strong><span style="font-size: 14pt; font-family: Verdana;">Detalhes do Pedido </span>
                                        </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-family: Verdana">
                                        <table style="width: 100%" class="table">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" CssClass="font" Text="Cliente"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbcliente" runat="server" CssClass="font"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" CssClass="font" Text="Cond. Pagamento"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbcondpg" runat="server" CssClass="font"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" CssClass="font" Text="Cod. Cliente"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbcdcliente" runat="server" CssClass="font"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" CssClass="font" Text="Data Criação"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbdtcriacao" runat="server" CssClass="font"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" CssClass="font" Text="CNPJ"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbcnpj" runat="server" CssClass="font"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" CssClass="font" Text="Número da Cotação"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="Lbnumcot" runat="server" CssClass="font"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DataGrid ID="DataGridDetail" runat="server" Width="100%" AutoGenerateColumns="False"
                                            Font-Size="11px" Font-Names="Arial" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <HeaderStyle Font-Bold="True" BackColor="#507CD1" ForeColor="White"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="Matnr" SortExpression="matnr" HeaderText="Mercadoria"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Descr" SortExpression="descr" HeaderText="Descri&#231;&#227;o">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Embal" SortExpression="embal" HeaderText="Emb"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Quant" SortExpression="quant" HeaderText="Quant."></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Unitrs" SortExpression="unitrs" HeaderText="Val. Unit.">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Total" SortExpression="total" HeaderText="Total"></asp:BoundColumn>
                                            </Columns>
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <EditItemStyle BackColor="#2461BF" />
                                            <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <AlternatingItemStyle BackColor="White" />
                                            <ItemStyle BackColor="#EFF3FB" />
                                        </asp:DataGrid></td>
                                </tr>
                                <tr>
                                    <td style="text-align: right; font-family: Verdana;">
                                        Total:
                                        <asp:Label ID="Lbtotal" runat="server" CssClass="font"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="rodape" class="rodape" style="font-family: Verdana">
                                            © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados.
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
