using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.payment
{
    public class vPayRequest
    {
        /// <summary>
        /// İşlem bilgileri bölümü
        /// </summary>
        public string VposRequest { get; set; }
        /// <summary>
        /// İşlem adı bilgisi Boşluk içermemelidir
        /// Sale -> Satış/Taksitli Satış 
        /// Cancel -> İptal 
        /// Refund -> İade 
        /// Auth -> Ön Prov. 
        /// Capture -> Ön Prov. 
        /// Kapama Reversal -> Teknik İptal 
        /// CampaignSearch BatchClosedSuccessSearch SurchargeSearch VFTSale -> Vade Farklı Satış 
        /// VFTSearch -> Vade Farklı Sorgu 
        /// TKSale -> Tarım Kart Eşit Taksitli Satış 
        /// TKFlexSale -> Tarım Kart Esnek Taksitli Satış 
        /// PointSale -> Puan harcama 
        /// PointSearch -> Puan Sorgu 
        /// CardTest -> Kart Kontrol
        /// </summary>
        public string TransactionType { get; set; }
        /// <summary>
        /// Üye işyeri numarası
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// Üye işyeri şifresi
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Benzersiz (Unique) İşlem numara bilgisi
        /// </summary>
        public string TransactionId => Guid.NewGuid().ToString("N");
        /// <summary>
        /// İşlemin hangi terminal üzerinden gönderileceği bilgisi
        /// </summary>
        public string TerminalNo { get; set; }
        /// <summary>
        /// İşlem tutar bilgisi
        /// </summary>
        public string CurrencyAmount { get; set; }
        /// <summary>
        /// Surchargelı işlem tutar bilgisidir. Surcharge uygulanmayacak işlemler için istek mesajında yer almamalıdır.
        /// </summary>
        public string SurchargeAmount { get; set; }
        /// <summary>
        /// İşlem kur bilgisi (YTL = 949)
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Puan tutarı
        /// </summary>
        public string PointAmount { get; set; }
        /// <summary>
        /// Puan birimi
        /// </summary>
        public string PointCode { get; set; }
        /// <summary>
        /// İşlem taksit sayısı Boş veya 0 gönderilmemelidir. Aksi takdirde hatalı taksit sayısı hatası alınacaktır. Taksitli ödemelerde en az 2 ile başlamalıdır.
        /// </summary>
        public string NumberOfInstallments { get; set; }
        /// <summary>
        /// Kredi kartı kart kuruluşu bilgisi
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// Kredi kart numarası bilgisi
        /// </summary>
        public string Pan { get; set; }
        /// <summary>
        /// Kredi kartı son kullanma tarihi bilgisi
        /// </summary>
        public string Expiry { get; set; }
        /// <summary>
        /// Kredi kartı güvenlik kodu bilgisi
        /// </summary>
        public string Cvv { get; set; }
        /// <summary>
        /// AMEX kartların ön yüzünde bulunan güvenlik numarasıdır
        /// </summary>
        public string SecurityCode { get; set; }
        /// <summary>
        /// Orjinal işlem üye işyeri işlem numarası bilgisi
        /// </summary>
        public string ReferenceTransactionId { get; set; }
        /// <summary>
        /// Kart sahibinin adı
        /// </summary>
        public string CardHoldersName { get; set; }
        /// <summary>
        /// İşlemi yapan son kullanıcının IP si Üye iş yeri tarafından alınıp sanal posa gönderilecektir
        /// </summary>
        public string ClientIp { get; set; }
        /// <summary>
        /// Sipariş numarası işlem başarılı olana kadar aynı sipariş numarası sisteme gönderilebilir(UyeRefNo)
        /// </summary>
        public string OrderId { get; set; }

        public string Cavv { get; set; }
        public string MpiTransactionId { get; set; }
        public string Eci { get; set; }

    }
}