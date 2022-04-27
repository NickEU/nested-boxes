using System.Collections.Generic;

namespace fancy_boxes.src.Interfaces
{
    public interface IInputSource
    {
        // I hate the design of this abstraction, can feel unintuitive and cryptic
        IResult<IList<string>> Read(string source = "");
    }
}
