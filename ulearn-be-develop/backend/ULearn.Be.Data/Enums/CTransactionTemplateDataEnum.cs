using System.ComponentModel;

namespace ULearn.Be.Data.Enums;

public enum CTransactionTemplateDataEnum
{
    [Description("id")] Id,
    [Description("created")] Created,
    [Description("username")] UserName,
    [Description("place_of_business")] PlaceOfBusiness,
    [Description("ico")] Ico,
    [Description("count")] Count,
    [Description("product_description")] ProductDescription,
    [Description("price")] Price,
    [Description("tax")] Tax,
    [Description("total_price")] TotalPrice,
    [Description("reservation_number")] ReservationNumber,
    [Description("withdraw_description")] WithdrawDescription,
    [Description("withdraw_amount")] WithdrawAmount,
    [Description("fee_description")] FeeDescription,
    [Description("fee_amount")] FeeAmount,
    [Description("total_amount")] TotalAmount,
    [Description("state")] State,
    [Description("fee_count")] FeeCount,
    [Description("due_date")] DueDate,
    [Description("execution_date")] ExecutionDate,
    [Description("customer_username")] CustomerUserName,
    [Description("customer_ico")] CustomerIco,
    [Description("customer_dic")] CustomerDic,
    [Description("reason")] Reason,
    [Description("sender_username")] SenderUsername,
    [Description("recepient_username")] RecipientUsername,
    [Description("transfer_description")] TransferDescription,
    [Description("sender")] Sender,
    [Description("recipient")] Recipient,
    [Description("coupon_count")] CouponCount,
    [Description("coupon_description")] CouponDescription,
    [Description("coupon_amount")] CouponAmount,
}