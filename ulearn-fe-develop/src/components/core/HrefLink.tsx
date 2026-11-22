import Link from "next/link";
import { UrlObject } from 'url';

const HrefLink = ({ href, width, children }: { href: string | UrlObject, width?: string | undefined, children: JSX.Element }) => {
    return (
        <Link href={href} style={{ textDecoration: "none", color: "inherit", width: width ?? "auto" }} >
            {children}
        </Link>
    )
}

export default HrefLink;