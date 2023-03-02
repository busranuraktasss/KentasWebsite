using Newtonsoft.Json;
using System.Collections.Generic;

namespace KENTAS.Helper
{
    public class ClientResult : ClientResult<object>, IClientResult
    {

    }

    /// <summary>
    /// Client Result
    /// </summary>
    public class ClientResult<T> : IClientResult
    {
        /// <summary>
        /// Basarili
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Kod
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Mesaj Tipi
        /// </summary>
        public ClientResultMessageType MessageType { get; set; }

        /// <summary>
        /// Mesaj
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// Mesaj
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InternalMessage { get; set; }

        /// <summary>
        /// Veri
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        /// <summary>
        /// Validasyon Mesajlari
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ClientResultValidationMessage> Validations { get; set; }
    }

    /// <summary>
    /// Client Result
    /// </summary>
    public interface IClientResult
    {
    }

    public enum ClientResultMessageType : byte
    {
        /// <summary>
        /// Bilgilendirme
        /// </summary>
        Information = 0,
        /// <summary>
        /// Hata
        /// </summary>
        Error = 1,
        /// <summary>
        /// Basarili
        /// </summary>
        Success = 2,
        /// <summary>
        /// Uyari
        /// </summary>
        Warning = 3
    }

    public class ClientResultValidationMessage
    {
        /// <summary>
        /// Alan Adi
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Uyari Mesaji
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validasyon Mesaji
        /// </summary>
        public ClientResultValidationMessage(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
}