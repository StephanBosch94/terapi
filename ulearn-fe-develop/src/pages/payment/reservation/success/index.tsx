import { withAppClientSideProvider } from "components/core/providers/AppClientSideProvider";
import PaymentSuccessPage from "components/pages/PaymentSuccessPage";

const SuccessPage = () => <PaymentSuccessPage type="successfull_reservation_payment" />

export default withAppClientSideProvider(SuccessPage);