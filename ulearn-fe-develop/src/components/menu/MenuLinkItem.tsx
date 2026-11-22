import Box from "@mui/material/Box";
import { styled } from "@mui/material/styles";
import { COLORS } from "components/styles/constants/ColorsConstatnts";
import CaptionTextStyled from "components/styles/typography/CaptionTextStyled";
import Link from "next/link";

const LinkStyled = styled(Box)({
    display: "inline-flex",
    padding: "4px 12px",
    alignItems: "center",
    gap: "8px",
    width: "100%",
    textDecoration: "none",
    color: COLORS.BLACK,
    ":hover": {
        cursor: "pointer",
        backgroundColor: COLORS.PRIMARY_5
    }
}) as typeof Box;

const MenuItemTypographyStyled = styled(CaptionTextStyled)({
    fontWeight: "500",
    color: COLORS.BLACK,
    fontSize: "0.875rem"
}) as typeof CaptionTextStyled;

const MenuLinkItem = ({ text, href, rel, locale, icon = null }: { text: string, href: string, icon?: JSX.Element | null, locale?: string | false | undefined, rel?: string | undefined }) => (
    <Link href={href} rel={rel} locale={locale}>
        <LinkStyled>
            {icon}
            <MenuItemTypographyStyled>{text}</MenuItemTypographyStyled>
        </LinkStyled>
    </Link>
)

export default MenuLinkItem;