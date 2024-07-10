using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.core.Auditory.Repository.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace jim.wiki.core.Auditory.Models
{

    [Table("Audit",Schema =Constantes.SCHEMA_BBDD)]
    public class Audit : Entity
    {
        public string Method { get; set; }
        public string Parameters { get; set; }
        public DateTime CreatedAt { get; set; }
        public string User { get; set; }
        public string Ip { get; set; }
        public Guid Guid { get; set; }
    }
}
