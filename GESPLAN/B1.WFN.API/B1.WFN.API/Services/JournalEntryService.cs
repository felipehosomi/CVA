using B1.WFN.API.Infrastructure;
using B1.WFN.API.Infrastructure.Exceptions;
using B1.WFN.API.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace B1.WFN.API.Services
{

    public class JournalEntryService
    {

        private DIAPIConnection _conn;
        public JournalEntryService(DIAPIConnection conn)
        {

            _conn = conn;
        }

        public int InsertJournalEntries(JournalEntryModel journalEntry)
        {
            Validate(journalEntry);
            if (!string.IsNullOrEmpty(_conn._LastError))
            {
                throw new DIAPIException(0, "Erro ao criar DI API " + _conn._LastError);
            }

            JournalEntries oJournalEntries = null;
            try
            {
                oJournalEntries = (JournalEntries)_conn.Connection.GetBusinessObject(BoObjectTypes.oJournalEntries);
            }
            catch (Exception ex)
            {
                throw new DIAPIException(0, "Erro ao instaciar JournalEntries: " + ex.Message);
            }

            if (journalEntry.ReferenceDate.HasValue)
                oJournalEntries.ReferenceDate = journalEntry.ReferenceDate.Value;

            if (journalEntry.DueDate.HasValue)
                oJournalEntries.DueDate = journalEntry.DueDate.Value;

            if (journalEntry.TaxDate.HasValue)
                oJournalEntries.TaxDate = journalEntry.TaxDate.Value;

            foreach (var entry in journalEntry.JournalEntryLines)
            {
                oJournalEntries.Lines.AccountCode = entry.AccountCode;
                oJournalEntries.Lines.Debit = entry.Debit;
                oJournalEntries.Lines.Credit = entry.Credit;

                if (entry.DueDate.HasValue)
                    oJournalEntries.Lines.DueDate = entry.DueDate.Value;

                oJournalEntries.Lines.CostingCode = entry.CostingCode;
                oJournalEntries.Lines.LineMemo = entry.LineMemo;

                if (entry.BPLID.HasValue)
                    oJournalEntries.Lines.BPLID = entry.BPLID.Value;
                oJournalEntries.Lines.Add();
            }

            var result = oJournalEntries.Add();

            if (result != 0)
                throw new DIAPIException(result, _conn.Connection.GetLastErrorDescription());

            var key = _conn.Connection.GetNewObjectKey();

            return Convert.ToInt32(key);

        }

        private void Validate(JournalEntryModel journalEntry)
        {
            if (journalEntry == null || journalEntry.JournalEntryLines == null || !journalEntry.JournalEntryLines.Any())
                throw new AppException("Nenhum item informado para inserção");

            var items = journalEntry.JournalEntryLines.ToArray();
            for (int i = 0; i < items.Count(); i++)
            {
                if (string.IsNullOrEmpty(items[i].AccountCode.Trim()))
                    throw new AppException($"O campo 'AccountCode' da posição [{i}] não foi preenchido");

            }

        }
    }
}
