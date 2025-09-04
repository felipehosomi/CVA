echo off
sqlcmd -S SERVERSAP -U sa -P alessi@2017 -Q "EXEC [SBOAlessi].dbo.[spc_cva_profitability]"