<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroCliente.aspx.cs"
    Inherits="WebImcopa.CadastroCliente" EnableEventValidation="false" %>

<%@ Register Src="menu/cmenu.ascx" TagName="cmenu" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cadastro de Clientes</title>
    <link href="Config.css" rel="stylesheet" type="text/css" />
    <link href="../Config.css" rel="stylesheet" type="text/css" />
    <script src="Script/jquery-2.1.4.js" type="text/javascript"></script>
    <script src="Script/menu.js" type="text/javascript"></script>
</head>
<body class="body" style="text-align: center;">
    <form id="form1" runat="server" class="form">
        <asp:ScriptManager ID="MasterScript" runat="server" />
        <asp:UpdatePanel ID="uppGeral" runat="server">
            <ContentTemplate>
                <div>
                    <div id="cabecalho" class="background">
                        <img alt="Imcopa" src="Imagens/LogoTeste.png" class="table" />
                    </div>
                    <div id="menu" class="div_menu">
                        <table>
                            <tr>
                                <td style="width: 40%; ">
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
                    <div id="div_mensagem">
                        <asp:Panel ID="pnlAvisos" runat="server" CssClass="avisos" Width="100%" Visible="false">
                            <asp:Image ID="imgAvisos" runat="server" ImageAlign="AbsMiddle" />
                            <asp:Label ID="lblAvisos" runat="server"></asp:Label>
                        </asp:Panel>
                    </div>
                    <div id="campos" class="background" style="text-align: center;">
                        <br />
                        <table id="tbGrupos" style="text-align: left; width: 100%; background-color: #f5f5f5; color: #696969;"">
                            <%--Dados Cadastrais--%>
                            <div>
                                <tr>
                                    <td class="linha">
                                        <b>
                                            <asp:Label runat="server" CssClass="font" Style="font-size: medium" Width="200px">Dados Cadastrais</asp:Label></b>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblCNPJ" runat="server" CssClass="font" Width="120px">CNPJ*:</asp:Label>
                                        <asp:TextBox ID="txtCNPJ" runat="server" CssClass="textBox" Width="150px" MaxLength="16"
                                            TabIndex="1" AutoPostBack="True"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" CssClass="linkbotao" OnClick="Busca_CNPJ"
                                            Text="Pesquisar CNPJ" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblNome" runat="server" CssClass="font" Width="120px">Razão Social*:</asp:Label>
                                        <asp:TextBox ID="txtNome" runat="server" CssClass="textBox" Width="290px" TabIndex="2"
                                            MaxLength="35"></asp:TextBox>
                                        <asp:Label ID="lblInscricaoEstadual" runat="server" CssClass="font" Width="90px" Style="padding-left: 20px">Insc. Estadual*:</asp:Label>
                                        <asp:TextBox ID="txtInscricaoEstadual" runat="server" CssClass="textBox" Width="150px"
                                            TabIndex="4" MaxLength="18" EnableTheming="True"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblNomeFantasia" runat="server" CssClass="font" Width="120px">Nome Fantasia*:</asp:Label>
                                        <asp:TextBox ID="txtNomeFantasia" runat="server" CssClass="textBox" Width="290px"
                                            TabIndex="3" MaxLength="20"></asp:TextBox>
                                        <asp:Label ID="lblRede" runat="server" CssClass="font" Width="90px" Style="padding-left: 20px">Rede*:</asp:Label>
                                        <asp:TextBox ID="txtRede" runat="server" CssClass="textBox" Width="150px" TabIndex="5"
                                            MaxLength="20"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblCEP" runat="server" CssClass="font" Width="120px">CEP*:</asp:Label>
                                        <asp:TextBox ID="txtCEP" runat="server" CssClass="textBox" Width="85px" TabIndex="6"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Button ID="Button2" runat="server" CssClass="linkbotao" OnClick="Busca_CEP"
                                            Text="Pesquisar CEP" />
                                        <asp:Label ID="lblNumero" runat="server" CssClass="font" Width="92px" Style="padding-left: 110px">Nº*:</asp:Label>
                                        <asp:TextBox ID="txtNumero" runat="server" CssClass="textBox" Width="85px" MaxLength="10"
                                            TabIndex="7"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblEndereco" runat="server" CssClass="font" Width="120px">Endereço*:</asp:Label>
                                        <asp:TextBox ID="txtEndereco" runat="server" CssClass="textBox" Width="290px" TabIndex="8"
                                            MaxLength="35"></asp:TextBox>
                                        <asp:Label ID="lblCidade" runat="server" CssClass="font" Width="90px" Style="padding-left: 20px">Cidade*:</asp:Label>
                                        <asp:TextBox ID="txtCidade" runat="server" CssClass="textBox" Width="300px" TabIndex="10"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblBairro" runat="server" CssClass="font" Width="120px">Bairro*:</asp:Label>
                                        <asp:TextBox ID="txtBairro" runat="server" CssClass="textBox" Width="290px" TabIndex="9"
                                            MaxLength="35"></asp:TextBox>
                                        <asp:Label ID="lblUF" runat="server" CssClass="font" Width="90px" Style="padding-left: 20px">UF*:</asp:Label>
                                        <asp:TextBox ID="txtUF" runat="server" CssClass="textBox" Width="30px" TabIndex="11"
                                            MaxLength="3"></asp:TextBox>
                                        &nbsp;                                        
                                    </td>
                                </tr>
                                  <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label30" runat="server" CssClass="font" Width="120px">Complemento:</asp:Label>
                                        <asp:TextBox ID="txtComplemento" runat="server" CssClass="textBox" Width="290px" TabIndex="9"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblComercial" runat="server" CssClass="font" Width="120px">Telefone*:</asp:Label>
                                        <asp:TextBox ID="txtComercial" runat="server" CssClass="textBox" Width="110px" TabIndex="12"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="lblFax" runat="server" CssClass="font" Width="30px" Style="padding-left: 20px">Fax:</asp:Label>
                                        <asp:TextBox ID="txtFax" runat="server" CssClass="textBox" Width="120px" TabIndex="13"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="lblEmail" runat="server" CssClass="font" Width="90px" Style="padding-left: 20px">E-mail NF-e*:</asp:Label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="textBox" Width="300px" TabIndex="14"></asp:TextBox>
                                        <hr />
                                    </td>

                                </tr>
                            </div>

                            <%--Dados Financeiros--%>
                            <div>
                                <tr>
                                    <td class="linha">
                                        <b>
                                            <asp:Label runat="server" CssClass="font" Style="font-size: medium" Width="200px">Dados Financeiros</asp:Label></b>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label21" runat="server" CssClass="font" Width="120px">Contato*:</asp:Label>
                                        <asp:TextBox ID="txtContatoFinan" runat="server" CssClass="textBox" Width="290px" TabIndex="15"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label1" runat="server" CssClass="font" Width="120px">Telefone*:</asp:Label>
                                        <asp:TextBox ID="txtFoneFinan" runat="server" CssClass="textBox" Width="110px" TabIndex="16"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="Label8" runat="server" CssClass="font" Width="40px" Style="padding-left: 60px">Email*:</asp:Label>
                                        <asp:TextBox ID="txtEmailFinan" runat="server" CssClass="textBox" Width="325px" TabIndex="17"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label2" runat="server" CssClass="font" Width="120px">Condição de Pgto:</asp:Label>
                                        <asp:DropDownList ID="cbCondPgto" runat="server" DataTextField="Desc" DataValueField="Code"
                                            Width="150px" TabIndex="18">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblLimite" runat="server" CssClass="font" Width="150px" Style="padding-left: 20px">Limite de crédito sugerido:</asp:Label>
                                        <asp:TextBox ID="txtLimite" runat="server" type="number" CssClass="textBoxMoney" Width="80px" TabIndex="19"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label19" runat="server" CssClass="font" Width="120px">Forma de Pgto*:</asp:Label>
                                        <asp:DropDownList ID="cbFormaPgto" runat="server" DataTextField="Desc" DataValueField="Code"
                                            Width="150px" TabIndex="20">
<%--                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Boleto - Banco do Brasil" Value="A" />
                                            <asp:ListItem Text="Boleto - HSBC" Value="D" />
                                            <asp:ListItem Text="Cobrança em Carteira" Value="I" />--%>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblCNPJCobranca" runat="server" CssClass="font" Width="120px">CNPJ de Cobrança:</asp:Label>
                                        <asp:TextBox ID="txtCNPJCobranca" runat="server" CssClass="textBox" Width="150px"
                                            TabIndex="21" MaxLength="16"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label22" Width="350px" runat="server" CssClass="font">Observações:</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtObservacoes" runat="server" TextMode="MultiLine" Width="665px"
                                            Height="40px" MaxLength="2000" CssClass="textBox" TabIndex="21"></asp:TextBox>
                                    </td>
                                </tr>
                            </div>

                            <%--Dados Logísticos--%>
                            <div>
                                <tr>
                                    <td class="linha">
                                        <b>
                                            <asp:Label runat="server" CssClass="font" Style="font-size: medium" Width="200px">Dados Logísticos</asp:Label></b>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label20" runat="server" CssClass="font" Width="120px">Contato*:</asp:Label>
                                        <asp:TextBox ID="txtContatoLog" runat="server" CssClass="textBox" Width="290px" TabIndex="21"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label3" runat="server" CssClass="font" Width="120px">Telefone*:</asp:Label>
                                        <asp:TextBox ID="txtFoneLog" runat="server" CssClass="textBox" Width="110px" TabIndex="22"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="Label4" runat="server" CssClass="font" Width="38px" Style="padding-left: 60px">Email*:</asp:Label>
                                        <asp:TextBox ID="txtEmailLog" runat="server" CssClass="textBox" Width="325px" TabIndex="23">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="font" Width="123px">Mão de Obra (descarga)*:</asp:Label>
                                        <asp:DropDownList ID="cbMaoObra" runat="server" Width="110px" TabIndex="24">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Própria" Value="P" />
                                            <asp:ListItem Text="Terceirizada" Value="T" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label6" runat="server" CssClass="font" Width="90px" Style="padding-left: 10px">Tipo de Veículo*:</asp:Label>
                                        <asp:DropDownList ID="cbTipoVeiculo" runat="server" Width="110px" TabIndex="25">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Carreta" Value="C" />
                                            <asp:ListItem Text="Truck" Value="T" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label7" runat="server" CssClass="font" Width="90px" Style="padding-left: 10px">Tipo de Carga*:</asp:Label>
                                        <asp:DropDownList ID="cbTipoCarga" runat="server" Width="110px" TabIndex="26">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Batida" Value="B" />
                                            <asp:ListItem Text="Paletizada" Value="P" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label13" runat="server" CssClass="font" Width="80px" Style="padding-left: 10px">Cad. CHEP*:</asp:Label>
                                        <asp:DropDownList ID="cbCHEP" runat="server" Width="110px" TabIndex="27">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Sim" Value="S" />
                                            <asp:ListItem Text="Não" Value="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" CssClass="font" Width="123px">Estabelecimento*:</asp:Label>
                                        <asp:DropDownList ID="cbTipoEstabelecimento" runat="server" Width="110px" TabIndex="28">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Loja" Value="L" />
                                            <asp:ListItem Text="CD" Value="C" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label15" runat="server" CssClass="font" Width="90px" Style="padding-left: 10px">Agendamento*:</asp:Label>
                                        <asp:DropDownList ID="cbAgendamento" runat="server" Width="110px" TabIndex="29">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Sim" Value="S" />
                                            <asp:ListItem Text="Não" Value="N" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" CssClass="font">Local de entrega*:</asp:Label>
                                        <br />
                                        <asp:TextBox ID="txtLocalEntrega" runat="server" TextMode="MultiLine" Width="665px"
                                            Height="40px" MaxLength="2000" CssClass="textBox" TabIndex="30"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" CssClass="font">Não recebe entre </asp:Label>
                                        <asp:Label ID="Label16" runat="server" CssClass="font"></asp:Label>
                                        <asp:TextBox ID="txtDe" runat="server" CssClass="textBox" Width="50px" TabIndex="31"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="Label17" runat="server" CssClass="font">e</asp:Label>
                                        <asp:TextBox ID="txtAte" runat="server" CssClass="textBox" Width="50px" TabIndex="32"
                                            MaxLength="10"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="Label24" runat="server" CssClass="font">Não recebe entre </asp:Label>
                                        <asp:Label ID="Label28" runat="server" CssClass="font"></asp:Label>
                                        <asp:DropDownList ID="cbDeDia" runat="server" Width="110px" TabIndex="33">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Domingo" Value="DOM" />
                                            <asp:ListItem Text="Segunda" Value="SEG" />
                                            <asp:ListItem Text="Terça" Value="TER" />
                                            <asp:ListItem Text="Quarta" Value="QUA" />
                                            <asp:ListItem Text="Quinta" Value="QUI" />
                                            <asp:ListItem Text="Sexta" Value="SEX" />
                                            <asp:ListItem Text="Sábado" Value="SAB" />
                                        </asp:DropDownList>
                                        <asp:Label ID="Label25" runat="server" CssClass="font">e</asp:Label>
                                        <asp:DropDownList ID="cbAteDia" runat="server" Width="110px" TabIndex="34">
                                            <asp:ListItem Text="<Selecione>" Value="" />
                                            <asp:ListItem Text="Domingo" Value="DOM" />
                                            <asp:ListItem Text="Segunda" Value="SEG" />
                                            <asp:ListItem Text="Terça" Value="TER" />
                                            <asp:ListItem Text="Quarta" Value="QUA" />
                                            <asp:ListItem Text="Quinta" Value="QUI" />
                                            <asp:ListItem Text="Sexta" Value="SEX" />
                                            <asp:ListItem Text="Sábado" Value="SAB" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label26" runat="server" CssClass="font" Width="90px">Não recebe entre os dias</asp:Label>
                                        <asp:Label ID="Label27" runat="server" CssClass="font"></asp:Label>
                                        <asp:TextBox ID="txtDeMes" runat="server" CssClass="textBox" Width="50px" TabIndex="35"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="Label29" runat="server" CssClass="font">e</asp:Label>
                                        <asp:TextBox ID="txtAteMes" runat="server" CssClass="textBox" Width="50px" TabIndex="36"
                                            MaxLength="10"></asp:TextBox>
                                        <hr />
                                    </td>
                                </tr>
                            </div>

                            <%--Dados do Comprador--%>
                            <div>
                                <tr>
                                    <td class="linha">
                                        <b>
                                            <asp:Label runat="server" CssClass="font" Style="font-size: medium" Width="200px">Dados do Comprador</asp:Label></b>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="lblContato" runat="server" CssClass="font" Width="120px">Contato*:</asp:Label>
                                        <asp:TextBox ID="txtContato" runat="server" CssClass="textBox" Width="290px" TabIndex="37"
                                            MaxLength="35"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="linha">
                                        <asp:Label ID="Label9" runat="server" CssClass="font" Width="120px">Telefone*:</asp:Label>
                                        <asp:TextBox ID="txtFoneComp" runat="server" CssClass="textBox" Width="110px" TabIndex="38"
                                            MaxLength="10"></asp:TextBox>
                                        <asp:Label ID="lblCelular" runat="server" CssClass="font" Width="50px" Style="padding-left: 10px">Celular:</asp:Label>
                                        <asp:TextBox ID="txtCelular" runat="server" CssClass="textBox" Width="110px" TabIndex="39"
                                            MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                            </div>

                            <tr>
                                <td class="linha">
                                    <asp:Label ID="Label10" runat="server" CssClass="font" Width="120px">Email*:</asp:Label>
                                    <asp:TextBox ID="txtEmailComp" runat="server" CssClass="textBox" Width="325px" TabIndex="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="linha">
                                    <asp:Label ID="Label23" runat="server" CssClass="font" Width="120px">Aniversário:</asp:Label>
                                    <asp:TextBox ID="txtAniversario" runat="server" CssClass="textBox" Width="50px" TabIndex="41"></asp:TextBox>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td class="linha">
                                    <b>
                                        <asp:Label runat="server" CssClass="font" Style="font-size: medium" Width="200px">Dados de Venda</asp:Label></b>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td class="linha">
                                    <table>
                                        <tr>
                                            <td class="linha" style="width: 255px">
                                                <asp:Label ID="Label18" runat="server" CssClass="font" Width="100px">N° Pedido Cliente*:</asp:Label>
                                                <asp:DropDownList ID="cbPedido" runat="server" Width="150px" TabIndex="42">
                                                    <asp:ListItem Text="<Selecione>" Value="" />
                                                    <asp:ListItem Text="Não utiliza" Value="N" />
                                                    <asp:ListItem Text="Utiliza" Value="S" />
                                                </asp:DropDownList>
                                                <asp:Label ID="lblAreaNielsen" runat="server" CssClass="font" Width="100px">Área Nielsen*:</asp:Label>
                                                <asp:DropDownList ID="ddlAreaNielsen" runat="server" DataTextField="BEZEI" DataValueField="NIELS"
                                                    Width="150px" TabIndex="43">
                                                </asp:DropDownList><br />
                                                <asp:Label ID="lblCanalVenda" runat="server" CssClass="font" Width="100px">Canal de Venda*:</asp:Label>
                                                <asp:DropDownList ID="ddlCanalVenda" runat="server" DataTextField="VTEXT" DataValueField="VTWEG"
                                                    Width="150px" TabIndex="44">
                                                </asp:DropDownList><br />
                                                <asp:Label ID="lblTipoCliente" runat="server" CssClass="font" Width="100px">Tipo Cliente*:</asp:Label>
                                                <asp:DropDownList ID="ddlTipoCliente" runat="server" DataTextField="VTEXT" DataValueField="KUKLA"
                                                    Width="150px" TabIndex="45">
                                                </asp:DropDownList><br />
                                            </td>
                                            <td class="linha">
                                                <asp:Label ID="lblVendas" runat="server" CssClass="font" Width="180px">Escritório / Equipe / Descrição</asp:Label>
                                                <br />
                                                <asp:ListBox ID="lstVendas" runat="server" CssClass="textBox" Height="50px" Width="250px" TabIndex="46"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td class="linha" style="text-align: right;">
                                    <asp:Button ID="btnOK" runat="server" CssClass="linkbotao" OnClick="Salvar_Cliente"
                                        Text="Salvar" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />
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
                <cc1:MaskedEditExtender ID="meeCNPJ" runat="server" MaskType="Number" TargetControlID="txtCNPJ"
                    Mask="99,999,999/9999-99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" ClearMaskOnLostFocus="False" InputDirection="RightToLeft">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="txtAniversario"
                    Mask="99/99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" ClearMaskOnLostFocus="False" InputDirection="RightToLeft">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="meeCNPJ2" runat="server" MaskType="Number" TargetControlID="txtCNPJCobranca"
                    Mask="99,999,999/9999-99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError" ClearMaskOnLostFocus="False" InputDirection="RightToLeft">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="meeTelComercial" runat="server" MaskType="Number" TargetControlID="txtComercial"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="meeTelFax" runat="server" MaskType="Number" TargetControlID="txtFax"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="meeTelCelular" runat="server" MaskType="Number" TargetControlID="txtCelular"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="meeCEP" runat="server" MaskType="Number" TargetControlID="txtCEP"
                    Mask="99999-999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" MaskType="Number" TargetControlID="txtFoneLog"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" MaskType="Number" TargetControlID="txtFoneFinan"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number" TargetControlID="txtFoneComp"
                    Mask="(99)9999-9999" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender4" runat="server" MaskType="Time" TargetControlID="txtDe"
                    Mask="99:99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditExtender ID="MaskedEditExtender5" runat="server" MaskType="Time" TargetControlID="txtAte"
                    Mask="99:99" CultureName="pt-BR" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ClearMaskOnLostFocus="False">
                </cc1:MaskedEditExtender>

            </ContentTemplate>
        </asp:UpdatePanel>
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
                            <td>Por favor, aguarde...
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
