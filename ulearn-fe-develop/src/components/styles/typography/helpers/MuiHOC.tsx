import { TypographyProps } from "@mui/material/Typography";

const withTypohraphyVariant = <P extends TypographyProps>(
    Component: React.ComponentType<P>,
    variant: TypographyProps['variant']
) => {
    // Create a new component that wraps the Typography component with the specified "variant"
    const WithVariant: React.FC<P> = (props) => {
        const { variant: variantProp, ...rest } = props;
        return <Component variant={variant || variantProp} {...rest as P} />;
    };

    WithVariant.displayName = `WithVariant(${Component.displayName || Component.name || 'Component'})`;

    return WithVariant;
}

export default withTypohraphyVariant;