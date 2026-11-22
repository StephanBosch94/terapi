import { FlagBoxStyled } from "components/styles/boxes/FlagBoxStyled";
import { FlagIcon, FlagIconCode } from "react-flag-kit";

const FlagIconWrapper = ({ countryCode }: { countryCode: string | null | undefined }) => {

    if (!countryCode) return null;

    return (
        <FlagBoxStyled sx={{ width: "20px", height: "15px", overflow: "hidden" }}>
            <FlagIcon code={countryCode as FlagIconCode} width="20" height="15" />
        </FlagBoxStyled>
    )
}

export default FlagIconWrapper;