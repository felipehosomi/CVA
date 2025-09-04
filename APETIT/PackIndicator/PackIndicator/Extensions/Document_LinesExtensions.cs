using PackIndicator.Models;
using SAPbobsCOM;

namespace PackIndicator.Extensions
{
    public static class Document_LinesExtensions
    {
        public static Document_Lines SetCurrentSalesLineByLineNum(this Document_Lines lines, int lineNum)
        {
            if (lines.Count > lineNum)
            {
                lines.SetCurrentLine(lineNum);
            }

            if (lines.LineNum != lineNum)
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    lines.SetCurrentLine(i);

                    if (lines.LineNum == lineNum) break;
                }
            }

            if (lines.LineNum != lineNum)
            {
                throw new System.Exception($"Linha {lineNum} não encontrada no pedido de vendas.");
            }

            return lines;
        }

        public static Document_Lines SetCurrentPurchaseLineByLineNum(this Document_Lines lines, int lineNum, PickingData line, int docnum)
        {
            if (lines.Count > lineNum)
            {
                lines.SetCurrentLine(lineNum);
            }

            if (lines.LineNum != lineNum)
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    lines.SetCurrentLine(i);

                    if (lines.LineNum == lineNum && (lines.ItemCode == line.ItemCode || lines.SupplierCatNum == line.ItemCode)) break;
                }
            }

            if (lines.LineNum != lineNum || (lines.ItemCode != line.ItemCode && lines.SupplierCatNum != line.ItemCode))
            {
                throw new System.Exception($@"Linha {lineNum} com o Item {line.ItemCode} do Pedido de Compra {docnum}, espelhada do pedido de venda {line.DocNum} linha {line.LineNum}, não encontrada.");
            }

            return lines;
        }
    }
}