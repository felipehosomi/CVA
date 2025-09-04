CREATE VIEW  [dbo].[CVA_LIVRODEENTRADA_ITENS]    
    
AS     
    
SELECT   
 OPCH.BPLId					 BPLId,    
 PCH1.DocEntry               DocEntry,    
 PCH1.ObjType                ObjType,  
 OPCH.DocDate                DocDate,    
 OPCH.Serial                 Serial,
 OPCH.SeriesStr				 SeriesStr,
 OPCH.Cardcode				 CardCode,
 PCH12.StateS				 States,
 PCH12.CountryB				 CountryB, 
 PCH1.LineNum				 LineNum,    
 PCH1.ItemCode               ItemCode,    
 PCH1.Dscription             Dscription,     
 PCH1.Quantity               Quantity,     
 PCH1.Price                  Price,     
 PCH1.LineTotal              LineTotal,    
 ISNULL(PCH1.DistribSum,0)   DistribSum,    
 PCH1.CSTCode                CSTCode,     
 PCH1.CFOPCode               CFOPCode,    
 ISNULL(ICMS.BaseSum,0)      ICMS_BaseSum,    
 ISNULL(ICMS.TaxSum,0)       ICMS_TaxSum,    
 ISNULL(ICMS.U_ExcAmtL,0)    ICMS_U_ExcAmtL,    
 ISNULL(ICMS.U_OthAmtL,0)    ICMS_U_OthAmtL,     
 ISNULL(ICMSST.BaseSum,0)    ICMSST_BaseSum,    
 ISNULL(ICMSST.TaxSum,0)     ICMSST_TaxSum,    
 PCH1.CSTfIPI                CSTfIPI,    
 IPI.NonDdctPrc              IPI_NonDdctPrc,
  CASE WHEN 
	OUSG.U_ApropCred = 'N'
	THEN 0.00 
	ELSE ISNULL(IPI.BaseSum,0)
	END                      IPI_BaseSum,    
 CASE WHEN 
	OUSG.U_ApropCred = 'N'
	THEN 0.00 
	ELSE ISNULL(IPI.TaxSum,0) 
	END                      IPI_TaxSum,    
 ISNULL(IPI.U_ExcAmtL,0)     IPI_U_ExcAmtL,    
 ISNULL(IPI.U_OthAmtL,0)     IPI_U_OthAmtL,    
 PCH1.CSTfPIS                CSTfPIS,    
 ISNULL(PIS.BaseSum,0)       PIS_BaseSum,    
 ISNULL(PIS.TaxSum,0)        PIS_TaxSum,    
 ISNULL(PIS.U_ExcAmtL,0)     PIS_U_ExcAmtL,    
 ISNULL(PIS.U_OthAmtL,0)     PIS_U_OthAmtL,    
 PCH1.CSTfCOFINS             CSTfCOFINS,    
 ISNULL(COFINS.BaseSum,0)    COFINS_BaseSum,    
 ISNULL(COFINS.TaxSum,0)     COFINS_TaxSum,    
 ISNULL(COFINS.U_ExcAmtL,0)  COFINS_U_ExcAmtL,    
 ISNULL(COFINS.U_OthAmtL,0)  COFINS_U_OthAmtL    
    
    
 FROM PCH1 WITH(NOLOCK)     
     INNER JOIN OPCH WITH(NOLOCK) ON PCH1.Docentry = OPCH.DocEntry    
  LEFT JOIN     
   (    
   SELECT PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PCH4 WITH(NOLOCK)              
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'       
      INNER JOIN PCH1 WITH(NOLOCK) ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum         
     GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc    
   ) ICMS ON ICMS.DocEntry = PCH1.DocEntry AND ICMS.LineNum = PCH1.LineNum     
  LEFT JOIN     
   (    
   SELECT PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PCH4 WITH(NOLOCK)               
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'       
      INNER JOIN PCH1 WITH(NOLOCK) ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum         
     GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc    
   ) IPI ON IPI.DocEntry = PCH1.DocEntry AND IPI.LineNum = PCH1.LineNum       
  LEFT JOIN     
   (    
   SELECT PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PCH4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'       
      INNER JOIN PCH1 WITH(NOLOCK) ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum         
     GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc    
   ) ICMSST ON ICMSST.DocEntry = PCH1.DocEntry AND ICMSST.LineNum = PCH1.LineNum       
  LEFT JOIN     
   (    
   SELECT PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PCH4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'       
      INNER JOIN PCH1 WITH(NOLOCK) ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum         
     GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc    
   ) PIS ON PIS.DocEntry = PCH1.DocEntry AND PIS.LineNum = PCH1.LineNum      
  LEFT JOIN     
   (    
   SELECT PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PCH4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PCH4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'       
      INNER JOIN PCH1 WITH(NOLOCK) ON PCH4.DocEntry = PCH1.DocEntry AND PCH4.LineNum = PCH1.LineNum         
     GROUP BY PCH4.DocEntry, PCH4.LineNum, PCH4.NonDdctPrc    
   ) COFINS ON COFINS.DocEntry = PCH1.DocEntry AND COFINS.LineNum = PCH1.LineNum 
   LEFT JOIN OUSG WITH(NOLOCK) ON PCH1.Usage = OUSG.ID  
   LEFT JOIN PCH12 WITH(NOLOCK) ON OPCH.DocEntry = PCH12.DocEntry
    
     
 WHERE OPCH.Canceled = 'N'    
    
UNION ALL     
    
--------------------------------------------------------------------------------------------------------    
----------------------------------- DEVOLUÇÃO NOTA DE ENTRADA ------------------------------------------    
--------------------------------------------------------------------------------------------------------    
    
SELECT   
 ORIN.BPLId      BPLId,      
 RIN1.DocEntry               DocEntry,    
 RIN1.ObjType                ObjType,    
 ORIN.DocDate                DocDate,    
 ORIN.Serial                 Serial, 
 ORIN.SeriesStr				 SeriesStr,  
 ORIN.Cardcode				 CardCode,
 RIN12.StateS				 States,
 RIN12.CountryB				 CountryB,  
 RIN1.LineNum				 LineNum,      
 RIN1.ItemCode               ItemCode,    
 RIN1.Dscription             Dscription,     
 RIN1.Quantity               Quantity,     
 RIN1.Price                  Price,     
 RIN1.LineTotal              LineTotal,    
 ISNULL(RIN1.DistribSum,0)   DistribSum,    
 RIN1.CSTCode                CSTCode,     
 RIN1.CFOPCode               CFOPCode,    
 ISNULL(ICMS.BaseSum,0)      ICMS_BaseSum,    
 ISNULL(ICMS.TaxSum,0)       ICMS_TaxSum,    
 ISNULL(ICMS.U_ExcAmtL,0)    ICMS_U_ExcAmtL,    
 ISNULL(ICMS.U_OthAmtL,0)    ICMS_U_OthAmtL,     
 ISNULL(ICMSST.BaseSum,0)    ICMSST_BaseSum,    
 ISNULL(ICMSST.TaxSum,0)     ICMSST_TaxSum,    
 RIN1.CSTfIPI                CSTfIPI,    
 IPI.NonDdctPrc              IPI_NonDdctPrc,    
 ISNULL(IPI.BaseSum,0)       IPI_BaseSum,    
 ISNULL(IPI.TaxSum,0)        IPI_TaxSum,    
 ISNULL(IPI.U_ExcAmtL,0)     IPI_U_ExcAmtL,    
 ISNULL(IPI.U_OthAmtL,0)     IPI_U_OthAmtL,    
 RIN1.CSTfPIS                CSTfPIS,    
 ISNULL(PIS.BaseSum,0)       PIS_BaseSum,    
 ISNULL(PIS.TaxSum,0)        PIS_TaxSum,    
 ISNULL(PIS.U_ExcAmtL,0)     PIS_U_ExcAmtL,    
 ISNULL(PIS.U_OthAmtL,0)     PIS_U_OthAmtL,    
 RIN1.CSTfCOFINS             CSTfCOFINS,    
 ISNULL(COFINS.BaseSum,0)    COFINS_BaseSum,    
 ISNULL(COFINS.TaxSum,0)     COFINS_TaxSum,    
 ISNULL(COFINS.U_ExcAmtL,0)  COFINS_U_ExcAmtL,    
 ISNULL(COFINS.U_OthAmtL,0)  COFINS_U_OthAmtL    
    
    
 FROM RIN1 WITH(NOLOCK)     
     INNER JOIN ORIN WITH(NOLOCK) ON RIN1.Docentry = ORIN.DocEntry    
  LEFT JOIN     
   (    
   SELECT RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RIN4 WITH(NOLOCK)              
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'       
      INNER JOIN RIN1 WITH(NOLOCK) ON RIN4.DocEntry = RIN1.DocEntry AND RIN4.LineNum = RIN1.LineNum         
     GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc    
   ) ICMS ON ICMS.DocEntry = RIN1.DocEntry AND ICMS.LineNum = RIN1.LineNum     
  LEFT JOIN     
   (    
   SELECT RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RIN4 WITH(NOLOCK)               
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'       
      INNER JOIN RIN1 WITH(NOLOCK) ON RIN4.DocEntry = RIN1.DocEntry AND RIN4.LineNum = RIN1.LineNum         
     GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc    
   ) IPI ON IPI.DocEntry = RIN1.DocEntry AND IPI.LineNum = RIN1.LineNum       
  LEFT JOIN     
   (    
   SELECT RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RIN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'       
      INNER JOIN RIN1 WITH(NOLOCK) ON RIN4.DocEntry = RIN1.DocEntry AND RIN4.LineNum = RIN1.LineNum         
     GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc    
   ) ICMSST ON ICMSST.DocEntry = RIN1.DocEntry AND ICMSST.LineNum = RIN1.LineNum       
  LEFT JOIN     
   (    
   SELECT RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RIN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'       
      INNER JOIN RIN1 WITH(NOLOCK) ON RIN4.DocEntry = RIN1.DocEntry AND RIN4.LineNum = RIN1.LineNum         
     GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc    
   ) PIS ON PIS.DocEntry = RIN1.DocEntry AND PIS.LineNum = RIN1.LineNum      
  LEFT JOIN     
   (    
   SELECT RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RIN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RIN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'       
      INNER JOIN RIN1 WITH(NOLOCK) ON RIN4.DocEntry = RIN1.DocEntry AND RIN4.LineNum = RIN1.LineNum         
     GROUP BY RIN4.DocEntry, RIN4.LineNum, RIN4.NonDdctPrc    
   ) COFINS ON COFINS.DocEntry = RIN1.DocEntry AND COFINS.LineNum = RIN1.LineNum      
    LEFT JOIN RIN12 WITH(NOLOCK) ON ORIN.DocEntry = RIN12.DocEntry
     
 WHERE ORIN.Canceled = 'N'    
    
    
UNION ALL     
    
--------------------------------------------------------------------------------------------------------    
----------------------------------- DEVOLUÇÃO ----------------------------------------------------------    
--------------------------------------------------------------------------------------------------------    
    
    
    
SELECT   
 ORDN.BPLId					 BPLId,        
 RDN1.DocEntry               DocEntry,    
 RDN1.ObjType                ObjType,    
 ORDN.DocDate                DocDate,    
 ORDN.Serial                 Serial,
 ORDN.SeriesStr				 SeriesStr,
 ORDN.Cardcode				 CardCode,
 RDN12.StateS				 States,
 RDN12.CountryB				 CountryB,   
 RDN1.LineNum				 LineNum,      
 RDN1.ItemCode               ItemCode,    
 RDN1.Dscription             Dscription,     
 RDN1.Quantity               Quantity,     
 RDN1.Price                  Price,     
 RDN1.LineTotal              LineTotal,    
 ISNULL(RDN1.DistribSum,0)   DistribSum,    
 RDN1.CSTCode                CSTCode,     
 RDN1.CFOPCode               CFOPCode,    
 ISNULL(ICMS.BaseSum,0)      ICMS_BaseSum,    
 ISNULL(ICMS.TaxSum,0)       ICMS_TaxSum,    
 ISNULL(ICMS.U_ExcAmtL,0)    ICMS_U_ExcAmtL,    
 ISNULL(ICMS.U_OthAmtL,0)    ICMS_U_OthAmtL,     
 ISNULL(ICMSST.BaseSum,0)    ICMSST_BaseSum,    
 ISNULL(ICMSST.TaxSum,0)     ICMSST_TaxSum,    
 RDN1.CSTfIPI                CSTfIPI,    
 IPI.NonDdctPrc              IPI_NonDdctPrc,    
 ISNULL(IPI.BaseSum,0)       IPI_BaseSum,    
 ISNULL(IPI.TaxSum,0)        IPI_TaxSum,    
 ISNULL(IPI.U_ExcAmtL,0)     IPI_U_ExcAmtL,    
 ISNULL(IPI.U_OthAmtL,0)     IPI_U_OthAmtL,    
 RDN1.CSTfPIS                CSTfPIS,    
 ISNULL(PIS.BaseSum,0)       PIS_BaseSum,    
 ISNULL(PIS.TaxSum,0)        PIS_TaxSum,    
 ISNULL(PIS.U_ExcAmtL,0)     PIS_U_ExcAmtL,    
 ISNULL(PIS.U_OthAmtL,0)     PIS_U_OthAmtL,    
 RDN1.CSTfCOFINS             CSTfCOFINS,    
 ISNULL(COFINS.BaseSum,0)    COFINS_BaseSum,    
 ISNULL(COFINS.TaxSum,0)     COFINS_TaxSum,    
 ISNULL(COFINS.U_ExcAmtL,0)  COFINS_U_ExcAmtL,    
 ISNULL(COFINS.U_OthAmtL,0)  COFINS_U_OthAmtL    
    
    
 FROM RDN1 WITH(NOLOCK)     
     INNER JOIN ORDN WITH(NOLOCK) ON RDN1.Docentry = ORDN.DocEntry    
  LEFT JOIN     
   (    
   SELECT RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RDN4 WITH(NOLOCK)              
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'       
      INNER JOIN RDN1 WITH(NOLOCK) ON RDN4.DocEntry = RDN1.DocEntry AND RDN4.LineNum = RDN1.LineNum         
     GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc    
   ) ICMS ON ICMS.DocEntry = RDN1.DocEntry AND ICMS.LineNum = RDN1.LineNum     
  LEFT JOIN     
   (    
   SELECT RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RDN4 WITH(NOLOCK)               
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'       
      INNER JOIN RDN1 WITH(NOLOCK) ON RDN4.DocEntry = RDN1.DocEntry AND RDN4.LineNum = RDN1.LineNum         
     GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc    
   ) IPI ON IPI.DocEntry = RDN1.DocEntry AND IPI.LineNum = RDN1.LineNum       
  LEFT JOIN     
   (    
   SELECT RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'       
      INNER JOIN RDN1 WITH(NOLOCK) ON RDN4.DocEntry = RDN1.DocEntry AND RDN4.LineNum = RDN1.LineNum         
     GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc    
   ) ICMSST ON ICMSST.DocEntry = RDN1.DocEntry AND ICMSST.LineNum = RDN1.LineNum       
  LEFT JOIN     
   (    
   SELECT RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'       
      INNER JOIN RDN1 WITH(NOLOCK) ON RDN4.DocEntry = RDN1.DocEntry AND RDN4.LineNum = RDN1.LineNum         
     GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc    
   ) PIS ON PIS.DocEntry = RDN1.DocEntry AND PIS.LineNum = RDN1.LineNum      
  LEFT JOIN     
   (    
   SELECT RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  RDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = RDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'       
      INNER JOIN RDN1 WITH(NOLOCK) ON RDN4.DocEntry = RDN1.DocEntry AND RDN4.LineNum = RDN1.LineNum         
     GROUP BY RDN4.DocEntry, RDN4.LineNum, RDN4.NonDdctPrc    
   ) COFINS ON COFINS.DocEntry = RDN1.DocEntry AND COFINS.LineNum = RDN1.LineNum      
   LEFT JOIN RDN12 WITH(NOLOCK) ON ORDN.DocEntry = RDN12.DocEntry 
     
 WHERE ORDN.Canceled = 'N'    

 UNION ALL

--------------------------------------------------------------------------------------------------------    
----------------------------------- RECEBIMENTO --------------------------------------------------------  
--------------------------------------------------------------------------------------------------------    
 
 SELECT   
 OPDN.BPLId      	         BPLId,      
 PDN1.DocEntry               DocEntry,    
 PDN1.ObjType                ObjType,    
 OPDN.DocDate                DocDate,    
 OPDN.Serial                 Serial, 
 OPDN.SeriesStr		         SeriesStr,  
 OPDN.Cardcode		         CardCode,
 PDN12.StateS				 States,
 PDN12.CountryB				 CountryB,  
 PDN1.LineNum		         LineNum,      
 PDN1.ItemCode               ItemCode,    
 PDN1.Dscription             Dscription,     
 PDN1.Quantity               Quantity,     
 PDN1.Price                  Price,     
 PDN1.LineTotal              LineTotal,    
 ISNULL(PDN1.DistribSum,0)   DistribSum,    
 PDN1.CSTCode                CSTCode,     
 PDN1.CFOPCode               CFOPCode,    
 ISNULL(ICMS.BaseSum,0)      ICMS_BaseSum,    
 ISNULL(ICMS.TaxSum,0)       ICMS_TaxSum,    
 ISNULL(ICMS.U_ExcAmtL,0)    ICMS_U_ExcAmtL,    
 ISNULL(ICMS.U_OthAmtL,0)    ICMS_U_OthAmtL,     
 ISNULL(ICMSST.BaseSum,0)    ICMSST_BaseSum,    
 ISNULL(ICMSST.TaxSum,0)     ICMSST_TaxSum,    
 PDN1.CSTfIPI                CSTfIPI,    
 IPI.NonDdctPrc              IPI_NonDdctPrc,    
 ISNULL(IPI.BaseSum,0)       IPI_BaseSum,    
 ISNULL(IPI.TaxSum,0)        IPI_TaxSum,    
 ISNULL(IPI.U_ExcAmtL,0)     IPI_U_ExcAmtL,    
 ISNULL(IPI.U_OthAmtL,0)     IPI_U_OthAmtL,    
 PDN1.CSTfPIS                CSTfPIS,    
 ISNULL(PIS.BaseSum,0)       PIS_BaseSum,    
 ISNULL(PIS.TaxSum,0)        PIS_TaxSum,    
 ISNULL(PIS.U_ExcAmtL,0)     PIS_U_ExcAmtL,    
 ISNULL(PIS.U_OthAmtL,0)     PIS_U_OthAmtL,    
 PDN1.CSTfCOFINS             CSTfCOFINS,    
 ISNULL(COFINS.BaseSum,0)    COFINS_BaseSum,    
 ISNULL(COFINS.TaxSum,0)     COFINS_TaxSum,    
 ISNULL(COFINS.U_ExcAmtL,0)  COFINS_U_ExcAmtL,    
 ISNULL(COFINS.U_OthAmtL,0)  COFINS_U_OthAmtL    
    
    
 FROM PDN1 WITH(NOLOCK)     
     INNER JOIN OPDN WITH(NOLOCK) ON PDN1.Docentry = OPDN.DocEntry    
  LEFT JOIN     
   (    
   SELECT PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PDN4 WITH(NOLOCK)              
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS'       
      INNER JOIN PDN1 WITH(NOLOCK) ON PDN4.DocEntry = PDN1.DocEntry AND PDN4.LineNum = PDN1.LineNum         
     GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc    
   ) ICMS ON ICMS.DocEntry = PDN1.DocEntry AND ICMS.LineNum = PDN1.LineNum     
  LEFT JOIN     
   (    
   SELECT PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PDN4 WITH(NOLOCK)               
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'IPI'       
      INNER JOIN PDN1 WITH(NOLOCK) ON PDN4.DocEntry = PDN1.DocEntry AND PDN4.LineNum = PDN1.LineNum         
     GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc    
   ) IPI ON IPI.DocEntry = PDN1.DocEntry AND IPI.LineNum = PDN1.LineNum       
  LEFT JOIN     
   (    
   SELECT PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'ICMS-ST'       
      INNER JOIN PDN1 WITH(NOLOCK) ON PDN4.DocEntry = PDN1.DocEntry AND PDN4.LineNum = PDN1.LineNum         
     GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc    
   ) ICMSST ON ICMSST.DocEntry = PDN1.DocEntry AND ICMSST.LineNum = PDN1.LineNum       
  LEFT JOIN     
   (    
   SELECT PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'PIS'       
      INNER JOIN PDN1 WITH(NOLOCK) ON PDN4.DocEntry = PDN1.DocEntry AND PDN4.LineNum = PDN1.LineNum         
     GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc    
   ) PIS ON PIS.DocEntry = PDN1.DocEntry AND PIS.LineNum = PDN1.LineNum      
  LEFT JOIN     
   (    
   SELECT PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc, SUM(TaxSum) TaxSum, SUM(BaseSum) BaseSum, SUM(U_ExcAmtL) U_ExcAmtL, SUM(U_OthAmtL) U_OthAmtL FROM  PDN4 WITH(NOLOCK)      
      INNER JOIN OSTT WITH(NOLOCK) ON OSTT.AbsId = PDN4.StaType              
      INNER JOIN ONFT WITH(NOLOCK) ON ONFT.AbsId = OSTT.NfTaxId AND ONFT.Code = 'COFINS'       
      INNER JOIN PDN1 WITH(NOLOCK) ON PDN4.DocEntry = PDN1.DocEntry AND PDN4.LineNum = PDN1.LineNum         
     GROUP BY PDN4.DocEntry, PDN4.LineNum, PDN4.NonDdctPrc    
   ) COFINS ON COFINS.DocEntry = PDN1.DocEntry AND COFINS.LineNum = PDN1.LineNum      
    LEFT JOIN PDN12 WITH(NOLOCK) ON OPDN.DocEntry = PDN12.DocEntry
     
 WHERE OPDN.Canceled = 'N'    
    
    
    
    

GO


