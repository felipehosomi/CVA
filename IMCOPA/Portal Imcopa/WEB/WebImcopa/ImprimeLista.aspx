<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ImprimeLista.aspx.cs"
    Inherits="WebImcopa.ImprimeLista" %>

<%@ Register Assembly="Shared.WebControls" Namespace="Shared.WebControls" TagPrefix="wc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Web Imcopa - Lista Clientes</title>
    <link rel="stylesheet" type="text/css" />
    <link href="Config.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td style="width: 20%;">
                    <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                </td>
                <td style="width: 10%;">
                </td>
                <td>
                    <asp:Label ID="lblTitulo" runat="server" Text="" CssClass="Titulo"></asp:Label>
                </td>
            </tr>
        </table>
        <wc:ReportGridView ID="gridViewClientes" OnRowDataBound="gridViewClientes_RowDataBound"
            runat="server" BorderWidth="2px" AutoGenerateColumns="False" PrintPageSize="40"
            AllowPrintPaging="True" Width="1024px" Visible="False" CssClass="font">
            <HeaderStyle CssClass="GridView-HeaderRow" />
            <Columns>
                <asp:BoundField DataField="BUKRS" HeaderText=" Empresa " SortExpression="BUKRS">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_e" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" CssClass="width_e" />
                </asp:BoundField>
                <asp:BoundField DataField="KUNNR" HeaderText=" Nº Cliente " SortExpression="KUNNR">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_e" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_e" />
                </asp:BoundField>
                <asp:BoundField DataField="NAME1" HeaderText="Nome do Cliente" SortExpression="NAME1">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_a" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_a" />
                </asp:BoundField>
                <asp:BoundField DataField="ORT01" HeaderText="Cidade" SortExpression="ORT01">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_b" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_b" />
                </asp:BoundField>
                <asp:BoundField DataField="STCD1" HeaderText="CNPJ" SortExpression="STCD1">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_c" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_c" />
                </asp:BoundField>
                <asp:BoundField DataField="KLIMK" HeaderText="Limite Cr&#233;dito" SortExpression="KLIMK"
                    DataFormatString="{0:N0}">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_d" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_d" />
                </asp:BoundField>
                <asp:BoundField DataField="SKFOR" HeaderText="T&#237;tulo Aberto" SortExpression="SKFOR"
                    DataFormatString="{0:N0}">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_d" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_d" />
                </asp:BoundField>
                <asp:BoundField DataField="SAUFT" HeaderText="Pedido Aberto" SortExpression="SAUFT"
                    DataFormatString="{0:N0}">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_d" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_d" />
                </asp:BoundField>
                <asp:BoundField DataField="SALDO" HeaderText="Saldo Limite" SortExpression="SALDO"
                    DataFormatString="{0:N0}">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_d" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_d" />
                </asp:BoundField>
                <asp:BoundField DataField="VTEXT" HeaderText="Mot. Bloqueio" SortExpression="VTEXT">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_c" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_c" />
                </asp:BoundField>
                <asp:BoundField DataField="STRAS" HeaderText="Endere&#231;o/N&#186;" SortExpression="STRAS">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_a" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_a" />
                </asp:BoundField>
                <asp:BoundField DataField="ORT02" HeaderText="Bairro" SortExpression="ORT02">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_b" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_b" />
                </asp:BoundField>
                <asp:BoundField DataField="REGIO" HeaderText="UF" SortExpression="REGIO">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_f" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" CssClass="width_f" />
                </asp:BoundField>
                <asp:BoundField DataField="PSTLZ" HeaderText="CEP" SortExpression="PSTLZ">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_e" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_e" />
                </asp:BoundField>
                <asp:BoundField DataField="VKGRP" HeaderText=" Escr.Ven " SortExpression="VKGRP">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_e" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" CssClass="width_e" />
                </asp:BoundField>
                <asp:BoundField DataField="BEZEI" HeaderText=" Corretor " SortExpression="BEZEI">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" CssClass="width_a" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" CssClass="width_a" />
                </asp:BoundField>
            </Columns>
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT1" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# gridViewClientes.CurrentPrintPage.ToString() %>
                    /
                    <%# gridViewClientes.PrintPageCount.ToString() %>
                </p>
            </PageFooterTemplate>
        </wc:ReportGridView>
        <wc:ReportGridView ID="grvConsulta" runat="server" BorderWidth="2px" AutoGenerateColumns="False"
            PrintPageSize="38" AllowPrintPaging="True" Width="1024px" Visible="False" CssClass="font">
            <HeaderStyle CssClass="GridView-HeaderRow" />
            <Columns>
                <asp:BoundField DataField="KUNNR" HeaderText="Cliente" SortExpression="KUNNR">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Filial" DataField="WERKS" SortExpression="WERKS">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Cota&#231;&#227;o" DataField="DCCOT">
                    <ItemStyle CssClass="branco" HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Fatura" DataField="DTFAT" SortExpression="DTFAT">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Nome do Cliente" DataField="NAME1" SortExpression="NAME1">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Cidade" DataField="ORT01" SortExpression="ORT01">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Ord. Venda" DataField="DCVEN" SortExpression="DCVEN">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Cod. Produto" DataField="MATNR" SortExpression="MATNR">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qtde. OV" DataField="RFMNG" DataFormatString="{0:N0}"
                    SortExpression="RFMNG">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Pre&#231;o OV" DataField="RFWRT" DataFormatString="{0:N0}"
                    SortExpression="RFWRT">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Cond. Pagto" DataField="ZTERM" SortExpression="ZTERM">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Descri&#231;&#227;o CP" DataField="DZTERM" SortExpression="DZTERM">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Nota Fiscal" DataField="DCNF" SortExpression="DCNF">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Qtde. NF" DataField="QTNF" DataFormatString="{0:N0}"
                    SortExpression="QTNF">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Pre&#231;o NF" DataField="PRNF" DataFormatString="{0:N0}"
                    SortExpression="PRNF">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Valor NF" DataField="VLNF" DataFormatString="{0:N0}"
                    SortExpression="VLNF">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Venc. 01" DataField="ZFBDT" SortExpression="ZFBDT">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Venc. 02" DataField="ZFBDT2" SortExpression="ZFBDT2">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Venc. 03" DataField="ZFBDT3" SortExpression="ZFBDT3">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="VKGRP" HeaderText=" Escr.Ven " SortExpression="VKGRP">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="BEZEI" HeaderText=" Corretor " SortExpression="BEZEI">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:CheckBoxField HeaderText="F" DataField="FAT" ReadOnly="True" />
                <asp:CheckBoxField HeaderText="C" DataField="CRD" ReadOnly="True" />
            </Columns>
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT2" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# grvConsulta.CurrentPrintPage.ToString()%>
                    /
                    <%# grvConsulta.PrintPageCount.ToString()%>
                </p>
            </PageFooterTemplate>
        </wc:ReportGridView>
        <wc:ReportGridView ID="grvTitulos" runat="server" BorderWidth="2px" AutoGenerateColumns="False"
            PrintPageSize="36" AllowPrintPaging="True" Width="1024px" Visible="False" CssClass="font"
            OnRowDataBound="grvTitulos_RowDataBound" ShowFooter="True">
            <HeaderStyle CssClass="GridView-HeaderRow" />
            <Columns>
                <asp:BoundField DataField="STCD1" HeaderText=" CNPJ " SortExpression="STCD1">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="NAME1" HeaderText=" Raz&#227;o Social " SortExpression="NAME1">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="XBLNR" HeaderText=" Nr. T&#237;tulo " SortExpression="XBLNR"
                    DataFormatString="{0:N0}">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="ZFBDT2" HeaderText=" Data Vencimento " SortExpression="ZFBDT2"
                    DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="DMBTR" HeaderText=" Valor do T&#237;tulo " SortExpression="DMBTR"
                    DataFormatString="{0:N0}">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="SGTXT" HeaderText=" Observa&#231;&#227;o " SortExpression="SGTXT">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="BUKRS" HeaderText=" Empresa " SortExpression="BUKRS">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT3" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# grvTitulos.CurrentPrintPage.ToString()%>
                    /
                    <%# grvTitulos.PrintPageCount.ToString()%>
                </p>
            </PageFooterTemplate>
            <FooterStyle CssClass="GridView-FooterRow" />
        </wc:ReportGridView>
        <wc:ReportGridView ID="gridViewFaturamentos" runat="server" CssClass="GridView" Width="1024px"
            PrintPageSize="30" AllowPrintPaging="True" AutoGenerateColumns="False" CellSpacing="1"
            OnRowDataBound="gridViewFaturamentos_RowDataBound" ShowFooter="True" Visible="False">
            <HeaderStyle CssClass="GridView-HeaderRow" />
            <FooterStyle CssClass="GridView-FooterRow" />
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
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="KZWI1" HeaderText="Valor" SortExpression="KZWI1" DataFormatString="{0:N2}">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="INCO1" HeaderText="Frete" SortExpression="INCO1">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT4" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# gridViewFaturamentos.CurrentPrintPage.ToString()%>
                    /
                    <%# gridViewFaturamentos.PrintPageCount.ToString()%>
                </p>
            </PageFooterTemplate>
        </wc:ReportGridView>
        <wc:ReportGridView ID="gridViewHistoricoVenda" runat="server" BorderWidth="2px" AutoGenerateColumns="False"
            PrintPageSize="40" AllowPrintPaging="True" Width="1024px" Visible="False" CssClass="font"
            OnRowDataBound="gridViewHistoricoVenda_RowDataBound">
            <HeaderStyle CssClass="GridView-HeaderRow" />
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
                <asp:BoundField DataField="NETWR" HeaderText=" Valor " SortExpression="NETWR">
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
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT5" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# gridViewClientes.CurrentPrintPage.ToString() %>
                    /
                    <%# gridViewClientes.PrintPageCount.ToString() %>
                </p>
            </PageFooterTemplate>
        </wc:ReportGridView>
        <wc:ReportGridView ID="gridViewMetas" runat="server" BorderWidth="2px" AutoGenerateColumns="False"
            PrintPageSize="40" AllowPrintPaging="True" Width="1024px" Visible="False" CssClass="font">
            <HeaderStyle CssClass="GridView-HeaderRow" />
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
                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
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
            <PageHeaderTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 20%;">
                            <img alt="IMCOPA" src="Imagens/logo_imcopan2.jpg" />
                        </td>
                        <td style="width: 10%;">
                        </td>
                        <td>
                            <asp:Label ID="lblT6" runat="server" Text="<%# lblTitulo.Text %>" CssClass="Titulo"></asp:Label>
                        </td>
                    </tr>
                </table>
            </PageHeaderTemplate>
            <PageFooterTemplate>
                <br />
                <hr />
                <p class="font">
                    Pagina
                    <%# gridViewMetas.CurrentPrintPage.ToString()%>
                    /
                    <%# gridViewMetas.PrintPageCount.ToString()%>
                </p>
            </PageFooterTemplate>
        </wc:ReportGridView>
        <asp:Label ID="lblTotal" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblValor" runat="server" Visible="False"></asp:Label>
        <asp:Label ID="lblQuant" runat="server" Visible="False"></asp:Label>
    </div>
    </form>
</body>
</html>
