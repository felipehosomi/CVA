<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ConsultaCliente.aspx.cs"
    Inherits="WebImcopa.ConsultaCliente" EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web Imcopa - Consulta de Clientes</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
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

        function onCancel() {
            CreateGridHeader('DataDiv', 'gridViewClientes', 'HeaderDiv');
        }

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

            var tableWidth = DataGridObj.clientWidth;
            //********* Hidding the original header of GridView *******

            //********* Setting the same width of all the componets **********
            DataGridObj.rows[0].style.display = 'none';
            HeaderDivObj.style.width = DataDivWidth + 'px';
            DataDivObj.style.width = DataDivWidth + 'px';
            DataGridObj.style.width = tableWidth + 'px';
            HeadertableObj.style.width = tableWidth + 10 + 'px';
                       

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
                                        <td colspan="3">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="linkbotao" OnClick="btnBuscar_Click">
                                            </asp:Button>
                                            <asp:Button runat="server" ID="btnHidden" CssClass="ocultar" />
                                        </td>
                                        <td style="text-align:center">
                                            <asp:Button ID="btnExportarExcel" runat="server" Text="Exportar Para Excel" CssClass="linkbotao" OnClick="btnExportarExcel_Click"></asp:Button>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ImprimeLista.aspx?GRID=DATATABLECLIENTES"
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
                        </div>
                        <br />
                        <%--<div style="width:100%;border-bottom:1px solid grey;"></div>--%>
                        <center>
                            
                            <div id="HeaderDiv" runat="server">
                                
                            </div>    
                            <div id="DataDiv" style="width:98%; overflow:auto;height:300px;" onscroll="Onscrollfnction();" >
                                
                                <asp:GridView ID="gridViewClientes" runat="server" CssClass="GridView" 
                                    Width="100%" CellPadding="6"
                                    AutoGenerateColumns="False" CellSpacing="1" OnRowDataBound="gridViewClientes_RowDataBound"
                                    AllowSorting="True" OnSorting="gridViewClientes_Sorting" 
                                    onrowcommand="gridViewClientes_RowCommand" DataKeyNames="KUNNR">
                                    <RowStyle CssClass="GridView-Row"></RowStyle>
                                    <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                    <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                    <HeaderStyle CssClass="GridView-HeaderRow controlHead"></HeaderStyle>
                                    <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lkClientesAtrelados" runat="server" CommandArgument='<%# Eval("KUNNR") + "_" + Eval("KLIMK") %>' CommandName="ClientesAtrelados">
                                                    <img src="Imagens/search.png" style="max-height:12px;" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="24px" HorizontalAlign="Center" />
                                            <HeaderStyle Width="24px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BUKRS" HeaderText=" Empresa " SortExpression="BUKRS">
                                            <HeaderStyle Wrap="False" Width="40px" />
                                            <ItemStyle HorizontalAlign="center" Width="40px"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KUNNR" HeaderText=" Nº Cliente " SortExpression="KUNNR">
                                            <HeaderStyle Wrap="False"  Width="90px"/>
                                            <ItemStyle HorizontalAlign="Right" Wrap="False"  Width="90px"/>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NAME1" HeaderText=" Nome do Cliente " SortExpression="NAME1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORT01" HeaderText=" Cidade " SortExpression="ORT01">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="STCD1" HeaderText=" CNPJ " SortExpression="STCD1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KLIMK" HeaderText=" Limite Cr&#233;dito " SortExpression="KLIMK"
                                            DataFormatString="{0:N0}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKFOR" HeaderText=" T&#237;tulo Aberto " SortExpression="SKFOR"
                                            DataFormatString="{0:N0}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SAUFT" HeaderText=" Pedido Aberto " SortExpression="SAUFT"
                                            DataFormatString="{0:N0}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SALDO" HeaderText=" Saldo Limite " SortExpression="SALDO"
                                            DataFormatString="{0:N0}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VTEXT" HeaderText=" Mot. Bloqueio " SortExpression="VTEXT">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TELF1" HeaderText=" 1° N° Telefone " SortExpression="TELF1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SMTP_ADDR" HeaderText=" Endereço de e-mail " SortExpression="SMTP_ADDR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="STRAS" HeaderText=" Endere&#231;o/N&#186; " SortExpression="STRAS">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORT02" HeaderText=" Bairro " SortExpression="ORT02">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REGIO" HeaderText=" UF " SortExpression="REGIO">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PSTLZ" HeaderText=" CEP " SortExpression="PSTLZ">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VKGRP" HeaderText=" Escr.Ven " SortExpression="VKGRP">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BEZEI" HeaderText=" Corretor " SortExpression="BEZEI">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>                                        
                                    </Columns>
                                </asp:GridView>
                            </div>
                                                        
                        </center>
                        <br />
                    </div>
                    <div id="rodape" class="rodape">
                        © Copyright&nbsp;IMCOPA&nbsp;- Todos os direitos reservados. Desenvolvido por <a
                            href="http://www.cvaconsultoria.com.br">CVA Consultoria</a>
                    </div>
                </div>
                <asp:Panel ID="pnlModal" runat="server" CssClass="modalPanel" Style="display: none"
                    Width="350px">
                    <center>
                        <div id="div1" class="borda_up">
                            Consulta
                        </div>
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
                                        <asp:Label ID="lblCodCliente" runat="server" CssClass="font">Código Cliente:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtCodCliente" runat="server" CssClass="textBox" Width="100px" AutoPostBack="True"
                                            MaxLength="10" TabIndex="2"></asp:TextBox>
                                        <br />
                                        <cc1:MaskedEditExtender ID="meeCodCli" runat="server" MaskType="Number" TargetControlID="txtCodCliente"
                                            ClearTextOnInvalid="True" Mask="9999999999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" InputDirection="RightToLeft">
                                        </cc1:MaskedEditExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDataIni" runat="server" CssClass="font">Data Inicial:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtDataIni" runat="server" CssClass="textBox" Width="100px" TabIndex="3"></asp:TextBox>
                                        <cc1:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="MaskedEditExtender5"
                                            ControlToValidate="txtDataIni" EmptyValueMessage="Data requerida" InvalidValueMessage="Data inválida"
                                            Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                                            CssClass="font" />
                                        <br />
                                        <br />
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="txtDataIni"
                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="None" ErrorTooltipEnabled="True" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDataIni" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDataFin" runat="server" CssClass="font">Data Final:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtDataFin" runat="server" CssClass="textBox" Width="100px" TabIndex="4"></asp:TextBox>
                                        <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                            ControlToValidate="txtDataFin" EmptyValueMessage="Data requerida" InvalidValueMessage="Data inválida"
                                            Display="Dynamic" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*" ValidationGroup="MKE"
                                            CssClass="font" />
                                        <br />
                                        <br />
                                        <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDataFin"
                                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="None" ErrorTooltipEnabled="True" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDataFin" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                        <asp:Label ID="lblEscritorio" runat="server" CssClass="font">Escritório / Equipe / Descrição:</asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlVendas" runat="server" CssClass="textBox" Width="250px"
                                            TabIndex="5" DataTextField="DESCR" DataValueField="VKGRP">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <asp:Label ID="lblEmpresa" runat="server" CssClass="font">Empresa:</asp:Label>
                                        <br />
                                        <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="textBox" DataTextField="Text"
                                            DataValueField="Id" TabIndex="6" Width="150px">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
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
                                        <asp:Button ID="btnOk" TabIndex="6" runat="server" Text="Atualizar" CssClass="linkbotao"
                                            OnClick="btnOk_Click" Height="20px"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" TabIndex="7" runat="server" Text="Cancelar" CssClass="linkbotao"
                                            Height="20px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </asp:Panel>
                <asp:Panel ID="pnlClientesAtrelados" runat="server" CssClass="modalPanel" style="display:none;width:500px">
                    <center>
                        <div class="borda_up">
                            Clientes Atrelados por Conta de Crédito
                        </div>
                        <div style="padding:20px 10px 20px 10px;">
                            <asp:GridView ID="gridViewClientesAtrelados" ShowHeaderWhenEmpty="true" EmptyDataText="Nenhum registro encontrado" 
                                runat="server" CssClass="GridView" Width="100%" CellPadding="6" AutoGenerateColumns="False" CellSpacing="1" AllowSorting="false">
                                <RowStyle CssClass="GridView-Row"></RowStyle>
                                <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                <HeaderStyle CssClass="GridView-HeaderRow controlHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField DataField="KUNNR" HeaderText=" Nº Cliente " >
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>

                            <div style="text-align:right;">
                                <br />
                                <strong>Limite de Crédito: </strong> <span id="sLimiteCredito" runat="server"></span>
                            </div>
                        </div>
                        <div class="borda_down">
                            <asp:Button ID="btnFechar" TabIndex="7" runat="server" Text="Fechar" CssClass="linkbotao" Height="20px" style="margin-top:5px;"></asp:Button>
                        </div>
                    </center>
                </asp:Panel>                
                <cc1:ModalPopupExtender ID="extModal" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlModal" TargetControlID="btnHidden" CancelControlID="btnCancel">
                </cc1:ModalPopupExtender>
                <cc1:ModalPopupExtender ID="mpeClientesAtrelados" runat="server" BackgroundCssClass="modalBackground"
                    PopupControlID="pnlClientesAtrelados" TargetControlID="btnHidden" CancelControlID="btnFechar" OnCancelScript="onCancel()" >
                </cc1:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
