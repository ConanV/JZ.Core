using System.Runtime.Serialization;

namespace JZ.Core.Utility
{
    /// <summary>
    /// 返回对象
    /// 注意：请使用CreateResult.For()创建对象
    /// </summary>
    [DataContract()]
    public class ResponseMessage<T> where T : class
    {
        /// <summary>
        /// 返回对象
        /// 注意：请使用CreateResult.For()创建对象
        /// </summary>
        public ResponseMessage()
        {
            Success = true;
            this.Message = "success";
            this.Code = "00000";
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        public ResponseMessage(T data, int total = 1, string message = "")
        {
            Success = true;
            this.Message = message;
            this.Code = "00000";
            Body = new ResponseBody<T> { Data = data, Total = total };
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        public ResponseMessage(string code, string msg)
        {
            Success = false;
            this.Code = code;
            this.Message = msg;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool Success { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 返回对象
        /// </summary>
        [DataMember]
        public ResponseBody<T> Body { get; set; }
    }

    /// <summary>
    /// 返回数据
    /// </summary>
    [DataContract()]
    public class ResponseBody<T>
    {
        /// <summary>
        /// 数据实体
        /// </summary>
        [DataMember]
        public T Data { get; set; }
        /// <summary>
        /// 集合条数（如果是集合）
        /// </summary>
        [DataMember]
        public int? Total { get; set; }

        
    }


    public static class CreateResult
    {
        public static ResponseMessage<T> For<T>(T t, int total = 1, string msg = "") where T : class
        {
            return new ResponseMessage<T>(t, total, msg);
        }

        public static ResponseMessage<object> For(string code, string msg)
        {
            return new ResponseMessage<object>(code, msg);
        }
        public static ResponseMessage<object> For()
        {
            return new ResponseMessage<object>();
        }
    }
}