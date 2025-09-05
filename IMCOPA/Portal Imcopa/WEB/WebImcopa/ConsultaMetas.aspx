<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaMetas.aspx.cs"
    Inherits="WebImcopa.ConsultaMetas" EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Consulta de Metas por Produto</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
    <style type="text/css">
        .GridView-HeaderRow
        {
            border-color: transparent !important;
        }
    </style>
    <script type="text/javascript">
        var GridId = "gridViewMetas";
        var ScrollHeight = 300;

        $(document).ready(function () {

            var grid = document.getElementById(GridId);
            var gridWidth = grid.clientWidth;
            var gridHeight = grid.offsetHeight;
            var headerCellWidths = new Array();
            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].clientWidth;
            }
            grid.parentNode.appendChild(document.createElement("div"));
            var parentDiv = grid.parentNode;

            //Cria table de header
            var table = document.createElement("table");
            for (i = 0; i < grid.attributes.length; i++) {
                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
                }
            }
            table.style.cssText = grid.style.cssText;
            table.style.width = gridWidth + "px";
            table.style.height = '';
            table.appendChild(document.createElement("tbody"));
            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
            var cells = table.getElementsByTagName("TH");

            var gridRow = grid.getElementsByTagName("TR")[0];
            for (var i = 0; i < cells.length; i++) {
                var width;
                //width = headerCellWidths[i];
                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].clientWidth) {
                    width = headerCellWidths[i];
                }
                else {
                    width = gridRow.getElementsByTagName("TD")[i].clientWidth;
                }
                cells[i].style.width = parseInt(width + 3) + "px";
                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width + 3) + "px";
            }
            parentDiv.removeChild(grid);

            var dummyHeader = document.createElement("div");
            dummyHeader.appendChild(table);
            parentDiv.appendChild(dummyHeader);
            var scrollableDiv = document.createElement("div");
            if (parseInt(gridHeight) > ScrollHeight) {
                //gridWidth = parseInt(gridWidth) + 17;
                gridWidth = parseInt(gridWidth);
            }
            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
            grid.style.width = "100%";
            scrollableDiv.appendChild(grid);
            parentDiv.appendChild(scrollableDiv);
        });

        function CreateGridHeader(DataDiv, GridView1, HeaderDiv) {
            return false;
        }
    
    </script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
    <asp:ScriptManager ID="MasterScript" runat="server" EnableScriptGlobalization="True"
        EnableScriptLocalization="True" AsyncPostBackTimeout="1800" EnablePartialRendering="false" />
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
    <asp:UpdatePanel ID="uppGeral" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div id="cabecalho">
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
                        <asp:Label ID="lblAvisos" runat="server"></asp:Label></asp:Panel>
                </div>
                <div id="campos" class="background">
                    <div style="text-align: left;">
                        <center>
                            <table width="98%">
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="linkbotao" OnClick="btnBuscar_Click">
                                        </asp:Button>
                                        <asp:Button runat="server" ID="btnHidden" CssClass="ocultar" />
                                        <asp:Label ID="lblValor" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblQuant" runat="server" Visible="False"></asp:Label>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ImprimeLista.aspx?GRID=DATATABLEMETAS"
                                            Target="_blank" CssClass="linkbotao" BorderColor="DimGray" BorderStyle="Solid"
                                            BorderWidth="1px" Height="16px" Width="110px">Imprimir...</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <center>
                        <div id="HeaderDiv" runat="server"></div>
                        <div id="DataDiv">
                            <asp:GridView ID="gridViewMetas" runat="server" CssClass="GridView" Width="98%"
                                Height="300px" CellPadding="6" AutoGenerateColumns="False" CellSpacing="1" OnRowDataBound="gridViewMetas_RowDataBound"
                                AllowSorting="True" OnSorting="gridViewMetas_Sorting" ShowFooter="False">
                                <RowStyle CssClass="GridView-Row"></RowStyle>
                                <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                <HeaderStyle CssClass="GridView-HeaderRow header"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField SortExpression="MES" HeaderText="Mês/Ano">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# string.Format("{0}/{1}", Eval("MES").ToString().Substring(4), Eval("MES").ToString().Substring(0, 4)) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MATNR" HeaderText="Material" SortExpression="MATNR">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:TemplateField SortExpression="META" HeaderText="Meta">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# string.Format("{0:N}", Convert.ToDouble(Eval("META"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="REALIZADO" HeaderText="Realizado">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# string.Format("{0:N}", Convert.ToDouble(Eval("REALIZADO"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="PORCENT" HeaderText="Percentual">
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# string.Format("{0:N}%", Convert.ToDouble(Eval("PORCENT"))) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--</asp:Panel>--%>
                        </div>
                    </center>
                    <br />
                </div>
                <div id="rodape" class="rodape">
                    © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                        href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
            </div>
            <cc1:ModalPopupExtender ID="extModal" runat="server" BackgroundCssClass="modalBackground"
                BehaviorID="programmaticModalPopupBehavior" PopupControlID="pnlModal" TargetControlID="btnHidden"
                CancelControlID="btnCancel">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
