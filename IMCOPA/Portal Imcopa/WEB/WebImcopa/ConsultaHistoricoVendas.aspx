<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaHistoricoVendas.aspx.cs" Inherits="WebImcopa.ConsultaHistoricoVendas" EnableEventValidation="false" %>


<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web Imcopa - Histórico de Vendas</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
       
    <style type="text/css">
        .GridView-HeaderRow
        {
            border-color:transparent !important;
        }
        .GridView-HeaderRow
        {
            height:26px !important;
        }
    </style>

    <script type="text/javascript">
    function CreateGridHeader(DataDiv, GridView1, HeaderDiv) {

            var DataDivObj = document.getElementById(DataDiv);
            var DataGridObj = document.getElementById(GridView1);
            var HeaderDivObj = document.getElementById(HeaderDiv);

            //********* Creating new table which contains the header row ***********
            var HeadertableObj = HeaderDivObj.appendChild(document.createElement('table'));

            DataDivObj.style.paddingTop = '0px';
            var DataDivWidth = DataDivObj.clientWidth;
            DataDivObj.style.width = '100%';

            //********** Setting the style of Header Div as per the Data Div ************
            HeaderDivObj.className = DataDivObj.className;
            HeaderDivObj.style.cssText = DataDivObj.style.cssText;
            //**** Making the Header Div scrollable. *****
            HeaderDivObj.style.overflow = 'auto';
            //*** Hiding the horizontal scroll bar of Header Div ****
            HeaderDivObj.style.overflowX = 'hidden';
            //**** Hiding the vertical scroll bar of Header Div **** 
            HeaderDivObj.style.overflowY = 'hidden';
            HeaderDivObj.style.height = DataGridObj.rows[0].clientHeight + 'px';
            HeaderDivObj.style.width = DataGridObj.rows[0].clientWidth + 'px';
            //**** Removing any border between Header Div and Data Div ****
            HeaderDivObj.style.borderBottomWidth = '0px';

            //********** Setting the style of Header Table as per the GridView ************
            HeadertableObj.className = DataGridObj.className;
            //**** Setting the Headertable css text as per the GridView css text 
            HeadertableObj.style.cssText = DataGridObj.style.cssText;
            HeadertableObj.border = '1px';
            HeadertableObj.rules = 'all';
            HeadertableObj.cellPadding = DataGridObj.cellPadding;
            HeadertableObj.cellSpacing = DataGridObj.cellSpacing;


            //********** Creating the new header row **********
            var Row = HeadertableObj.insertRow(0);
            Row.className = DataGridObj.rows[0].className;
            Row.style.cssText = DataGridObj.rows[0].style.cssText;
            Row.style.fontWeight = 'bold';

            //var widths =  DataGridObj.rows[0].cells.length;
            var widths = new Array();

            var tableWidth = DataGridObj.clientWidth;
            //********* Hidding the original header of GridView *******
            
            //********* Setting the same width of all the componets **********
            DataGridObj.rows[0].style.display = 'none';
            HeaderDivObj.style.width = DataDivWidth + 'px';
            DataDivObj.style.width = DataDivWidth + 'px';
            DataGridObj.style.width = tableWidth + 'px';
            HeadertableObj.style.width = tableWidth + 10 + 'px';

            //******** This loop will create each header cell *********
            for (var iCntr = 0; iCntr < DataGridObj.rows[0].cells.length; iCntr++) {
                var spanTag = Row.appendChild(document.createElement('td'));
                spanTag.innerHTML = DataGridObj.rows[0].cells[iCntr].innerHTML;

                var width = 0;
                width = DataGridObj.rows[1].cells[iCntr].clientWidth;

                if (iCntr == 0) {
                    spanTag.style.width = width - 10 + 'px';
                } else {
                    spanTag.style.width = width - 2 + 'px';
                }
                widths.push(width);
            }

            return false;
        }

        function Onscrollfnction() {
            var div = document.getElementById('DataDiv');
            var div2 = document.getElementById('HeaderDiv');
            //****** Scrolling HeaderDiv along with DataDiv ******
            div2.scrollLeft = div.scrollLeft;
            return false;
        }
            
    </script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" EnableScriptGlobalization="True" EnableScriptLocalization="True" AsyncPostBackTimeout="1800" EnablePartialRendering="false" />
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
                        <asp:Label ID="lblAvisos" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                <div id="campos" class="background">
                    <div style="text-align: left;">
                        <center>
                            <table width="98%">
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="linkbotao"  TabIndex="1" onclick="btnBuscar_Click"></asp:Button>
                                        <asp:Button runat="server" ID="btnHidden" CssClass="ocultar" />
                                    </td>
                                    <td style="text-align:center">
                                        <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar Para Excel" CssClass="linkbotao" onclick="btnExportarExcel_Click" ></asp:Button>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ImprimeLista.aspx?GRID=GRIDHISTORICO"
                                            Target="_blank" CssClass="linkbotao" BorderColor="DimGray" BorderStyle="Solid"
                                            BorderWidth="1px" Height="16px" Width="110px">Imprimir...</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </table>
                        </center>

                        <center>
                            
                            <div id="HeaderDiv" runat="server">
                               
                            </div>
                            <asp:Label ID="lblValor" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblQuant" runat="server" Visible="False"></asp:Label>
                            <div id="DataDiv" style="width:98%;height:300px;overflow-y:auto;overflow-x:hidden;" onscroll="Onscrollfnction();" >
                                <asp:GridView ID="gridViewHistoricoVenda" runat="server" CssClass="GridView" 
                                    Width="100%" CellPadding="6"
                                    AutoGenerateColumns="False" CellSpacing="1" 
                                    AllowSorting="True"
                                    onpageindexchanging="gridViewHistoricoVenda_PageIndexChanging" 
                                    onrowdatabound="gridViewHistoricoVenda_RowDataBound" 
                                    onsorting="gridViewHistoricoVenda_Sorting" ShowFooter="True">
                                    <RowStyle CssClass="GridView-Row"></RowStyle>
                                    <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                    <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                    <HeaderStyle CssClass="GridView-HeaderRow controlHead"></HeaderStyle>
                                    <FooterStyle CssClass="GridView-FooterRow" />
                                    <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="AUBEL" HeaderText=" Número da OV " SortExpression="AUBEL">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VBELN" HeaderText=" Fatura " SortExpression="VBELN">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FKIMG" HeaderText=" Quantidade " SortExpression="FKIMG">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField  DataField="NETWR" HeaderText=" Valor " SortExpression="NETWR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORT01" HeaderText=" Cidade " SortExpression="ORT01">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KUNAG" HeaderText=" Código Cliente " SortExpression="KUNAG">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NAME1" HeaderText=" Cliente " SortExpression="NAME1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="left" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </center>
                    </div>
                </div>
                <div id="rodape" class="rodape">
                        © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                </div>
                <asp:Panel ID="pnlModal" runat="server" CssClass="modalPanel" Style="display: none"
                    Width="450px">
                    <center>
                        <div id="div1" class="borda_up">
                            Consulta
                        </div>
                        <div id="div2">
                            <br />
                            <table class="table" style="text-align: left; width: 90%;">                                
                                <tr>
                                    <td style="padding-bottom:10px;">
                                        <asp:Label ID="Label4" runat="server" CssClass="font">Código do Cliente</asp:Label>                                        
                                        <br />
                                        <asp:TextBox ID="txtCodigoCliente" runat="server" CssClass="textBox" Width="90%" TabIndex="1"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="meeCodigoCliente" runat="server" MaskType="Number" TargetControlID="txtCodigoCliente"
                                            ClearTextOnInvalid="True" Mask="9999999999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                                                        
                                </tr>
                                <tr colspan="2">
                                    <td style="padding-bottom:10px;">
                                        <asp:Label ID="lblDtInicio" runat="server" CssClass="font">Data Início</asp:Label>                                        
                                        <br />
                                        <asp:TextBox ID="txtDataInicio" runat="server" CssClass="textBox" Width="90%" TabIndex="1"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="meeDataInicio" runat="server" MaskType="Date" TargetControlID="txtDataInicio"
                                            ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" Mask="99/99/9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                    <td style="padding-bottom:10px;">
                                        
                                        <asp:Label ID="lblDataFim" runat="server" CssClass="font">Data Fim</asp:Label>                                        
                                        <br />
                                        <asp:TextBox ID="txtDataFim" runat="server" CssClass="textBox" Width="90%" TabIndex="1"></asp:TextBox>
                                        <cc1:MaskedEditExtender ID="meeDataFim" runat="server" MaskType="Date" TargetControlID="txtDataFim"
                                            ClearTextOnInvalid="True" UserDateFormat="DayMonthYear" Mask="99/99/9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" InputDirection="LeftToRight">
                                        </cc1:MaskedEditExtender>
                                    </td>                                    
                                </tr>
                            </table>

                        </div>
                        <div id="div3" class="borda_down">
                            <table>
                                <tr style="height: 5px;">
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnOk" TabIndex="6" runat="server" Text="Atualizar" CssClass="linkbotao" OnClick="btnOK_Click" Height="20px"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" TabIndex="7" runat="server" Text="Cancelar" CssClass="linkbotao" Height="20px"></asp:Button>
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
