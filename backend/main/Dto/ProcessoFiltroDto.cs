namespace main.Dto
{
    using System;
    using System.Collections.Generic;

    namespace main.Dto
    {
        public class ProcessoFiltroDto
        {
            public string? Numero { get; set; }
            public int? Orgao { get; set; }
            public string? Assunto { get; set; }
            public int? Status { get; set; }
            public List<int>? ClientesIds { get; set; }
            public int? ProcuradorId { get; set; }
            public int? PrazoId { get; set; }
            public int? DocumentoId { get; set; }
        }
    }

}
