namespace fancy_boxes.src.Interfaces
{
    public interface IResult<T>
    {
        T Value
        {
            get;
            set;
        }

        bool IsSuccess
        {
            get;
            set;
        }

        string ErrorMessage
        {
            get;
            set;
        }
    }
}
