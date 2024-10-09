namespace Alpha.Infrastructure.Captcha
{
    public class NumericCaptcha
    {
        public virtual int FirstNumber { get; set; }
        public virtual int SecondNumber { get; set; }
        public virtual int Result { get; set; }

        //public NumericCaptcha()
        //{
        //    this.FirstNumber = new Random(DateTime.Now.Millisecond).Next(1, 50);
        //    this.SecondNumber = new Random(DateTime.Now.Millisecond).Next(1, 50);
        //}

        public bool IsResultCorrect(int firstNum, int secondNum, int result)
        {
            return (result == firstNum + secondNum);
        }
    }
}
