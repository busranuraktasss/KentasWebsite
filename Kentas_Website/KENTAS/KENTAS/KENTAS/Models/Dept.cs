using Newtonsoft.Json;

namespace KENTAS.Models
{
    public class Dept : Result
    {
        public Data Data { get; set; }
    }

    public class Result
    {
        public string object_name { get; set; }
        public ResultCode code { get; set; }
        public string message { get; set; }

        public Result(string objName)
        {
            this.object_name = objName;
        }

        public Result()
        {

        }
    }

    public enum ResultCode
    {
        successful = 0,
        unsuccessful = 1,
        company_blocked = 2,
        agent_blocked = 3,
        agent_not_found = 4,
        duplicate_data = 5,
        no_process = 6,
        unauthorized = 7,
        server_error = 8,
        not_implemented = 9,
        time_out = 10,
        bad_request = 11,
        no_data = 12,
        paynetj_no_session = 13,
        paynetj_wrong_bin = 14,
        paynetj_unmatch_tran = 15,
        paynetj_3d_error = 16,
        paynetj_used_session = 17,
        wrong_card_data = 18,
        wrong_transaction_type = 19,
        wrong_pos_type = 20,
        wrong_ratio_get = 21,
        paynetj_expire_date_error = 22,
        ratio_code_not_found = 23,
        invoice_no_not_found = 24,
        card_not_found = 25,
        card_key_undefined = 26,
        old_successful = 100,
        subscription_on = 200,
        subscription_off = 201
    }
}