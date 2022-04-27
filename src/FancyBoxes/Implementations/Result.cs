using fancy_boxes.src.Interfaces;

namespace fancy_boxes.src.Implementations
{
    public class Result<T> : IResult<T>
    {
        private T _value;
        private bool _isSuccess;
        private string _errorMessage;
        public T Value { get => _value; set => _value = value; }
        public bool IsSuccess { get => _isSuccess; set => _isSuccess = value; }
        public string ErrorMessage { get => _errorMessage; set => _errorMessage = value; }
    }
}
