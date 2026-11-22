import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import PaymentSuccessPage from "components/pages/PaymentSuccessPage";

const SuccessPage = () => <PaymentSuccessPage type="successfull_money_refill" />

export default withAppClientSideProvider(SuccessPage);