<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaComissoes.aspx.cs" Inherits="WebImcopa.ConsultaComissoes" EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Web Imcopa - Consulta Comissões</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
    <style type="text/css">
        .GridView-HeaderRow
        {
            border-color:transparent !important;
        }
    </style>
</head>

<script type="text/javascript">
    // Add click handlers for buttons to show and hide modal popup on pageLoad
    function NLoad() {
        $addHandler($get("btnOk"), 'click', hideModalPopupViaClient);
    }

    function hideModalPopupViaClient(ev) {
        ev.preventDefault();
        var modalPopupBehavior = $find('programmaticModalPopupBehavior');
        modalPopupBehavior.hide();
    }
</script>

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
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ImprimeLista.aspx?GRID=DATATABLEFATURAMENTOS"
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
                            <%--<asp:Panel ID="Panel1" runat="server" ScrollBars="Both" CssClass="table" Width="95%" Height="350px">--%>
                            <div id="DataDiv" style="overflow: auto; width: 98%; height: 300px;" onscroll="Onscrollfnction();">
                                <asp:GridView ID="gridViewComissoes" runat="server" CssClass="GridView" Width="100%" CellPadding="6"
                                    AutoGenerateColumns="False" CellSpacing="1" OnRowDataBound="gridViewComissoes_RowDataBound"
                                    AllowSorting="True" OnSorting="gridViewComissoes_Sorting" ShowFooter="True">
                                    <RowStyle CssClass="GridView-Row"></RowStyle>
                                    <SelectedRowStyle CssClass="GridView-RowSelec"></SelectedRowStyle>
                                    <PagerStyle CssClass="GridView-FooterRow"></PagerStyle>
                                    <HeaderStyle CssClass="GridView-HeaderRow"></HeaderStyle>
                                    <FooterStyle CssClass="GridView-FooterRow" />
                                    <AlternatingRowStyle CssClass="GridView-RowAlt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:BoundField DataField="VKGRP" HeaderText="Equipe Vendas" SortExpression="VKGRP">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VKBUR" HeaderText="Escr. Vendas" SortExpression="VKBUR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REGIO" HeaderText="Estado" SortExpression="REGIO">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BSTNK" HeaderText="Pedido/Cliente" SortExpression="BSTNK">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NAME1" HeaderText="Emissor da ordem" SortExpression="NAME1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KUNNR" HeaderText="C&#243;d.cliente" SortExpression="KUNNR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="XBLNR" HeaderText="Nota Fiscal" SortExpression="XBLNR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FKDAT" HeaderText="Data Fatur." SortExpression="FKDAT">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MATNR" HeaderText="Material" SortExpression="MATNR">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="INCO2" HeaderText="Cidade" SortExpression="INCO2">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FKIMG" HeaderText="Qtd.fat." SortExpression="FKIMG" DataFormatString="{0:N0}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="KZWI1" HeaderText="Valor" SortExpression="KZWI1" DataFormatString="{0:N2}">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="INCO1" HeaderText="Frete" SortExpression="INCO1">
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
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
                                    <td>
                                        <asp:Label ID="lblDataIni" runat="server" CssClass="font">Data Inicial:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtDataIni" runat="server" CssClass="textBox" Width="100px" TabIndex="1"></asp:TextBox>
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
                                        <asp:TextBox ID="txtDataFin" runat="server" CssClass="textBox" Width="100px" TabIndex="2"></asp:TextBox>
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
                                            TabIndex="3" DataTextField="DESCR" DataValueField="VKGRP">
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
                                    <td style="height: 20px">
                                        <asp:Button ID="btnOk" TabIndex="4" runat="server" Text="Atualizar" CssClass="linkbotao"
                                            Height="20px" OnClick="btnOk_Click" OnInit="btnOk_Init"></asp:Button>
                                    </td>
                                    <td style="height: 20px">
                                        <asp:Button ID="btnCancel" TabIndex="5" runat="server" Text="Cancelar" CssClass="linkbotao"
                                            Height="20px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                </asp:Panel>
                <cc1:ModalPopupExtender ID="extModal" runat="server" BackgroundCssClass="modalBackground"
                    BehaviorID="programmaticModalPopupBehavior" PopupControlID="pnlModal" TargetControlID="btnHidden"
                    CancelControlID="btnCancel">
                </cc1:ModalPopupExtender>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
