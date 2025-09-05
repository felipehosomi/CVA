<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cmenu.ascx.cs" Inherits="WebImcopa.menu.cmenu" %>
<%@ Register Assembly="skmMenu" Namespace="skmMenu" TagPrefix="cc1" %>
<link href="../Config.css" rel="stylesheet" type="text/css" />
<cc1:Menu ID="MnuPrincipal" runat="server" Layout="Horizontal" ItemPadding="5" ItemSpacing="0" MenuFadeDelay="1" Cursor="Pointer" HighlightTopMenu="false">
    <UnselectedMenuItemStyle CssClass="Menu" />
    <SelectedMenuItemStyle CssClass="MenuSelecionado" />
</cc1:Menu>