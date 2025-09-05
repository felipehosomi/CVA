using B1.WFN.API.Infrastructure;
using B1.WFN.API.Infrastructure.BasicAuth;
using B1.WFN.API.Infrastructure.Exceptions;
using B1.WFN.API.Models;
using B1.WFN.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace B1.WFN.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JournalEntriesController : ControllerBase
    {
        private readonly JournalEntryService _journalEntryService;
        private string _errorInstance;

        public JournalEntriesController(JournalEntryService journalEntryService)
        {
            try
            {
                _journalEntryService = journalEntryService;
            }
            catch (Exception ex)
            {
                _errorInstance = ex.Message;
            }
            
        }



        [HttpPost]
        [BasicAuth]
        public IActionResult Post([FromBody] JournalEntryModel entry)
        {
            if(!string.IsNullOrEmpty(_errorInstance))
                return BadRequest(new
                {
                    message = "Erro Instancia = " + _errorInstance
                });

            try
            {
                return Created(string.Empty, new
                {
                    journalEntryId = _journalEntryService.InsertJournalEntries(entry)
                });
            }
            catch (AppException ex)
            {
                return Ok(new
                {
                    message = ex.Message
                });
            }
            catch (DIAPIException ex)
            {
                return Ok(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    message = "Erro Inesperado" + ex.Message
                });
            }
        }

    }
}
