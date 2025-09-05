<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaTitulos.aspx.cs"
    Inherits="WebImcopa.ConsultaTitulos" EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web Imcopa - Consulta Títulos em Aberto</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
    
    <style type="text/css">
        .GridView-HeaderRow
        {
            border-color:transparent !important;
        }
    </style>

    <script type="text/javascript">

        var GridId = "gridViewClientes";
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
    <asp:UpdatePanel ID="uppGeral" runat="server">
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
                                    </td>
                                    <td style="text-align: right">
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ImprimeLista.aspx?GRID=DATATABLETITULOS"
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
                        
                        <div id="DataDiv" > 
                            <asp:GridView ID="gridViewClientes" runat="server" CssClass="GridView" Width="100%"
                                Height="300px" CellPadding="6" AutoGenerateColumns="False" CellSpacing="1" OnRowDataBound="gridViewClientes_RowDataBound"
                                AllowSorting="True" OnSorting="gridViewClientes_Sorting" ShowFooter="True">
                                <RowStyle CssClass="GridView-Row"></RowStyle>
                                <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                <HeaderStyle CssClass="GridView-HeaderRow header"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="STCD1" HeaderText=" CNPJ " SortExpression="STCD1">
                                        <HeaderStyle Wrap="False"  Width="200px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="NAME1" HeaderText=" Raz&#227;o Social " SortExpression="NAME1">
                                        <HeaderStyle Wrap="False" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="XBLNR" HeaderText=" Nr. T&#237;tulo " SortExpression="XBLNR"
                                        DataFormatString="{0:N0}">
                                        <HeaderStyle Wrap="False" Width="300px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZFBDT2" HeaderText=" Data Vencimento " SortExpression="ZFBDT2"
                                        DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                                        <HeaderStyle Wrap="False" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DMBTR" HeaderText=" Valor do T&#237;tulo " SortExpression="DMBTR"
                                        DataFormatString="{0:N2}" HtmlEncode="False">
                                        <HeaderStyle Wrap="False" Width="100px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" Width="100px" />
                                        <FooterStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SGTXT" HeaderText=" Observa&#231;&#227;o " SortExpression="SGTXT">
                                        <HeaderStyle Wrap="False" Width="300px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" Width="300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BUKRS" HeaderText=" Empresa " SortExpression="BUKRS">
                                        <HeaderStyle Wrap="False" Width="100px" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle CssClass="GridView-FooterRow" />
                            </asp:GridView>
                        </div>
                    </center>
                    <div style="text-align: right">
                        <br />
                        &nbsp;<asp:Label ID="lblTotal" runat="server" CssClass="font" Font-Size="Small" Visible="False"></asp:Label>
                        &nbsp; &nbsp;<br />
                    </div>
                </div>
                <div id="rodape" class="rodape">
                    © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                        href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
            </div>
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPanel" Style="display: none"
                Width="450px">
                <center>
                    <div id="div1" class="borda_up">
                        Consulta Titulos em Aberto&nbsp;</div>
                    <div id="div2">
                        <br />
                        <table class="table" style="text-align: left; width: 90%;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCNPJ" runat="server" CssClass="font">CNPJ:</asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtCNPJ" runat="server" CssClass="textBox" Width="140px" TabIndex="1"></asp:TextBox>
                                    <br />
                                    <br />
                                    <cc1:MaskedEditExtender ID="meeCNPJ" runat="server" MaskType="Number" TargetControlID="txtCNPJ"
                                        Mask="99,999,999/9999-99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" ClearMaskOnLostFocus="False" InputDirection="RightToLeft">
                                    </cc1:MaskedEditExtender>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="font">Data Inicial de Vencimento:</asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtDataIni" runat="server" CssClass="textBox" Width="100px">
                                    </asp:TextBox>
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                        ControlToValidate="txtDataIni" EmptyValueMessage="Data requerida" InvalidValueMessage="Data inválida"
                                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                                        CssClass="font" IsValidEmpty="False" /><br />
                                    <br />
                                    &nbsp;<cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDataIni"
                                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="None" ErrorTooltipEnabled="True" />
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDataIni" />
                                </td>
                                <td>
                                    <asp:Label ID="lblDataIni" runat="server" CssClass="font">Data Final de Vencimento:</asp:Label>
                                    <br />
                                    <asp:TextBox ID="txtDataFin" runat="server" CssClass="textBox" Width="100px">
                                    </asp:TextBox>
                                    <cc1:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="MaskedEditExtender5"
                                        ControlToValidate="txtDataFin" EmptyValueMessage="Data requerida" InvalidValueMessage="Data inválida"
                                        Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE2"
                                        CssClass="font" IsValidEmpty="False" /><br />
                                    <br />
                                    &nbsp;<cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtDataFin"
                                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="None" ErrorTooltipEnabled="True" />
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDataFin" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="div3" class="borda_down">
                        <table style="vertical-align: middle;">
                            <tr style="height: 5px;">
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnOk" TabIndex="7" runat="server" Text="Atualizar" CssClass="linkbotao"
                                        OnClick="btnOk_Click" Height="20px"></asp:Button>
                                </td>
                                <td>
                                    <asp:Button ID="btnCancel" TabIndex="8" runat="server" Text="Cancelar" CssClass="linkbotao"
                                        Height="20px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="extModal" runat="server" BackgroundCssClass="modalBackground"
                PopupControlID="pnlModal" TargetControlID="btnHidden" CancelControlID="btnCancel">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
