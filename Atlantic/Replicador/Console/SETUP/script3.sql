USE [CVA_ATL_REP]
GO
SET IDENTITY_INSERT [dbo].[CVA_OBJ] ON
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (11, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Item', N'Dados mestre dos itens', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (12, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_ParceiroNegocio', N'Dados mestre dos parceiros de negocio', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (13, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Imposto', N'Codigos de imposto', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (14, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_ContasBancarias', N'Contas bancarias da empresa', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (15, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_VendedoresCompradores', N'Vendedores e Compradores', 2)
GO
SET IDENTITY_INSERT [dbo].[CVA_OBJ] OFF
GO
SET IDENTITY_INSERT [dbo].[CVA_TIM] ON
GO
INSERT [dbo].[CVA_TIM] ([ID], [INS], [UPD], [STU], [TIM], [NUM_OBJ]) VALUES (1, CAST(N'2016-09-16 10:07:18.680' AS DateTime), NULL, 2, 1, 10)
GO
SET IDENTITY_INSERT [dbo].[CVA_TIM] OFF