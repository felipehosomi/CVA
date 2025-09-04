Use SBOAlessi

go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa_CARGA_Analitico]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa_CARGA_Analitico]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa_CARGA]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa_CARGA]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCFluxoCaixa]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCFluxoCaixa]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasRecebidasPorCliente]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasRecebidasPorCliente]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasReceberUnificado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasReceberUnificado]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasPagasPorFornecedor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasPagasPorFornecedor]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasPagarUnificado]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasPagarUnificado]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasAReceberPorVencimento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasAReceberPorVencimento]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCContasAPagarPorVencimento]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCContasAPagarPorVencimento]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spcJBCBalancete1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spcJBCBalancete1]
GO