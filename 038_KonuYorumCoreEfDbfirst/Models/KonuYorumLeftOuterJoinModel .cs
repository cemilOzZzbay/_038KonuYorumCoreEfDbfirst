namespace _038_KonuYorumCoreEfDbfirst.Models
{
    public class KonuYorumLeftOuterJoinModel // KonuYorumInnerJoinDTO, DTO: Data Transfer Object  MODEL/DTO
    {
        #region Konu entity'sinden gelen özellikler
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        #endregion

        #region Yorum entity'sinden gelen özellikler
        public string Icerik { get; set; }
        public string Yorumcu { get; set; }
        public int? Puan { get; set; }
        #endregion
    }
}
