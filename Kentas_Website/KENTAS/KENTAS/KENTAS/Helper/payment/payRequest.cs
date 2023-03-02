using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KENTAS.Helper.payment
{
    public class payRequest
    {
        /// <summary>
        /// Üye İşyeri Numarası
        /// </summary>
        public string MerchantId { get; set; }
        /// <summary>
        /// ÜİY Şifresi Api Şifresi Api şifresi, değişken
        /// </summary>
        public string MerchantPassword { get; set; }
        /// <summary>
        /// Kredi kartı numarası
        /// </summary>
        public string Pan { get; set; }
        /// <summary>
        /// Kredi kartı son kullanma tarihi 
        /// YYAA formatında 4 karakter olarak gönderilmelidir.
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// Satış tutarı ile küsuratı nokta işareti ile ayrılmalı, küsurat kısmı 2 hane olmalıdır. Bu alan en fazla 12 hane uzunluğunda olabilir. Tutar kısmı 3 haneyi aştığında nokta işareti kullanılmamalıdır.
        /// </summary>
        public string PurchaseAmount { get; set; }
        /// <summary>
        /// İşlemin yapıldığı sayısal para birimi kodu
        /// Sayısal – Tamsayı - 3 Hane. Bu alan boş iken işlem gönderildiğinde default olarak 949 CurrencyCode’a göre işlem gerçekleştirilmektedir.
        /// 949:TRY 840:USD 978:EUR 826:GBP
        /// </summary>
        public int Currency { get; set; }
        /// <summary>
        /// Kredi kartı Kart Kuruluşu Bilgisi
        /// Sayısal – Tamsayı – 3 Hane. Kart logosuna uygun gönderilmelidir.
        /// 100:VISA 200:MASTERCARD 300:TROY
        /// </summary>
        public int BrandName { get; set; }
        /// <summary>
        /// Oturum Bilgisi
        /// ÜİY tarafında gönderilmesi Opsiyonel, sadece bilgi amaçlı tutulan bir alandır
        /// Max 500 karakter gönderilmelidir.
        /// </summary>
        public string SessionInfo { get; set; }
        /// <summary>
        /// ÜİY’nin işlem sonucun başarılı olması durumunda, dönüş yapılmasını istediği sayfa URL i.
        /// Eğer bu alan gönderilmediyse GET 7/24 MPI, ÜİY için tanımlı SuccessUrl e dönüş yapacaktır. Maksimum 255 karakter olmalıdır. Gönderilen değer bir url olmalıdır.
        /// Gönderilen değer Regex paternine uymalıdır: http[s]?://(([^/:\.[:space:]]+(\.[^/:\.[:space:]]+)*)|([0-9](\.[0-9]{3})))(:[0-9]+)?((/[^?#[:space:]]+)(\?[^#[:space:]]+)?(\#.+)?)?
        /// </summary>
        public string SuccessUrl { get; set; }
        /// <summary>
        /// ÜİY’nin işlem sonucunun başarısız olması durumunda, dönüş yapılmasını istediği sayfa URL i.
        /// Eğer bu alan gönderilmediyse GET 7/24 MPI, ÜİY için tanımlı FailureUrl e dönüş yapacaktır. Maksimum 255 karakter olmalıdır. Gönderilen değer bir url olmalıdır.
        /// Gönderilen değer Regex paternine uymalıdır: http[s]?://(([^/:\.[:space:]]+(\.[^/:\.[:space:]]+)*)|([0-9](\.[0-9]{3})))(:[0-9]+)?((/[^?#[:space:]]+)(\?[^#[:space:]]+)?(\#.+)?)?
        /// </summary>
        public string FailureUrl { get; set; }
        /// <summary>
        /// İşlem taksit sayısı
        /// Eğer ÜİY tarafından gönderilirse, 1'den büyük bir değer olmalıdır. 0 ya da 1 gönderildiğinde MPI işlemi reddeder.
        /// Ödeme taksitli olacaksa taksit değerinin Sanal POS aşamasında da belirtilmesi gerekmektedir.
        /// </summary>
        public int InstallmentCount { get; set; }

        public string CardHoldersName { get; set; }
        public string Cvv { get; set; }


        public string VerifyEnrollmentRequestId => Guid.NewGuid().ToString("N");


    }
}