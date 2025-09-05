alter view "DOC1_View" ( "DocEntry",
	 "DocType",
	 "LineNum",
	 "Model",
	 "Serial",
	 "SeriesStr",
	 "SubStr",
	 "TaxDate",
	 "CFOPCode",
	 "AcctCode") AS (((((((((((((((((((((((SELECT
	 T1."DocEntry",
	 132 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode" 	
																							FROM "OCIN" T0 
																							INNER JOIN "CIN1" T1 ON T0."DocEntry" = T1."DocEntry") 
																						UNION ALL (SELECT
	 T1."DocEntry",
	 163 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																							FROM "OCPI" T0 
																							INNER JOIN "CPI1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																					UNION ALL (SELECT
	 T1."DocEntry",
	 164 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																						FROM "OCPV" T0 
																						INNER JOIN "CPV1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																				UNION ALL (SELECT
	 T1."DocEntry",
	 165 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																					FROM "OCSI" T0 
																					INNER JOIN "CSI1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																			UNION ALL (SELECT
	 T1."DocEntry",
	 166 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																				FROM "OCSV" T0 
																				INNER JOIN "CSV1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																		UNION ALL (SELECT
	 T1."DocEntry",
	 15 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																			FROM "ODLN" T0 
																			INNER JOIN "DLN1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																	UNION ALL (SELECT
	 T1."DocEntry",
	 203 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																		FROM "ODPI" T0 
																		INNER JOIN "DPI1" T1 ON T0."DocEntry" = T1."DocEntry")) 
																UNION ALL (SELECT
	 T1."DocEntry",
	 204 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																	FROM "ODPO" T0 
																	INNER JOIN "DPO1" T1 ON T0."DocEntry" = T1."DocEntry")) 
															UNION ALL (SELECT
	 T1."DocEntry",
	 140000010 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
																FROM "OIEI" T0 
																INNER JOIN "IEI1" T1 ON T0."DocEntry" = T1."DocEntry")) 
														UNION ALL (SELECT
	 T1."DocEntry",
	 60 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
															FROM "OIGE" T0 
															INNER JOIN "IGE1" T1 ON T0."DocEntry" = T1."DocEntry")) 
													UNION ALL (SELECT
	 T1."DocEntry",
	 59 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
														FROM "OIGN" T0 
														INNER JOIN "IGN1" T1 ON T0."DocEntry" = T1."DocEntry")) 
												UNION ALL (SELECT
	 T1."DocEntry",
	 13 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
													FROM "OINV" T0 
													INNER JOIN "INV1" T1 ON T0."DocEntry" = T1."DocEntry")) 
											UNION ALL (SELECT
	 T1."DocEntry",
	 140000009 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
												FROM "OOEI" T0 
												INNER JOIN "OEI1" T1 ON T0."DocEntry" = T1."DocEntry")) 
										UNION ALL (SELECT
	 T1."DocEntry",
	 18 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
											FROM "OPCH" T0 
											INNER JOIN "PCH1" T1 ON T0."DocEntry" = T1."DocEntry")) 
									UNION ALL (SELECT
	 T1."DocEntry",
	 20 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
										FROM "OPDN" T0 
										INNER JOIN "PDN1" T1 ON T0."DocEntry" = T1."DocEntry")) 
								UNION ALL (SELECT
	 T1."DocEntry",
	 22 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
									FROM "OPOR" T0 
									INNER JOIN "POR1" T1 ON T0."DocEntry" = T1."DocEntry")) 
							UNION ALL (SELECT
	 T1."DocEntry",
	 23 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
								FROM "OQUT" T0 
								INNER JOIN "QUT1" T1 ON T0."DocEntry" = T1."DocEntry")) 
						UNION ALL (SELECT
	 T1."DocEntry",
	 16 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
							FROM "ORDN" T0 
							INNER JOIN "RDN1" T1 ON T0."DocEntry" = T1."DocEntry")) 
					UNION ALL (SELECT
	 T1."DocEntry",
	 17 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
						FROM "ORDR" T0 
						INNER JOIN "RDR1" T1 ON T0."DocEntry" = T1."DocEntry")) 
				UNION ALL (SELECT
	 T1."DocEntry",
	 14 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
					FROM "ORIN" T0 
					INNER JOIN "RIN1" T1 ON T0."DocEntry" = T1."DocEntry")) 
			UNION ALL (SELECT
	 T1."DocEntry",
	 19 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
				FROM "ORPC" T0 
				INNER JOIN "RPC1" T1 ON T0."DocEntry" = T1."DocEntry")) 
		UNION ALL (SELECT
	 T1."DocEntry",
	 21 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
			FROM "ORPD" T0 
			INNER JOIN "RPD1" T1 ON T0."DocEntry" = T1."DocEntry")) 
	UNION ALL (SELECT
	 T1."DocEntry",
	 67 AS "DocType",
	 T1."LineNum",
	 T0."Model",
	 T0."Serial",
	 T0."SeriesStr",
	 T0."SubStr",
	 T0."TaxDate",
	 T1."CFOPCode",
	 T1."AcctCode"  
		FROM "OWTR" T0 
		INNER JOIN "WTR1" T1 ON T0."DocEntry" = T1."DocEntry")) WITH READ ONLY