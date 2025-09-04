USE [CVA_ATL_REP]
GO
ALTER TABLE [dbo].[CVA_REG_LOG]  WITH CHECK ADD FOREIGN KEY([STU]) REFERENCES [dbo].[CVA_STU] ([ID])
GO
ALTER TABLE [dbo].[CVA_TIM]  WITH CHECK ADD FOREIGN KEY([STU]) REFERENCES [dbo].[CVA_STU] ([ID])
GO
ALTER TABLE [dbo].[CVA_TIP_REG]  WITH CHECK ADD FOREIGN KEY([STU]) REFERENCES [dbo].[CVA_STU] ([ID])
GO
USE [CVA_ATL_REP]
GO
SET IDENTITY_INSERT [dbo].[CVA_STU] ON 
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (1, CAST(N'2016-09-16 10:06:23.207' AS DateTime), NULL, N'Inativo')
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (2, CAST(N'2016-09-16 10:06:23.207' AS DateTime), NULL, N'Ativo')
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (3, CAST(N'2016-09-16 10:06:23.207' AS DateTime), NULL, N'Aguardando')
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (4, CAST(N'2016-09-16 10:06:23.207' AS DateTime), NULL, N'Replicado')
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (5, CAST(N'2016-09-16 10:06:23.210' AS DateTime), NULL, N'Erro')
GO
INSERT [dbo].[CVA_STU] ([ID], [INS], [UPD], [STU]) VALUES (6, CAST(N'2016-09-16 10:06:23.210' AS DateTime), NULL, N'Em execução')
GO
SET IDENTITY_INSERT [dbo].[CVA_STU] OFF
GO
SET IDENTITY_INSERT [dbo].[CVA_FUNC] ON
GO
INSERT [dbo].[CVA_FUNC] ([ID], [INS], [UPD], [STU], [FUNC], [DSCR]) VALUES (1, CAST(N'2016-09-16 10:21:57.487' AS DateTime), NULL, 2, N'A', N'Add')
GO
INSERT [dbo].[CVA_FUNC] ([ID], [INS], [UPD], [STU], [FUNC], [DSCR]) VALUES (2, CAST(N'2016-09-16 10:21:57.487' AS DateTime), NULL, 2, N'U', N'Update')
GO
INSERT [dbo].[CVA_FUNC] ([ID], [INS], [UPD], [STU], [FUNC], [DSCR]) VALUES (3, CAST(N'2016-09-16 10:21:57.487' AS DateTime), NULL, 2, N'D', N'Delete')
GO
INSERT [dbo].[CVA_FUNC] ([ID], [INS], [UPD], [STU], [FUNC], [DSCR]) VALUES (4, CAST(N'2016-09-16 10:21:57.487' AS DateTime), NULL, 2, N'C', N'Cancel')
GO
INSERT [dbo].[CVA_FUNC] ([ID], [INS], [UPD], [STU], [FUNC], [DSCR]) VALUES (5, CAST(N'2016-09-16 10:21:57.487' AS DateTime), NULL, 2, N'L', N'Close')
GO
SET IDENTITY_INSERT [dbo].[CVA_FUNC] OFF
GO
SET IDENTITY_INSERT [dbo].[CVA_OBJ] ON
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (1, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_GrupoParceiroNegocio', N'Grupos dos parceiros de negocio', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (2, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_GrupoItem', N'Grupos de itens', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (3, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Usuario', N'Usuarios', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (4, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_FormaPagamento', N'Formas de pagamento', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (5, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_CondicaoPagamento', N'Condicoes de pagamento', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (6, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_CentroCusto', N'Centros de custo', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (7, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Indicador', N'Indicadores', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (8, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Utilizacao', N'Utilizacoes', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (9, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_PlanoContas', N'Contas do plano de contas', 2)
GO
INSERT [dbo].[CVA_OBJ] ([ID], [INS], [UPD], [OBJ], [DSCR], [STU]) VALUES (10, CAST(N'2016-09-16 10:28:56.563' AS DateTime), NULL, N'CVA_Obj_Depositos', N'Depositos', 1)
GO
SET IDENTITY_INSERT [dbo].[CVA_OBJ] OFF