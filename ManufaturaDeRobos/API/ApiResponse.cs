namespace ManufaturaDeRobos.API
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Error { get; set; }

        public T Results { get; set; }

        public string Message { get; set; }
    }
}

