using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace N01511170_PassionProject.Models
{
    public class Song
    {
        [Key]
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string SingerName { get; set; }

        [Column(TypeName ="Date")]
        public DateTime ReleaseDate { get; set; }

        public string Language { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
    public class SongDTO
    {
      
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string SingerName { get; set; }

       
        public DateTime ReleaseDate { get; set; }

        public string Language { get; set; }

       
    }
}