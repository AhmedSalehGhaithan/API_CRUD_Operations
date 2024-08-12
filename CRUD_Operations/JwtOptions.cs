namespace CRUD_Operations
{
    public class JwtOptions
    {
        public string Isuuer { get; set; }

        public string Audience { get; set; }

            public int Lifetime { get; set; }

            public string SigningKey { get; set; }
    }
}
