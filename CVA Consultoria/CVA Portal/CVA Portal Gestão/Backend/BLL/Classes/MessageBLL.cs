using System;
using MODEL.Classes;

namespace BLL.Classes
{
    public class MessageBLL
    {
        public static MessageModel Generate(string message, int code, bool isError = false)
        {
            try
            {
                var msg = new MessageModel();
                if(isError)
                {
                    msg.Error = new ErrorMessage()
                    {
                        Code = code,
                        Message = message
                    };
                }
                else
                {
                    msg.Success = new SuccessMessage()
                    {
                        Code = code,
                        Message = message
                    };
                }
                return msg;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
