using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatPayPlatform.Models
{
    public class Score
    {
        public int ScoreId { get; set; }

        public int CountA { get; set; }

        public int CountB { get; set; }

        public int CountC { get; set; }

        public double ScoreA { get; set; }

        public double ScoreB { get; set; }

        public double ScoreC { get; set; }

        public string Remarks { get; set; }

        public Score AddScore(int s1, int s2, int s3)
        {
            var db = new ModelContext();
            this.ScoreA = (this.ScoreA * this.CountA + s1) / (this.CountA + 1);
            this.CountA++;

            this.ScoreB = (this.ScoreB * this.CountB + s2) / (this.CountB + 1);
            this.CountB++;

            this.ScoreC = (this.ScoreC * this.CountC + s3) / (this.CountC + 1);
            this.CountC++;
            db.SaveChanges();
            return this;
        }

        public double GetAverage()
        {
            return (this.ScoreA + this.ScoreB + this.ScoreC) / 3;
        }
    }

    public class ScoreLog
    {
        public int ScoreLogId { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Userid { get; set; }

        [ForeignKey("Userid")]
        public WechatUser User { get; set; }

        public int? BillNumber { get; set; }

        public int? ScoreId { get; set; }

        [ForeignKey("ScoreId")]
        public virtual Score Score { get; set; }

        public int s1 { get; set; }

        public int s2 { get; set; }

        public int s3 { get; set; }
    }
}
