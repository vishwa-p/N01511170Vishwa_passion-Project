using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace N01511170_PassionProject.Models
{
    public class Album
    {
        [Key]
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string Createdby { get; set; }

        [Column(TypeName = "Date")]
        public DateTime RelaseDate { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
    public class AlbumDTO
    {
       
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public string Createdby { get; set; }

       
        public DateTime RelaseDate { get; set; }
       
       
    }
    public class AlbumViewModel
    {
        public AlbumDTO AlbumDto { get; set; }
        public IEnumerable<SongDTO> AlreadyAdded { get; set; }
        public IEnumerable<SongDTO> AvailableSongs { get; set; }

    }
}